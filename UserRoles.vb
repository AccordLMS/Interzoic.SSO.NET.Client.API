Imports Microsoft.Http
Imports System.Net
Imports System.Runtime.Serialization
Imports Interzoic.SSO.Shared

Partial Public Class Connection

#Region "User Roles API"

    ''' <summary>
    ''' Get the list of roles for a DNN user.
    ''' </summary>
    ''' <param name="portalid">The target user's portal.</param>
    ''' <param name="username">The target user's username.</param>
    ''' <param name="statusCode">Will return the HTTP status code of the operation.</param>
    ''' <returns>Complete list of roles for a User.</returns>
    ''' <remarks>statusCode is also a return value for this method.</remarks>
    Public Function UserRolesGet(ByVal portalid As Integer, ByVal username As String, ByRef statusCode As HttpStatusCode) As List(Of UserRoleInfoDataContract)
        Return UserRolesGet(Me.Credentials, portalid, username, statusCode)
    End Function

    ''' <summary>
    ''' Get the list of roles for a DNN user.
    ''' </summary>
    ''' <param name="credentials">The user's credentials to make the connection to the SSO server.</param>
    ''' <param name="portalid">The target user's portal.</param>
    ''' <param name="username">The target user's username.</param>
    ''' <param name="statusCode">Will return the HTTP status code of the operation.</param>
    ''' <returns>Complete list of roles for a User.</returns>
    ''' <remarks>statusCode is also a return value for this method.</remarks>
    Public Function UserRolesGet(ByVal Credentials As Credentials, ByVal portalid As Integer, ByVal username As String, ByRef statusCode As HttpStatusCode) As List(Of UserRoleInfoDataContract)
        Dim userRolesInfoList As List(Of UserRoleInfoDataContract) = Nothing

        VerifyPropertiesAreSet(Credentials)

        Try

            Using request As New HttpRequestMessage()

                ConfigureRequest(request, Credentials, "GET", String.Format("portal/{0}/users/{1}/roles", portalid, username))

                Using response As HttpResponseMessage = Client.Send(request)
                    ' Get The HTTP Status Code
                    statusCode = response.StatusCode
                    If statusCode = HttpStatusCode.OK Then
                        ' Get the data from the response body
                        userRolesInfoList = response.Content.ReadAsDataContract(Of List(Of UserRoleInfoDataContract))()
                    End If
                End Using
            End Using
            Return userRolesInfoList

        Catch ex As Exception
            Throw New SSOClientException("Error processing the UserRolesGet request: " & ex.Message, ex)
        End Try
    End Function


    ''' <summary>
    ''' Get a given role for a DNN user.
    ''' </summary>
    ''' <param name="portalid">The target user's portal.</param>
    ''' <param name="username">The target user's username.</param>
    ''' <param name="rolename">The rolename from which to get the user's information.</param>
    ''' <param name="statusCode">Will return the HTTP status code of the operation.</param>
    ''' <returns>The user's role information.</returns>
    ''' <remarks>statusCode is also a return value for this method.</remarks>
    Public Function UserRoleGet(ByVal portalid As Integer, ByVal username As String, ByVal rolename As String, ByRef statusCode As HttpStatusCode) As UserRoleInfoDataContract
        Return UserRoleGet(Me.Credentials, portalid, username, rolename, statusCode)
    End Function

    ''' <summary>
    ''' Get a given role for a DNN user.
    ''' </summary>
    ''' <param name="credentials">The user's credentials to make the connection to the SSO server.</param>
    ''' <param name="portalid">The target user's portal.</param>
    ''' <param name="username">The target user's username.</param>
    ''' <param name="rolename">The rolename from which to get the user's information.</param>
    ''' <param name="statusCode">Will return the HTTP status code of the operation.</param>
    ''' <returns>The user's role information.</returns>
    ''' <remarks>statusCode is also a return value for this method.</remarks>
    Public Function UserRoleGet(ByVal Credentials As Credentials, ByVal portalid As Integer, ByVal username As String, ByVal rolename As String, ByRef statusCode As HttpStatusCode) As UserRoleInfoDataContract
        Dim userRoleInfo As UserRoleInfoDataContract = Nothing

        VerifyPropertiesAreSet(Credentials)

        Try
            Using request As New HttpRequestMessage()

                ConfigureRequest(request, Credentials, "GET", String.Format("portal/{0}/users/{1}/roles/{2}", portalid, username, rolename))

                Using response As HttpResponseMessage = Client.Send(request)
                    ' Get The HTTP Status Code
                    statusCode = response.StatusCode
                    If statusCode = HttpStatusCode.OK Then
                        ' Get the data from the response body
                        userRoleInfo = response.Content.ReadAsDataContract(Of UserRoleInfoDataContract)()
                    End If
                End Using
            End Using
            Return userRoleInfo

        Catch ex As Exception
            Throw New SSOClientException("Error processing the UserRoleGet request: " & ex.Message, ex)
        End Try

    End Function

    ''' <summary>
    ''' Add or update a role for a DNN user.
    ''' </summary>
    ''' <param name="portalid">The target user's portal.</param>
    ''' <param name="username">The target user's username.</param>
    ''' <param name="rolename">The rolename to add or update for user.</param>
    ''' <param name="userRoleInfoContract">The role information to add or update for the user.</param>
    ''' <returns>The HTTP status code of the operation.</returns>
    ''' <remarks></remarks>
    Public Function UserRoleUpdate(ByVal portalid As Integer, ByVal username As String, ByVal rolename As String, ByVal userRoleInfoContract As UserRoleInfoDataContract) As HttpStatusCode
        Return UserRoleUpdate(Me.Credentials, portalid, username, rolename, userRoleInfoContract)
    End Function

    ''' <summary>
    ''' Add or update a role for a DNN user.
    ''' </summary>
    ''' <param name="credentials">The user's credentials to make the connection to the SSO server.</param>
    ''' <param name="portalid">The target user's portal.</param>
    ''' <param name="username">The target user's username.</param>
    ''' <param name="rolename">The rolename to add or update for user.</param>
    ''' <param name="userRoleInfoContract">The role information to add or update for the user.</param>
    ''' <returns>The HTTP status code of the operation.</returns>
    ''' <remarks></remarks>
    Public Function UserRoleUpdate(ByVal Credentials As Credentials, ByVal portalid As Integer, ByVal username As String, ByVal rolename As String, ByVal userRoleInfoContract As UserRoleInfoDataContract) As HttpStatusCode
        ' Create/Update the user role
        Dim statusCode As HttpStatusCode = HttpStatusCode.InternalServerError

        VerifyPropertiesAreSet(Credentials)

        Try

            Using request As New HttpRequestMessage()

                Dim content As HttpContent = HttpContentExtensions.CreateDataContract(userRoleInfoContract)
                ConfigureRequest(request, Credentials, "POST", String.Format("portal/{0}/users/{1}/roles/{2}", portalid, username, rolename), content)

                Using response As HttpResponseMessage = Client.Send(request)
                    statusCode = response.StatusCode
                End Using

            End Using
            Return statusCode

        Catch ex As Exception
            Throw New SSOClientException("Error processing the UserRoleUpdate request: " & ex.Message, ex)
        End Try
    End Function


    ''' <summary>
    ''' Deletes a role for a DNN user.
    ''' </summary>
    ''' <param name="portalid">The target user's portal.</param>
    ''' <param name="username">The target user's username.</param>
    ''' <param name="rolename">The rolename to add or update for user.</param>
    ''' <returns>The HTTP status code of the operation.</returns>
    ''' <remarks></remarks>
    Public Function UserRoleDelete(ByVal portalid As Integer, ByVal username As String, ByVal rolename As String) As HttpStatusCode
        Return UserRoleDelete(Me.Credentials, portalid, username, rolename)
    End Function

    ''' <summary>
    ''' Deletes a role for a DNN user.
    ''' </summary>
    ''' <param name="credentials">The user's credentials to make the connection to the SSO server.</param>
    ''' <param name="portalid">The target user's portal.</param>
    ''' <param name="username">The target user's username.</param>
    ''' <param name="rolename">The rolename to add or update for user.</param>
    ''' <returns>The HTTP status code of the operation.</returns>
    ''' <remarks></remarks>
    Public Function UserRoleDelete(ByVal Credentials As Credentials, ByVal portalid As Integer, ByVal username As String, ByVal rolename As String) As HttpStatusCode
        Dim statusCode As HttpStatusCode

        Try
            Using request As New HttpRequestMessage()

                ConfigureRequest(request, Credentials, "POST", "DELETE", String.Format("portal/{0}/users/{1}/roles/{2}", portalid, username, rolename))

                Using response As HttpResponseMessage = Client.Send(request)
                    statusCode = response.StatusCode
                End Using

            End Using
            Return statusCode

        Catch ex As Exception
            Throw New SSOClientException("Error processing the UserRoleDelete request: " & ex.Message, ex)
        End Try
    End Function

#End Region

End Class
