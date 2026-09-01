<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmEnvironment
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
      TabControl1 = New TabControl()
      TabPage1 = New TabPage()
      lvEnv1 = New ListView()
      ColumnHeader1 = New ColumnHeader()
      ColumnHeader2 = New ColumnHeader()
      TabPage2 = New TabPage()
      lvEnv2 = New ListView()
      ColumnHeader4 = New ColumnHeader()
      ColumnHeader5 = New ColumnHeader()
      ColumnHeader6 = New ColumnHeader()
      TabControl1.SuspendLayout()
      TabPage1.SuspendLayout()
      TabPage2.SuspendLayout()
      SuspendLayout()
      ' 
      ' TabControl1
      ' 
      TabControl1.Controls.Add(TabPage1)
      TabControl1.Controls.Add(TabPage2)
      TabControl1.Dock = DockStyle.Fill
      TabControl1.Location = New Point(0, 0)
      TabControl1.Name = "TabControl1"
      TabControl1.SelectedIndex = 0
      TabControl1.Size = New Size(784, 601)
      TabControl1.TabIndex = 1
      ' 
      ' TabPage1
      ' 
      TabPage1.Controls.Add(lvEnv1)
      TabPage1.Location = New Point(4, 24)
      TabPage1.Name = "TabPage1"
      TabPage1.Padding = New Padding(3)
      TabPage1.Size = New Size(776, 573)
      TabPage1.TabIndex = 0
      TabPage1.Text = "Method 1"
      TabPage1.UseVisualStyleBackColor = True
      ' 
      ' lvEnv1
      ' 
      lvEnv1.BorderStyle = BorderStyle.None
      lvEnv1.Columns.AddRange(New ColumnHeader() {ColumnHeader1, ColumnHeader2})
      lvEnv1.Dock = DockStyle.Fill
      lvEnv1.FullRowSelect = True
      lvEnv1.Location = New Point(3, 3)
      lvEnv1.Name = "lvEnv1"
      lvEnv1.Size = New Size(770, 567)
      lvEnv1.TabIndex = 4
      lvEnv1.UseCompatibleStateImageBehavior = False
      lvEnv1.View = View.Details
      ' 
      ' ColumnHeader1
      ' 
      ColumnHeader1.Text = "Variable"
      ' 
      ' ColumnHeader2
      ' 
      ColumnHeader2.Text = "Value"
      ' 
      ' TabPage2
      ' 
      TabPage2.Controls.Add(lvEnv2)
      TabPage2.Location = New Point(4, 24)
      TabPage2.Name = "TabPage2"
      TabPage2.Padding = New Padding(3)
      TabPage2.Size = New Size(776, 573)
      TabPage2.TabIndex = 1
      TabPage2.Text = "Method 2"
      TabPage2.UseVisualStyleBackColor = True
      ' 
      ' lvEnv2
      ' 
      lvEnv2.BorderStyle = BorderStyle.None
      lvEnv2.Columns.AddRange(New ColumnHeader() {ColumnHeader4, ColumnHeader5, ColumnHeader6})
      lvEnv2.Dock = DockStyle.Fill
      lvEnv2.FullRowSelect = True
      lvEnv2.Location = New Point(3, 3)
      lvEnv2.Name = "lvEnv2"
      lvEnv2.Size = New Size(770, 567)
      lvEnv2.TabIndex = 5
      lvEnv2.UseCompatibleStateImageBehavior = False
      lvEnv2.View = View.Details
      ' 
      ' ColumnHeader4
      ' 
      ColumnHeader4.Text = "User"
      ' 
      ' ColumnHeader5
      ' 
      ColumnHeader5.Text = "Variable"
      ' 
      ' ColumnHeader6
      ' 
      ColumnHeader6.Text = "Value"
      ' 
      ' frmEnvironment
      ' 
      AutoScaleDimensions = New SizeF(7F, 15F)
      AutoScaleMode = AutoScaleMode.Font
      ClientSize = New Size(784, 601)
      Controls.Add(TabControl1)
      Name = "frmEnvironment"
      Text = "Environment"
      TabControl1.ResumeLayout(False)
      TabPage1.ResumeLayout(False)
      TabPage2.ResumeLayout(False)
      ResumeLayout(False)
   End Sub

   Friend WithEvents TabControl1 As TabControl
   Friend WithEvents TabPage1 As TabPage
   Friend WithEvents TabPage2 As TabPage
   Friend WithEvents lvEnv1 As ListView
   Friend WithEvents ColumnHeader1 As ColumnHeader
   Friend WithEvents ColumnHeader2 As ColumnHeader
   Friend WithEvents lvEnv2 As ListView
   Public WithEvents ColumnHeader4 As ColumnHeader
   Friend WithEvents ColumnHeader5 As ColumnHeader
   Friend WithEvents ColumnHeader6 As ColumnHeader
End Class
