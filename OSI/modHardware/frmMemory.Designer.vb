<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMemory
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
      lvMemory = New ListView()
      ColumnHeader1 = New ColumnHeader()
      ColumnHeader2 = New ColumnHeader()
      SuspendLayout()
      ' 
      ' lvMemory
      ' 
      lvMemory.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
      lvMemory.Columns.AddRange(New ColumnHeader() {ColumnHeader1, ColumnHeader2})
      lvMemory.FullRowSelect = True
      lvMemory.HeaderStyle = ColumnHeaderStyle.None
      lvMemory.Location = New Point(110, 0)
      lvMemory.Name = "lvMemory"
      lvMemory.Size = New Size(687, 450)
      lvMemory.TabIndex = 1
      lvMemory.UseCompatibleStateImageBehavior = False
      lvMemory.View = View.Details
      ' 
      ' ColumnHeader1
      ' 
      ColumnHeader1.Text = "Property"
      ' 
      ' ColumnHeader2
      ' 
      ColumnHeader2.Text = "Value"
      ' 
      ' frmMemory
      ' 
      AutoScaleDimensions = New SizeF(7F, 15F)
      AutoScaleMode = AutoScaleMode.Font
      ClientSize = New Size(800, 450)
      Controls.Add(lvMemory)
      Name = "frmMemory"
      Text = "Memory"
      ResumeLayout(False)
   End Sub

   Friend WithEvents lvMemory As ListView
   Friend WithEvents ColumnHeader1 As ColumnHeader
   Friend WithEvents ColumnHeader2 As ColumnHeader
End Class
