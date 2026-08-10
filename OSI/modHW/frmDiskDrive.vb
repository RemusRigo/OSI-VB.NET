'--------------------------------------------------------------------------------------------------
' Win32_DiskDrive class
' https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-diskdrive
'
'    © Remus Rigo
'       v1.0.20260810
'--------------------------------------------------------------------------------------------------

Imports SharedInterfaces
Imports System.ComponentModel
Imports System.Management

Public Class frmDiskDrive
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
   Private Sub BackgroundWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
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

         Dim myQuery As New ObjectQuery("SELECT * FROM Win32_DiskDrive")
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

         For Each obj As ManagementObject In searcher.Get()
            cnt += 1
            Dim groupName As String = "DiskDrive #" & cnt

            ' Info --------------------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "Info", .Value = ""}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Caption") Then
               If (obj("Caption") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Caption", .Value = obj("Caption").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Name") Then
               If (obj("Name") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Name", .Value = obj("Name").ToString(), .ImageIndex = 0}})
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

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Model") Then
               If (obj("Model") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Model", .Value = obj("Model").ToString(), .ImageIndex = 0}})
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

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SystemName") Then
               If (obj("SystemName") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "System Name", .Value = obj("SystemName").ToString(), .ImageIndex = 0}})
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

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "FirmwareRevision") Then
               If (obj("FirmwareRevision") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Firmware Revision", .Value = obj("FirmwareRevision").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            ' Details --------------------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "Details", .Value = ""}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "DeviceID") Then
               If (obj("DeviceID") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Device ID", .Value = obj("DeviceID").ToString(), .ImageIndex = 0}})
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

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Description") Then
               If (obj("Description") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Description", .Value = obj("Description").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "MediaType") Then
               If (obj("MediaType") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Media Type", .Value = obj("MediaType").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "InterfaceType") Then
               If (obj("InterfaceType") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Interface Type", .Value = obj("InterfaceType").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SCSILogicalUnit") Then
               If (obj("SCSILogicalUnit") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "SCSI Logical Unit", .Value = obj("SCSILogicalUnit").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SCSITargetId") Then
               If (obj("SCSITargetId") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "SCSI Target Id", .Value = obj("SCSITargetId").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SCSIPort") Then
               If (obj("SCSIPort") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "SCSI Port", .Value = obj("SCSIPort").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SCSIBus") Then
               If (obj("SCSIBus") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "SCSI Bus", .Value = obj("SCSIBus").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "MediaLoaded") Then
               If (obj("MediaLoaded") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Media Loaded", .Value = obj("MediaLoaded").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Partitions") Then
               If (obj("Partitions") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Partitions", .Value = obj("Partitions").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Size") Then
               If (obj("Size") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Size", .Value = DynamicFormatBytes(obj("Size")), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "TotalCylinders") Then
               If (obj("TotalCylinders") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Total Cylinders", .Value = obj("TotalCylinders").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "TotalHeads") Then
               If (obj("TotalHeads") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Total Heads", .Value = obj("TotalHeads").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "TotalTracks") Then
               If (obj("TotalTracks") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Total Tracks", .Value = obj("TotalTracks").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "TracksPerCylinder") Then
               If (obj("TracksPerCylinder") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Tracks Per Cylinder", .Value = obj("TracksPerCylinder").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "TotalSectors") Then
               If (obj("TotalSectors") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Total Sectors", .Value = obj("TotalSectors").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "SectorsPerTrack") Then
               If (obj("SectorsPerTrack") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Sectors Per Track", .Value = obj("SectorsPerTrack").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "BytesPerSector") Then
               If (obj("BytesPerSector") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Bytes Per Sector", .Value = obj("BytesPerSector").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "DefaultBlockSize") Then
               If (obj("DefaultBlockSize") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Default Block Size", .Value = obj("DefaultBlockSize").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "MinBlockSize") Then
               If (obj("MinBlockSize") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Min Block Size", .Value = obj("MinBlockSize").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "MaxBlockSize") Then
               If (obj("MaxBlockSize") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Max Block Size", .Value = obj("MaxBlockSize").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "MaxMediaSize") Then
               If (obj("MaxMediaSize") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Max Media Size", .Value = obj("MaxMediaSize").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            ' Error -------------------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "Error", .Value = ""}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Status") Then
               If (obj("Status") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Status", .Value = obj("Status").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "StatusInfo") Then
               If (obj("StatusInfo") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Status Info", .Value = obj("StatusInfo").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Availability") Then
               If (obj("Availability") IsNot Nothing) Then
                  Dim tmp As String = ""
                  Select Case obj("availability").ToString
                     Case 1 : tmp = "other"
                     Case 2 : tmp = "unknown"
                     Case 3 : tmp = "running/Full Power"
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
                     Case 21 : tmp = "Quiesced"
                  End Select
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Availability", .Value = tmp, .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ErrorDescription") Then
               If (obj("ErrorDescription") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Error Description", .Value = obj("ErrorDescription").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ErrorMethodology") Then
               If (obj("ErrorMethodology") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Error Methodology", .Value = obj("ErrorMethodology").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ErrorCleared") Then
               If (obj("ErrorCleared") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Error Cleared", .Value = obj("ErrorCleared").ToString(), .ImageIndex = 0}})
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

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ConfigManagerErrorCode") Then
               If (obj("ConfigManagerErrorCode") IsNot Nothing) Then
                  Dim tmp As String = ""
                  Select Case obj("ConfigManagerErrorCode")
                     Case 0 : tmp = "This device Is working properly."
                     Case 1 : tmp = "This device Is Not configured correctly."
                     Case 2 : tmp = "Windows cannot load the driver For this device."
                     Case 3 : tmp = "The driver For this device might be corrupted, Or your system may be running low On memory Or other resources."
                     Case 4 : tmp = "This device Is Not working properly. One Of its drivers Or the registry might be corrupted."
                     Case 5 : tmp = "The driver For this device needs a resource that Windows cannot manage."
                     Case 6 : tmp = "The boot configuration For this device conflicts With other devices."
                     Case 7 : tmp = "Cannot filter."
                     Case 8 : tmp = "The driver loader For the device Is missing."
                     Case 9 : tmp = "This device Is Not working properly because the controlling firmware Is reporting the resources For the device incorrectly."
                     Case 10 : tmp = "This device cannot start."
                     Case 11 : tmp = "This device failed."
                     Case 12 : tmp = "This device cannot find enough free resources that it can use."
                     Case 13 : tmp = "Windows cannot verify this device's resources."
                     Case 14 : tmp = "This device cannot work properly until you restart your computer."
                     Case 15 : tmp = "This device is not working properly because there is probably a re-enumeration problem."
                     Case 16 : tmp = "Windows cannot identify all the resources this device uses."
                     Case 17 : tmp = "This device is asking for an unknown resource type"
                     Case 18 : tmp = "Reinstall the drivers for this device"
                     Case 19 : tmp = "Failure using the VxD loader"
                     Case 20 : tmp = "Your registry might be corrupted"
                     Case 21 : tmp = "System failure: Try changing the driver for this device. If that does not work, see your hardware documentation. Windows is removing this device"
                     Case 22 : tmp = "This device is disabled"
                     Case 23 : tmp = "System failure: Try changing the driver for this device. If that doesn't work, see your hardware documentation."
                     Case 24 : tmp = "This device is not present, is not working properly, or does not have all its drivers installed"
                     Case 25 : tmp = "Windows is still setting up this device"
                     Case 26 : tmp = "Windows is still setting up this device"
                     Case 27 : tmp = "This device does not have valid log configuration"
                     Case 28 : tmp = "The drivers for this device are not installed"
                     Case 29 : tmp = "This device is disabled because the firmware of the device did not give it the required resources"
                     Case 30 : tmp = "This device is using an IRQ resource that another device is using"
                     Case 31 : tmp = "This device is not working properly because Windows cannot load the drivers required for this device"
                  End Select
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Config Manager Error Code", .Value = tmp, .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "ConfigManagerUserConfig") Then
               If (obj("ConfigManagerUserConfig") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                  .Group = groupName,
                     .Label = "Config Manager User Config",
                     .Value = "Device is " & If(obj("ConfigManagerUserConfig"), "", "NOT ") & "using a user-defined configuration",
                     .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            ' Capabilities ------------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "Capabilities", .Value = ""}})

            Dim cap = CType(obj("Capabilities"), UInt16())
            Dim capDesc = CType(obj("CapabilityDescriptions"), String())

            If cap IsNot Nothing AndAlso capDesc IsNot Nothing Then
               For i As Integer = 0 To cap.Length - 1
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Capability " & cap(i), .Value = capDesc(i), .ImageIndex = 0}})
               Next
            Else
               worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                  .Group = groupName, .Label = "Capability", .Value = "No capability data returned", .ImageIndex = 0}})
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 2 ' Capabilities + CapabilityDescriptions

            ' Power Management --------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "Power Management", .Value = ""}})

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "PowerManagementSupported") Then
               If (obj("PowerManagementSupported") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Power Management Supported", .Value = obj("PowerManagementSupported").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            Dim pmCaps = CType(obj("PowerManagementCapabilities"), UInt16())
            If pmCaps IsNot Nothing AndAlso pmCaps.Length > 0 Then
               For i As Integer = 0 To pmCaps.Length - 1
                  Dim tmp As String = ""
                  Select Case pmCaps(i)
                     Case 0 : tmp = "Unknown"
                     Case 1 : tmp = "Not Supported"
                     Case 2 : tmp = "Disabled"
                     Case 3 : tmp = "Enabled"
                     Case 4 : tmp = "Power Saving Modes Entered Automatically"
                     Case 5 : tmp = "Power State Settable"
                     Case 6 : tmp = "Power Cycling Supported"
                     Case 7 : tmp = "Timed Power-On Supported"
                     Case Else
                        tmp = "Vendor-specific / Undefined"
                  End Select
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Power Management Capability " & pmCaps(i), .Value = tmp, .ImageIndex = 0}})
               Next
            Else
               worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                  .Group = groupName, .Label = "Power Management Capability", .Value = "No Power Management Capability data returned", .ImageIndex = 0}})
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            ' Other -------------------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
               .Group = groupName, .Label = "Other", .Value = ""}})


            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "CompressionMethod") Then
               If (obj("CompressionMethod") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Compression Method", .Value = obj("CompressionMethod").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "NeedsCleaning") Then
               If (obj("NeedsCleaning") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Needs Cleaning", .Value = obj("NeedsCleaning").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "NumberOfMediaSupported") Then
               If (obj("NumberOfMediaSupported") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Number Of Media Supported", .Value = obj("NumberOfMediaSupported").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            If obj.Properties.Cast(Of PropertyData)().Any(Function(p) p.Name = "Signature") Then
               If (obj("Signature") IsNot Nothing) Then
                  worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.AppendItem, .Item = New ProcListItem With {
                     .Group = groupName, .Label = "Signature", .Value = obj("Signature").ToString(), .ImageIndex = 0}})
               End If
            End If
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1

            ' Excluded ----------------------------------------------------------------------------
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1 ' CreationClassName
            worker.ReportProgress(0, New ProgressInfo With {.Kind = ProgressKind.SetValue, .Value = crtAction}) : crtAction += 1 ' Index
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
      lvDiskDrive.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
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
         lvDiskDrive.Groups.Add(grp)
      End If

      Dim lvi As New ListViewItem(item.Label)
      lvi.SubItems.Add(item.Value)
      lvi.Group = grp
      lvi.ImageIndex = item.ImageIndex

      If String.IsNullOrWhiteSpace(item.Value) Then
         lvi.BackColor = Color.LightGray
         lvi.Font = New Font(lvi.Font, FontStyle.Bold)
      End If

      lvDiskDrive.Items.Add(lvi)
   End Sub

   '-----------------------------------------------------------------------------------------------
   ' BackgroundScan
   Private Sub BackgroundScan()
      lvDiskDrive.Items.Clear()
      lvDiskDrive.Groups.Clear()
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
   ' frmDiskDrive: OnLoad
   Private Sub frmDiskDrive_Load(sender As Object, e As EventArgs) Handles MyBase.Load
      lvDiskDrive.BackColor = Color.FromArgb(224, 234, 213)

      If MainForm IsNot Nothing Then
         If remoteHost <> "" Then
            MainForm.SetTitle("Remus Rigo OSI: DiskDrive v1.1 on " & remoteHost)
         Else
            MainForm.SetTitle("Remus Rigo OSI: DiskDrive v1.1")
         End If
      End If

      BackgroundScan()
   End Sub

End Class