'--------------------------------------------------------------------------------------------------
' Win32_Processor class
' https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-processor
'
'    © Remus Rigo
'       v1.0.20260810
'--------------------------------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.Management
Imports SharedInterfaces

Public Class frmProcessor
   Implements IModuleForm
   <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
   Public Property MainForm As IMainForm Implements IModuleForm.MainForm
   Public remoteHost, remoteUser, remotePass As String

   Private Class ProcListItem
      Public Property Group As String
      Public Property Label As String
      Public Property Value As String
      Public Property ImageIndex As Integer = -1
   End Class

   Private Enum ProgressKind
      SetMax
      SetValue
      AppendItem
   End Enum

   Private Class ProgressInfo
      Public Property Kind As ProgressKind
      Public Property Max As Integer
      Public Property Value As Integer
      Public Property ImageIndex As Integer
      Public Property Item As ProcListItem
   End Class

   ' Tracks groups already created on the ListView, so new items are added to the correct group without recreating it each time.
   Private groupCache As New Dictionary(Of String, ListViewGroup)

   '-----------------------------------------------------------------------------------------------
   ' BackgroundWorker: DoWork
   Private Sub BackgroundWorker_DoWork(sender As Object, e As DoWorkEventArgs)
      Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)
      Dim myConnection As New ConnectionOptions()
      Dim scopePath As String
      Dim items As New List(Of ProcListItem)

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

         Dim myQuery As New ObjectQuery("SELECT * FROM Win32_Processor")
         Dim searcher As New ManagementObjectSearcher(scope, myQuery)
         Dim cnt As Integer = 0
         Dim crtAction As Integer = 1
         Dim objItems = searcher.Get()
         Dim objCounter As Integer = objItems.Count
         Dim propsPerObj As Integer = 0

         If objCounter > 0 Then
            propsPerObj = objItems.Cast(Of ManagementObject)().First().Properties.Cast(Of PropertyData)().Count(Function(p) p.Name <> "Class" AndAlso p.Name <> "Path")
         End If

         Dim totalProps As Integer = propsPerObj * objCounter
         worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetMax, .Max = totalProps})

         For Each obj As ManagementObject In objItems
            cnt += 1
            Dim groupName As String = "Processor #" & cnt

            ' Info --------------------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "Info", .Value = ""}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Name") Then
               If (obj("Name") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Name", .Value = obj("Name").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Caption") Then
               If (obj("Caption") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Caption", .Value = obj("Caption").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Description") Then
               If (obj("Description") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Description", .Value = obj("Description").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Manufacturer") Then
               If (obj("Manufacturer") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Manufacturer", .Value = obj("Manufacturer").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ProcessorId") Then
               If (obj("ProcessorId") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Processor ID", .Value = obj("ProcessorId").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ProcessorType") Then
               If (obj("ProcessorType") IsNot Nothing) Then
                  Dim tmp As String = ""
                  Select Case obj("ProcessorType")
                     Case 1 : tmp = "Other"
                     Case 2 : tmp = "Unknown"
                     Case 3 : tmp = "Central Processor"
                     Case 4 : tmp = "Math Processor"
                     Case 5 : tmp = "DSP Processor"
                     Case 6 : tmp = "Video Processor"
                  End Select
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Processor Type", .Value = tmp, .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Role") Then
               If (obj("Role") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Role", .Value = obj("Role").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SerialNumber") Then
               If (obj("SerialNumber") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Serial Number", .Value = obj("SerialNumber").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SocketDesignation") Then
               If (obj("SocketDesignation") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Socket Designation", .Value = obj("SocketDesignation").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1


            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SystemName") Then
               If (obj("SystemName") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "System Name", .Value = obj("SystemName").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Version") Then
               If (obj("Version") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Version", .Value = obj("Version").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Revision") Then
               If (obj("Revision") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Revision", .Value = obj("Revision").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Stepping") Then
               If (obj("Stepping") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Stepping", .Value = obj("Stepping").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "UniqueId") Then
               If (obj("UniqueId") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Unique ID", .Value = obj("UniqueId").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            ' Specifications ----------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "Specifications", .Value = ""}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Architecture") Then
               If (obj("Architecture") IsNot Nothing) Then
                  Dim tmp As String = ""
                  Select Case obj("Architecture")
                     Case 0 : tmp = "x86"
                     Case 1 : tmp = "MIPS"
                     Case 2 : tmp = "Alpha"
                     Case 3 : tmp = "PowerPC"
                     Case 5 : tmp = "ARM"
                     Case 6 : tmp = "Itanium-based systems"
                     Case 9 : tmp = "x64"
                     Case 12 : tmp = "ARM64"
                     Case Else : tmp = "Unknown"
                  End Select
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Architecture", .Value = tmp, .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "AddressWidth") Then
               If (obj("AddressWidth") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Address Width", .Value = obj("AddressWidth").ToString() & "-bit", .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "DataWidth") Then
               If (obj("DataWidth") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Data Width", .Value = obj("DataWidth").ToString() & "-bit processor", .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ExtClock") Then
               If (obj("ExtClock") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "External clock frequency", .Value = obj("ExtClock").ToString() & " MHz", .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Family") Then
               If (obj("Family") IsNot Nothing) Then
                  Dim tmp As String = ""
                  Select Case obj("Family")
                     Case 1 : tmp = "Other"
                     Case 2 : tmp = "Unknown"
                     Case 3 : tmp = "8086"
                     Case 4 : tmp = "80286"
                     Case 5 : tmp = "80386"
                     Case 6 : tmp = "80486"
                     Case 7 : tmp = "8087"
                     Case 8 : tmp = "80287"
                     Case 9 : tmp = "80387"
                     Case 10 : tmp = "80487"
                     Case 11 : tmp = "Pentium"
                     Case 12 : tmp = "Pentium Pro"
                     Case 13 : tmp = "Pentium II"
                     Case 14 : tmp = "Pentium MMX"
                     Case 15 : tmp = "Celeron"
                     Case 16 : tmp = "Pentium II Xeon"
                     Case 17 : tmp = "Pentium III"
                     Case 18 : tmp = "M1 Family"
                     Case 19 : tmp = "M2 Family"
                     Case 20 : tmp = "Intel Celeron M"
                     Case 21 : tmp = "Intel Pentium 4 HT processor"
                     Case 24 : tmp = "K5 Family"
                     Case 25 : tmp = "K6 Family"
                     Case 26 : tmp = "K6-2"
                     Case 27 : tmp = "K6-3"
                     Case 28 : tmp = "AMD Athlon(TM) Processor Family"
                     Case 29 : tmp = "AMD Duron Processor"
                     Case 30 : tmp = "AMD29000 Family"
                     Case 31 : tmp = "K6-2+"
                     Case 32 : tmp = "Power PC Family"
                     Case 33 : tmp = "Power PC 601"
                     Case 34 : tmp = "Power PC 603"
                     Case 35 : tmp = "Power PC 603+"
                     Case 36 : tmp = "Power PC 604"
                     Case 37 : tmp = "Power PC 620"
                     Case 38 : tmp = "Power PC X704"
                     Case 39 : tmp = "Power PC 750"
                     Case 40 : tmp = "Intel Core Duo processor"
                     Case 41 : tmp = "Intel Core Duo mobile processor"
                     Case 42 : tmp = "Intel Core Solo mobile processor"
                     Case 43 : tmp = "Intel Atom processor"
                     Case 48 : tmp = "Alpha Family"
                     Case 49 : tmp = "Alpha 21064"
                     Case 50 : tmp = "Alpha 21066"
                     Case 51 : tmp = "Alpha 21164"
                     Case 52 : tmp = "Alpha 21164PC"
                     Case 53 : tmp = "Alpha 21164a"
                     Case 54 : tmp = "Alpha 21264"
                     Case 55 : tmp = "Alpha 21364"
                     Case 56 : tmp = "AMD Turion II Ultra Dual-Core Mobile M Processor Family"
                     Case 57 : tmp = "AMD Turion II Dual-Core Mobile M Processor Family"
                     Case 58 : tmp = "AMD Athlon II Dual-Core Mobile M Processor Family"
                     Case 59 : tmp = "AMD Opteron 6100 Series Processor"
                     Case 60 : tmp = "AMD Opteron 4100 Series Processor"
                     Case 64 : tmp = "MIPS Family"
                     Case 65 : tmp = "MIPS R4000"
                     Case 66
                        tmp = "MIPS R4200"
                     Case 67
                        tmp = "MIPS R4400"
                     Case 68
                        tmp = "MIPS R4600"
                     Case 69
                        tmp = "MIPS R10000"
                     Case 80
                        tmp = "SPARC Family"
                     Case 81
                        tmp = "SuperSPARC"
                     Case 82
                        tmp = "microSPARC II"
                     Case 83
                        tmp = "microSPARC IIep"
                     Case 84
                        tmp = "UltraSPARC"
                     Case 85
                        tmp = "UltraSPARC II"
                     Case 86
                        tmp = "UltraSPARC IIi"
                     Case 87
                        tmp = "UltraSPARC III"
                     Case 88
                        tmp = "UltraSPARC IIIi"
                     Case 96
                        tmp = "68040"
                     Case 97
                        tmp = "68xxx Family"
                     Case 98
                        tmp = "68000"
                     Case 99
                        tmp = "68010"
                     Case 100
                        tmp = "68020"
                     Case 101
                        tmp = "68030"
                     Case 107
                        tmp = "AMD Ryzen Family"
                     Case 112
                        tmp = "Hobbit Family"
                     Case 120
                        tmp = "Crusoe TM5000 Family"
                     Case 121
                        tmp = "Crusoe TM3000 Family"
                     Case 122
                        tmp = "Efficeon TM8000 Family"
                     Case 128
                        tmp = "Weitek"
                     Case 130
                        tmp = "Itanium Processor"
                     Case 131
                        tmp = "AMD Athlon 64 Processor Family"
                     Case 132
                        tmp = "AMD Opteron Processor Family"
                     Case 133
                        tmp = "AMD Sempron Processor Family"
                     Case 134
                        tmp = "AMD Turion 64 Mobile Technology"
                     Case 135
                        tmp = "Dual-Core AMD Opteron Processor Family"
                     Case 136
                        tmp = "AMD Athlon 64 X2 Dual-Core Processor Family"
                     Case 137
                        tmp = "AMD Turion 64 X2 Mobile Technology"
                     Case 138
                        tmp = "Quad-Core AMD Opteron Processor Family"
                     Case 139
                        tmp = "Third-Generation AMD Opteron Processor Family"
                     Case 140
                        tmp = "AMD Phenom FX Quad-Core Processor Family"
                     Case 141
                        tmp = "AMD Phenom X4 Quad-Core Processor Family"
                     Case 142
                        tmp = "AMD Phenom X2 Dual-Core Processor Family"
                     Case 143
                        tmp = "AMD Athlon X2 Dual-Core Processor Family"
                     Case 144
                        tmp = "PA-RISC Family"
                     Case 145
                        tmp = "PA-RISC 8500"
                     Case 146
                        tmp = "PA-RISC 8000"
                     Case 147
                        tmp = "PA-RISC 7300LC"
                     Case 148
                        tmp = "PA-RISC 7200"
                     Case 149
                        tmp = "PA-RISC 7100LC"
                     Case 150
                        tmp = "PA-RISC 7100"
                     Case 160
                        tmp = "V30 Family"
                     Case 161
                        tmp = "Quad-Core Intel Xeon processor 3200 Series"
                     Case 162
                        tmp = "Dual-Core Intel Xeon processor 3000 Series"
                     Case 163
                        tmp = "Quad-Core Intel Xeon processor 5300 Series"
                     Case 164
                        tmp = "Dual-Core Intel Xeon processor 5100 Series"
                     Case 165
                        tmp = "Dual-Core Intel Xeon processor 5000 Series"
                     Case 166
                        tmp = "Dual-Core Intel Xeon processor LV"
                     Case 167
                        tmp = "Dual-Core Intel Xeon processor ULV"
                     Case 168
                        tmp = "Dual-Core Intel Xeon processor 7100 Series"
                     Case 169
                        tmp = "Quad-Core Intel Xeon processor 5400 Series"
                     Case 170
                        tmp = "Quad-Core Intel Xeon processor"
                     Case 171
                        tmp = "Dual-Core Intel Xeon processor 5200 Series"
                     Case 172
                        tmp = "Dual-Core Intel Xeon processor 7200 Series"
                     Case 173
                        tmp = "Quad-Core Intel Xeon processor 7300 Series"
                     Case 174
                        tmp = "Quad-Core Intel Xeon processor 7400 Series"
                     Case 175
                        tmp = "Multi-Core Intel Xeon processor 7400 Series"
                     Case 176
                        tmp = "Pentium III Xeon"
                     Case 177
                        tmp = "Pentium III Processor With Intel SpeedStep Technology"
                     Case 178
                        tmp = "Pentium 4"
                     Case 179
                        tmp = "Intel Xeon"
                     Case 180
                        tmp = "AS400 Family"
                     Case 181
                        tmp = "Intel Xeon processor MP"
                     Case 182
                        tmp = "AMD Athlon XP Family"
                     Case 183
                        tmp = "AMD Athlon MP Family"
                     Case 184
                        tmp = "Intel Itanium 2"
                     Case 185
                        tmp = "Intel Pentium M processor"
                     Case 186
                        tmp = "Intel Celeron D processor"
                     Case 187
                        tmp = "Intel Pentium D processor"
                     Case 188
                        tmp = "Intel Pentium Processor Extreme Edition"
                     Case 189
                        tmp = "Intel Core Solo Processor"
                     Case 190
                        tmp = "K7"
                     Case 191
                        tmp = "Intel Core 2 Duo Processor"
                     Case 192
                        tmp = "Intel Core 2 Solo processor"
                     Case 193
                        tmp = "Intel Core 2 Extreme processor"
                     Case 194
                        tmp = "Intel Core 2 Quad processor"
                     Case 195
                        tmp = "Intel Core 2 Extreme mobile processor"
                     Case 196
                        tmp = "Intel Core 2 Duo mobile processor"
                     Case 197
                        tmp = "Intel Core 2 Solo mobile processor"
                     Case 198
                        tmp = "Intel Core i7 processor"
                     Case 199
                        tmp = "Dual-Core Intel Celeron Processor"
                     Case 200
                        tmp = "S/390 And zSeries Family"
                     Case 201
                        tmp = "ESA/390 G4"
                     Case 202
                        tmp = "ESA/390 G5"
                     Case 203
                        tmp = "ESA/390 G6"
                     Case 204
                        tmp = "z/Architecture base"
                     Case 205
                        tmp = "Intel Core i5 processor"
                     Case 206
                        tmp = "Intel Core i3 processor"
                     Case 207
                        tmp = "Intel Core i9 processor"
                     Case 210
                        tmp = "VIA C7-M Processor Family"
                     Case 211
                        tmp = "VIA C7-D Processor Family"
                     Case 212
                        tmp = "VIA C7 Processor Family"
                     Case 213
                        tmp = "VIA Eden Processor Family"
                     Case 214
                        tmp = "Multi-Core Intel Xeon processor"
                     Case 215
                        tmp = "Dual-Core Intel Xeon processor 3xxx Series"
                     Case 216
                        tmp = "Quad-Core Intel Xeon processor 3xxx Series"
                     Case 217
                        tmp = "VIA Nano Processor Family"
                     Case 218
                        tmp = "Dual-Core Intel Xeon processor 5xxx Series"
                     Case 219
                        tmp = "Quad-Core Intel Xeon processor 5xxx Series"
                     Case 221
                        tmp = "Dual-Core Intel Xeon processor 7xxx Series"
                     Case 222
                        tmp = "Quad-Core Intel Xeon processor 7xxx Series"
                     Case 223
                        tmp = "Multi-Core Intel Xeon processor 7xxx Series"
                     Case 224
                        tmp = "Multi-Core Intel Xeon processor 3400 Series"
                     Case 230
                        tmp = "Embedded AMD Opteron Quad-Core Processor Family"
                     Case 231
                        tmp = "AMD Phenom Triple-Core Processor Family"
                     Case 232
                        tmp = "AMD Turion Ultra Dual-Core Mobile Processor Family"
                     Case 233
                        tmp = "AMD Turion Dual-Core Mobile Processor Family"
                     Case 234 : tmp = "AMD Athlon Dual-Core Processor Family"
                     Case 235 : tmp = "AMD Sempron SI Processor Family"
                     Case 236 : tmp = "AMD Phenom II Processor Family"
                     Case 237 : tmp = "AMD Athlon II Processor Family"
                     Case 238 : tmp = "Six-Core AMD Opteron Processor Family"
                     Case 239 : tmp = "AMD Sempron M Processor Family"
                     Case 250 : tmp = "i860"
                     Case 251 : tmp = "i960"
                     Case 254 : tmp = "Reserved (SMBIOS Extension)"
                     Case 255 : tmp = "Reserved (Un-initialized Flash Content-Lo)"
                     Case 260 : tmp = "SH-3"
                     Case 261 : tmp = "SH-4"
                     Case 280 : tmp = "ARM"
                     Case 281 : tmp = "StrongARM"
                     Case 300 : tmp = "6x86"
                     Case 301 : tmp = "MediaGX"
                     Case 302 : tmp = "MII"
                     Case 320 : tmp = "WinChip"
                     Case 350 : tmp = "DSP"
                     Case 500 : tmp = "Video Processor"
                     Case 65534 : tmp = "Reserved (For Future Special Purpose Assignment)"
                     Case 65535 : tmp = "Reserved (Un-initialized Flash Content-Hi)"
                     Case Else : tmp = "Unknown (" & obj("Family").ToString() & ")"
                  End Select
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Family", .Value = tmp, .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "OtherFamilyDescription") Then
               If (obj("OtherFamilyDescription") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Other Family Description", .Value = obj("OtherFamilyDescription").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SecondLevelAddressTranslationExtensions") Then
               If (obj("SecondLevelAddressTranslationExtensions") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName,
                     .Label = "Second Level Address Translation Extensions",
                     .Value = If(obj("SecondLevelAddressTranslationExtensions"), "Supported", "Not Supported"),
                     .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "VirtualizationFirmwareEnabled") Then
               If (obj("VirtualizationFirmwareEnabled") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName,
                     .Label = "Firmware has enabled virtualization extensions",
                     .Value = If(obj("VirtualizationFirmwareEnabled"), "Enabled", "Disabled"),
                     .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "VMMonitorModeExtensions") Then ' processor supports Intel or AMD Virtual Machine Monitor extensions
               If (obj("VMMonitorModeExtensions") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName,
                     .Label = "VM Monitor Mode Extensions",
                     .Value = If(obj("VMMonitorModeExtensions"), "Supported", "Not Supported"),
                     .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ThreadCount") Then ' threads per processor socket
               If (obj("ThreadCount") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Thread Count", .Value = obj("ThreadCount").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Value = crtAction}) : crtAction += 1

            ' Cache -------------------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {.Group = groupName, .Label = "Cache", .Value = ""}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "L2CacheSize") Then
               If (obj("L2CacheSize") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "L2 Cache Size", .Value = DynamicFormatBytes(obj("L2CacheSize") * 1024), .ImageIndex = 0}}) ' default in KB
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "L2CacheSpeed") Then
               If (obj("L2CacheSpeed") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName,
                     .Label = "L2 Cache Speed",
                     .Value = obj("L2CacheSpeed").ToString() & " MHz",
                     .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "L3CacheSize") Then
               If (obj("L3CacheSize") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName,
                     .Label = "L3 Cache Size",
                     .Value = DynamicFormatBytes(obj("L3CacheSize") * 1024),
                     .ImageIndex = 0}}) ' default in KB
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "L3CacheSpeed") Then
               If (obj("L3CacheSpeed") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "L3 Cache Speed", .Value = obj("L3CacheSpeed").ToString() & " MHz", .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Value = crtAction}) : crtAction += 1

            ' Core --------------------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {.Group = groupName, .Label = "Core", .Value = ""}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "NumberOfCores") Then
               If (obj("NumberOfCores") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Number of Cores", .Value = obj("NumberOfCores").ToString()}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "NumberOfEnabledCore") Then
               If (obj("NumberOfEnabledCore") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Number of Enabled Cores", .Value = obj("NumberOfEnabledCore").ToString()}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "NumberOfLogicalProcessors") Then
               If (obj("NumberOfLogicalProcessors") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Number of Logical Processors", .Value = obj("NumberOfLogicalProcessors").ToString()}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Value = crtAction}) : crtAction += 1


            ' Characteristics ---------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "Characteristics", .Value = ""}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Characteristics") Then
               If (obj("Characteristics") IsNot Nothing) Then
                  Dim ch As UInt32 = CUInt(obj("Characteristics"))
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "64-bit capable", .Value = If((ch And 2) <> 0, "Yes", "No"), .ImageIndex = 0}})
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Enhanced Virtualization", .Value = If((ch And 4) <> 0, "Yes", "No"), .ImageIndex = 0}})
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Hardware Threading", .Value = If((ch And 8) <> 0, "Yes", "No"), .ImageIndex = 0}})
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Execute Disable (NX)", .Value = If((ch And 16) <> 0, "Yes", "No"), .ImageIndex = 0}})
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Enhanced SpeedStep", .Value = If((ch And 32) <> 0, "Yes", "No"), .ImageIndex = 0}})
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Turbo Boost", .Value = If((ch And 64) <> 0, "Yes", "No"), .ImageIndex = 0}})
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Hyper-Threading", .Value = If((ch And 128) <> 0, "Yes", "No"), .ImageIndex = 0}})
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Secure Virtual Machine (AMD-V)", .Value = If((ch And 256) <> 0, "Yes", "No"), .ImageIndex = 0}})
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Trusted Execution", .Value = If((ch And 512) <> 0, "Yes", "No"), .ImageIndex = 0}})
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "RAS features", .Value = If((ch And 1024) <> 0, "Yes", "No"), .ImageIndex = 0}})
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Power Management", .Value = If((ch And 2048) <> 0, "Yes", "No"), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            ' Status ------------------------------------------------------------------------------   
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "Status", .Value = ""}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Status") Then
               If (obj("Status") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Status", .Value = obj("Status").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "StatusInfo") Then
               If (obj("StatusInfo") IsNot Nothing) Then
                  Dim tmp As String = ""
                  Select Case CInt(obj("StatusInfo"))
                     Case 1 : tmp = "Other"
                     Case 2 : tmp = "Unknown"
                     Case 3 : tmp = "Enabled"
                     Case 4 : tmp = "Disabled"
                     Case 5 : tmp = "Not Applicable"
                  End Select
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Status Info", .Value = tmp, .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            ' Error -------------------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "Error", .Value = ""}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ErrorDescription") Then
               If (obj("ErrorDescription") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Error Description", .Value = obj("ErrorDescription").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ErrorCleared") Then
               If (obj("ErrorCleared") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Error Cleared", .Value = If(obj("ErrorCleared"), "Yes", "No"), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "LastErrorCode") Then
               If (obj("LastErrorCode") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Last Error Code", .Value = obj("LastErrorCode").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            ' Power Management --------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "Power Management", .Value = "", .ImageIndex = 0}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "PowerManagementSupported") Then
               If (obj("PowerManagementSupported") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Power Management Supported", .Value = obj("PowerManagementSupported").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "PowerManagementCapabilities") Then
               If (obj("PowerManagementCapabilities") IsNot Nothing) Then
                  Dim pmCap = TryCast(obj("PowerManagementCapabilities"), UInt16())
                  Dim cap As New HashSet(Of UInt16)

                  If pmCap IsNot Nothing Then
                     For Each c As UInt16 In pmCap
                        cap.Add(c)
                     Next
                  End If

                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Unknown", .Value = If(cap.Contains(0), "True", "False"), .ImageIndex = 0}})
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Not Supported", .Value = If(cap.Contains(1), "True", "False"), .ImageIndex = 0}})
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Disabled", .Value = If(cap.Contains(2), "True", "False"), .ImageIndex = 0}})
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Enabled", .Value = If(cap.Contains(3), "True", "False"), .ImageIndex = 0}})
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Power Saving Modes Entered Automatically", .Value = If(cap.Contains(4), "True", "False"), .ImageIndex = 0}})
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Power State Settable", .Value = If(cap.Contains(5), "True", "False"), .ImageIndex = 0}})
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Power Cycling Supported", .Value = If(cap.Contains(6), "True", "False"), .ImageIndex = 0}})
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Timed Power-On Supported", .Value = If(cap.Contains(7), "True", "False"), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Value = crtAction}) : crtAction += 1

            ' Other -------------------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "Other", .Value = ""}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "AssetTag") Then
               If (obj("AssetTag") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Asset Tag", .Value = obj("AssetTag").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Availability") Then
               If (obj("Availability") IsNot Nothing) Then
                  Dim tmp As String = ""
                  Select Case obj("Availability")
                     Case 1 : tmp = "Other"
                     Case 2 : tmp = "Unknown"
                     Case 3 : tmp = "Running/Full Power"
                     Case 4 : tmp = "Warning"
                     Case 5 : tmp = "In Test"
                     Case 6 : tmp = "Not Applicable"
                     Case 7 : tmp = "Power Off"
                     Case 8 : tmp = "Off Line"
                     Case 9 : tmp = "Off Duty"
                     Case 10 : tmp = "Degraded"
                     Case 11 : tmp = "Not Installed"
                     Case 12 : tmp = "Install Error"
                     Case 13 : tmp = "Power Save - Unknown"
                     Case 14 : tmp = "Power Save - Low Power Mode"
                     Case 15 : tmp = "Power Save - Standby"
                     Case 16 : tmp = "Power Cycle"
                     Case 17 : tmp = "Power Save - Warning"
                     Case 18 : tmp = "Paused"
                     Case 19 : tmp = "Not Ready"
                     Case 20 : tmp = "Not Configured"
                     Case 21 : tmp = "Quiesced (device is quiet)"
                  End Select
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Availability", .Value = tmp, .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ConfigManagerErrorCode") Then
               If (obj("ConfigManagerErrorCode") IsNot Nothing) Then
                  Dim tmp As String = ""
                  Select Case obj("ConfigManagerErrorCode")
                     Case 0 : tmp = "Device is working properly"
                     Case 1 : tmp = "Device is not configured correctly"
                     Case 2 : tmp = "Windows cannot load the driver for this device"
                     Case 3 : tmp = "Driver for this device might be corrupted, or your system may be running low on memory or other resources"
                     Case 4 : tmp = "Device is not working properly. One of its drivers or the registry might be corrupted"
                     Case 5 : tmp = "Driver for the device requires a resource that Windows cannot manage"
                     Case 6 : tmp = "Boot configuration for the device conflicts with other devices"
                     Case 7 : tmp = "Cannot filter"
                     Case 8 : tmp = "Driver loader for the device is missing"
                     Case 9 : tmp = "Device is not working properly because the controlling firmware is reporting the resources for the device incorrectly"
                     Case 10 : tmp = "Device cannot start"
                     Case 11 : tmp = "Device failed"
                     Case 12 : tmp = "Device cannot find enough free resources that it can use"
                     Case 13 : tmp = "Windows cannot verify the device's resources"
                     Case 14 : tmp = "Device cannot work properly until you restart your computer"
                     Case 15 : tmp = "Device is not working properly because there is probably a re-enumeration problem"
                     Case 16 : tmp = "Windows cannot identify all of the resources that the device uses"
                     Case 17 : tmp = "Device is requesting an unknown resource type"
                     Case 18 : tmp = "Reinstall the drivers for this device"
                     Case 19 : tmp = "Failure using the VxD loader"
                     Case 20 : tmp = "Registry might be corrupted"
                     Case 21 : tmp = "System failure: Try changing the driver for this device. If that does not work, see your hardware documentation. Windows is removing the device"
                     Case 22 : tmp = "Device is disabled"
                     Case 23 : tmp = "System failure: Try changing the driver for this device. If that doesn't work, see your hardware documentation"
                     Case 24 : tmp = "Device is not present, is not working properly, or does not have all its drivers installed"
                     Case 25 : tmp = "Windows is still setting up the device"
                     Case 26 : tmp = "Windows is still setting up the device"
                     Case 27 : tmp = "Device does not have valid log configuration"
                     Case 28 : tmp = "Drivers for this device are not installed"
                     Case 29 : tmp = "Device is disabled because the firmware of the device did not give it the required resources"
                     Case 30 : tmp = "Device is using an IRQ resource that another device is using"
                     Case 31 : tmp = "Device is not working properly because Windows cannot load the drivers required for this device"
                  End Select
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Config Manager Error Code", .Value = tmp, .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ConfigManagerUserConfig") Then
               If (obj("ConfigManagerUserConfig") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName,
                     .Label = "Config Manager User Configuration",
                     .Value = If(obj("ConfigManagerUserConfig"), "Device is using a user-defined configuration", "Device is not using a user-defined configuration"),
                     .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "CpuStatus") Then
               If (obj("CpuStatus") IsNot Nothing) Then
                  Dim tmp As String = ""
                  Select Case obj("CpuStatus")
                     Case 0 : tmp = "Unknown"
                     Case 1 : tmp = "CPU Enabled"
                     Case 2 : tmp = "CPU Disabled by User via BIOS Setup"
                     Case 3 : tmp = "CPU Disabled By BIOS (POST Error)"
                     Case 4 : tmp = "CPU is Idle"
                     Case 5 : tmp = "Reserved"
                     Case 6 : tmp = "Reserved"
                     Case 7 : tmp = "Other"
                  End Select
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "CPU Status", .Value = tmp, .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "CurrentClockSpeed") Then
               If (obj("CurrentClockSpeed") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Current Clock Speed", .Value = obj("CurrentClockSpeed").ToString() & " MHz", .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "CurrentVoltage") Then
               If (obj("CurrentVoltage") IsNot Nothing) Then
                  Dim cv As Byte = CByte(obj("CurrentVoltage"))
                  Dim voltage As Double = (cv And &H7F) * 0.1
                  Dim valid As Boolean = (cv And &H80) <> 0

                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName,
                     .Label = "Current Voltage",
                     .Value = If(valid, voltage.ToString("0.0") & " V", voltage.ToString("0.0") & " V (not validated)"),
                     .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "DeviceID") Then
               If (obj("DeviceID") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Device ID", .Value = obj("DeviceID").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "InstallDate") Then
               If (obj("InstallDate") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "InstallDate", .Value = obj("InstallDate").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Level") Then
               If (obj("Level") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Level", .Value = obj("Level").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "LoadPercentage") Then
               If (obj("LoadPercentage") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Load Percentage", .Value = obj("LoadPercentage").ToString() & "%", .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "MaxClockSpeed") Then
               If (obj("MaxClockSpeed") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Max Clock Speed", .Value = obj("MaxClockSpeed").ToString() & " MHz", .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1


            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "PartNumber") Then
               If (obj("PartNumber") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Part Number", .Value = obj("PartNumber").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "PNPDeviceID") Then
               If (obj("PNPDeviceID") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "PNP Device ID", .Value = obj("PNPDeviceID").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "UpgradeMethod") Then
               If (obj("UpgradeMethod") IsNot Nothing) Then
                  Dim tmp As String = ""
                  Select Case obj("UpgradeMethod")
                     Case 1 : tmp = "Other"
                     Case 2 : tmp = "Unknown"
                     Case 3 : tmp = "Daughter Board"
                     Case 4 : tmp = "ZIF Socket"
                     Case 5 : tmp = "Replacement/Piggy Back"
                     Case 6 : tmp = "None"
                     Case 7 : tmp = "LIF Socket"
                     Case 8 : tmp = "Slot 1"
                     Case 9 : tmp = "Slot 2"
                     Case 10 : tmp = "370 Pin Socket"
                     Case 11 : tmp = "Slot A"
                     Case 12 : tmp = "Slot M"
                     Case 13 : tmp = "Socket 423"
                     Case 14 : tmp = "Socket A (Socket 462)"
                     Case 15 : tmp = "Socket 478"
                     Case 16 : tmp = "Socket 754"
                     Case 17 : tmp = "Socket 940"
                     Case 18 : tmp = "Socket 939"
                     Case Else : tmp = "Not defined / Vendor Specific"
                  End Select
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Upgrade Method", .Value = tmp, .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "VoltageCaps") Then
               If (obj("VoltageCaps") IsNot Nothing) Then
                  Dim vc As UInt32 = CUInt(obj("VoltageCaps"))
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Supports 5.0 V", .Value = If((vc And 1) <> 0, "True", "False"), .ImageIndex = 0}})
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Supports 3.3 V", .Value = If((vc And 2) <> 0, "True", "False"), .ImageIndex = 0}})
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Supports 2.9 V", .Value = If((vc And 4) <> 0, "True", "False"), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            ' Excluded ----------------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1 ' CreationClassName
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1 ' SystemCreationClassName

         Next
      Catch ex As Exception
         MsgBox(ex.Message)
      End Try
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' BackgroundWorker: RunWorkerCompleted (Update ListView when background work is completed)
   Private Sub BackgroundWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
      If e.Error IsNot Nothing Then
         MessageBox.Show("Error: " & e.Error.Message, "BackgroundWorker Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
         Return
      End If

      If e.Cancelled Then
         MessageBox.Show("Operation was cancelled.", "BackgroundWorker Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
         Return
      End If

      ' Optional: Auto-resize columns for better display
      lvProcessor.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' BackgroundWorker: ProgressChanged (Runs on the UI thread automatically)
   Private Sub BackgroundWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs)
      Dim info As ProgressInfo = CType(e.UserState, ProgressInfo)

      Select Case info.Kind
         Case ProgressKind.SetMax
            MainForm?.SetProgressMax(info.Max)
         Case ProgressKind.SetValue
            MainForm?.SetProgressValue(info.Value)
         Case ProgressKind.AppendItem
            AppendLiveItem(info.Item)
      End Select
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' UpdateListView (with the retrieved items)
   Private Sub AppendLiveItem(item As ProcListItem)
      If item Is Nothing Then Return

      Dim grp As ListViewGroup = Nothing
      If Not groupCache.TryGetValue(item.Group, grp) Then
         grp = New ListViewGroup(item.Group, HorizontalAlignment.Left)
         groupCache.Add(item.Group, grp)
         lvProcessor.Groups.Add(grp)
      End If

      Dim lvi As New ListViewItem(item.Label)
      lvi.SubItems.Add(item.Value)
      lvi.Group = grp
      lvi.ImageIndex = item.ImageIndex

      If String.IsNullOrWhiteSpace(item.Value) Then
         lvi.BackColor = Color.LightGray
         lvi.Font = New Font(lvi.Font, FontStyle.Bold)
      End If

      lvProcessor.Items.Add(lvi)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' BackgroundScan
   Private Sub BackgroundScan()
      lvProcessor.Items.Clear()
      lvProcessor.Groups.Clear()
      groupCache.Clear()

      Dim backgroundWorker As New BackgroundWorker()
      backgroundWorker.WorkerReportsProgress = True
      AddHandler backgroundWorker.DoWork, AddressOf BackgroundWorker_DoWork
      AddHandler backgroundWorker.ProgressChanged, AddressOf BackgroundWorker_ProgressChanged
      AddHandler backgroundWorker.RunWorkerCompleted, AddressOf BackgroundWorker_RunWorkerCompleted
      MainForm?.ResetProgress()
      backgroundWorker.RunWorkerAsync()
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' frmProcessor: OnLoad
   Private Sub frmProcessor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
      lvProcessor.BackColor = Color.FromArgb(224, 234, 213)

      If MainForm IsNot Nothing Then
         If remoteHost <> "" Then
            MainForm.SetTitle("Remus Rigo OSI: Processor v1.0.20260808 on " & remoteHost)
         Else
            MainForm.SetTitle("Remus Rigo OSI: Processor v1.0.20260808 " & remoteHost)
         End If
      End If

      BackgroundScan()
   End Sub

End Class