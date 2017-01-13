Imports Microsoft.Http
Imports System.Net
Imports System.Runtime.Serialization
Imports Interzoic.SSO.Shared

Partial Public Class Connection

#Region "Single Sign On - API"

    ''' <summary>
    ''' For a given PortalID and Username, returns an integer specifying the current on-line status for the user.
    ''' </summary>
    ''' <param name="portalid">The target user's portal.</param>
    ''' <param name="username">The target user's username.</param>
    ''' <param name="statusCode">Will return the HTTP status code of the operation.</param>
    ''' <returns><para>The user's on-line status.</para>
    ''' <para>See <codeEntityReference qualifyHint="true">T:Interzoic.SSO.Shared.IsUserOnlineResults</codeEntityReference></para>
    ''' </returns>
    ''' <remarks>
    ''' <para>See <codeEntityReference qualifyHint="true">T:Interzoic.SSO.Shared.IsUserOnlineResults</codeEntityReference> for possible return values.</para>
    ''' <para>This method requires the <legacyBold>UsersOnline</legacyBold> feature enabled (<literal>Host > Host Settings > Advanced Settings > Other Settings > Enable Users Online?</literal>)</para>
    ''' <para>This feature relays on the <legacyBold>userIsOnlineTimeWindow</legacyBold> configured on the web.config file</para>
    '''</remarks>
    Public Function UserIsOnLine(ByVal portalid As Integer, ByVal username As String, ByRef statusCode As HttpStatusCode) As SSOConstants.IsUserOnlineResults

        Dim isOnline As SSOConstants.IsUserOnlineResults = SSOConstants.IsUserOnlineResults.UsersOnlineIsDisabled

        VerifyPropertiesAreSet(Credentials)

        Try
            Using request As New HttpRequestMessage()

                ConfigureRequest(request, Credentials, "GET", String.Format("portal/{0}/users/{1}/isonline", portalid, username))

                Using response As HttpResponseMessage = Client.Send(request)
                    ' Get The HTTP Status Code
                    statusCode = response.StatusCode
                    If statusCode = HttpStatusCode.OK Then
                        ' Get the data from the response body
                        Dim r As Integer = response.Content.ReadAsDataContract(Of Integer)()
                        isOnline = CType(r, SSOConstants.IsUserOnlineResults)
                    End If
                End Using

            End Using

            Return isOnline

        Catch ex As Exception
            Throw New SSOClientException("Error processing the IsUserOnline request: " & ex.Message, ex)
        End Try

    End Function

    ''' <summary>
    ''' For a given PortalID and Username, returns the temporary User's Login Token.
    ''' </summary>
    ''' <param name="portalid">The target user's portal.</param>
    ''' <param name="username">The target user's username.</param>
    ''' <param name="statusCode">Will return the HTTP status code of the operation.</param>
    ''' <returns>The SSO login token</returns>
    ''' <remarks>
    ''' <para><legacyBold>Workflow</legacyBold></para>
    ''' <list class="bullet">
    ''' <listItem><para>A remote “Client” application calls the SSO web service hosted on the “DNN/LMS” server to get a temporary Token.</para></listItem> 
    ''' <listItem><para>The Token is valid for a short period of time.  The duration is configured in the administrative UI for the web service on the DNN/LMS server.</para></listItem>
    ''' <listItem><para>The DNN Username AND the Token must be included in the Link/URL from the Client application when directing Users/learners to the DNN/LMS server.</para></listItem>
    ''' <listItem><para>The DNN User is automatically signed/logged in into the DNN/LMS site if the Token in the URL is still valid.</para></listItem>
    ''' </list> 
    '''</remarks>
    Public Function GetUserLoginToken(ByVal portalid As Integer, ByVal username As String, ByRef statusCode As HttpStatusCode) As String

        Dim token As String = String.Empty

        VerifyPropertiesAreSet(Credentials)

        Try
            Using request As New HttpRequestMessage()

                ConfigureRequest(request, Credentials, "GET", String.Format("portal/{0}/users/{1}/logintoken", portalid, username))

                Using response As HttpResponseMessage = Client.Send(request)
                    ' Get The HTTP Status Code
                    statusCode = response.StatusCode
                    If statusCode = HttpStatusCode.OK Then
                        ' Get the data from the response body
                        token = response.Content.ReadAsDataContract(Of String)() 'response.Content.ReadAsString()
                    End If
                End Using

            End Using

            Return token

        Catch ex As Exception
            Throw New SSOClientException("Error processing the GetUserLoginToken request: " & ex.Message, ex)
        End Try

    End Function

#End Region

End Class
