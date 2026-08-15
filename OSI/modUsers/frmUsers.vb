'--------------------------------------------------------------------------------------------------
' Win32_UserAccount class
' https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-useraccount
'
'    © Remus Rigo
'       v1.0.20260810
'--------------------------------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.Management
Imports System.Runtime.InteropServices.JavaScript.JSType
Imports SharedInterfaces

Public Class frmUsers
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
   Private Sub BackgroundWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
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

         Dim myQuery As New ObjectQuery("SELECT * FROM Win32_UserAccount")
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

         For Each obj As ManagementObject In searcher.Get()
            cnt += 1
            Dim groupName As String = "User #" & cnt

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

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "FullName") Then
               If (obj("FullName") IsNot Nothing) Then
                  Dim tmp As String = obj("FullName").ToString()
                  If tmp Is Nothing OrElse tmp = "" Then
                     tmp = "n/a"
                  End If
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Full Name", .Value = tmp, .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Domain") Then
               If (obj("Domain") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Domain", .Value = obj("Domain").ToString(), .ImageIndex = 0}})
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

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "AccountType") Then
               If (obj("AccountType") IsNot Nothing) Then
                  Dim tmp As String = ""
                  Select Case obj("AccountType")
                     Case 256 : tmp = "Temporary duplicate account"
                     Case 512 : tmp = "Normal account"
                     Case 2048 : tmp = "Interdomain trust account"
                     Case 4096 : tmp = "Workstation trust account"
                     Case 8192 : tmp = "Server trust account"
                     Case Else : tmp = "not defined"
                  End Select
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Account Type", .Value = tmp, .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "LocalAccount") Then
               If (obj("LocalAccount") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Local Account", .Value = obj("LocalAccount").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Disabled") Then
               If (obj("Disabled") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Disabled", .Value = obj("Disabled").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Lockout") Then
               If (obj("Lockout") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Lockout", .Value = obj("Lockout").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "PasswordChangeable") Then
               If (obj("PasswordChangeable") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Password Changeable", .Value = obj("PasswordChangeable").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "PasswordExpires") Then
               If (obj("PasswordExpires") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Password Expires", .Value = obj("PasswordExpires").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "PasswordRequired") Then
               If (obj("PasswordRequired") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Password Required", .Value = obj("PasswordRequired").ToString(), .ImageIndex = 0}})
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

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SID") Then
               If (obj("SID") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "SID", .Value = obj("SID").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SIDType") Then
               If (obj("SIDType") IsNot Nothing) Then
                  Dim tmp As String = ""
                  Select Case obj("SIDType")
                     Case 1 : tmp = "User"
                     Case 2 : tmp = "Group"
                     Case 3 : tmp = "Domain"
                     Case 4 : tmp = "Alias"
                     Case 5 : tmp = "Well Known Group"
                     Case 6 : tmp = "Deleted Account"
                     Case 7 : tmp = "Invalid"
                     Case 8 : tmp = "Unknown"
                     Case 9 : tmp = "Computer"
                     Case Else : tmp = "not defined"
                  End Select
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "SID Type", .Value = tmp, .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

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
      lvUsers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
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
         lvUsers.Groups.Add(grp)
      End If

      Dim lvi As New ListViewItem(item.Label)
      lvi.SubItems.Add(item.Value)
      lvi.Group = grp
      lvi.ImageIndex = item.ImageIndex

      If String.IsNullOrWhiteSpace(item.Value) Then
         lvi.BackColor = Color.LightGray
         lvi.Font = New Font(lvi.Font, FontStyle.Bold)
      End If

      lvUsers.Items.Add(lvi)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' BackgroundScan
   Private Sub BackgroundScan()
      lvUsers.Items.Clear()
      lvUsers.Groups.Clear()
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
   Private Sub frmBaseBoard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
      lvUsers.BackColor = Color.FromArgb(224, 234, 213)

      If MainForm IsNot Nothing Then
         MainForm.SetTitle("Remus Rigo OSI: User Accounts v1.0.20260810" & If(remoteHost <> "", "on " & "[" & remoteHost & "]", ""))
      End If

      BackgroundScan()
   End Sub

End Class