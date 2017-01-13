
Imports DotNetNuke.Common.Utilities

''' <summary>
''' Holds the information (credentials) needed to make a connection to the DNN site where the Interzoic SSO services are installed.
''' i.e. PortalID + LOWERCASE(UserName) + UserPassword
''' </summary>
''' <remarks>Remember that the User's Password is case-sensitive.</remarks>
Public Class Credentials

#Region "Private Properties"

    Private _PortalId As Integer = Null.NullInteger
    Private _UserName As String = Null.NullString
    Private _UserPassword As String = Null.NullString

#End Region

#Region "Constructors"

    ''' <summary>
    ''' Default constructor.
    ''' </summary>
    ''' <remarks>You'll have set each propery before you can use this class.</remarks>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' This consctuctor includes all the required parameters.
    ''' </summary>
    ''' <param name="portalID">DNN portal id</param>
    ''' <param name="userName">DNN username</param>
    ''' <param name="userPassword">DNN user password</param>
    ''' <remarks>Remember that the password is case sensitive.</remarks>
    Public Sub New(ByVal portalID As Integer, _
                   ByVal userName As String, _
                   ByVal userPassword As String)
        _PortalId = portalID
        _UserName = userName
        _UserPassword = userPassword
    End Sub

#End Region


#Region "Public Properties"

    ''' <summary>
    ''' PortalID for the DNN user making the connection to the DNN site where the SSO services are running.
    ''' </summary>
    ''' <value>DNN user's PortalId</value>
    ''' <returns>DNN user's PortalId</returns>
    ''' <remarks>This data is regarding the user making the SSO service request.</remarks>
    Public Property PortalId() As Integer
        Get
            Return _PortalId
        End Get
        Set(ByVal Value As Integer)
            _PortalId = Value
        End Set
    End Property

    ''' <summary>
    ''' UserName for the DNN user making the connection to the DNN site where the SSO services are running.
    ''' </summary>
    ''' <value>DNN user's Username</value>
    ''' <returns>DNN user's Username</returns>
    ''' <remarks>This data is case insentitive.</remarks>
    Public Property UserName() As String
        Get
            Return _UserName
        End Get
        Set(ByVal Value As String)
            _UserName = Value
        End Set
    End Property

    ''' <summary>
    ''' Password for the DNN user making the connection to the DNN site where the SSO services are running.
    ''' </summary>
    ''' <value>DNN user's Password</value>
    ''' <returns>DNN user's Password</returns>
    ''' <remarks>This data is case-sensitive.</remarks>
    Public Property UserPassword() As String
        Get
            Return _UserPassword
        End Get
        Set(ByVal Value As String)
            _UserPassword = Value
        End Set
    End Property

    ''' <summary>
    ''' Build the string which will sign all SSO services requests.
    ''' </summary>
    ''' <value>String used to sign SSO services requests.</value>
    ''' <returns>String used to sign SSO services requests.</returns>
    ''' <remarks>PortalID + LOWERCASE(UserName) + UserPassword</remarks>
    Public ReadOnly Property SecretKey() As String
        Get
            Return _PortalId.ToString & _UserName.ToLower & _UserPassword
        End Get
    End Property

#End Region

End Class
