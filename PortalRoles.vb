Imports Microsoft.Http
Imports System.Net
Imports System.Runtime.Serialization
Imports Interzoic.SSO.Shared

Partial Public Class Connection

#Region "Portal Roles API"

    ''' <summary>
    ''' Get the list of roles found in a given portal.
    ''' </summary>
    ''' <param name="portalid">The target portal from which to get the roles list.</param>
    ''' <param name="statusCode">Will return the HTTP status code of the operation.</param>
    ''' <returns>List of roles found in a given portal.</returns>
    ''' <remarks>statusCode is also a return value for this method.</remarks>
    Public Function PortalRolesGet(ByVal portalid As Integer, ByRef statusCode As HttpStatusCode) As List(Of PortalRoleInfoDataContract)
        Return PortalRolesGet(Me.Credentials, portalid, statusCode)
    End Function

    ''' <summary>
    ''' Get the list of roles found in a given portal.
    ''' </summary>
    ''' <param name="credentials">The user's credentials to make the connection to the SSO server.</param>
    ''' <param name="portalid">The target portal from which to get the roles list.</param>
    ''' <param name="statusCode">Will return the HTTP status code of the operation.</param>
    ''' <returns>List of roles found in a given portal.</returns>
    ''' <remarks>statusCode is also a return value for this method.</remarks>
    Public Function PortalRolesGet(ByVal credentials As Credentials, ByVal portalid As Integer, ByRef statusCode As HttpStatusCode) As List(Of PortalRoleInfoDataContract)
        Dim portalRolesInfoList As List(Of PortalRoleInfoDataContract) = Nothing

        VerifyPropertiesAreSet(credentials)

        Try
            Using request As New HttpRequestMessage()
                ConfigureRequest(request, credentials, "GET", String.Format("portal/{0}/roles", portalid))
                Using response As HttpResponseMessage = Client.Send(request)
                    ' Get The HTTP Status Code
                    statusCode = response.StatusCode
                    If statusCode = HttpStatusCode.OK Then
                        ' Get the data from the response body
                        portalRolesInfoList = response.Content.ReadAsDataContract(Of List(Of PortalRoleInfoDataContract))()
                    End If
                End Using
            End Using
            Return portalRolesInfoList

        Catch ex As Exception
            Throw New SSOClientException("Error processing the PortalRolesGet request: " & ex.Message, ex)
        End Try
    End Function

#End Region

End Class
