Module Common

   Public Function DarkenColor(c As Color, percent As Integer) As Color
      Dim factor As Double = (100 - percent) / 100.0
      Return Color.FromArgb(c.A, CInt(c.R * factor), CInt(c.G * factor), CInt(c.B * factor))
   End Function

   '-----------------------------------------------------------------------------------------------
   ' DynamicFormatBytes
   ''' <summary>Convert bytes to KB/MB/GB/... </summary>
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

   ''' <summary>Decodes WMI/CIM/WBEM Status values (ReturnValue or CIM_Status). Handles both WBEM return codes (0–21) and WMI HRESULTs (0x800410xx). </summary>
   Public Function GetWMIStatus(code As Integer) As String
      Select Case code
      ' --- WBEM / CIM return codes (0–21) ---
         Case 0 : Return "Success"
         Case 1 : Return "Not supported"
         Case 2 : Return "Unknown failure"
         Case 3 : Return "Invalid parameter"
         Case 4 : Return "Invalid class"
         Case 5 : Return "Provider not available"
         Case 6 : Return "Out of memory"
         Case 7 : Return "Access denied"
         Case 8 : Return "Invalid operation"
         Case 9 : Return "Invalid query"
         Case 10 : Return "Invalid query type"
         Case 11 : Return "Provider not capable"
         Case 12 : Return "Class already exists"
         Case 13 : Return "Class conflict"
         Case 14 : Return "Invalid namespace"
         Case 15 : Return "Provider load failure"
         Case 16 : Return "Method not found"
         Case 17 : Return "Invalid method parameter"
         Case 18 : Return "System failure"
         Case 19 : Return "Out of disk space"
         Case 20 : Return "Shutdown in progress"
         Case 21 : Return "Request timeout"
      ' --- WMI HRESULTs (0x800410xx) ---
         Case &H80041001 : Return "WBEM_E_FAILED"
         Case &H80041002 : Return "WBEM_E_NOT_FOUND"
         Case &H80041003 : Return "WBEM_E_ACCESS_DENIED"
         Case &H80041004 : Return "WBEM_E_PROVIDER_FAILURE"
         Case &H80041005 : Return "WBEM_E_TYPE_MISMATCH"
         Case &H80041006 : Return "WBEM_E_OUT_OF_MEMORY"
         Case &H80041007 : Return "WBEM_E_INVALID_CONTEXT"
         Case &H80041008 : Return "WBEM_E_INVALID_PARAMETER"
         Case &H80041009 : Return "WBEM_E_NOT_AVAILABLE"
         Case &H8004100A : Return "WBEM_E_CRITICAL_ERROR"
         Case &H8004100B : Return "WBEM_E_INVALID_STREAM"
         Case &H8004100C : Return "WBEM_E_NOT_SUPPORTED"
         Case &H8004100D : Return "WBEM_E_INVALID_SUPERCLASS"
         Case &H8004100E : Return "WBEM_E_INVALID_NAMESPACE"
         Case &H8004100F : Return "WBEM_E_INVALID_OBJECT"
         Case &H80041010 : Return "WBEM_E_INVALID_CLASS"
         Case &H80041011 : Return "WBEM_E_PROVIDER_NOT_FOUND"
         Case &H80041012 : Return "WBEM_E_INVALID_PROVIDER_REGISTRATION"
         Case &H80041013 : Return "WBEM_E_PROVIDER_LOAD_FAILURE"
         Case &H80041014 : Return "WBEM_E_INITIALIZATION_FAILURE"
         Case &H80041015 : Return "WBEM_E_TRANSPORT_FAILURE"
         Case &H80041016 : Return "WBEM_E_INVALID_OPERATION"
         Case &H80041017 : Return "WBEM_E_ALREADY_EXISTS"
         Case &H80041018 : Return "WBEM_E_NO_SUCH_PROPERTY"
         Case &H80041019 : Return "WBEM_E_TYPE_MISMATCH"
         Case &H8004101A : Return "WBEM_E_OUT_OF_RANGE"
         Case &H8004101B : Return "WBEM_E_NULL_REFERENCE"
         Case &H8004101C : Return "WBEM_E_INVALID_PROPERTY"
         Case &H8004101D : Return "WBEM_E_CALL_CANCELLED"
         Case &H8004101E : Return "WBEM_E_SHUTTING_DOWN"
         Case &H8004101F : Return "WBEM_E_PROPAGATED_METHOD"
         Case &H80041020 : Return "WBEM_E_UNEXPECTED"
         Case &H80041021 : Return "WBEM_E_ILLEGAL_OPERATION"
         Case &H80041022 : Return "WBEM_E_CANNOT_BE_KEY"
         Case &H80041023 : Return "WBEM_E_INCOMPLETE_CLASS"
         Case &H80041024 : Return "WBEM_E_INVALID_SYNTAX"
         Case &H80041025 : Return "WBEM_E_NONDECORATED_OBJECT"
         Case &H80041026 : Return "WBEM_E_READ_ONLY"
         Case &H80041027 : Return "WBEM_E_PROVIDER_NOT_CAPABLE"
         Case &H80041028 : Return "WBEM_E_CLASS_HAS_CHILDREN"
         Case &H80041029 : Return "WBEM_E_CLASS_HAS_INSTANCES"
         Case &H8004102A : Return "WBEM_E_QUERY_NOT_IMPLEMENTED"
         Case &H8004102B : Return "WBEM_E_ILLEGAL_NULL"
         Case &H8004102C : Return "WBEM_E_INVALID_QUALIFIER"
         Case &H8004102D : Return "WBEM_E_INVALID_DUPLICATE"
         Case &H8004102E : Return "WBEM_E_INVALID_NAMESPACE"
         Case &H8004102F : Return "WBEM_E_INVALID_PROPERTY_TYPE"
         Case &H80041030 : Return "WBEM_E_VALUE_OUT_OF_RANGE"
         Case &H80041031 : Return "WBEM_E_CANNOT_BE_SINGLETON"
         Case &H80041032 : Return "WBEM_E_INVALID_CIM_TYPE"
         Case &H80041033 : Return "WBEM_E_INVALID_DELETE"
         Case &H80041034 : Return "WBEM_E_INVALID_ASSOCIATION"
         Case &H80041035 : Return "WBEM_E_INVALID_REFERENCE"
         Case &H80041036 : Return "WBEM_E_INVALID_DERIVATION"
         Case &H80041037 : Return "WBEM_E_INVALID_CLASS"
         Case &H80041038 : Return "WBEM_E_PROVIDER_NOT_FOUND"
         Case &H80041039 : Return "WBEM_E_INVALID_PROVIDER_REGISTRATION"
         Case &H8004103A : Return "WBEM_E_PROVIDER_LOAD_FAILURE"
         Case Else
            Return "Unknown / Vendor-specific WMI status"
      End Select

   End Function

   '-----------------------------------------------------------------------------------------------
   ' GetWMIAvailability
   Public Function GetWMIAvailability(code As Integer) As String
      Select Case code
         Case 1 : Return "Other"
         Case 2 : Return "Unknown"
         Case 3 : Return "Running/Full Power"
         Case 4 : Return "Warning"
         Case 5 : Return "In Test"
         Case 6 : Return "Not Applicable"
         Case 7 : Return "Power Off"
         Case 8 : Return "Off Line"
         Case 9 : Return "Off Duty"
         Case 10 : Return "Degraded"
         Case 11 : Return "Not Installed"
         Case 12 : Return "Install Error"
         Case 13 : Return "Power Save - Unknown"
         Case 14 : Return "Power Save - Low Power Mode"
         Case 15 : Return "Power Save - Standby"
         Case 16 : Return "Power Cycle"
         Case 17 : Return "Power Save - Warning"
         Case 18 : Return "Paused"
         Case 19 : Return "Not Ready"
         Case 20 : Return "Not Configured"
         Case 21 : Return "Quiesced"
         Case Else
            Return "Unknown / Vendor-specific error"
      End Select
   End Function

   '-----------------------------------------------------------------------------------------------
   ' GetWMIConfigManagerErrorCode
   ''' <summary>Decodes the PnP/Device Manager ConfigManagerErrorCode values </summary>
   Public Function GetWMIConfigManagerErrorCode(ErrorCode As Integer) As String
      Select Case ErrorCode
         Case 0 : Return "This device Is working properly."
         Case 1 : Return "This device Is Not configured correctly."
         Case 2 : Return "Windows cannot load the driver For this device."
         Case 3 : Return "The driver For this device might be corrupted, Or your system may be running low On memory Or other resources."
         Case 4 : Return "This device Is Not working properly. One Of its drivers Or the registry might be corrupted."
         Case 5 : Return "The driver For this device needs a resource that Windows cannot manage."
         Case 6 : Return "The boot configuration For this device conflicts With other devices."
         Case 7 : Return "Cannot filter."
         Case 8 : Return "The driver loader For the device Is missing."
         Case 9 : Return "This device Is Not working properly because the controlling firmware Is reporting the resources For the device incorrectly."
         Case 10 : Return "This device cannot start."
         Case 11 : Return "This device failed."
         Case 12 : Return "This device cannot find enough free resources that it can use."
         Case 13 : Return "Windows cannot verify this device's resources."
         Case 14 : Return "This device cannot work properly until you restart your computer."
         Case 15 : Return "This device is not working properly because there is probably a re-enumeration problem."
         Case 16 : Return "Windows cannot identify all the resources this device uses."
         Case 17 : Return "This device is asking for an unknown resource type"
         Case 18 : Return "Reinstall the drivers for this device"
         Case 19 : Return "Failure using the VxD loader"
         Case 20 : Return "Your registry might be corrupted"
         Case 21 : Return "System failure: Try changing the driver for this device. If that does not work, see your hardware documentation. Windows is removing this device"
         Case 22 : Return "This device is disabled"
         Case 23 : Return "System failure: Try changing the driver for this device. If that doesn't work, see your hardware documentation."
         Case 24 : Return "This device is not present, is not working properly, or does not have all its drivers installed"
         Case 25 : Return "Windows is still setting up this device"
         Case 26 : Return "Windows is still setting up this device"
         Case 27 : Return "This device does not have valid log configuration"
         Case 28 : Return "The drivers for this device are not installed"
         Case 29 : Return "This device is disabled because the firmware of the device did not give it the required resources"
         Case 30 : Return "This device is using an IRQ resource that another device is using"
         Case 31 : Return "This device is not working properly because Windows cannot load the drivers required for this device"
         Case Else
            Return "Unknown / Vendor-specific error"
      End Select
   End Function

   Public Function GetBIOSCharacteristic(code As UInt16) As String
      Select Case code
         Case 0 : Return "Reserved"
         Case 1 : Return "Reserved"
         Case 2 : Return "Unknown"
         Case 3 : Return "BIOS Characteristics Not Supported"
         Case 4 : Return "ISA is supported"
         Case 5 : Return "MCA is supported"
         Case 6 : Return "EISA is supported"
         Case 7 : Return "PCI is supported"
         Case 8 : Return "PC Card (PCMCIA) is supported"
         Case 9 : Return "PNP is supported"
         Case 10 : Return "APM is supported"
         Case 11 : Return "BIOS is Upgradeable (Flash)"
         Case 12 : Return "BIOS shadowing is allowed"
         Case 13 : Return "VL-VESA is supported"
         Case 14 : Return "ESCD support is available"
         Case 15 : Return "Boot from CD is supported"
         Case 16 : Return "Selectable Boot is supported"
         Case 17 : Return "BIOS ROM is socketed"
         Case 18 : Return "Boot From PC Card (PCMCIA) is supported"
         Case 19 : Return "EDD (Enhanced Disk Drive) Specification is supported"
         Case 20 : Return "Int 13h - Japanese Floppy for NEC 9800 1.2mb (3.5\, 1024 bytes/sector, 360 RPM) is supported"
         Case 21 : Return "Int 13h - Japanese Floppy for Toshiba 1.2mb (3.5\, 360 RPM) is supported"
         Case 22 : Return "Int 13h - 5.25\ / 360 KB Floppy Services are supported"
         Case 23 : Return "Int 13h - 5.25\ /1.2MB Floppy Services are supported"
         Case 24 : Return "Int 13h - 3.5\ / 720 KB Floppy Services are supported"
         Case 25 : Return "Int 13h - 3.5\ / 2.88 MB Floppy Services are supported"
         Case 26 : Return "Int 5h, Print Screen Service is supported"
         Case 27 : Return "Int 9h, 8042 Keyboard services are supported"
         Case 28 : Return "Int 14h, Serial Services are supported"
         Case 29 : Return "Int 17h, printer services are supported"
         Case 30 : Return "Int 10h, CGA/Mono Video Services are supported"
         Case 31 : Return "NEC PC-98"
         Case 32 : Return "ACPI supported"
         Case 33 : Return "USB Legacy is supported"
         Case 34 : Return "AGP is supported"
         Case 35 : Return "I2O boot is supported"
         Case 36 : Return "LS-120 boot is supported"
         Case 37 : Return "ATAPI ZIP Drive boot is supported"
         Case 38 : Return "1394 boot is supported"
         Case 39 : Return "Smart Battery supported"
         Case 40 : Return "BIOS Boot Specification supported"
         Case 41 : Return "Function key initiated network service boot supported"
         Case 42 : Return "Targeted content distribution enabled"
         Case 43 : Return "UEFI Specification supported"
         Case 44 : Return "Virtual machine supported"
         Case Else : Return "Reserved/Unassigned"
      End Select
   End Function

End Module
