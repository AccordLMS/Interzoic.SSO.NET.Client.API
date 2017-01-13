
''' <summary>
''' Any exception within this API will be returned as this Exception class.
''' </summary>
''' <remarks></remarks>
Public Class SSOClientException
    Inherits ApplicationException

    Sub New()
    End Sub

    Sub New(ByVal message As String)
        MyBase.New(message)
    End Sub

    Sub New(ByVal message As String, ByVal innerException As Exception)
        MyBase.New(message, innerException)
    End Sub

    Sub New(ByVal info As Runtime.Serialization.SerializationInfo, ByVal context As Runtime.Serialization.StreamingContext)
        MyBase.New(info, context)
    End Sub

End Class