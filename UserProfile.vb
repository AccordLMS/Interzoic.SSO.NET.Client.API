Imports Microsoft.Http
Imports System.Net
Imports System.Runtime.Serialization
Imports Interzoic.SSO.Shared

Partial Public Class Connection

#Region "User Profile API"

    ''' <summary>
    ''' Get the user profile for DNN user.
    ''' </summary>
    ''' <param name="portalid">The target user's portal.</param>
    ''' <param name="username">The target user's username.</param>
    ''' <param name="statusCode">Will return the HTTP status code of the operation.</param>
    ''' <returns>A list of user profile properties with their values.</returns>
    ''' <remarks>statusCode is also a return value for this method.</remarks>
    Public Function UserProfileGet(ByVal portalid As Integer, ByVal username As String, ByRef statusCode As HttpStatusCode) As List(Of UserProfileInfoDataContract)
        Return UserProfileGet(Me.Credentials, portalid, username, statusCode)
    End Function

    ''' <summary>
    ''' Get the user profile for DNN user.
    ''' </summary>
    ''' <param name="credentials">The user's credentials to make the connection to the SSO server.</param>
    ''' <param name="portalid">The target user's portal.</param>
    ''' <param name="username">The target user's username.</param>
    ''' <param name="statusCode">Will return the HTTP status code of the operation.</param>
    ''' <returns>A list of user profile properties with their values.</returns>
    ''' <remarks>statusCode is also a return value for this method.</remarks>
    Public Function UserProfileGet(ByVal credentials As Credentials, ByVal portalid As Integer, ByVal username As String, ByRef statusCode As HttpStatusCode) As List(Of UserProfileInfoDataContract)
        Dim userProfile As List(Of UserProfileInfoDataContract) = Nothing

        VerifyPropertiesAreSet(credentials)

        Try

            Using request As New HttpRequestMessage()
                ConfigureRequest(request, credentials, "GET", String.Format("portal/{0}/users/{1}/profile", portalid, username))
                Using response As HttpResponseMessage = Client.Send(request)
                    ' Get The HTTP Status Code
                    statusCode = response.StatusCode
                    If statusCode = HttpStatusCode.OK Then
                        ' Get the data from the response body
                        userProfile = response.Content.ReadAsDataContract(Of List(Of UserProfileInfoDataContract))()
                    End If
                End Using
            End Using
            Return userProfile

        Catch ex As Exception
            Throw New SSOClientException("Error processing the UserProfileGet request: " & ex.Message, ex)
        End Try
    End Function


    ''' <summary>
    ''' Updates the user profile for DNN user.
    ''' </summary>
    ''' <param name="portalid">The target user's portal.</param>
    ''' <param name="username">The target user's username.</param>
    ''' <param name="profilePropertiesList">List of new or updated properties for the user's profile.</param>
    ''' <returns>The HTTP status code of the operation.</returns>
    ''' <remarks></remarks>
    Public Function UserProfileUpdate(ByVal portalid As Integer, ByVal username As String, ByVal profilePropertiesList As List(Of UserProfileInfoDataContract)) As HttpStatusCode
        Return UserProfileUpdate(Me.Credentials, portalid, username, profilePropertiesList)
    End Function

    ''' <summary>
    ''' Updates the user profile for DNN user.
    ''' </summary>
    ''' <param name="credentials">The user's credentials to make the connection to the SSO server.</param>
    ''' <param name="portalid">The target user's portal.</param>
    ''' <param name="username">The target user's username.</param>
    ''' <param name="profilePropertiesList">List of new or updated properties for the user's profile.</param>
    ''' <returns>The HTTP status code of the operation.</returns>
    ''' <remarks></remarks>
    Public Function UserProfileUpdate(ByVal credentials As Credentials, ByVal portalid As Integer, ByVal username As String, ByVal profilePropertiesList As List(Of UserProfileInfoDataContract)) As HttpStatusCode
        ' Create/Update the user's profile
        Dim statusCode As HttpStatusCode = HttpStatusCode.InternalServerError

        VerifyPropertiesAreSet(credentials)
        Try

            Using request As New HttpRequestMessage()
                Dim content As HttpContent = HttpContentExtensions.CreateDataContract(profilePropertiesList)
                ConfigureRequest(request, Credentials, "POST", String.Format("portal/{0}/users/{1}/profile", portalid, username), content)
                Using response As HttpResponseMessage = Client.Send(request)
                    statusCode = response.StatusCode
                End Using
            End Using
            Return statusCode

        Catch ex As Exception
            Throw New SSOClientException("Error processing the UserProfileUpdate request: " & ex.Message, ex)
        End Try
    End Function

#End Region

End Class
