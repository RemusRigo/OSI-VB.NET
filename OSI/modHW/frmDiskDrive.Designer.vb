<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDiskDrive
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
      lvDiskDrive = New ListView()
      ColumnHeader1 = New ColumnHeader()
      ColumnHeader2 = New ColumnHeader()
      SuspendLayout()
      ' 
      ' lvDiskDrive
      ' 
      lvDiskDrive.Columns.AddRange(New ColumnHeader() {ColumnHeader1, ColumnHeader2})
      lvDiskDrive.Dock = DockStyle.Fill
      lvDiskDrive.FullRowSelect = True
      lvDiskDrive.HeaderStyle = ColumnHeaderStyle.None
      lvDiskDrive.Location = New Point(0, 0)
      lvDiskDrive.Name = "lvDiskDrive"
      lvDiskDrive.Size = New Size(800, 450)
      lvDiskDrive.TabIndex = 1
      lvDiskDrive.UseCompatibleStateImageBehavior = False
      lvDiskDrive.View = View.Details
      ' 
      ' ColumnHeader1
      ' 
      ColumnHeader1.Text = "Property"
      ' 
      ' ColumnHeader2
      ' 
      ColumnHeader2.Text = "Value"
      ' 
      ' frmDiskDrive
      ' 
      AutoScaleDimensions = New SizeF(7F, 15F)
      AutoScaleMode = AutoScaleMode.Font
      ClientSize = New Size(800, 450)
      Controls.Add(lvDiskDrive)
      Name = "frmDiskDrive"
      Text = "DiskDrive"
      ResumeLayout(False)
   End Sub

   Friend WithEvents lvDiskDrive As ListView
   Friend WithEvents ColumnHeader1 As ColumnHeader
   Friend WithEvents ColumnHeader2 As ColumnHeader
End Class
