Imports System.DirectoryServices.ActiveDirectory
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports modWindows, modHardware, SharedInterfaces

Public Class frmOSI
   '-----------------------------------------------------------------------------------------------
   Implements IMainForm

   Public Sub SetTitle(text As String) Implements IMainForm.SetTitle
      Me.Text = text
   End Sub

   Public Sub ResetProgress() Implements IMainForm.ResetProgress
      pbLoad.Value = 0
   End Sub

   Public Sub SetProgressMax(value As Integer) Implements IMainForm.SetProgressMax
      pbLoad.Maximum = value
   End Sub

   Public Sub SetProgressValue(value As Integer) Implements IMainForm.SetProgressValue
      pbLoad.Value = Math.Min(value, pbLoad.Maximum)
   End Sub

   '-----------------------------------------------------------------------------------------------

   Private pbLoad As rrProgressBar
   Private isRemote As Boolean = False
   Private remoteHost As String = ""
   Private remoteUser As String = ""
   Private remotePass As String = ""

   Private Function AppHasCommandLineArgs() As Boolean
      Return My.Application.CommandLineArgs.Count > 0
   End Function

   Private Sub ParseCommandLineArgs()
      For Each arg As String In My.Application.CommandLineArgs
         If arg.StartsWith("/", StringComparison.OrdinalIgnoreCase) Then
            Dim separatorIndex As Integer = arg.IndexOf(":"c)

            If separatorIndex > 0 Then
               Dim key As String = arg.Substring(1, separatorIndex - 1).ToLowerInvariant()
               Dim value As String = arg.Substring(separatorIndex + 1)

               Select Case key
                  Case "host"
                     remoteHost = value
                     MessageBox.Show("host" & remoteHost)
                  Case "user"
                     remoteUser = value
                     MessageBox.Show("user" & remoteUser)
                  Case "pass"
                     remotePass = value
                     MessageBox.Show("pass" & remotePass)
                  Case Else
                     ' Unknown switch - ignore or log
               End Select
            End If
         End If
      Next
   End Sub

   Public Function DarkenColor(c As Color, percent As Integer) As Color
      Dim factor As Double = (100 - percent) / 100.0
      Return Color.FromArgb(c.A, CInt(c.R * factor), CInt(c.G * factor), CInt(c.B * factor))
   End Function

   Public Sub BuildTreeView()
      tvOptions.BeginUpdate()
      tvOptions.BackColor = Color.FromArgb(224, 234, 213)

      Dim nodeOS As TreeNode = tvOptions.Nodes.Add("OS")

      Dim nodeWindows As TreeNode = tvOptions.Nodes.Add("Windows")
      nodeWindows.Nodes.Add("Environment Variables")
      nodeWindows.Nodes.Add("User Account")

      Dim nodeHW As TreeNode = tvOptions.Nodes.Add("Hardware")
      Dim nodeBaseBoard As TreeNode = nodeHW.Nodes.Add("BaseBoard")
      nodeBaseBoard.Nodes.Add("BIOS")
      nodeHW.Nodes.Add("Battery")
      nodeHW.Nodes.Add("Disk Drive")
      nodeHW.Nodes.Add("KeyBoard")
      nodeHW.Nodes.Add("Processor")

      tvOptions.ExpandAll()
      tvOptions.EndUpdate()
   End Sub

   Public Sub ProcessOptions(path As String)
      Dim frmChild As Form = Nothing
      scMain.Panel2.Controls.Clear()
      Me.Text = "OSI"
      Select Case path
         Case "OS"
            frmChild = New frmOperatingSystem()
            If isRemote Then
               CType(frmChild, frmOperatingSystem).remoteHost = remoteHost
               CType(frmChild, frmOperatingSystem).remoteUser = remoteUser
               CType(frmChild, frmOperatingSystem).remotePass = remotePass
            End If
         Case "Windows\Environment Variables"
            frmChild = New frmEnvironment()
            If isRemote Then
               CType(frmChild, frmEnvironment).remoteHost = remoteHost
               CType(frmChild, frmEnvironment).remoteUser = remoteUser
               CType(frmChild, frmEnvironment).remotePass = remotePass
            End If
         Case "Windows\User Account"
            frmChild = New frmUserAccount()
            If isRemote Then
               CType(frmChild, frmUserAccount).remoteHost = remoteHost
               CType(frmChild, frmUserAccount).remoteUser = remoteUser
               CType(frmChild, frmUserAccount).remotePass = remotePass
            End If
         Case "Hardware\BaseBoard"
            frmChild = New frmBaseBoard()
            If isRemote Then
               CType(frmChild, frmBaseBoard).remoteHost = remoteHost
               CType(frmChild, frmBaseBoard).remoteUser = remoteUser
               CType(frmChild, frmBaseBoard).remotePass = remotePass
            End If
         Case "Hardware\BaseBoard\BIOS"
            frmChild = New frmBattery()
            If isRemote Then
               CType(frmChild, frmBaseBoardBIOS).remoteHost = remoteHost
               CType(frmChild, frmBaseBoardBIOS).remoteUser = remoteUser
               CType(frmChild, frmBaseBoardBIOS).remotePass = remotePass
            End If
         Case "Hardware\Battery"
            frmChild = New frmBattery()
            If isRemote Then
               CType(frmChild, frmBattery).remoteHost = remoteHost
               CType(frmChild, frmBattery).remoteUser = remoteUser
               CType(frmChild, frmBattery).remotePass = remotePass
            End If
         Case "Hardware\Disk Drive"
            frmChild = New frmDiskDrive()
            If isRemote Then
               CType(frmChild, frmDiskDrive).remoteHost = remoteHost
               CType(frmChild, frmDiskDrive).remoteUser = remoteUser
               CType(frmChild, frmDiskDrive).remotePass = remotePass
            End If
         Case "Hardware\KeyBoard"
            frmChild = New frmKeyBoard()
            If isRemote Then
               CType(frmChild, frmKeyBoard).remoteHost = remoteHost
               CType(frmChild, frmKeyBoard).remoteUser = remoteUser
               CType(frmChild, frmKeyBoard).remotePass = remotePass
            End If
         Case "Hardware\Processor"
            frmChild = New frmProcessor()
            If isRemote Then
               CType(frmChild, frmProcessor).remoteHost = remoteHost
               CType(frmChild, frmProcessor).remoteUser = remoteUser
               CType(frmChild, frmProcessor).remotePass = remotePass
            End If
         Case Else
      End Select

      If frmChild IsNot Nothing Then
         CType(frmChild, IModuleForm).MainForm = Me

         ' Embed the form inside the right panel
         frmChild.TopLevel = False
         frmChild.FormBorderStyle = FormBorderStyle.None
         frmChild.Dock = DockStyle.Fill
         scMain.Panel2.Controls.Add(frmChild)
         frmChild.Show()
      End If

   End Sub

   Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
      If AppHasCommandLineArgs() Then
         isRemote = True
         ParseCommandLineArgs()
      Else
         isRemote = False
      End If

      BuildTreeView()

      ' resize
      Me.Width = Screen.PrimaryScreen.WorkingArea.Width * 0.75 ' 75%
      Me.Height = Screen.PrimaryScreen.WorkingArea.Height * 0.75
      ' recenter
      Me.StartPosition = FormStartPosition.Manual
      Me.Left = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2
      Me.Top = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2

      pbLoad = New rrProgressBar()
      pbLoad.Dock = DockStyle.Bottom
      pbLoad.Location = New Point(0, 451)
      pbLoad.Size = New Size(300, 16)
      pbLoad.BarColor = DarkenColor(tvOptions.BackColor, 15)
      pbLoad.BarColorDone = DarkenColor(tvOptions.BackColor, 30)
      Me.Controls.Add(pbLoad)

      ' auto-start
      ProcessOptions("OS")
   End Sub

      Private Sub tvOptions_DoubleClick(sender As Object, e As EventArgs) Handles tvOptions.DoubleClick
         If tvOptions.SelectedNode Is Nothing Then
            MsgBox("Please select a node first.", MsgBoxStyle.Exclamation, "No Node Selected")
            Return
         Else
            ProcessOptions(tvOptions.SelectedNode.FullPath)
         End If
      End Sub

      Private Sub tvOptions_NodeMouseDoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles tvOptions.NodeMouseDoubleClick
         e.Node.Toggle()
      End Sub

   End Class
