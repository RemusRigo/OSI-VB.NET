<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBaseBoard
   Inherits System.Windows.Forms.Form

   'Form overrides dispose to clean up the component list.
   <System.Diagnostics.DebuggerNonUserCode()> _
   Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
   <System.Diagnostics.DebuggerStepThrough()> _
   Private Sub InitializeComponent()
      components = New ComponentModel.Container()
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBaseBoard))
      lvBaseBoard = New ListView()
      ColumnHeader1 = New ColumnHeader()
      ColumnHeader2 = New ColumnHeader()
      imgListItems = New ImageList(components)
      SuspendLayout()
      ' 
      ' lvBaseBoard
      ' 
      lvBaseBoard.Columns.AddRange(New ColumnHeader() {ColumnHeader1, ColumnHeader2})
      lvBaseBoard.Dock = DockStyle.Fill
      lvBaseBoard.FullRowSelect = True
      lvBaseBoard.HeaderStyle = ColumnHeaderStyle.None
      lvBaseBoard.Location = New Point(0, 0)
      lvBaseBoard.Name = "lvBaseBoard"
      lvBaseBoard.Size = New Size(800, 450)
      lvBaseBoard.SmallImageList = imgListItems
      lvBaseBoard.TabIndex = 1
      lvBaseBoard.UseCompatibleStateImageBehavior = False
      lvBaseBoard.View = View.Details
      ' 
      ' ColumnHeader1
      ' 
      ColumnHeader1.Text = "Property"
      ' 
      ' ColumnHeader2
      ' 
      ColumnHeader2.Text = "Value"
      ' 
      ' imgListItems
      ' 
      imgListItems.ColorDepth = ColorDepth.Depth32Bit
      imgListItems.ImageStream = CType(resources.GetObject("imgListItems.ImageStream"), ImageListStreamer)
      imgListItems.TransparentColor = Color.Transparent
      imgListItems.Images.SetKeyName(0, "MMC.ico")
      ' 
      ' frmBaseBoard
      ' 
      AutoScaleDimensions = New SizeF(7F, 15F)
      AutoScaleMode = AutoScaleMode.Font
      ClientSize = New Size(800, 450)
      Controls.Add(lvBaseBoard)
      Name = "frmBaseBoard"
      Text = "BaseBoard"
      ResumeLayout(False)
   End Sub

   Friend WithEvents lvBaseBoard As ListView
   Friend WithEvents ColumnHeader1 As ColumnHeader
   Friend WithEvents ColumnHeader2 As ColumnHeader
   Friend WithEvents imgListItems As ImageList
End Class
