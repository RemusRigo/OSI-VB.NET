<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmProcessor
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
      lvProcessor = New ListView()
      ColumnHeader1 = New ColumnHeader()
      ColumnHeader2 = New ColumnHeader()
      SuspendLayout()
      ' 
      ' lvProcessor
      ' 
      lvProcessor.Columns.AddRange(New ColumnHeader() {ColumnHeader1, ColumnHeader2})
      lvProcessor.Dock = DockStyle.Fill
      lvProcessor.FullRowSelect = True
      lvProcessor.HeaderStyle = ColumnHeaderStyle.None
      lvProcessor.Location = New Point(0, 0)
      lvProcessor.Name = "lvProcessor"
      lvProcessor.Size = New Size(800, 450)
      lvProcessor.TabIndex = 1
      lvProcessor.UseCompatibleStateImageBehavior = False
      lvProcessor.View = View.Details
      ' 
      ' ColumnHeader1
      ' 
      ColumnHeader1.Text = "Property"
      ' 
      ' ColumnHeader2
      ' 
      ColumnHeader2.Text = "Value"
      ' 
      ' frmProcessor
      ' 
      AutoScaleDimensions = New SizeF(7F, 15F)
      AutoScaleMode = AutoScaleMode.Font
      ClientSize = New Size(800, 450)
      Controls.Add(lvProcessor)
      Name = "frmProcessor"
      Text = "frmProcessor"
      ResumeLayout(False)
   End Sub

   Friend WithEvents lvProcessor As ListView
   Friend WithEvents ColumnHeader1 As ColumnHeader
   Friend WithEvents ColumnHeader2 As ColumnHeader
End Class
