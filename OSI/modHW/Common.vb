Module Common
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

End Module
