Imports System.Collections
Imports System.Collections.Generic
Imports System.Runtime.Serialization
Imports Microsoft.Http

Imports Interzoic.SSO.Shared
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Common.Utilities
Imports System.Net
Imports System.Text

Partial Public Class Connection

#Region "Utility"

    Private Shared Function HTTPHeadersToHeadersCollection(ByVal headers As Microsoft.Http.Headers.RequestHeaders) As WebHeaderCollection
        If headers Is Nothing Then
            Return Nothing
        End If
        'TODO: do this better. Is this needed?
        Dim reqHeaders As New WebHeaderCollection()
        For Each key In headers.Keys
            reqHeaders.Add(key, headers(key))
        Next
        Return reqHeaders
    End Function

    Private Shared Function CreateAuthorizationHeader(ByVal credentials As Credentials, _
                                                     ByVal verb As String, _
                                                     ByVal absoulteURI As String, _
                                                     ByVal requestHeaders As Headers.RequestHeaders) _
                                                     As String
        ' NOTE: Builds the string to be hashed (the client application must do exactly the same)
        Dim headersCollection As WebHeaderCollection = HTTPHeadersToHeadersCollection(requestHeaders)
        Dim textToHash As String = SSOSecurity.BuildStringToHash(verb, absoulteURI, headersCollection)

        ' Adds the Authorization header
        Dim portallogin As Integer = credentials.PortalId
        Dim userlogin As String = credentials.UserName
        Dim requesthash As String = SSOSecurity.ComputeHash(credentials.SecretKey, textToHash)
        Dim authorizationHeader As String = String.Format("{0}:{1}:{2}", portallogin, userlogin, requesthash)

        Return authorizationHeader
    End Function


    Private Sub ConfigureRequest(ByRef request As HttpRequestMessage, ByVal credentials As Credentials, ByVal verb As String, ByVal verbOverride As String, ByVal uriTemplate As String)
        ConfigureRequest(request, credentials, verb, verbOverride, uriTemplate, Nothing)
    End Sub

    Private Sub ConfigureRequest(ByRef request As HttpRequestMessage, ByVal credentials As Credentials, ByVal verb As String, ByVal uriTemplate As String)
        ConfigureRequest(request, credentials, verb, String.Empty, uriTemplate, Nothing)
    End Sub

    Private Sub ConfigureRequest(ByRef request As HttpRequestMessage, ByVal credentials As Credentials, ByVal verb As String, ByVal uriTemplate As String, ByVal content As HttpContent)
        ConfigureRequest(request, credentials, verb, String.Empty, uriTemplate, content)
    End Sub

    Private Sub ConfigureRequest(ByRef request As HttpRequestMessage, ByVal credentials As Credentials, ByVal verb As String, ByVal verbOverride As String, ByVal uriTemplate As String, ByVal content As HttpContent)
        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
        VerifyPropertiesAreSet(credentials)

        If Not request Is Nothing Then
            ' Basic information
            request.Method = verb
            request.Uri = New Uri(Me.ServiceURI & uriTemplate)

            If content Is Nothing And (verb.ToUpper = "PUT" Or verb.ToUpper = "POST") Then
                ' If you do not include the Content-Length header in a PUT or POST request, 
                ' the request fails with a 411 Length Required error.
                request.Headers.ContentLength = 0
            End If

            If verbOverride <> String.Empty Then
                ' Overrides the Verb
                request.Headers.Add(SSOConstants.MethodOverrideHeader, verbOverride)
                'verb = verbOverride // keep the real verb
            End If

            If Not content Is Nothing Then
                ' Content Information
                content.LoadIntoBuffer()
                request.Content = content
                request.Headers(SSOConstants.ContentTypeHeader) = content.ContentType
                request.Headers(SSOConstants.ContentMD5Header) = SSOSecurity.GenerateChecksumForContent(content.ReadAsString())
            End If

            ' Authorization Headers
            request.Headers(SSOConstants.IzmDateHeader) = SSOSecurity.CurrentUTCDateTimeString
            request.Headers(SSOConstants.AuthorizationHeader) = CreateAuthorizationHeader(credentials, verb, request.Uri.AbsoluteUri, request.Headers)
        End If

    End Sub

#End Region

End Class

