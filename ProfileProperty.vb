Imports Microsoft.Http
Imports System.Net
Imports System.Runtime.Serialization
Imports Interzoic.SSO.Shared
Imports DotNetNuke.Entities.Profile

Partial Public Class Connection

#Region "Profile Property API"

    ''' <summary>
    ''' Get a ProfilePropertyDefinition for a DNN profileName.
    ''' </summary>
    ''' <param name="portalid">The target portal.</param>
    ''' <param name="profileName">The target profileName.</param>
    ''' <param name="statusCode">Will return the HTTP status code of the operation.</param>
    ''' <returns>Profile Property definition.</returns>
    ''' <remarks>statusCode is also a return value for this method.</remarks>
    Public Function ProfilePropertyGet(ByVal portalid As Integer, ByVal profileName As String, ByRef statusCode As HttpStatusCode) As ProfilePropertyDefinition
        Dim profilePropertyDefinition As ProfilePropertyDefinition = Nothing

        VerifyPropertiesAreSet(Me.Credentials)

        Try

            Using request As New HttpRequestMessage()

                ConfigureRequest(request, Credentials, "GET", String.Format("portal/{0}/profileName/{1}", portalid, profileName))

                Using response As HttpResponseMessage = Client.Send(request)
                    ' Get The HTTP Status Code
                    statusCode = response.StatusCode
                    If statusCode = HttpStatusCode.OK Then
                        ' Get the data from the response body
                        profilePropertyDefinition = response.Content.ReadAsDataContract(Of ProfilePropertyDefinition)()
                    End If
                End Using
            End Using
            Return profilePropertyDefinition

        Catch ex As Exception
            Throw New SSOClientException("Error processing the ProfilePropertyGet request: " & ex.Message, ex)
        End Try
    End Function

    ''' <summary>
    ''' Delete a ProfilePropertyDefinition for a DNN profileName.
    ''' </summary>
    ''' <param name="portalid">The target portal.</param>
    ''' <param name="profileName">The target profileName.</param>
    ''' <returns>The HTTP status code of the operation.</returns>
    ''' <remarks></remarks>
    Public Function ProfilePropertyDelete(ByVal portalid As Integer, ByVal profileName As String) As HttpStatusCode
        Dim statusCode As HttpStatusCode = HttpStatusCode.InternalServerError

        VerifyPropertiesAreSet(Me.Credentials)
        Try

            Using request As New HttpRequestMessage()
                ConfigureRequest(request, Credentials, "DELETE", String.Format("portal/{0}/profileName/{1}", portalid, profileName))
                Using response As HttpResponseMessage = Client.Send(request)
                    statusCode = response.StatusCode
                End Using
            End Using
            Return statusCode

        Catch ex As Exception
            Throw New SSOClientException("Error processing the ProfilePropertyDelete request: " & ex.Message, ex)
        End Try
    End Function

    ''' <summary>
    ''' Create a ProfilePropertyDefinition for a DNN profileName.
    ''' </summary>
    ''' <param name="portalid">The target portal.</param>
    ''' <param name="profileName">The target profileName.</param>
    ''' <param name="length">The target length.</param>
    ''' <returns>The HTTP status code of the operation.</returns>
    ''' <remarks></remarks>
    Public Function ProfilePropertyAddWithType(ByVal portalId As Integer, ByVal profileName As String, ByVal profileType As String, ByVal length As String) As HttpStatusCode
        Dim statusCode As HttpStatusCode = HttpStatusCode.InternalServerError

        VerifyPropertiesAreSet(Me.Credentials)
        Try

            Using request As New HttpRequestMessage()
                ConfigureRequest(request, Credentials, "POST", String.Format("portal/{0}/profileName/{1}/profileType/{2}/length/{3}", portalId, profileName, profileType, length))
                Using response As HttpResponseMessage = Client.Send(request)
                    statusCode = response.StatusCode
                End Using
            End Using
            Return statusCode

        Catch ex As Exception
            Throw New SSOClientException("Error processing the ProfilePropertyAddWithType request: " & ex.Message, ex)
        End Try
    End Function

    ''' <summary>
    ''' Create a ProfilePropertyDefinition for a DNN profileName.
    ''' </summary>
    ''' <param name="portalid">The target portal.</param>
    ''' <param name="profileName">The target profileName.</param>
    ''' <param name="length">The target length.</param>
    ''' <returns>The HTTP status code of the operation.</returns>
    ''' <remarks></remarks>
    Public Function ProfilePropertyAdd(ByVal portalId As Integer, ByVal profileName As String, ByVal length As String) As HttpStatusCode
        Dim statusCode As HttpStatusCode = HttpStatusCode.InternalServerError

        VerifyPropertiesAreSet(Me.Credentials)
        Try

            Using request As New HttpRequestMessage()
                ConfigureRequest(request, Credentials, "POST", String.Format("portal/{0}/profileName/{1}/length/{2}", portalId, profileName, length))
                Using response As HttpResponseMessage = Client.Send(request)
                    statusCode = response.StatusCode
                End Using
            End Using
            Return statusCode

        Catch ex As Exception
            Throw New SSOClientException("Error processing the ProfilePropertyAdd request: " & ex.Message, ex)
        End Try
    End Function

#End Region

End Class
