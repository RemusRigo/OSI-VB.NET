<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmOSI
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

   'NOTE: The following procedure is required by the Windows Form Designer
   'It can be modified using the Windows Form Designer.  
   'Do not modify it using the code editor.
   <System.Diagnostics.DebuggerStepThrough()>
   Private Sub InitializeComponent()
      components = New ComponentModel.Container()
      Dim TreeNode1 As TreeNode = New TreeNode("OS")
      Dim TreeNode2 As TreeNode = New TreeNode("Environment Variables", -2, 1)
      Dim TreeNode3 As TreeNode = New TreeNode("Users", 1, 2)
      Dim TreeNode4 As TreeNode = New TreeNode("SW", New TreeNode() {TreeNode2, TreeNode3})
      Dim TreeNode5 As TreeNode = New TreeNode("BaseBoard")
      Dim TreeNode6 As TreeNode = New TreeNode("Processor")
      Dim TreeNode7 As TreeNode = New TreeNode("Disk Drive", -2, 3)
      Dim TreeNode8 As TreeNode = New TreeNode("HW", New TreeNode() {TreeNode5, TreeNode6, TreeNode7})
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOSI))
      scMain = New SplitContainer()
      tvOptions = New TreeView()
      imgListNodes = New ImageList(components)
      pbLoad2 = New ProgressBar()
      CType(scMain, ComponentModel.ISupportInitialize).BeginInit()
      scMain.Panel1.SuspendLayout()
      scMain.SuspendLayout()
      SuspendLayout()
      ' 
      ' scMain
      ' 
      scMain.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
      scMain.FixedPanel = FixedPanel.Panel1
      scMain.Location = New Point(0, 0)
      scMain.Margin = New Padding(3, 2, 3, 2)
      scMain.Name = "scMain"
      ' 
      ' scMain.Panel1
      ' 
      scMain.Panel1.Controls.Add(tvOptions)
      scMain.Size = New Size(772, 448)
      scMain.SplitterDistance = 220
      scMain.TabIndex = 0
      ' 
      ' tvOptions
      ' 
      tvOptions.BackColor = Color.White
      tvOptions.Dock = DockStyle.Fill
      tvOptions.Font = New Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
      tvOptions.ImageIndex = 0
      tvOptions.ImageList = imgListNodes
      tvOptions.Location = New Point(0, 0)
      tvOptions.Margin = New Padding(3, 2, 3, 2)
      tvOptions.Name = "tvOptions"
      TreeNode1.Name = "Node6"
      TreeNode1.Text = "OS"
      TreeNode2.ImageIndex = -2
      TreeNode2.Name = "Node8"
      TreeNode2.SelectedImageIndex = 1
      TreeNode2.Text = "Environment Variables"
      TreeNode3.ImageIndex = 1
      TreeNode3.Name = "Node3"
      TreeNode3.SelectedImageIndex = 2
      TreeNode3.Text = "Users"
      TreeNode4.Name = "Node5"
      TreeNode4.Text = "SW"
      TreeNode5.Name = "Node1"
      TreeNode5.Text = "BaseBoard"
      TreeNode6.ImageIndex = 2
      TreeNode6.Name = "Node2"
      TreeNode6.Text = "Processor"
      TreeNode7.ImageIndex = -2
      TreeNode7.Name = "Node7"
      TreeNode7.SelectedImageIndex = 3
      TreeNode7.Text = "Disk Drive"
      TreeNode8.Name = "Node0"
      TreeNode8.Text = "HW"
      tvOptions.Nodes.AddRange(New TreeNode() {TreeNode1, TreeNode4, TreeNode8})
      tvOptions.SelectedImageIndex = 0
      tvOptions.Size = New Size(220, 448)
      tvOptions.TabIndex = 0
      ' 
      ' imgListNodes
      ' 
      imgListNodes.ColorDepth = ColorDepth.Depth32Bit
      imgListNodes.ImageStream = CType(resources.GetObject("imgListNodes.ImageStream"), ImageListStreamer)
      imgListNodes.TransparentColor = Color.White
      imgListNodes.Images.SetKeyName(0, "Windows.png")
      imgListNodes.Images.SetKeyName(1, "Users.png")
      imgListNodes.Images.SetKeyName(2, "Processor.png")
      imgListNodes.Images.SetKeyName(3, "Memory.png")
      ' 
      ' pbLoad2
      ' 
      pbLoad2.BackColor = Color.LightGray
      pbLoad2.Dock = DockStyle.Bottom
      pbLoad2.ForeColor = Color.DarkSeaGreen
      pbLoad2.Location = New Point(0, 451)
      pbLoad2.Name = "pbLoad2"
      pbLoad2.Size = New Size(772, 16)
      pbLoad2.TabIndex = 1
      pbLoad2.Visible = False
      ' 
      ' frmOSI
      ' 
      AutoScaleDimensions = New SizeF(7F, 15F)
      AutoScaleMode = AutoScaleMode.Font
      ClientSize = New Size(772, 467)
      Controls.Add(pbLoad2)
      Controls.Add(scMain)
      Icon = CType(resources.GetObject("$this.Icon"), Icon)
      Margin = New Padding(3, 2, 3, 2)
      Name = "frmOSI"
      StartPosition = FormStartPosition.CenterScreen
      Text = "OSI"
      scMain.Panel1.ResumeLayout(False)
      CType(scMain, ComponentModel.ISupportInitialize).EndInit()
      scMain.ResumeLayout(False)
      ResumeLayout(False)
   End Sub

   Friend WithEvents scMain As SplitContainer
   Friend WithEvents tvOptions As TreeView
   Friend WithEvents pbLoad2 As ProgressBar
   Friend WithEvents imgListNodes As ImageList

End Class
