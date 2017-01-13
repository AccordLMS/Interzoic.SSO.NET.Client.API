Imports System.Collections
Imports System.Collections.Generic
Imports System.Runtime.Serialization
Imports Microsoft.Http

Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Common.Utilities
Imports System.Net
Imports System.Text

Imports Interzoic.SSO.Shared

Partial Public Class Connection

#Region "User API"

    ''' <summary>
    ''' Get a DNN user information.
    ''' </summary>
    ''' <param name="portalid">The target user's portal.</param>
    ''' <param name="username">The target user's username.</param>
    ''' <param name="statusCode">Will return the HTTP status code of the operation.</param>
    ''' <returns>The user's information.</returns>
    ''' <remarks>statusCode is also a return value for this method.</remarks>
    Public Function UserGet(ByVal portalid As Integer, ByVal username As String, ByRef statusCode As HttpStatusCode) As UserInfoDataContract
        Return UserGet(Me.Credentials, portalid, username, statusCode)
    End Function

    ''' <summary>
    ''' Get a DNN user information.
    ''' </summary>
    ''' <param name="credentials">The user's credentials to make the connection to the SSO server.</param>
    ''' <param name="portalid">The target user's portal.</param>
    ''' <param name="username">The target user's username.</param>
    ''' <param name="statusCode">Will return the HTTP status code of the operation.</param>
    ''' <returns>The user's information.</returns>
    ''' <remarks>statusCode is also a return value for this method.</remarks>
    Public Function UserGet(ByVal credentials As Credentials, ByVal portalid As Integer, ByVal username As String, ByRef statusCode As HttpStatusCode) As UserInfoDataContract
        Dim userInfo As UserInfoDataContract = Nothing

        VerifyPropertiesAreSet(credentials)

        Try
            Using request As New HttpRequestMessage()

                ConfigureRequest(request, Credentials, "GET", String.Format("portal/{0}/users/{1}", portalid, username))

                Using response As HttpResponseMessage = Client.Send(request)
                    ' Get The HTTP Status Code
                    statusCode = response.StatusCode
                    If statusCode = HttpStatusCode.OK Then
                        ' Get the data from the response body
                        userInfo = response.Content.ReadAsDataContract(Of UserInfoDataContract)()
                    End If
                End Using

            End Using

            Return userInfo

        Catch ex As Exception
            Throw New SSOClientException("Error processing the UserGet request: " & ex.Message, ex)
        End Try
    End Function


    ''' <summary>
    ''' Deletes a DNN user.
    ''' </summary>
    ''' <param name="portalid">The target user's portal.</param>
    ''' <param name="username">The target user's username.</param>
    ''' <returns>The HTTP status code of the operation.</returns>
    ''' <remarks>Typically you will call this method by means of the X-HTTP-Method-Override request header.</remarks>
    Public Function UserDelete(ByVal portalid As Integer, ByVal username As String) As HttpStatusCode
        Return UserDeleteInternal(Me.Credentials, portalid, username, False)
    End Function

    ''' <summary>
    ''' Removes (physically deletes) a DNN user.
    ''' </summary>
    ''' <param name="portalid">The target user's portal.</param>
    ''' <param name="username">The target user's username.</param>
    ''' <returns>The HTTP status code of the operation.</returns>
    ''' <remarks>Typically you will call this method by means of the X-HTTP-Method-Override request header.</remarks>
    Public Function UserRemove(ByVal portalid As Integer, ByVal username As String) As HttpStatusCode
        Return UserDeleteInternal(Me.Credentials, portalid, username, True)
    End Function

    ''' <summary>
    ''' Deletes a DNN user.
    ''' </summary>
    ''' <param name="credentials">The user's credentials to make the connection to the SSO server.</param>
    ''' <param name="portalid">The target user's portal.</param>
    ''' <param name="username">The target user's username.</param>
    ''' <returns>The HTTP status code of the operation.</returns>
    ''' <remarks>Typically you will call this method by means of the X-HTTP-Method-Override request header.</remarks>
    Public Function UserDelete(ByVal credentials As Credentials, ByVal portalid As Integer, ByVal username As String) As HttpStatusCode
        Return UserDeleteInternal(credentials, portalid, username, False)
    End Function


    Private Function UserDeleteInternal(ByVal cred As Credentials, ByVal portalid As Integer, ByVal username As String, ByVal isHardDelete As Boolean) As HttpStatusCode
        Dim statusCode As HttpStatusCode = HttpStatusCode.InternalServerError

        VerifyPropertiesAreSet(cred)
        Dim cmd As String
        If isHardDelete Then
            cmd = "UserRemove"
        Else
            cmd = "UserDelete"
        End If

        Try

            Using request As New HttpRequestMessage()

                If isHardDelete Then
                    ConfigureRequest(request, cred, "POST", "DELETE", String.Format("portal/{0}/users/{1}/remove", portalid, username))
                Else
                    ConfigureRequest(request, cred, "POST", "DELETE", String.Format("portal/{0}/users/{1}", portalid, username))
                End If

                Using response As HttpResponseMessage = Client.Send(request)
                    statusCode = response.StatusCode
                End Using

            End Using

            Return statusCode

        Catch ex As Exception
            Throw New SSOClientException(String.Format("Error processing the {0} request: {1}", cmd, ex.Message), ex)
        End Try
    End Function

    ''' <summary>
    ''' Creates or Updates a DNN user.
    ''' </summary>
    ''' <param name="portalid">The target user's portal.</param>
    ''' <param name="username">The target user's username.</param>
    ''' <returns>The HTTP status code of the operation.</returns>
    ''' <remarks></remarks>
    Public Function UserUpdate(ByVal portalid As Integer, ByVal username As String, ByVal userInfoContract As UserInfoDataContract) As HttpStatusCode
        Return UserUpdate(Me.Credentials, portalid, username, userInfoContract)
    End Function

    ''' <summary>
    ''' Creates or Updates a DNN user.
    ''' </summary>
    ''' <param name="credentials">The user's credentials to make the connection to the SSO server.</param>
    ''' <param name="portalid">The target user's portal.</param>
    ''' <param name="username">The target user's username.</param>
    ''' <returns>The HTTP status code of the operation.</returns>
    ''' <remarks></remarks>
    Public Function UserUpdate(ByVal credentials As Credentials, ByVal portalid As Integer, ByVal username As String, ByVal userInfoContract As UserInfoDataContract) As HttpStatusCode
        ' Create/Update the user
        Dim statusCode As HttpStatusCode = HttpStatusCode.InternalServerError

        VerifyPropertiesAreSet(credentials)

        Try

            Using request As New HttpRequestMessage()

                Dim content As HttpContent = HttpContentExtensions.CreateDataContract(userInfoContract)
                ConfigureRequest(request, credentials, "POST", String.Format("portal/{0}/users/{1}", portalid, username), content)

                Using response As HttpResponseMessage = Client.Send(request)
                    statusCode = response.StatusCode
                End Using
            End Using

            Return statusCode

        Catch ex As Exception
            Throw New SSOClientException("Error processing the UserUpdate request: " & ex.Message, ex)
        End Try
    End Function

#End Region

End Class
