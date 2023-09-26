Imports System.Numerics

Module GenerateRFCodes

   'customer     Merchant's customer id (the last 4 digits, do not includ the first 9)
    'identifier   Payment identifier (15 digits)
    'value        Optional value to generate check code
    'type         Default is "normal"

    Public Function CreateRF(ByVal customer As String, ByVal identifier As String, Optional ByVal value As Double = 0, Optional ByVal Type_tmp As String = "normal") As String
        identifier = System.Text.RegularExpressions.Regex.Replace(identifier, "[^0-9]", "")

        Dim checkDigit As Integer

        If value <> 0 Then
            Dim factors() As Integer = {1, 7, 3, 1, 7, 3, 1, 7, 3, 1, 7}
            Dim strNum As String = StrReverse((value * 100).ToString())
            Dim sum As Integer = 0

            For i As Integer = 0 To strNum.Length - 1
                sum += Integer.Parse(strNum(i).ToString()) * factors(0)
                Array.Copy(factors, 1, factors, 0, factors.Length - 1)
            Next

            checkDigit = sum Mod 8
        Else
            checkDigit = If(Type_tmp = "normal", 8, 9)
        End If

        If customer.Length > 4 Then
            customer = customer.Substring(customer.Length - 4)
        End If

        Dim X As String = "9" & customer.PadLeft(4, "0"c).Substring(0, 4) & checkDigit & identifier.PadLeft(15, "0"c).Substring(0, 15)
        Dim Y As BigInteger = BigInteger.Remainder(BigInteger.Parse(X & "271500"), 97)
        Dim CD As String = (98 - Y).ToString().PadLeft(2, "0"c)


        Return "RF" & CD & X
    End Function


End Module
    End Function
