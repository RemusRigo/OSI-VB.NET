<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBattery
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
      Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBattery))
      lvBattery = New ListView()
      ColumnHeader1 = New ColumnHeader()
      ColumnHeader2 = New ColumnHeader()
      imgListItems = New ImageList(components)
      SuspendLayout()
      ' 
      ' lvBattery
      ' 
      lvBattery.BackColor = Color.White
      lvBattery.Columns.AddRange(New ColumnHeader() {ColumnHeader1, ColumnHeader2})
      lvBattery.Dock = DockStyle.Fill
      lvBattery.FullRowSelect = True
      lvBattery.HeaderStyle = ColumnHeaderStyle.None
      lvBattery.Location = New Point(0, 0)
      lvBattery.Name = "lvBattery"
      lvBattery.Size = New Size(800, 450)
      lvBattery.SmallImageList = imgListItems
      lvBattery.TabIndex = 1
      lvBattery.UseCompatibleStateImageBehavior = False
      lvBattery.View = View.Details
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
      ' frmBattery
      ' 
      AutoScaleDimensions = New SizeF(7F, 15F)
      AutoScaleMode = AutoScaleMode.Font
      ClientSize = New Size(800, 450)
      Controls.Add(lvBattery)
      Name = "frmBattery"
      Text = "Battery"
      ResumeLayout(False)
   End Sub

   Friend WithEvents lvBattery As ListView
   Friend WithEvents ColumnHeader1 As ColumnHeader
   Friend WithEvents ColumnHeader2 As ColumnHeader
   Friend WithEvents imgListItems As ImageList
End Class
