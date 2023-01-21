Public Class ValuesExtracter
    Public Shared Function ExtartValue(values As String, needvauleindex As Integer) As Integer
        Dim valuecount As Integer = CountCharaters(values, ",") + 1
        Dim va(valuecount - 1) As Integer
        Dim str As String = values

        For l As Integer = 1 To valuecount
            If l = valuecount Then
                va(0) = str
            Else
                va(valuecount - l) = Replace(str.Substring(str.LastIndexOf(",")), ",", "")
                str = Replace(str, str.Substring(str.LastIndexOf(",")), "")
            End If
        Next
        Return va(needvauleindex)
    End Function
    Public Shared Function CountCharaters(ByVal TextSearched As String, ByVal character As String) As Integer
        Dim location As Integer = 0
        Dim occurances As Integer = 0
        Do
            location = TextSearched.IndexOf(character, location)
            If location <> -1 Then
                occurances += 1
                location += character.Length
            End If
        Loop Until location = -1
        Return occurances
    End Function

End Class
