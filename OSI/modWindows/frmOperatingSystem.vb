'--------------------------------------------------------------------------------------------------
' Win32_OperatingSystem class
' https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-operatingsystem
'
'    © Remus Rigo
'       v1.0.20260818
'--------------------------------------------------------------------------------------------------

Imports System.ComponentModel
Imports System.Management
Imports System.Runtime.Versioning
Imports SharedInterfaces

Public Class frmOperatingSystem
   Implements IModuleForm
   <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
   Public Property MainForm As IMainForm Implements IModuleForm.MainForm
   Public remoteHost, remoteUser, remotePass As String

   <SupportedOSPlatform("windows")>
   Private Sub frmOS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
      lvOS.BackColor = Color.FromArgb(224, 234, 213)
      lvOSProd.BackColor = Color.FromArgb(224, 234, 213)

      Dim backgroundWorker As New System.ComponentModel.BackgroundWorker()
      AddHandler backgroundWorker.DoWork, AddressOf BackgroundWorker_DoWork
      AddHandler backgroundWorker.RunWorkerCompleted, AddressOf BackgroundWorker_RunWorkerCompleted
      backgroundWorker.RunWorkerAsync()

      If MainForm IsNot Nothing Then
         MainForm.SetTitle("Remus Rigo OSI: OS Info v1.1" & If(remoteHost <> "", "on " & "[" & remoteHost & "]", ""))
      End If

   End Sub

   <SupportedOSPlatform("windows")>
   Private Sub BackgroundWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
      If OperatingSystem.IsWindows() Then
         Dim myConnection As New ConnectionOptions()
         Dim scopePath As String
         Dim items As New List(Of ListViewItem)
         Dim itemsProd As New List(Of ListViewItem)

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
            MainForm.ResetProgress()
            scope.Connect()
            Dim myQuery As New ObjectQuery("SELECT * FROM Win32_OperatingSystem")
            Using searcher As New ManagementObjectSearcher(scope, myQuery)

               ' count properties per object * objects
               Dim crtAction As Integer = 1
               Dim objItems = searcher.Get()
               Dim objCounter As Integer = objItems.Count
               Dim propsPerObj As Integer = 0
               If objCounter > 0 Then
                  propsPerObj = objItems.Cast(Of ManagementObject)().First().Properties.Cast(Of PropertyData)().Count(Function(p) p.Name <> "Class" AndAlso p.Name <> "Path")
               End If
               Dim totalProps As Integer = propsPerObj * objCounter
               MainForm.SetProgressMax(totalProps)

               For Each obj As ManagementObject In searcher.Get()

                  items.Add(New ListViewItem({"Version", ""})) '-----------------------------------

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Caption") Then
                     If (obj("Caption") IsNot Nothing) Then
                        items.Add(New ListViewItem({"OS", obj("Caption").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Manufacturer") Then
                     If (obj("Manufacturer") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Manufacturer", obj("Manufacturer").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Version") Then
                     If (obj("Version") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Version", obj("Version").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "BuildNumber") Then
                     If (obj("BuildNumber") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Build Number", obj("BuildNumber").ToString()}))
                        Dim osVer, osCode, osRel, osEnd As String
                        osVer = "" : osCode = "" : osRel = "" : osEnd = ""
                        Select Case obj("BuildNumber")
                           Case "002"
                              osVer = "3.11"
                              osCode = ""
                              osRel = "1993-11-08"
                           Case "102"
                              osVer = "3.10"
                              osCode = "Sparta"
                              osRel = "October 31, 1992"
                              osEnd = "December 31, 2001"
                           Case "103"
                              osVer = "3.10"
                              osCode = "Janus"
                              osRel = "April 6, 1992"
                              osEnd = "December 31, 2001"
                           Case "153"
                              osVer = "3.2"
                              osCode = ""
                              osRel = "November 22, 1993"
                              osEnd = "December 31, 2001"
                           Case "300"
                              osVer = "3.11"
                              osCode = "Snowball"
                              osRel = "November 8, 1993"
                              osEnd = "2001-12-31"
                           Case "528"
                              osVer = "NT 3.1"
                              osCode = "Razzle"
                              osRel = "July 27, 1993"
                              osEnd = "2000-12-31"
                           Case "807"
                              osVer = "NT 3.5"
                              osCode = "Daytona"
                              osRel = "September 21, 1994"
                              osEnd = "December 31, 2001"
                           Case "950"
                              osVer = "4.00"
                              osCode = "Chicago"
                              osRel = "August 24, 1995"
                              osEnd = "December 31, 2001"
                           Case "1057"
                              osVer = "NT 3.51"
                              osCode = "Daytona"
                              osRel = "May 30, 1995"
                              osEnd = "December 31, 2001"
                           Case "1381"
                              osVer = "NT 4.0"
                              osCode = "Shell Update Release (Tukwila)"
                              osRel = "August 24, 1996"
                              osEnd = "June 30, 2004"
                           Case "1998"
                              osVer = "4.10"
                              osCode = "Memphis"
                              osRel = "June 25, 1998"
                              osEnd = "July 11, 2006"
                           Case "2195"
                              osVer = "NT 5.0"
                              osCode = "Windows NT 5.0"
                              osRel = "February 17, 2000"
                              osEnd = "July 13, 2010"
                           Case "2222A"
                              osVer = "4.10"
                              osCode = "Memphis"
                              osRel = "June 10, 1999"
                              osEnd = "July 11, 2006"
                           Case "2600"
                              osVer = "NT 5.1"
                              osCode = "Whistler / Freestyle / Harmony"
                              osRel = "October 25, 2001 / October 29, 2002 / September 30, 2003"
                              osEnd = "April 8, 2014"
                           Case "2700"
                              osVer = "NT 5.1"
                              osCode = "Symphony"
                              osRel = "	October 12, 2004"
                              osEnd = "April 8, 2014"
                           Case "2710"
                              osVer = "NT 5.1"
                              osCode = "Emerald"
                              osRel = "	October 14, 2005"
                              osEnd = "April 8, 2014"
                           Case "3000"
                              osVer = "4.90"
                              osCode = "Millennium"
                              osRel = "September 14, 2000"
                              osEnd = "July 11, 2006"
                           Case "3790"
                              osVer = "NT 5.2"
                              osCode = "Anvil"
                              osRel = "April 25, 2005"
                              osEnd = "April 8, 2014"
                           Case "6002"
                              osVer = "NT 6.0"
                              osCode = "Longhorn"
                              osRel = "January 30, 2007"
                              osEnd = "April 11, 2017"
                           Case "7601"
                              osVer = "NT 6.1"
                              osCode = "Windows 7"
                              osRel = "January 30, 2007"
                              osEnd = "	January 14, 2020"
                           Case "9200"
                              osVer = "NT 6.2"
                              osCode = "Windows 8"
                              osRel = "October 26, 2012"
                              osEnd = "January 12, 2016"
                           Case "9600"
                              osVer = "NT 6.3"
                              osCode = "Blue"
                              osRel = "October 17, 2013"
                              osEnd = "January 10, 2023"
                           Case "10240"
                              osVer = "NT 10.0 / 1507"
                              osCode = "Threshold"
                              osRel = "July 29, 2015"
                              osEnd = "May 9, 2017"
                           Case "10586"
                              osVer = "1511"
                              osCode = "Threshold 2"
                              osRel = "November 10, 2015"
                              osEnd = "October 10, 2017"
                           Case "14393"
                              osVer = "1607"
                              osCode = "Redstone 1"
                              osRel = "August 2, 2016"
                              osEnd = "April 10, 2018"
                           Case "15063"
                              osVer = "1703"
                              osCode = "Redstone 2"
                              osRel = "April 5, 2017"
                              osEnd = "October 9, 2018"
                           Case "16299"
                              osVer = "1709"
                              osCode = "Redstone 3"
                              osRel = "October 17, 2017"
                              osEnd = "April 9, 2019"
                           Case "17134"
                              osVer = "1803"
                              osCode = "Redstone 4"
                              osRel = "April 30, 2018"
                              osEnd = "November 12, 2019"
                           Case "17763"
                              osVer = "1809"
                              osCode = "Redstone 5"
                              osRel = "November 13, 2018"
                              osEnd = "November 10, 2020"
                           Case "18362"
                              osVer = "1903"
                              osCode = "19H1"
                              osRel = "May 21, 2019"
                              osEnd = "December 8, 2020"
                           Case "18363"
                              osVer = "1909"
                              osCode = "Vanadium"
                              osRel = "November 12, 2019"
                              osEnd = "May 11, 2021"
                           Case "19041"
                              osVer = "2004"
                              osCode = "Vibranium"
                              osRel = "May 27, 2020"
                              osEnd = "December 14, 2021"
                           Case "19042"
                              osVer = "20H2"
                              osCode = "Vibranium"
                              osRel = "October 20, 2020"
                              osEnd = "August 9, 2022"
                           Case "19043"
                              osVer = "21H1"
                              osCode = "Vibranium"
                              osRel = "May 18, 2021"
                              osEnd = "December 13, 2022"
                           Case "19044"
                              osVer = "21H2"
                              osCode = "Vibranium"
                              osRel = "November 16, 2021"
                              osEnd = "June 13, 2023"
                           Case "19045"
                              osVer = "22H2"
                              osCode = "Vibranium"
                              osRel = "October 18, 2022"
                              osEnd = "October 14, 2025"
                           Case "22000"
                              osVer = "21H2"
                              osCode = "Cobalt"
                              osRel = "October 4, 2021"
                              osEnd = "October 10, 2023"
                           Case "22621"
                              osVer = "22H2"
                              osCode = "Nickel"
                              osRel = "September 20, 2022"
                              osEnd = "October 8, 2024"
                           Case "22631"
                              osVer = "23H2"
                              osCode = "Nickel"
                              osRel = "October 31, 2023"
                              osEnd = "November 11, 2025"
                           Case "26100"
                              osVer = "24H2"
                              osCode = "Germanium"
                              osRel = "October 1, 2024"
                              osEnd = "October 13, 2026"
                           Case "26200"
                              osVer = "25H2"
                              osCode = "Germanium"
                              osRel = "September 30, 2025"
                              osEnd = "October 12, 2027"
                           Case Else
                              osVer = "Unknown"
                              osCode = "Unknown"
                              osRel = "Unknown"
                              osEnd = "Unknown"
                        End Select
                        items.Add(New ListViewItem({"Version", osVer}))
                        items.Add(New ListViewItem({"Codename", osCode}))
                        items.Add(New ListViewItem({"Release Date", osRel}))
                        items.Add(New ListViewItem({"End of support", osEnd}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "BuildType") Then
                     If (obj("BuildType") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Build Type", obj("BuildType").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ServicePackMajorVersion") Then
                     If (obj("ServicePackMajorVersion") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Service Pack Major Version", obj("ServicePackMajorVersion").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ServicePackMinorVersion") Then
                     If (obj("ServicePackMinorVersion") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Service Pack Minor Version", obj("ServicePackMinorVersion").ToString()}))
                     End If
                  End If

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "OSProductSuite") Then
                     If (obj("OSProductSuite") IsNot Nothing) Then
                        Dim suite As String = obj("OSProductSuite")
                        Dim suites As New Dictionary(Of Integer, String) From {
                           {1, "Microsoft Small Business Server was once installed, but may have been upgraded to another version of Windows"},
                           {2, "Windows Server 2008 Enterprise is installed"},
                           {4, "Windows BackOffice components are installed"},
                           {8, "Communication Server is installed"},
                           {16, "Terminal Services is installed"},
                           {32, "Microsoft Small Business Server is installed with the restrictive client license"},
                           {64, "Windows Embedded is installed"},
                           {128, "A Datacenter edition is installed"},
                           {256, "Terminal Services is installed, but only one interactive session is supported"},
                           {512, "Windows Home Edition is installed"},
                           {1024, "Web Server Edition is installed"},
                           {8192, "Storage Server Edition is installed"},
                           {16384, "Compute Cluster Edition is installed"}
                        }
                        Dim strSuite As String = ""
                        For Each kvp In suites
                           If (suite And kvp.Key) <> 0 Then
                              If strSuite = "" Then
                                 strSuite = kvp.Value
                              Else
                                 strSuite = strSuite & " / " & kvp.Value
                              End If
                           End If
                        Next
                        items.Add(New ListViewItem({"Product Suite", strSuite}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SuiteMask") Then
                     If (obj("SuiteMask") IsNot Nothing) Then

                        Dim suite As String = obj("SuiteMask")
                        Dim suites As New Dictionary(Of Integer, String) From {
                           {1, "Small Business"},
                           {2, "Enterprise"},
                           {4, "BackOffice"},
                           {8, "Communications"},
                           {16, "Terminal"},
                           {32, "Small Business Restricted"},
                           {64, "EmbeddedNT"},
                           {128, "Datacenter"},
                           {256, "Single User"},
                           {512, "Personal"},
                           {1024, "Blade"},
                           {2048, "Embedded Restricted"},
                           {4096, "Security Appliance"},
                           {8192, "Storage Server"},
                           {16384, "Compute Cluster"},
                           {32768, "WH Server"}
                        }
                        Dim strSuite As String = ""
                        For Each kvp In suites
                           If (suite And kvp.Key) <> 0 Then
                              If strSuite = "" Then
                                 strSuite = kvp.Value
                              Else
                                 strSuite = strSuite & " / " & kvp.Value
                              End If
                           End If
                        Next
                        'MessageBox.Show("Suite: " & kvp.Value)
                        items.Add(New ListViewItem({"Suite Mask", strSuite}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "OSType") Then
                     If (obj("OSType") IsNot Nothing) Then
                        Dim strType As String = ""
                        Select Case obj("OSType")
                           Case 0
                              strType = "Unknown"
                           Case 1
                              strType = "Other"
                           Case 2
                              strType = "MACOS"
                           Case 3
                              strType = "ATTUNIX"
                           Case 4
                              strType = "DGUX"
                           Case 5
                              strType = "DECNT"
                           Case 6
                              strType = "Digital Unix"
                           Case 7
                              strType = "OpenVMS"
                           Case 8
                              strType = "HPUX"
                           Case 9
                              strType = "AIX"
                           Case 10
                              strType = "MVS"
                           Case 11
                              strType = "OS400"
                           Case 12
                              strType = "OS/2"
                           Case 13
                              strType = "JavaVM"
                           Case 14
                              strType = "MS-DOS"
                           Case 15
                              strType = "WIN 3x"
                           Case 16
                              strType = "WIN 95"
                           Case 17
                              strType = "WIN98"
                           Case 18
                              strType = "WINNT"
                           Case 19
                              strType = "WINCE"
                           Case 20
                              strType = "NCR3000"
                           Case 21
                              strType = "NetWare"
                           Case 22
                              strType = "OSF"
                           Case 23
                              strType = "DC/OS"
                           Case 24
                              strType = "Reliant UNIX"
                           Case 25
                              strType = "SCO UnixWare"
                           Case 26
                              strType = "SCO OpenServer"
                           Case 27
                              strType = "Sequent"
                           Case 28
                              strType = "IRIX"
                           Case 29
                              strType = "Solaris"
                           Case 30
                              strType = "SunOS"
                           Case 31
                              strType = "U6000"
                           Case 32
                              strType = "ASERIES"
                           Case 33
                              strType = "TandemNSK"
                           Case 34
                              strType = "TandemNT"
                           Case 35
                              strType = "BS2000"
                           Case 36
                              strType = "LINUX"
                           Case 37
                              strType = "Lynx"
                           Case 38
                              strType = "XENIX"
                           Case 39
                              strType = "VM/ESA"
                           Case 40
                              strType = "Interactive UNIX"
                           Case 41
                              strType = "BSD UNIX"
                           Case 42
                              strType = "FreeBSD"
                           Case 43
                              strType = "NetBSD"
                           Case 44
                              strType = "GNU Hurd"
                           Case 45
                              strType = "OS9"
                           Case 46
                              strType = "MACH Kernel"
                           Case 47
                              strType = "Inferno"
                           Case 48
                              strType = "QNX"
                           Case 49
                              strType = "EPOC"
                           Case 50
                              strType = "IxWorks"
                           Case 51
                              strType = "VxWorks"
                           Case 52
                              strType = "MiNT"
                           Case 53
                              strType = "BeOS"
                           Case 54
                              strType = "HP MPE"
                           Case 55
                              strType = "NextStep"
                           Case 56
                              strType = "PalmPilot"
                           Case 57
                              strType = "Rhapsody"
                           Case 58
                              strType = "Windows 2000"
                           Case 59
                              strType = "Dedicated"
                           Case 60
                              strType = "OS/390"
                           Case 61
                              strType = "VSE"
                           Case 62
                              strType = "TPF"
                           Case Else
                              strType = "not defined"
                        End Select
                        items.Add(New ListViewItem({"OS Type", strType}))
                        strType = Nothing
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "OSArchitecture") Then
                     If (obj("OSArchitecture") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Architecture", obj("OSArchitecture").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  items.Add(New ListViewItem({"Instalation", ""})) '-------------------------------------

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "InstallDate") Then
                     If (obj("InstallDate") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Install Date", ConvertToDate(obj("InstallDate").ToString())}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "CSName") Then
                     If (obj("CSName") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Host Name", obj("CSName").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "PSComputerName") Then
                     If (obj("PSComputerName") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Computer Name", obj("PSComputerName").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Description") Then
                     If (obj("Description") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Description", obj("Description").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "BootDevice") Then
                     If (obj("BootDevice") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Boot Device", obj("BootDevice").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SystemDevice") Then
                     If (obj("SystemDevice") IsNot Nothing) Then
                        items.Add(New ListViewItem({"SystemDevice", obj("SystemDevice").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "WindowsDirectory") Then
                     If (obj("WindowsDirectory") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Windows Directory", obj("WindowsDirectory").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SystemDirectory") Then
                     If (obj("SystemDirectory") IsNot Nothing) Then
                        items.Add(New ListViewItem({"System Directory", obj("SystemDirectory").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SystemDrive") Then
                     If (obj("SystemDrive") IsNot Nothing) Then
                        items.Add(New ListViewItem({"System Drive", obj("SystemDrive").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  items.Add(New ListViewItem({"Registration", ""})) '------------------------------------

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Organization") Then
                     If (obj("Organization") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Organization", obj("Organization").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "RegisteredUser") Then
                     If (obj("RegisteredUser") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Registered User", obj("RegisteredUser").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SerialNumber") Then
                     If (obj("SerialNumber") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Serial Number", obj("SerialNumber").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  items.Add(New ListViewItem({"Memory", ""})) '------------------------------------------

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "TotalVisibleMemorySize") Then
                     If (obj("TotalVisibleMemorySize") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Total Visible Memory Size", DynamicFormatBytes(obj("TotalVisibleMemorySize").ToString() * 1024)}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "FreePhysicalMemory") Then
                     If (obj("FreePhysicalMemory") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Free Physical Memory", DynamicFormatBytes(obj("FreePhysicalMemory").ToString() * 1024)}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "TotalVirtualMemorySize") Then
                     If (obj("TotalVirtualMemorySize") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Total Virtual Memory Size", DynamicFormatBytes(obj("TotalVirtualMemorySize").ToString() * 1024)}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "FreeVirtualMemory") Then
                     If (obj("FreeVirtualMemory") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Free Virtual Memory", DynamicFormatBytes(obj("FreeVirtualMemory").ToString() * 1024)}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "TotalSwapSpaceSize") Then
                     If (obj("TotalSwapSpaceSize") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Total Swap Space Size", DynamicFormatBytes(obj("TotalSwapSpaceSize").ToString() * 1024)}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "FreeSpaceInPagingFiles") Then
                     If (obj("FreeSpaceInPagingFiles") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Free Space In Paging Files", DynamicFormatBytes(obj("FreeSpaceInPagingFiles").ToString() * 1024)}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  items.Add(New ListViewItem({"Features", " "})) '---------------------------------------

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "DataExecutionPrevention_Available") Then
                     If (obj("DataExecutionPrevention_Available") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Data Execution Prevention Available", obj("DataExecutionPrevention_Available").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "DataExecutionPrevention_32BitApplications") Then
                     If (obj("DataExecutionPrevention_32BitApplications") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Data Execution Prevention 32Bit Applications", obj("DataExecutionPrevention_32BitApplications").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "DataExecutionPrevention_Drivers") Then
                     If (obj("DataExecutionPrevention_Drivers") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Data Execution Prevention Drivers", obj("DataExecutionPrevention_Drivers").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "DataExecutionPrevention_SupportPolicy") Then
                     If (obj("DataExecutionPrevention_SupportPolicy") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Data Execution Prevention Support Policy", obj("DataExecutionPrevention_SupportPolicy").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "PAEEnabled") Then
                     If (obj("PAEEnabled") IsNot Nothing) Then
                        items.Add(New ListViewItem({"PAE Enabled", obj("PAEEnabled").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ForegroundApplicationBoost") Then
                     If (obj("ForegroundApplicationBoost") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Foreground Application Boost", obj("ForegroundApplicationBoost").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "MaxNumberOfProcesses") Then
                     If (obj("MaxNumberOfProcesses") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Max Number Of Processes", DynamicFormatBytes(obj("MaxNumberOfProcesses").ToString())}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "MaxProcessMemorySize") Then
                     If (obj("MaxProcessMemorySize") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Max Process Memory Size", DynamicFormatBytes(obj("MaxProcessMemorySize").ToString())}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  items.Add(New ListViewItem({"Regional settings", " "})) '---------------------------------------

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "CurrentTimeZone") Then
                     If (obj("CurrentTimeZone") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Current Time Zone", obj("CurrentTimeZone").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "LocalDateTime") Then
                     If (obj("LocalDateTime") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Local Date Time", ConvertToDate(obj("LocalDateTime").ToString())}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Locale") Then
                     If (obj("Locale") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Locale", obj("Locale").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "OSLanguage") Then
                     If (obj("OSLanguage") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Language", obj("OSLanguage").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "CodeSet") Then
                     If (obj("CodeSet") IsNot Nothing) Then
                        items.Add(New ListViewItem({"CodeSet", obj("CodeSet").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "CountryCode") Then
                     If (obj("CountryCode") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Country Code", obj("CountryCode").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  items.Add(New ListViewItem({" ", " "})) '----------------------------------------------

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "CSDVersion") Then
                     If (obj("CSDVersion") IsNot Nothing) Then
                        items.Add(New ListViewItem({"CSDVersion", obj("CSDVersion").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "LastBootUpTime") Then
                     If (obj("LastBootUpTime") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Last BootUp Time", ConvertToDate(obj("LastBootUpTime").ToString())}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Debug") Then
                     If (obj("Debug") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Debug", obj("Debug").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Status") Then
                     If (obj("Status") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Status", obj("Status").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Name") Then
                     If (obj("Name") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Name", obj("Name").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Distributed") Then
                     If (obj("Distributed") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Distributed", obj("Distributed").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "EncryptionLevel") Then
                     If (obj("EncryptionLevel") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Encryption Level", obj("EncryptionLevel").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "LargeSystemCache") Then
                     If (obj("LargeSystemCache") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Large System Cache", obj("LargeSystemCache").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "MUILanguages") Then
                     If (obj("MUILanguages") IsNot Nothing) Then
                        items.Add(New ListViewItem({"MUILanguages", obj("MUILanguages").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "NumberOfLicensedUsers") Then
                     If (obj("NumberOfLicensedUsers") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Number Of Licensed Users", obj("NumberOfLicensedUsers").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "NumberOfProcesses") Then
                     If (obj("NumberOfProcesses") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Number Of Processes", obj("NumberOfProcesses").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "NumberOfUsers") Then
                     If (obj("NumberOfUsers") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Number Of Users", obj("NumberOfUsers").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "OperatingSystemSKU") Then
                     If (obj("OperatingSystemSKU") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Operating System SKU", obj("OperatingSystemSKU").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "OtherTypeDescription") Then
                     If (obj("OtherTypeDescription") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Other Type Description", obj("OtherTypeDescription").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "PlusProductID") Then
                     If (obj("PlusProductID") IsNot Nothing) Then
                        items.Add(New ListViewItem({"PlusProductID", obj("PlusProductID").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "PlusVersionNumber") Then
                     If (obj("PlusVersionNumber") IsNot Nothing) Then
                        items.Add(New ListViewItem({"PlusVersionNumber", obj("PlusVersionNumber").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "PortableOperatingSystem") Then
                     If (obj("PortableOperatingSystem") IsNot Nothing) Then
                        items.Add(New ListViewItem({"PortableOperatingSystem", obj("PortableOperatingSystem").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Primary") Then
                     If (obj("Primary") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Primary", obj("Primary").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ProductType") Then
                     If (obj("ProductType") IsNot Nothing) Then
                        items.Add(New ListViewItem({"ProductType", obj("ProductType").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SizeStoredInPagingFiles") Then
                     If (obj("SizeStoredInPagingFiles") IsNot Nothing) Then
                        items.Add(New ListViewItem({"SizeStoredInPagingFiles", obj("SizeStoredInPagingFiles").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Scope") Then
                     If (obj("Scope") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Scope", obj("Scope").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Path") Then
                     If (obj("Path") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Path", obj("Path").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Options") Then
                     If (obj("Options") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Options", obj("Options").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Site") Then
                     If (obj("Site") IsNot Nothing) Then
                        items.Add(New ListViewItem({"Site", obj("Site").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "QuantumLength") Then
                     If (obj("QuantumLength") IsNot Nothing) Then
                        items.Add(New ListViewItem({"QuantumLength", obj("QuantumLength").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "QuantumType") Then
                     If (obj("QuantumType") IsNot Nothing) Then
                        items.Add(New ListViewItem({"QuantumType", obj("QuantumType").ToString()}))
                     End If
                  End If
                  MainForm.SetProgressValue(crtAction) : crtAction += 1

                  ' CreationClassName
                  MainForm.SetProgressValue(crtAction) : crtAction += 1
                  ' CSCreationClassName
                  MainForm.SetProgressValue(crtAction) : crtAction += 1
                  ' Name
                  MainForm.SetProgressValue(crtAction) : crtAction += 1
               Next
            End Using

            Dim myQueryProd As New ObjectQuery("SELECT * FROM Win32_ComputerSystemProduct")
            Using searcherProd As New ManagementObjectSearcher(scope, myQueryProd)

               For Each objProd As ManagementObject In searcherProd.Get()
                  If objProd.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Caption") Then
                     If (objProd("Caption") IsNot Nothing) Then
                        itemsProd.Add(New ListViewItem({"OS", objProd("Caption").ToString()}))
                     End If
                  End If
               Next
            End Using
         Catch ex As Exception
            MsgBox(ex.Message)
         End Try
         ' Pass the result to the RunWorkerCompleted event
         'e.Result = items
         e.Result = Tuple.Create(items, itemsProd)
      End If
   End Sub

   ' Update ListView when background work is completed
   <SupportedOSPlatform("windows")>
   Private Sub BackgroundWorker_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)
      If e.Error IsNot Nothing Then
         MessageBox.Show("Error: " & e.Error.Message, "BackgroundWorker Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
         Return
      End If

      If e.Cancelled Then
         MessageBox.Show("Operation was cancelled.", "BackgroundWorker Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
         Return
      End If

      ' result is valid
      If e.Result IsNot Nothing Then
         Dim resultTuple = CType(e.Result, Tuple(Of List(Of ListViewItem), List(Of ListViewItem)))
         Dim items = resultTuple.Item1
         Dim itemsProd = resultTuple.Item2

         If lvOS.InvokeRequired Then
            lvOS.Invoke(New Action(Of List(Of ListViewItem))(AddressOf UpdateListView), items)
         Else
            UpdateListView(items)
         End If

         lvOS.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)

         If lvOSProd.InvokeRequired Then
            lvOSProd.Invoke(New Action(Of List(Of ListViewItem))(AddressOf UpdateListViewProd), itemsProd)
         Else
            UpdateListViewProd(itemsProd)
         End If

         lvOSProd.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
      End If
   End Sub

   ' Update the ListView with the retrieved items
   <SupportedOSPlatform("windows")>
   Private Sub UpdateListView(items As List(Of ListViewItem))
      lvOS.Items.Clear()
      For Each item As ListViewItem In items
         If String.IsNullOrWhiteSpace(item.SubItems(1).Text) Then
            item.BackColor = Color.LightGray
            item.Font = New Font(item.Font, FontStyle.Bold)
         End If
      Next
      lvOS.Items.AddRange(items.ToArray())
   End Sub

   <SupportedOSPlatform("windows")>
   Private Sub UpdateListViewProd(items As List(Of ListViewItem))
      lvOSProd.Items.Clear()
      lvOSProd.Items.AddRange(items.ToArray())
   End Sub

   Public Function DynamicFormatBytes(ByVal lngFileSize As Long) As String

      Dim x As Integer : x = 0
      Dim Suffix As String : Suffix = ""
      Dim Result As Single : Result = lngFileSize

      Do Until Int(Result) < 1024
         x = x + 1
         Result = Result / 1024
      Loop
      Result = Math.Round(Result, 2)
      Select Case x
         Case 0
            Suffix = "Bytes"
         Case 1 'KiloBytes
            Suffix = "KB"
         Case 2 'MegaBytes
            Suffix = "MB"
         Case 3 'GigaBytes
            Suffix = "GB"
         Case 4 'TeraBytes
            Suffix = "TB"
         Case 5 'PetaBytes
            Suffix = "PB"
         Case 6 'ExaBytes
            Suffix = "EB"
         Case 7 'ZettaBytes
            Suffix = "ZB"
         Case 8 'YottaBytes
            Suffix = "YB"
         Case Else
            Suffix = "Too big to compute :)"
      End Select
      DynamicFormatBytes = Format(Result, "#,##0.00") & " " & Suffix
   End Function

   Public Function ConvertToDate(wmiDate As Object) As String
      If wmiDate IsNot Nothing Then
         Dim strDate As String = wmiDate.ToString()
         Return DateTime.ParseExact(strDate.Substring(0, 14), "yyyyMMddHHmmss", Nothing).ToString("yyyy-MM-dd HH:mm:ss")
      End If
      Return "N/A"
   End Function

   Public Function FormatBytes(ByVal bytesSize As Long) As String
      Dim suffix As String : suffix = ""
      Dim result As Long : result = bytesSize
      If bytesSize <= 1024 Then
         suffix = " bytes"
      ElseIf bytesSize <= 1048576 Then
         result = result / 1024
         suffix = " KB"
      ElseIf bytesSize <= 1073741824 Then
         result = result / 1048576
         suffix = " MB"
      ElseIf bytesSize <= 1099511627776 Then
         result = result / 1073741824
         suffix = " GB"
      End If
      FormatBytes = Format(result, "#,##0.00") & suffix
   End Function

End Class