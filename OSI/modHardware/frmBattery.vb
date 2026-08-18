'--------------------------------------------------------------------------------------------------
' Win32_Battery
' https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-battery
'
'    © Remus Rigo
'       v1.0.20260818
'--------------------------------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.Management
Imports System.[Private].Windows
Imports SharedInterfaces

Public Class frmBattery
   Implements IModuleForm
   <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
   Public Property MainForm As IMainForm Implements IModuleForm.MainForm
   Public remoteHost, remoteUser, remotePass As String

   Private Class ProcListItem
      Public Property Group As String
      Public Property Label As String
      Public Property Value As String
      Public Property ImageIndex As Integer = -1
   End Class

   Private Enum ProgressKind
      SetMax
      SetValue
      AppendItem
   End Enum

   Private Class ProgressInfo
      Public Property Kind As ProgressKind
      Public Property Max As Integer
      Public Property Value As Integer
      Public Property ImageIndex As Integer
      Public Property Item As ProcListItem
   End Class

   ' Tracks groups already created on the ListView, so new items are added to the correct group without recreating it each time.
   Private groupCache As New Dictionary(Of String, ListViewGroup)

   '-----------------------------------------------------------------------------------------------
   ' BackgroundWorker: DoWork
   Private Sub BackgroundWorker_DoWork(sender As Object, e As DoWorkEventArgs)
      Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)
      Dim myConnection As New ConnectionOptions()
      Dim scopePath As String

      If (remoteHost <> "" And remoteUser <> "") Then
         myConnection.Username = remoteUser
         myConnection.Password = remotePass
         myConnection.Impersonation = ImpersonationLevel.Impersonate
         myConnection.Authentication = AuthenticationLevel.PacketPrivacy
         scopePath = $"\\{remoteHost}\root\cimv2"
      Else
         scopePath = "\\.\root\cimv2"
      End If

      Dim scope As New ManagementScope(scopePath, myConnection)

      Try
         scope.Connect()

         Dim myQuery As New ObjectQuery("SELECT * FROM Win32_Battery")
         Dim searcher As New ManagementObjectSearcher(scope, myQuery)
         Dim cnt As Integer = 0
         Dim crtAction As Integer = 1
         Dim objItems = searcher.Get()
         Dim objCounter As Integer = objItems.Count
         Dim propsPerObj As Integer = 0

         If objCounter > 0 Then
            propsPerObj = objItems.Cast(Of ManagementObject)().First().Properties.Cast(Of PropertyData)().Count(Function(p) p.Name <> "Class" AndAlso p.Name <> "Path")
         End If

         Dim totalProps As Integer = propsPerObj * objCounter
         worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetMax, .Max = totalProps})

         For Each obj As ManagementObject In objItems
            cnt += 1
            Dim groupName As String = "Battery #" & cnt

            ' Info --------------------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "Info", .Value = ""}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Name") Then
               If (obj("Name") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Name", .Value = obj("Name").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Caption") Then
               If (obj("Caption") IsNot Nothing) And (obj("Caption") <> obj("Name")) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                  .Group = groupName, .Label = "Caption", .Value = obj("Caption").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Description") Then
               If (obj("Description") IsNot Nothing) And (obj("Description") <> obj("Name")) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                  .Group = groupName, .Label = "Description", .Value = obj("Description").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Chemistry") Then
               If (obj("Chemistry") IsNot Nothing) Then
                  Dim tmp As String = ""
                  Select Case obj("Chemistry")
                     Case 1 : tmp = "Other"
                     Case 2 : tmp = "Unknown"
                     Case 3 : tmp = "Lead Acid"
                     Case 4 : tmp = "Nickel Cadmium"
                     Case 5 : tmp = "Nickel Metal Hydride"
                     Case 6 : tmp = "Lithium-ion"
                     Case 7 : tmp = "Zinc air"
                     Case 8 : tmp = "Lithium Polymer"
                     Case Else : tmp = "Unrecognized code"
                  End Select
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Chemistry", .Value = tmp, .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "DeviceID") Then
               If (obj("DeviceID") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Device ID", .Value = obj("DeviceID").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "PNPDeviceID") Then
               If (obj("PNPDeviceID") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "PNP Device ID", .Value = obj("PNPDeviceID").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            ' Features ----------------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "Features", .Value = ""}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "DesignCapacity") Then
               If (obj("DesignCapacity") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Design Capacity", .Value = obj("DesignCapacity").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "DesignVoltage") Then
               If (obj("DesignVoltage") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Design Voltage", .Value = obj("DesignVoltage").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            ' Charging ----------------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "Features", .Value = ""}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "BatteryRechargeTime") Then
               If (obj("BatteryRechargeTime") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Battery Recharge Time", .Value = obj("BatteryRechargeTime").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "EstimatedChargeRemaining") Then
               If (obj("EstimatedChargeRemaining") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Estimated Charge Remaining", .Value = obj("EstimatedChargeRemaining").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "EstimatedRunTime") Then
               If (obj("EstimatedRunTime") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Estimated RunTime", .Value = obj("EstimatedRunTime").ToString() & " minutes", .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ExpectedBatteryLife") Then
               If (obj("ExpectedBatteryLife") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Expected Battery Life", .Value = obj("ExpectedBatteryLife").ToString() & " minutes", .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ExpectedLife") Then
               If (obj("ExpectedLife") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Expected Life", .Value = obj("ExpectedLife").ToString() & " minutes", .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "FullChargeCapacity") Then
               If (obj("FullChargeCapacity") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Full Charge Capacity", .Value = obj("FullChargeCapacity").ToString() & " milliwatt-hours", .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "MaxRechargeTime") Then
               If (obj("MaxRechargeTime") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Max Recharge Time", .Value = obj("MaxRechargeTime").ToString() & " minutes", .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "TimeOnBattery") Then
               If (obj("TimeOnBattery") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Time On Battery", .Value = obj("TimeOnBattery").ToString() & " seconds", .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "TimeToFullCharge") Then
               If (obj("TimeToFullCharge") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Time To Full Charge", .Value = obj("TimeToFullCharge").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            ' Config Manager -------------------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "Config Manager", .Value = ""}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ConfigManagerUserConfig") Then
               If (obj("ConfigManagerUserConfig") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                  .Group = groupName,
                     .Label = "User Config",
                     .Value = "Device is " & If(obj("ConfigManagerUserConfig"), "", "NOT ") & "using a user-defined configuration",
                     .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ConfigManagerErrorCode") Then
               If (obj("ConfigManagerErrorCode") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Error Code", .Value = GetWMIConfigManagerErrorCode(obj("ConfigManagerErrorCode")), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            ' Power Management --------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "Power Management", .Value = ""}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "PowerManagementSupported") Then
               If (obj("PowerManagementSupported") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Power Management Supported", .Value = If(obj("PowerManagementSupported"), "Yes", "No"), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            Dim pmCaps = CType(obj("PowerManagementCapabilities"), UInt16())
            If pmCaps IsNot Nothing AndAlso pmCaps.Length > 0 Then
               For i As Integer = 0 To pmCaps.Length - 1
                  Dim tmp As String = ""
                  Select Case pmCaps(i)
                     Case 0 : tmp = "Unknown"
                     Case 1 : tmp = "Not Supported"
                     Case 2 : tmp = "Disabled"
                     Case 3 : tmp = "Enabled"
                     Case 4 : tmp = "Power Saving Modes Entered Automatically"
                     Case 5 : tmp = "Power State Settable"
                     Case 6 : tmp = "Power Cycling Supported"
                     Case 7 : tmp = "Timed Power-On Supported"
                     Case Else
                        tmp = "Vendor-specific / Undefined"
                  End Select
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Power Management Capability " & pmCaps(i), .Value = tmp, .ImageIndex = 0}})
               Next
            Else
               worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                  .Group = groupName, .Label = "Power Management Capability", .Value = "No Power Management Capability data returned", .ImageIndex = 0}})
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            ' Error -------------------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "Error", .Value = ""}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Status") Then
               If (obj("Status") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Status", .Value = obj("Status").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "StatusInfo") Then
               If (obj("StatusInfo") IsNot Nothing) Then
                  Dim tmp As String = ""
                  Select Case obj("StatusInfo")
                     Case 1 : tmp = "Other"
                     Case 2 : tmp = "Unknown"
                     Case 3 : tmp = "Enabled"
                     Case 4 : tmp = "Disabled"
                     Case 5 : tmp = "Not Applicable"
                  End Select
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Status Info", .Value = tmp, .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "BatteryStatus") Then
               If (obj("BatteryStatus") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Battery Status", .Value = GetWMIBatteryStatus(obj("BatteryStatus")), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ErrorCleared") Then
               If (obj("ErrorCleared") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Error Cleared", .Value = obj("ErrorCleared").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ErrorDescription") Then
               If (obj("ErrorDescription") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Error Description", .Value = obj("ErrorDescription").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "LastErrorCode") Then
               If (obj("LastErrorCode") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Last Error Code", .Value = obj("LastErrorCode").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            ' Other -------------------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "Other", .Value = ""}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Availability") Then
               If (obj("Availability") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Availability", .Value = GetWMIAvailability(obj("Availability")), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "InstallDate") Then
               If (obj("InstallDate") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Install Date", .Value = obj("InstallDate").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SmartBatteryVersion") Then
               If (obj("SmartBatteryVersion") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Smart Battery Version", .Value = obj("SmartBatteryVersion").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SystemName") Then
               If (obj("SystemName") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "System Name", .Value = obj("SystemName").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            'Excluded ----------------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1 ' CreationClassName
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1 ' SystemCreationClassName

         Next
      Catch ex As Exception
         MsgBox(ex.Message)
      End Try
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' BackgroundWorker: RunWorkerCompleted (Update ListView when background work is completed)
   Private Sub BackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
      If e.Error IsNot Nothing Then
         MessageBox.Show("Error: " & e.Error.Message, "BackgroundWorker Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
         Return
      End If

      If e.Cancelled Then
         MessageBox.Show("Operation was cancelled.", "BackgroundWorker Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
         Return
      End If

      ' Optional: Auto-resize columns for better display
      lvBattery.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' BackgroundWorker: ProgressChanged (Runs on the UI thread automatically)
   Private Sub BackgroundWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs)
      Dim info As ProgressInfo = CType(e.UserState, ProgressInfo)
      Select Case info.Kind
         Case ProgressKind.SetMax
            MainForm?.SetProgressMax(info.Max)
         Case ProgressKind.SetValue
            MainForm?.SetProgressValue(info.Value)
         Case ProgressKind.AppendItem
            AppendLiveItem(info.Item)
      End Select
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' UpdateListView (with the retrieved items)
   Private Sub AppendLiveItem(item As ProcListItem)
      If item Is Nothing Then Return

      Dim grp As ListViewGroup = Nothing
      If Not groupCache.TryGetValue(item.Group, grp) Then
         grp = New ListViewGroup(item.Group, HorizontalAlignment.Left)
         groupCache.Add(item.Group, grp)
         lvBattery.Groups.Add(grp)
      End If

      Dim lvi As New ListViewItem(item.Label)
      lvi.SubItems.Add(item.Value)
      lvi.Group = grp
      lvi.ImageIndex = item.ImageIndex

      If String.IsNullOrWhiteSpace(item.Value) Then
         lvi.BackColor = Color.LightGray
         lvi.Font = New Font(lvi.Font, FontStyle.Bold)
      End If

      lvBattery.Items.Add(lvi)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' BackgroundScan
   Private Sub BackgroundScan()
      lvBattery.Items.Clear()
      lvBattery.Groups.Clear()
      groupCache.Clear()

      Dim backgroundWorker As New BackgroundWorker()
      backgroundWorker.WorkerReportsProgress = True
      AddHandler backgroundWorker.DoWork, AddressOf BackgroundWorker_DoWork
      AddHandler backgroundWorker.ProgressChanged, AddressOf BackgroundWorker_ProgressChanged
      AddHandler backgroundWorker.RunWorkerCompleted, AddressOf BackgroundWorker_RunWorkerCompleted
      MainForm?.ResetProgress()
      backgroundWorker.RunWorkerAsync()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmBaseBoard: OnLoad
   Private Sub frmBattery_Load(sender As Object, e As EventArgs) Handles MyBase.Load
      lvBattery.BackColor = Color.FromArgb(224, 234, 213)

      If MainForm IsNot Nothing Then
         MainForm.SetTitle("Remus Rigo OSI: Battery v1.0.20260818" & If(remoteHost <> "", "on " & "[" & remoteHost & "]", ""))
      End If

      BackgroundScan()
   End Sub

End Class