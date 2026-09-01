'--------------------------------------------------------------------------------------------------
' Win32_BIOS
' https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-bios
'
'    © Remus Rigo
'       v1.0.20260818
'--------------------------------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.Management
Imports SharedInterfaces

Public Class frmBaseBoardBIOS
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

         Dim myQuery As New ObjectQuery("SELECT * FROM Win32_BIOS")
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
            Dim groupName As String = "BIOS #" & cnt

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
               If (obj("Caption") IsNot Nothing) And (obj("Caption") <> obj("Name")) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                  .Group = groupName, .Label = "Caption", .Value = obj("Caption").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Description") Then
               If (obj("Description") IsNot Nothing) And (obj("Description") <> obj("Name")) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                  .Group = groupName, .Label = "Description", .Value = obj("Description").ToString(), .ImageIndex = 0}})
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

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SystemBiosMajorVersion") Then
               If (obj("SystemBiosMajorVersion") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Major Version", .Value = obj("SystemBiosMajorVersion").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SystemBiosMinorVersion") Then
               If (obj("SystemBiosMinorVersion") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Minor Version", .Value = obj("SystemBiosMinorVersion").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SerialNumber") Then
               Dim sn As String = "To be filled by O.E.M."
               If (obj("SerialNumber") <> "Default string") Then
                  sn = obj("SerialNumber").ToString()
               End If
               worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                  .Group = groupName, .Label = "Serial Number", .Value = sn, .ImageIndex = 0}})
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Manufacturer") Then
               If (obj("Manufacturer") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Manufacturer", .Value = obj("Manufacturer").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "BIOSVersion") Then
               If (obj("BIOSVersion") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "BIOS Version", .Value = String.Join(", ", DirectCast(obj("BIOSVersion"), String())), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "BuildNumber") Then
               If (obj("BuildNumber") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Build Number", .Value = obj("BuildNumber").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ReleaseDate") Then
               If (obj("ReleaseDate") IsNot Nothing) Then
                  Dim rawDate As String = obj("ReleaseDate")?.ToString()
                  Dim releaseDate As String = If(String.IsNullOrEmpty(rawDate), "(unknown)", ManagementDateTimeConverter.ToDateTime(rawDate).ToString("yyyy-MM-dd HH:mm:ss.ffffff tt"))
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "ReleaseDate", .Value = releaseDate, .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "PrimaryBIOS") Then
               If (obj("PrimaryBIOS") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Primary BIOS", .Value = If(obj("PrimaryBIOS"), "Yes", "No"), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1


            ' SMBIOS ------------------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "System Management BIOS", .Value = ""}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SMBIOSPresent") Then
               If (obj("SMBIOSPresent") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Present", .Value = If(obj("SMBIOSPresent"), "Yes", "No"), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SMBIOSBIOSVersion") Then
               If (obj("SMBIOSBIOSVersion") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "BIOS Version", .Value = obj("SMBIOSBIOSVersion").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SMBIOSMajorVersion") Then
               If (obj("SMBIOSMajorVersion") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Major Version", .Value = obj("SMBIOSMajorVersion").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SMBIOSMinorVersion") Then
               If (obj("SMBIOSMinorVersion") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Minor Version", .Value = obj("SMBIOSMinorVersion").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            ' Embedded Controller -----------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "Embedded Controller", .Value = ""}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "EmbeddedControllerMajorVersion") Then
               If (obj("EmbeddedControllerMajorVersion") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Major Version",
                     .Value = If(obj("EmbeddedControllerMajorVersion") = 255, "Not Set", obj("EmbeddedControllerMajorVersion")), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "EmbeddedControllerMinorVersion") Then
               If (obj("EmbeddedControllerMinorVersion") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Minor Version",
                     .Value = If(obj("EmbeddedControllerMinorVersion") = 255, "Not Set", obj("EmbeddedControllerMinorVersion")), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            ' Characteristics -----------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "Characteristics", .Value = ""}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "BiosCharacteristics") Then
               If (obj("BiosCharacteristics") IsNot Nothing) Then
                  Dim opt As UInt16() = TryCast(obj("BiosCharacteristics"), UInt16())
                  If opt IsNot Nothing AndAlso opt.Length > 0 Then
                     For Each code As UInt16 In opt
                        worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                           .Group = groupName, .Label = code, .Value = GetWMIBIOSCharacteristic(code), .ImageIndex = 0}})
                     Next
                  Else
                     worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                           .Group = groupName, .Label = "", .Value = "empty/null", .ImageIndex = 0}})
                  End If
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            ' Language ----------------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "Language", .Value = ""}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "LanguageEdition") Then
               If (obj("LanguageEdition") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                  .Group = groupName, .Label = "Language Edition", .Value = obj("LanguageEdition").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ListOfLanguages") Then
               If (obj("ListOfLanguages") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
         .Group = groupName, .Label = "ListOfLanguages", .Value = String.Join(", ", DirectCast(obj("ListOfLanguages"), String())), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1



            ' Other -------------------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "Other", .Value = ""}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "CodeSet") Then
               If (obj("CodeSet") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Code Set", .Value = obj("CodeSet").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "CurrentLanguage") Then
               If (obj("CurrentLanguage") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Current Language", .Value = obj("CurrentLanguage").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "IdentificationCode") Then
               If (obj("IdentificationCode") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Identification Code", .Value = obj("IdentificationCode").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "InstallableLanguages") Then
               If (obj("InstallableLanguages") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Installable Languages", .Value = obj("InstallableLanguages").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "InstallDate") Then
               If (obj("InstallDate") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                  .Group = groupName, .Label = "Install Date", .Value = obj("InstallDate").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SoftwareElementID") Then
               If (obj("SoftwareElementID") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Software Element ID", .Value = obj("SoftwareElementID").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SoftwareElementState") Then
               If (obj("SoftwareElementState") IsNot Nothing) Then
                  Dim tmp As String = ""
                  Select Case obj("SoftwareElementState")
                     Case 0 : tmp = "Deployable"
                     Case 1 : tmp = "Installable"
                     Case 2 : tmp = "Executable"
                     Case 3 : tmp = "Running"
                  End Select
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Software Element State", .Value = tmp, .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Status") Then
               If (obj("Status") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Status", .Value = obj("Status").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "TargetOperatingSystem") Then
               If (obj("TargetOperatingSystem") IsNot Nothing) Then
                  Dim tmp As String = ""
                  Select Case obj("TargetOperatingSystem")
                     Case 0 : tmp = "Unknown (default)"
                     Case 1 : tmp = "Other"
                     Case 2 : tmp = "MACOS"
                     Case 3 : tmp = "ATTUNIX"
                     Case 4 : tmp = "DGUX"
                     Case 5 : tmp = "DECNT"
                     Case 6 : tmp = "Digital Unix"
                     Case 7 : tmp = "OpenVMS"
                     Case 8 : tmp = "HPUX"
                     Case 9 : tmp = "AIX"
                     Case 10 : tmp = "MVS"
                     Case 11 : tmp = "OS400"
                     Case 12 : tmp = "OS/2"
                     Case 13 : tmp = "JavaVM"
                     Case 14 : tmp = "MSDOS"
                     Case 15 : tmp = "WIN3x"
                     Case 16 : tmp = "WIN95"
                     Case 17 : tmp = "WIN98"
                     Case 18 : tmp = "WINNT"
                     Case 19 : tmp = "WINCE"
                     Case 20 : tmp = "NCR3000"
                     Case 21 : tmp = "NetWare"
                     Case 22 : tmp = "OSF"
                     Case 23 : tmp = "DC/OS"
                     Case 24 : tmp = "Reliant UNIX"
                     Case 25 : tmp = "SCO UnixWare"
                     Case 26 : tmp = "SCO OpenServer"
                     Case 27 : tmp = "Sequent"
                     Case 28 : tmp = "IRIX"
                     Case 29 : tmp = "Solaris"
                     Case 30 : tmp = "SunOS"
                     Case 31 : tmp = "U6000"
                     Case 32 : tmp = "ASERIES"
                     Case 33 : tmp = "TandemNSK"
                     Case 34 : tmp = "TandemNT"
                     Case 35 : tmp = "BS2000"
                     Case 36 : tmp = "LINUX"
                     Case 37 : tmp = "Lynx"
                     Case 38 : tmp = "XENIX"
                     Case 39 : tmp = "VM/ESA"
                     Case 40 : tmp = "Interactive UNIX"
                     Case 41 : tmp = "BSDUNIX"
                     Case 42 : tmp = "FreeBSD"
                     Case 43 : tmp = "NetBSD"
                     Case 44 : tmp = "GNU Hurd"
                     Case 45 : tmp = "OS9"
                     Case 46 : tmp = "MACH Kernel"
                     Case 47 : tmp = "Inferno"
                     Case 48 : tmp = "QNX"
                     Case 49 : tmp = "EPOC"
                     Case 50 : tmp = "IxWorks"
                     Case 51 : tmp = "VxWorks"
                     Case 52 : tmp = "MiNT"
                     Case 53 : tmp = "BeOS"
                     Case 54 : tmp = "HP MPE"
                     Case 55 : tmp = "NextStep"
                     Case 56 : tmp = "PalmPilot"
                     Case 57 : tmp = "Rhapsody"
                     Case 58 : tmp = "Windows 2000"
                     Case 59 : tmp = "Dedicated"
                     Case 60 : tmp = "VSE"
                     Case 61 : tmp = "TPF"
                     Case Else : tmp = "Not defined"
                  End Select

                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Target Operating System", .Value = tmp, .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "OtherTargetOS") Then
               If (obj("OtherTargetOS") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                  .Group = groupName, .Label = "Other Target OS", .Value = obj("OtherTargetOS").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            'Excluded ----------------------------------------------------------------------------

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
      lvBaseBoardBIOS.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
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
         lvBaseBoardBIOS.Groups.Add(grp)
      End If

      Dim lvi As New ListViewItem(item.Label)
      lvi.SubItems.Add(item.Value)
      lvi.Group = grp
      lvi.ImageIndex = item.ImageIndex

      If String.IsNullOrWhiteSpace(item.Value) Then
         lvi.BackColor = Color.LightGray
         lvi.Font = New Font(lvi.Font, FontStyle.Bold)
      End If

      lvBaseBoardBIOS.Items.Add(lvi)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' BackgroundScan
   Private Sub BackgroundScan()
      lvBaseBoardBIOS.Items.Clear()
      lvBaseBoardBIOS.Groups.Clear()
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
   ' frmBaseBoard: OnLoad
   Private Sub frmBaseBoardBIOS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
      lvBaseBoardBIOS.BackColor = Color.FromArgb(224, 234, 213)

      If MainForm IsNot Nothing Then
         MainForm.SetTitle("Remus Rigo OSI: BaseBoard v1.0.20260809" & If(remoteHost <> "", "on " & "[" & remoteHost & "]", ""))
      End If

      BackgroundScan()
   End Sub

End Class