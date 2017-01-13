Imports Microsoft.Http
Imports DotNetNuke.Common.Utilities

''' <summary>
''' Establishes a connection to the SSO server and handles all the services requests.
''' </summary>
''' <remarks>DNN user credentials (portalid, username, password) and the web location (URI) for /Interzoic-SSO/Service.svc are required.</remarks>
Partial Public Class Connection
    Implements IDisposable

#Region "Private Memebers"

    Private _client As HttpClient = New HttpClient()
    Private _credentials As Credentials = Nothing
    Private _ServiceURI As String = Null.NullString

#End Region

#Region "Private Properties"

    ''' <summary>
    ''' HttpClient who will be doing the actual connection to the SSO server.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private ReadOnly Property Client() As HttpClient
        Get
            Return _client
        End Get
    End Property

#End Region

#Region "Constructors and destructors"

    Private disposedValue As Boolean ' To detect redundant calls
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If Not Client Is Nothing Then
                    Client.Dispose()
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub

    ''' <summary>
    ''' This destructor ensures the 'dispose' of the HttpClient
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub Finalize()
        Dispose(False)
        MyBase.Finalize()
    End Sub

    ''' <summary>
    ''' You'll have set each property before you can use this class.
    ''' </summary>
    ''' <remarks>You'll have set each propery before you can use this class.</remarks>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' You'll have to set the credentials before you can use this class.
    ''' </summary>
    ''' <param name="serviceURI"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal serviceURI As String)
        _ServiceURI = serviceURI
    End Sub

    ''' <summary>
    ''' You'll have to set the credentials before you can use this class.
    ''' </summary>
    ''' <param name="credentials"></param>
    ''' <param name="serviceURI">Base URI of the web services on the target server. E.g. https://www.example.com/DesktopModules/Interzoic-SSO/Service.svc/</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal credentials As Credentials, ByVal serviceURI As String)
        _credentials = credentials
        _ServiceURI = serviceURI
    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Base URI for the SSO services (the location of "/Interzoic-SSO/Service.svc" in the target server).
    ''' </summary>
    ''' <value>Base URI for the SSO web services.</value>
    ''' <returns>Base URI for the SSO web services.</returns>
    ''' <remarks> E.g. https://www.example.com/DesktopModules/Interzoic-SSO/Service.svc/</remarks>
    Public Property ServiceURI() As String
        Get
            Return _ServiceURI
        End Get
        Set(ByVal Value As String)
            _ServiceURI = Value
        End Set
    End Property


    ''' <summary>
    ''' Encapsulates the user credentials utilized to connect to the SSO services.
    ''' </summary>
    ''' <value>Credentials object.</value>
    ''' <returns>Credentials object.</returns>
    ''' <remarks></remarks>
    Public Property Credentials() As Credentials
        Get
            Return _credentials
        End Get
        Set(ByVal Value As Credentials)
            _credentials = Value
        End Set
    End Property

#End Region

#Region "Private Methods"

    Private Sub VerifyPropertiesAreSet(ByVal credentials As Credentials)

        If Me.Client Is Nothing Then
            Throw New SSOClientException("The HTTPClient has not been set.")
        End If

        If Me.Credentials Is Nothing Then
            Throw New SSOClientException("The Credentials has not been set.")
        End If

        If Me.ServiceURI = Null.NullString Then
            Throw New SSOClientException("The ServiceURI has not been set.")
        End If

    End Sub

#End Region

End Class
