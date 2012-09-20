#Facebook Easy Access
###.NET project that facilitates the use of the Facebook API

##Current implementations

```csharp
Uri GenerateLoginUrl(string pCSRFstate, string pPermissions, string pRedirectUrl);

dynamic GetAuthenticatedUserInfo(string pAccessTokenRequest, string pCSRFstateRequest, IHttpSessionState pSessionState);

void GenerateCSRFStateCode(IHttpSessionState pSessionState);

dynamic GetUserInfo(string pUserId);

dynamic GetFriends(string pUserId);

dynamic GetPictureInfo(string pUserId);

dynamic GetPictureInfo(string pUserId,string pGraph);
```