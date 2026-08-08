'--------------------------------------------------------------------------------------------------
' Win32_DiskDrive class
' https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-diskdrive
'
'    © Remus Rigo
'       v1.0 2026-06-21
'--------------------------------------------------------------------------------------------------

Imports System.Collections.Concurrent
Imports System.ComponentModel
Imports System.Management
Imports System.Private
Imports System.Runtime.Intrinsics.Arm
Imports System.Security.Cryptography.Xml
Imports System.Threading
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Microsoft.VisualBasic.Logging
Imports SharedInterfaces

Public Class frmDiskDrive
   Implements IModuleForm
   <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
   Public Property MainForm As IMainForm Implements IModuleForm.MainForm
   Public remoteHost, remoteUser, remotePass As String

   Private Sub frmDiskDrive_Load(sender As Object, e As EventArgs) Handles MyBase.Load
      lvDiskDrive.View = View.Details
      lvDiskDrive.FullRowSelect = True
      lvDiskDrive.HeaderStyle = ColumnHeaderStyle.None
      lvDiskDrive.Columns.Add("Item", 100, HorizontalAlignment.Left)
      lvDiskDrive.Columns.Add("Value", 200, HorizontalAlignment.Left)

      lvDiskDrive.BackColor = Color.FromArgb(224, 234, 213)

      Dim backgroundWorker As New System.ComponentModel.BackgroundWorker()
      AddHandler backgroundWorker.DoWork, AddressOf BackgroundWorker_DoWork
      AddHandler backgroundWorker.RunWorkerCompleted, AddressOf BackgroundWorker_RunWorkerCompleted
      backgroundWorker.RunWorkerAsync()

      If MainForm IsNot Nothing Then
         If remoteHost <> "" Then
            MainForm.SetTitle("OSI: DiskDrive v1.0 on " & remoteHost & remoteHost & ChrW(&H2003) & ChrW(&H2003) & ChrW(&H2003) & " [Remus Rigo]")
         Else
            MainForm.SetTitle("OSI: DiskDrive v1.0 " & remoteHost & ChrW(&H2003) & ChrW(&H2003) & ChrW(&H2003) & " [Remus Rigo]")
         End If
      End If

   End Sub

   Private Sub BackgroundWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
      Dim myConnection As New ConnectionOptions()
      Dim scopePath As String
      Dim items As New List(Of ListViewItem)

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
         Dim myQuery As New ObjectQuery("SELECT * FROM Win32_DiskDrive")
         Dim searcher As New ManagementObjectSearcher(scope, myQuery)
         Dim cnt As Integer = 0
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
            cnt += 1
            Dim grpDD As New ListViewGroup("Disk Drive #" & cnt, HorizontalAlignment.Left)
            lvDiskDrive.Groups.Add(grpDD)

            Dim itmInfo As New ListViewItem("Info") '---------------------------------------------
            itmInfo.SubItems.Add("")
            itmInfo.Group = grpDD
            items.Add(itmInfo)

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Caption") Then
               If (obj("Caption") IsNot Nothing) Then
                  Dim item As New ListViewItem("Caption")
                  item.SubItems.Add(obj("Caption").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Name") Then
               If (obj("Name") IsNot Nothing) Then
                  Dim item As New ListViewItem("Name")
                  item.SubItems.Add(obj("Name").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Manufacturer") Then
               If (obj("Manufacturer") IsNot Nothing) Then
                  Dim item As New ListViewItem("Manufacturer")
                  item.SubItems.Add(obj("Manufacturer").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Model") Then
               If (obj("Model") IsNot Nothing) Then
                  Dim item As New ListViewItem("Model")
                  item.SubItems.Add(obj("Model").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SerialNumber") Then
               If (obj("SerialNumber") IsNot Nothing) Then
                  Dim item As New ListViewItem("Serial Number")
                  item.SubItems.Add(obj("SerialNumber").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SystemName") Then
               If (obj("SystemName") IsNot Nothing) Then
                  Dim item As New ListViewItem("System Name")
                  item.SubItems.Add(obj("SystemName").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "InstallDate") Then
               If (obj("InstallDate") IsNot Nothing) Then
                  Dim item As New ListViewItem("Install Date")
                  item.SubItems.Add(obj("InstallDate").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "FirmwareRevision") Then
               If (obj("FirmwareRevision") IsNot Nothing) Then
                  Dim item As New ListViewItem("Firmware Revision")
                  item.SubItems.Add(obj("FirmwareRevision").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            Dim itmDetails As New ListViewItem("Details") '---------------------------------------------
            itmDetails.SubItems.Add("")
            itmDetails.Group = grpDD
            items.Add(itmDetails)

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "DeviceID") Then
               If (obj("DeviceID") IsNot Nothing) Then
                  Dim item As New ListViewItem("Device ID")
                  item.SubItems.Add(obj("DeviceID").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "PNPDeviceID") Then
               If (obj("PNPDeviceID") IsNot Nothing) Then
                  Dim item As New ListViewItem("PNP Device ID")
                  item.SubItems.Add(obj("PNPDeviceID").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Description") Then
               If (obj("Description") IsNot Nothing) Then
                  Dim item As New ListViewItem("Description")
                  item.SubItems.Add(obj("Description").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "MediaType") Then
               If (obj("MediaType") IsNot Nothing) Then
                  Dim item As New ListViewItem("Media Type")
                  item.SubItems.Add(obj("MediaType").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "InterfaceType") Then
               If (obj("InterfaceType") IsNot Nothing) Then
                  Dim item As New ListViewItem("Interface Type")
                  item.SubItems.Add(obj("InterfaceType").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SCSILogicalUnit") Then
               If (obj("SCSILogicalUnit") IsNot Nothing) Then
                  Dim item As New ListViewItem("SCSI Logical Unit")
                  item.SubItems.Add(obj("SCSILogicalUnit").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SCSITargetId") Then
               If (obj("SCSITargetId") IsNot Nothing) Then
                  Dim item As New ListViewItem("SCSI Target Id")
                  item.SubItems.Add(obj("SCSITargetId").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SCSIPort") Then
               If (obj("SCSIPort") IsNot Nothing) Then
                  Dim item As New ListViewItem("SCSI Port")
                  item.SubItems.Add(obj("SCSIPort").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SCSIBus") Then
               If (obj("SCSIBus") IsNot Nothing) Then
                  Dim item As New ListViewItem("SCSI Bus")
                  item.SubItems.Add(obj("SCSIBus").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "MediaLoaded") Then
               If (obj("MediaLoaded") IsNot Nothing) Then
                  Dim item As New ListViewItem("Media Loaded")
                  item.SubItems.Add(obj("MediaLoaded").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Partitions") Then
               If (obj("Partitions") IsNot Nothing) Then
                  Dim item As New ListViewItem("Partitions")
                  item.SubItems.Add(obj("Partitions").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Size") Then
               If (obj("Size") IsNot Nothing) Then
                  Dim item As New ListViewItem("Size")
                  item.SubItems.Add(DynamicFormatBytes(obj("Size")))
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "TotalCylinders") Then
               If (obj("TotalCylinders") IsNot Nothing) Then
                  Dim item As New ListViewItem("Total Cylinders")
                  item.SubItems.Add(obj("TotalCylinders").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "TotalHeads") Then
               If (obj("TotalHeads") IsNot Nothing) Then
                  Dim item As New ListViewItem("Total Heads")
                  item.SubItems.Add(obj("TotalHeads").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "TotalTracks") Then
               If (obj("TotalTracks") IsNot Nothing) Then
                  Dim item As New ListViewItem("Total Tracks")
                  item.SubItems.Add(obj("TotalTracks").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "TracksPerCylinder") Then
               If (obj("TracksPerCylinder") IsNot Nothing) Then
                  Dim item As New ListViewItem("Tracks Per Cylinder")
                  item.SubItems.Add(obj("TracksPerCylinder").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "TotalSectors") Then
               If (obj("TotalSectors") IsNot Nothing) Then
                  Dim item As New ListViewItem("Total Sectors")
                  item.SubItems.Add(obj("TotalSectors").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SectorsPerTrack") Then
               If (obj("SectorsPerTrack") IsNot Nothing) Then
                  Dim item As New ListViewItem("Sectors Per Track")
                  item.SubItems.Add(obj("SectorsPerTrack").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "BytesPerSector") Then
               If (obj("BytesPerSector") IsNot Nothing) Then
                  Dim item As New ListViewItem("Bytes Per Sector")
                  item.SubItems.Add(obj("BytesPerSector").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "DefaultBlockSize") Then
               If (obj("DefaultBlockSize") IsNot Nothing) Then
                  Dim item As New ListViewItem("Default Block Size")
                  item.SubItems.Add(obj("DefaultBlockSize").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "MinBlockSize") Then
               If (obj("MinBlockSize") IsNot Nothing) Then
                  Dim item As New ListViewItem("Min Block Size")
                  item.SubItems.Add(obj("MinBlockSize").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "MaxBlockSize") Then
               If (obj("MaxBlockSize") IsNot Nothing) Then
                  Dim item As New ListViewItem("Max Block Size")
                  item.SubItems.Add(obj("MaxBlockSize").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "MaxMediaSize") Then
               If (obj("MaxMediaSize") IsNot Nothing) Then
                  Dim item As New ListViewItem("Max Media Size")
                  item.SubItems.Add(obj("MaxMediaSize").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1


            Dim itmError As New ListViewItem("Error") '---------------------------------------------
            itmError.SubItems.Add("")
            itmError.Group = grpDD
            items.Add(itmError)

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Status") Then
               If (obj("Status") IsNot Nothing) Then
                  Dim item As New ListViewItem("Status")
                  item.SubItems.Add(obj("Status").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "StatusInfo") Then
               If (obj("StatusInfo") IsNot Nothing) Then
                  Dim item As New ListViewItem("Status Info")
                  item.SubItems.Add(obj("StatusInfo").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Availability") Then
               If (obj("Availability") IsNot Nothing) Then
                  Dim item As New ListViewItem("Availability")
                  Select Case obj("Availability")
                     Case 1
                        item.SubItems.Add("Other")
                     Case 2
                        item.SubItems.Add("Unknown")
                     Case 3
                        item.SubItems.Add("Running/ Full Power")
                     Case 4
                        item.SubItems.Add("Warning")
                     Case 5
                        item.SubItems.Add("In Test")
                     Case 6
                        item.SubItems.Add("Not Applicable")
                     Case 7
                        item.SubItems.Add("Power Off")
                     Case 8
                        item.SubItems.Add("Off Line")
                     Case 9
                        item.SubItems.Add("Off Duty")
                     Case 10
                        item.SubItems.Add("Degraded")
                     Case 11
                        item.SubItems.Add("Not Installed")
                     Case 12
                        item.SubItems.Add("Install Error")
                     Case 13
                        item.SubItems.Add("Power Save - Unknown")
                     Case 14
                        item.SubItems.Add("Power Save - Low Power Mode")
                     Case 15
                        item.SubItems.Add("Power Save - Standby")
                     Case 16
                        item.SubItems.Add("Power Cycle")
                     Case 17
                        item.SubItems.Add("Power Save - Warning")
                     Case 18
                        item.SubItems.Add("Paused")
                     Case 19
                        item.SubItems.Add("Not Ready")
                     Case 20
                        item.SubItems.Add("Not Configured")
                     Case 21
                        item.SubItems.Add("Quiesced")
                  End Select
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ErrorDescription") Then
               If (obj("ErrorDescription") IsNot Nothing) Then
                  Dim item As New ListViewItem("Error Description")
                  item.SubItems.Add(obj("ErrorDescription").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ErrorMethodology") Then
               If (obj("ErrorMethodology") IsNot Nothing) Then
                  Dim item As New ListViewItem("Error Methodology")
                  item.SubItems.Add(obj("ErrorMethodology").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ErrorCleared") Then
               If (obj("ErrorCleared") IsNot Nothing) Then
                  Dim item As New ListViewItem("Error Cleared")
                  item.SubItems.Add(obj("ErrorCleared").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "LastErrorCode") Then
               If (obj("LastErrorCode") IsNot Nothing) Then
                  Dim item As New ListViewItem("Last Error Code")
                  item.SubItems.Add(obj("LastErrorCode").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ConfigManagerErrorCode") Then
               If (obj("ConfigManagerErrorCode") IsNot Nothing) Then
                  Dim item As New ListViewItem("Config Manager Error Code")
                  Select Case obj("ConfigManagerErrorCode")
                     Case 0
                        item.SubItems.Add("This device is working properly.")
                     Case 1
                        item.SubItems.Add("This device is not configured correctly.")
                     Case 2
                        item.SubItems.Add("Windows cannot load the driver for this device.")
                     Case 3
                        item.SubItems.Add("The driver for this device might be corrupted, or your system may be running low on memory or other resources.")
                     Case 4
                        item.SubItems.Add("This device is not working properly. One of its drivers or the registry might be corrupted.")
                     Case 5
                        item.SubItems.Add("The driver for this device needs a resource that Windows cannot manage.")
                     Case 6
                        item.SubItems.Add("The boot configuration for this device conflicts with other devices.")
                     Case 7
                        item.SubItems.Add("Cannot filter.")
                     Case 8
                        item.SubItems.Add("The driver loader for the device is missing.")
                     Case 9
                        item.SubItems.Add("This device is not working properly because the controlling firmware is reporting the resources for the device incorrectly.")
                     Case 10
                        item.SubItems.Add("This device cannot start.")
                     Case 11
                        item.SubItems.Add("This device failed.")
                     Case 12
                        item.SubItems.Add("This device cannot find enough free resources that it can use.")
                     Case 13
                        item.SubItems.Add("Windows cannot verify this device's resources.")
                     Case 14
                        item.SubItems.Add("This device cannot work properly until you restart your computer.")
                     Case 15
                        item.SubItems.Add("This device is not working properly because there is probably a re-enumeration problem.")
                     Case 16
                        item.SubItems.Add("Windows cannot identify all the resources this device uses.")
                     Case 17
                        item.SubItems.Add("This device is asking for an unknown resource type")
                     Case 18
                        item.SubItems.Add("Reinstall the drivers for this device")
                     Case 19
                        item.SubItems.Add("Failure using the VxD loader")
                     Case 20
                        item.SubItems.Add("Your registry might be corrupted")
                     Case 21
                        item.SubItems.Add("System failure: Try changing the driver for this device. If that does not work, see your hardware documentation. Windows is removing this device")
                     Case 22
                        item.SubItems.Add("This device is disabled")
                     Case 23
                        item.SubItems.Add("System failure: Try changing the driver for this device. If that doesn't work, see your hardware documentation.")
                     Case 24
                        item.SubItems.Add("This device is not present, is not working properly, or does not have all its drivers installed")
                     Case 25
                        item.SubItems.Add("Windows is still setting up this device")
                     Case 26
                        item.SubItems.Add("Windows is still setting up this device")
                     Case 27
                        item.SubItems.Add("This device does not have valid log configuration")
                     Case 28
                        item.SubItems.Add("The drivers for this device are not installed")
                     Case 29
                        item.SubItems.Add("This device is disabled because the firmware of the device did not give it the required resources")
                     Case 30
                        item.SubItems.Add("This device is using an IRQ resource that another device is using")
                     Case 31
                        item.SubItems.Add("This device is not working properly because Windows cannot load the drivers required for this device")
                  End Select
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ConfigManagerUserConfig") Then
               If (obj("ConfigManagerUserConfig") IsNot Nothing) Then
                  Dim item As New ListViewItem("Config Manager User Config")
                  If obj("ConfigManagerUserConfig") Then
                     item.SubItems.Add("device is using a user-defined configuration")
                  Else
                     item.SubItems.Add("device is not using a user-defined configuration")
                  End If
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1


            Dim itmCapabilities As New ListViewItem("Capabilities") '---------------------------------------------
            itmCapabilities.SubItems.Add("")
            itmCapabilities.Group = grpDD
            items.Add(itmCapabilities)

            Dim cap = CType(obj("Capabilities"), UInt16())
            Dim capDesc = CType(obj("CapabilityDescriptions"), String())

            If cap IsNot Nothing AndAlso capDesc IsNot Nothing Then
               For i As Integer = 0 To cap.Length - 1
                  Dim item As New ListViewItem($"  Capability {cap(i)}")
                  item.SubItems.Add($"{capDesc(i)}")
                  item.Group = grpDD
                  items.Add(item)
               Next
            Else
               Console.WriteLine("  No capability data returned.")
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 2 ' Capabilities + CapabilityDescriptions

            Dim itmPMCapabilities As New ListViewItem("Power Management Capabilities") '---------------------------------------------
            itmPMCapabilities.SubItems.Add("")
            itmPMCapabilities.Group = grpDD
            items.Add(itmPMCapabilities)

            Dim pmCaps = CType(obj("PowerManagementCapabilities"), UInt16())
            If pmCaps IsNot Nothing AndAlso pmCaps.Length > 0 Then
               For i As Integer = 0 To pmCaps.Length - 1
                  Dim item As New ListViewItem($"Power Management Capability {pmCaps(i)}")
                  Select Case pmCaps(i)
                     Case 0
                        item.SubItems.Add("Unknown")
                     Case 1
                        item.SubItems.Add("Not Supported")
                     Case 2
                        item.SubItems.Add("Disabled")
                     Case 3
                        item.SubItems.Add("Enabled")
                     Case 4
                        item.SubItems.Add("Power Saving Modes Entered Automatically")
                     Case 5
                        item.SubItems.Add("Power State Settable")
                     Case 6
                        item.SubItems.Add("Power Cycling Supported")
                     Case 7
                        item.SubItems.Add("Timed Power-On Supported")
                     Case Else
                        item.SubItems.Add("Vendor-specific / Undefined")
                  End Select
                  item.Group = grpDD
                  items.Add(item)
               Next
            Else
               Dim noItem As New ListViewItem(" ")
               noItem.SubItems.Add("No Power Management Capability data returned")
               noItem.Group = grpDD
               items.Add(noItem)
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1


            Dim itmOther As New ListViewItem("Other") '---------------------------------------------
            itmOther.SubItems.Add("")
            itmOther.Group = grpDD
            items.Add(itmOther)

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "CompressionMethod") Then
               If (obj("CompressionMethod") IsNot Nothing) Then
                  Dim item As New ListViewItem("Compression Method")
                  item.SubItems.Add(obj("CompressionMethod").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "NeedsCleaning") Then
               If (obj("NeedsCleaning") IsNot Nothing) Then
                  Dim item As New ListViewItem("Needs Cleaning")
                  item.SubItems.Add(obj("NeedsCleaning").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "NumberOfMediaSupported") Then
               If (obj("NumberOfMediaSupported") IsNot Nothing) Then
                  Dim item As New ListViewItem("Number Of Media Supported")
                  item.SubItems.Add(obj("NumberOfMediaSupported").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "PowerManagementSupported") Then
               If (obj("PowerManagementSupported") IsNot Nothing) Then
                  Dim item As New ListViewItem("Power Management Supported")
                  item.SubItems.Add(obj("PowerManagementSupported").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1


            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Signature") Then
               If (obj("Signature") IsNot Nothing) Then
                  Dim item As New ListViewItem("Signature")
                  item.SubItems.Add(obj("Signature").ToString())
                  item.Group = grpDD
                  items.Add(item)
               End If
            End If
            MainForm.SetProgressValue(crtAction) : crtAction += 1

            ' Excluded ----------------------------------------------------------------------------
            MainForm.SetProgressValue(crtAction) : crtAction += 1 ' CreationClassName
            MainForm.SetProgressValue(crtAction) : crtAction += 1 ' Index
            MainForm.SetProgressValue(crtAction) : crtAction += 1 ' SystemCreationClassName

         Next
      Catch ex As Exception
         MsgBox(ex.Message)
      End Try
      e.Result = items
   End Sub

   ' Update ListView when background work is completed
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
         Dim items As List(Of ListViewItem) = CType(e.Result, List(Of ListViewItem))

         ' Safely update the ListView on the UI thread
         If lvDiskDrive.InvokeRequired Then
            lvDiskDrive.Invoke(New Action(Of List(Of ListViewItem))(AddressOf UpdateListView), items)
         Else
            UpdateListView(items)
         End If

         ' Optional: Auto-resize columns for better display
         lvDiskDrive.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
      End If
   End Sub

   ' Update the ListView with the retrieved items
   Private Sub UpdateListView(items As List(Of ListViewItem))
      lvDiskDrive.Items.Clear()
      For Each item As ListViewItem In items
         If String.IsNullOrWhiteSpace(item.SubItems(1).Text) Then
            item.BackColor = Color.LightGray
            item.Font = New Font(item.Font, FontStyle.Bold)
         End If
      Next
      lvDiskDrive.Items.AddRange(items.ToArray())
   End Sub

End Class