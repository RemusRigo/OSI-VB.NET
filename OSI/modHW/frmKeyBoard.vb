'--------------------------------------------------------------------------------------------------
' Win32_Keyboard
' https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-keyboard
'
'    © Remus Rigo
'       v1.0.20260815
'--------------------------------------------------------------------------------------------------

Imports SharedInterfaces
Imports System.ComponentModel
Imports System.Management

Public Class frmKeyBoard
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

         Dim myQuery As New ObjectQuery("SELECT * FROM Win32_Keyboard")
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
            Dim groupName As String = "KeyBoard #" & cnt

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
               If (obj("Caption") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Caption", .Value = obj("Caption").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Description") Then
               If (obj("Description") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Description", .Value = obj("Description").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            ' Other -------------------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "Other", .Value = ""}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Availability") Then
               If (obj("Availability") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Availability", .Value = obj("Availability").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ConfigManagerErrorCode") Then
               If (obj("ConfigManagerErrorCode") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "ConfigManagerErrorCode", .Value = obj("ConfigManagerErrorCode").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ConfigManagerUserConfig") Then
               If (obj("ConfigManagerUserConfig") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "ConfigManagerUserConfig", .Value = obj("ConfigManagerUserConfig").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "DeviceID") Then
               If (obj("DeviceID") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "DeviceID", .Value = obj("DeviceID").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ErrorCleared") Then
               If (obj("ErrorCleared") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "ErrorCleared", .Value = obj("ErrorCleared").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ErrorDescription") Then
               If (obj("ErrorDescription") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "ErrorDescription", .Value = obj("ErrorDescription").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "InstallDate") Then
               If (obj("InstallDate") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "InstallDate", .Value = obj("InstallDate").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "IsLocked") Then
               If (obj("IsLocked") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "IsLocked", .Value = obj("IsLocked").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "LastErrorCode") Then
               If (obj("LastErrorCode") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "LastErrorCode", .Value = obj("LastErrorCode").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Layout") Then
               If (obj("Layout") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Layout", .Value = obj("Layout").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "NumberOfFunctionKeys") Then
               If (obj("NumberOfFunctionKeys") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "NumberOfFunctionKeys", .Value = obj("NumberOfFunctionKeys").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Password") Then
               If (obj("Password") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Password", .Value = obj("Password").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "PNPDeviceID") Then
               If (obj("PNPDeviceID") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "PNPDeviceID", .Value = obj("PNPDeviceID").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "PowerManagementCapabilities") Then
               If (obj("PowerManagementCapabilities") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "PowerManagementCapabilities", .Value = String.Join(", ", DirectCast(obj("PowerManagementCapabilities"), UInt16())), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "PowerManagementSupported") Then
               If (obj("PowerManagementSupported") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "PowerManagementSupported", .Value = obj("PowerManagementSupported").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Status") Then
               If (obj("Status") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Status", .Value = obj("Status").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "StatusInfo") Then
               If (obj("StatusInfo") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "StatusInfo", .Value = obj("StatusInfo").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SystemName") Then
               If (obj("SystemName") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "SystemName", .Value = obj("SystemName").ToString(), .ImageIndex = 0}})
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
      lvKeyBoard.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
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
         lvKeyBoard.Groups.Add(grp)
      End If

      Dim lvi As New ListViewItem(item.Label)
      lvi.SubItems.Add(item.Value)
      lvi.Group = grp
      lvi.ImageIndex = item.ImageIndex

      If String.IsNullOrWhiteSpace(item.Value) Then
         lvi.BackColor = Color.LightGray
         lvi.Font = New Font(lvi.Font, FontStyle.Bold)
      End If

      lvKeyBoard.Items.Add(lvi)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' BackgroundScan
   Private Sub BackgroundScan()
      lvKeyBoard.Items.Clear()
      lvKeyBoard.Groups.Clear()
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
   Private Sub frmKeyBoard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
      lvKeyBoard.BackColor = Color.FromArgb(224, 234, 213)

      If MainForm IsNot Nothing Then
         MainForm.SetTitle("Remus Rigo OSI: KeyBoard v1.0.20260815" & If(remoteHost <> "", "on " & "[" & remoteHost & "]", ""))
      End If

      BackgroundScan()
   End Sub

End Class