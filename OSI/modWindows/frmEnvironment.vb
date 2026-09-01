'--------------------------------------------------------------------------------------------------
' Win32_Environment class
' https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-environment
'
'    © Remus Rigo
'       v1.0.20260818
'--------------------------------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.Management
Imports SharedInterfaces

Public Class frmEnvironment
   Implements IModuleForm
   <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
   Public Property MainForm As IMainForm Implements IModuleForm.MainForm
   Public remoteHost, remoteUser, remotePass As String

   Private Sub BackgroundWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
      Dim myConnection As New ConnectionOptions()
      Dim scopePath As String
      Dim items1 As New List(Of ListViewItem)
      Dim items2 As New List(Of ListViewItem)

      ' Method 1: Local environment variables
      If (remoteHost = "" And remoteUser = "") Then
         Dim envVars As IDictionary = Environment.GetEnvironmentVariables()
         For Each key As String In envVars.Keys
            Dim item1 As New ListViewItem(key)
            item1.SubItems.Add(envVars(key))
            items1.Add(item1)
         Next
      End If

      'Dim dictEntry As System.Collections.DictionaryEntry
      'For Each dictEntry In Environment.GetEnvironmentVariables()
      '   li = lvEnv.Items.Add("")
      '   li.Text = dictEntry.Key.ToString()
      '   li.SubItems.Add(dictEntry.Value.ToString())
      'Next

      ' Method 2: Local/Remote environment variables via WMI   
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
         Dim myQuery As New ObjectQuery("SELECT * FROM Win32_Environment")
         Dim searcher As New ManagementObjectSearcher(scope, myQuery)
         Dim cnt As Integer = 0

         For Each obj As ManagementObject In searcher.Get()
            Dim item2 As New ListViewItem(obj("UserName").ToString())
            item2.SubItems.Add(obj("Name").ToString())
            item2.SubItems.Add(obj("VariableValue").ToString())
            items2.Add(item2)
         Next
      Catch ex As Exception
         MsgBox(ex.Message)
      End Try

      e.Result = Tuple.Create(items1, items2)
   End Sub

   ' Update ListView when background work is completed
   Private Sub BackgroundWorker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)
      If e.Error IsNot Nothing Then
         MessageBox.Show("Error: " & e.Error.Message, "BackgroundWorker Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
         Return
      End If

      If e.Cancelled Then
         MessageBox.Show("Operation was cancelled.", "BackgroundWorker Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
         Return
      End If

      ' result is valid
      If e.Result IsNot Nothing Then
         Dim resultTuple = CType(e.Result, Tuple(Of List(Of ListViewItem), List(Of ListViewItem)))
         Dim items1 = resultTuple.Item1
         Dim items2 = resultTuple.Item2

         If lvEnv1.InvokeRequired Then
            lvEnv1.Invoke(New Action(Of List(Of ListViewItem))(AddressOf UpdateListView1), items1)
         Else
            UpdateListView1(items1)
         End If
         lvEnv1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)

         If lvEnv2.InvokeRequired Then
            lvEnv2.Invoke(New Action(Of List(Of ListViewItem))(AddressOf UpdateListView2), items2)
         Else
            UpdateListView2(items2)
         End If
         lvEnv2.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
      End If

   End Sub

   Private Sub UpdateListView1(items As List(Of ListViewItem))
      lvEnv1.Items.Clear()
      lvEnv1.Items.AddRange(items.ToArray())
   End Sub
   Private Sub UpdateListView2(items As List(Of ListViewItem))
      lvEnv2.Items.Clear()
      lvEnv2.Items.AddRange(items.ToArray())
   End Sub

   Private Sub frmEnv_Load(sender As Object, e As EventArgs) Handles MyBase.Load
      lvEnv1.BackColor = Color.FromArgb(224, 234, 213)
      lvEnv2.BackColor = Color.FromArgb(224, 234, 213)

      Dim backgroundWorker As New System.ComponentModel.BackgroundWorker()
      AddHandler backgroundWorker.DoWork, AddressOf BackgroundWorker_DoWork
      AddHandler backgroundWorker.RunWorkerCompleted, AddressOf BackgroundWorker_RunWorkerCompleted
      backgroundWorker.RunWorkerAsync()

      If MainForm IsNot Nothing Then
         MainForm.SetTitle("Remus Rigo OSI: Environment variables v1.0.20260621" & If(remoteHost <> "", "on " & "[" & remoteHost & "]", ""))
      End If
   End Sub

End Class