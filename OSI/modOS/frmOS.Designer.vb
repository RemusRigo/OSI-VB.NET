<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmOS
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
      TabControlOS = New TabControl()
      TabPage1 = New TabPage()
      lvOS = New ListView()
      ColumnHeader1 = New ColumnHeader()
      ColumnHeader2 = New ColumnHeader()
      TabPage2 = New TabPage()
      lvOSProd = New ListView()
      ColumnHeader3 = New ColumnHeader()
      ColumnHeader4 = New ColumnHeader()
      TabControlOS.SuspendLayout()
      TabPage1.SuspendLayout()
      TabPage2.SuspendLayout()
      SuspendLayout()
      ' 
      ' TabControlOS
      ' 
      TabControlOS.Controls.Add(TabPage1)
      TabControlOS.Controls.Add(TabPage2)
      TabControlOS.Dock = DockStyle.Fill
      TabControlOS.Location = New Point(0, 0)
      TabControlOS.Name = "TabControlOS"
      TabControlOS.SelectedIndex = 0
      TabControlOS.Size = New Size(800, 450)
      TabControlOS.TabIndex = 1
      ' 
      ' TabPage1
      ' 
      TabPage1.Controls.Add(lvOS)
      TabPage1.Location = New Point(4, 24)
      TabPage1.Name = "TabPage1"
      TabPage1.Padding = New Padding(3)
      TabPage1.Size = New Size(792, 422)
      TabPage1.TabIndex = 0
      TabPage1.Text = "Operating System"
      TabPage1.UseVisualStyleBackColor = True
      ' 
      ' lvOS
      ' 
      lvOS.Columns.AddRange(New ColumnHeader() {ColumnHeader1, ColumnHeader2})
      lvOS.Dock = DockStyle.Fill
      lvOS.FullRowSelect = True
      lvOS.HeaderStyle = ColumnHeaderStyle.None
      lvOS.Location = New Point(3, 3)
      lvOS.Name = "lvOS"
      lvOS.Size = New Size(786, 416)
      lvOS.TabIndex = 1
      lvOS.UseCompatibleStateImageBehavior = False
      lvOS.View = View.Details
      ' 
      ' ColumnHeader1
      ' 
      ColumnHeader1.Text = "Property"
      ColumnHeader1.Width = 200
      ' 
      ' ColumnHeader2
      ' 
      ColumnHeader2.Text = "Value"
      ColumnHeader2.Width = 400
      ' 
      ' TabPage2
      ' 
      TabPage2.Controls.Add(lvOSProd)
      TabPage2.Location = New Point(4, 24)
      TabPage2.Name = "TabPage2"
      TabPage2.Padding = New Padding(3)
      TabPage2.Size = New Size(792, 422)
      TabPage2.TabIndex = 1
      TabPage2.Text = "Product"
      TabPage2.UseVisualStyleBackColor = True
      ' 
      ' lvOSProd
      ' 
      lvOSProd.Columns.AddRange(New ColumnHeader() {ColumnHeader3, ColumnHeader4})
      lvOSProd.Dock = DockStyle.Fill
      lvOSProd.FullRowSelect = True
      lvOSProd.HeaderStyle = ColumnHeaderStyle.None
      lvOSProd.Location = New Point(3, 3)
      lvOSProd.Name = "lvOSProd"
      lvOSProd.Size = New Size(786, 416)
      lvOSProd.TabIndex = 2
      lvOSProd.UseCompatibleStateImageBehavior = False
      lvOSProd.View = View.Details
      ' 
      ' ColumnHeader3
      ' 
      ColumnHeader3.Text = "Property"
      ColumnHeader3.Width = 200
      ' 
      ' ColumnHeader4
      ' 
      ColumnHeader4.Text = "Value"
      ColumnHeader4.Width = 400
      ' 
      ' frmOS
      ' 
      AutoScaleDimensions = New SizeF(7F, 15F)
      AutoScaleMode = AutoScaleMode.Font
      ClientSize = New Size(800, 450)
      Controls.Add(TabControlOS)
      Name = "frmOS"
      Text = "OS"
      TabControlOS.ResumeLayout(False)
      TabPage1.ResumeLayout(False)
      TabPage2.ResumeLayout(False)
      ResumeLayout(False)
   End Sub

   Friend WithEvents TabControlOS As TabControl
   Friend WithEvents TabPage1 As TabPage
   Friend WithEvents TabPage2 As TabPage
   Friend WithEvents lvOS As ListView
   Friend WithEvents ColumnHeader1 As ColumnHeader
   Friend WithEvents ColumnHeader2 As ColumnHeader
   Friend WithEvents lvOSProd As ListView
   Friend WithEvents ColumnHeader3 As ColumnHeader
   Friend WithEvents ColumnHeader4 As ColumnHeader
End Class
