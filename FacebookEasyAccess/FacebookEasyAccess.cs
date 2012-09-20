using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FacebookEasyAccess.Interfaces;
using System.Dynamic;
using System.Configuration;
using System.Web.SessionState;
using Facebook;

namespace FacebookEasyAccess
{
    public class FacebookEasyAccess : IFacebookEasyAccess
    {
        private readonly string _facebookAppId;
        private readonly string _facebookAppSecretId;
        private readonly string _facebookAppNamespace;
        private readonly string _facebookSiteUrl; // ex: http://localhost:2601/
        private readonly string _facebookSiteDomain; // ex: condomundo.com.br

        public FacebookEasyAccess()
        {
            _facebookAppId = ConfigurationManager.AppSettings["FacebookAppId"];
            _facebookAppSecretId = ConfigurationManager.AppSettings["FacebookAppSecretId"];
            _facebookAppNamespace = ConfigurationManager.AppSettings["FacebookAppNamespace"];
            _facebookSiteUrl = ConfigurationManager.AppSettings["FacebookSiteUrl"];
            _facebookSiteDomain = ConfigurationManager.AppSettings["FacebookSiteDomain"];
        }

        public FacebookEasyAccess(string pFacebookAppId, string pFacebookAppSecretId, string pFacebookAppNamespace, string pFacebookSiteUrl, string pFacebookSiteDomain)
        {
            _facebookAppId = pFacebookAppId;
            _facebookAppSecretId = pFacebookAppSecretId;
            _facebookAppNamespace = pFacebookAppNamespace;
            _facebookSiteUrl = pFacebookSiteUrl;
            _facebookSiteDomain = pFacebookSiteDomain;
        }

        private string GetAppAccessToken()
        {
            return string.Concat(_facebookAppId, "|", _facebookAppSecretId);
        }

        /// <summary>
        /// Generates the login URL.
        /// </summary>
        /// <param name="pCSRFstate">The CSRF web session state. <seealso cref="https://developers.facebook.com/docs/authentication/server-side/" /><seealso cref="http://en.wikipedia.org/wiki/Cross-site_request_forgery" /></param>
        /// <param name="pPermissions">comma separated list of permission names.<example>email,user_about_me</example></param>
        /// <param name="pRedirectUrl">The redirect URL.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">pCSRFstate or pRedirectUrl cannot be null</exception>
        public Uri GenerateLoginUrl(string pCSRFstate, string pPermissions, string pRedirectUrl)
        {
            if (string.IsNullOrEmpty(pCSRFstate)) throw new ApplicationException("pCSRFstate cannot be null");
            if (string.IsNullOrEmpty(pRedirectUrl)) throw new ApplicationException("pRedirectUrl cannot be null");

            // for .net 3.5
            // var parameters = new Dictionary<string,object>
            // parameters["client_id"] = appId;
            dynamic parameters = new ExpandoObject();
            parameters.client_id = this._facebookAppId;
            parameters.redirect_uri = pRedirectUrl;

            // The requested response: an access token (token), an authorization code (code), or both (code token).
            parameters.response_type = "token";

            // list of additional display modes can be found at http://developers.facebook.com/docs/reference/dialogs/#display
            parameters.display = "popup";

            parameters.state = pCSRFstate;

            // add the 'scope' parameter only if we have extendedPermissions.
            if (!string.IsNullOrWhiteSpace(pPermissions))
                parameters.scope = pPermissions;

            // generate the login url
            var fb = new FacebookClient();
            return fb.GetLoginUrl(parameters);
        }

        /// <summary>
        /// Gets the authenticated user info.
        /// </summary>
        /// <param name="pAccessTokenRequest">The access token request.</param>
        /// <param name="pCSRFstateRequest">The CSRF state code request.</param>
        /// <param name="pSessionState">State of the IHttpSessionState.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">pAccessTokenRequest or pCSRFstateRequest cannot be null</exception>
        /// <exception cref="System.Security.SecurityException">invalid CSRF</exception>
        public dynamic GetAuthenticatedUserInfo(string pAccessTokenRequest, string pCSRFstateRequest, IHttpSessionState pSessionState)
        {
            if (string.IsNullOrEmpty(pAccessTokenRequest)) throw new ApplicationException("pAccessTokenRequest cannot be null");
            if (pSessionState == null) throw new ApplicationException("pSessionState cannot be null");
            if (pSessionState["state"] != pCSRFstateRequest) throw new System.Security.SecurityException("Invalid pCSRFstateRequest value");

            var fb = new Facebook.FacebookClient(pAccessTokenRequest);
            return fb.Get("me");
        }

        /// <summary>
        /// Generates the CSRF state code.
        /// </summary>
        /// <param name="pSessionState">State of the p session.</param>
        /// <exception cref="System.ApplicationException">pSessionState cannot be null</exception>
        public string GenerateCSRFStateCode(IHttpSessionState pSessionState)
        {
            if (pSessionState == null) throw new ApplicationException("pSessionState cannot be null");
            var statecode = Guid.NewGuid();
            pSessionState["state"] = statecode;
            return statecode;
        }

        /// <summary>
        /// Gets the user info.
        /// </summary>
        /// <param name="pUserId">The user id.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">pUserId cannot be null</exception>
        public dynamic GetUserInfo(string pUserId)
        {
            if (pUserId == null) throw new ApplicationException("pUserId cannot be null");

            var fb = new Facebook.FacebookClient(this.GetAppAccessToken());
            return fb.Get(pUserId);
        }

        /// <summary>
        /// Gets the friends.
        /// </summary>
        /// <param name="pUserId">The user id.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">pUserId cannot be null</exception>
        public dynamic GetFriends(string pUserId)
        {
            if (pUserId == null) throw new ApplicationException("pUserId cannot be null");

            var fb = new Facebook.FacebookClient(this.GetAppAccessToken());
            return fb.Get(string.Concat(pUserId, "/friends"));
        }

        /// <summary>
        /// Gets the picture info.
        /// </summary>
        /// <param name="pUserId">The user id.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">pUserId cannot be null</exception>
        public dynamic GetPictureInfo(string pUserId)
        {
            if (pUserId == null) throw new ApplicationException("pUserId cannot be null");

            var fb = new Facebook.FacebookClient(this.GetAppAccessToken());
            return fb.Get(string.Concat(pUserId, "/picture"));
        }


        /// <summary>
        /// Gets the custom graph data.
        /// </summary>
        /// <param name="pUserId">The user id.</param>
        /// <param name="pGraph">The graph term. <example>/picture</example><example>/books</example></param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">pUserId or pGraph cannot be null</exception>
        public dynamic GetCustomGraphData(string pUserId, string pGraph)
        {
            if (pUserId == null) throw new ApplicationException("pUserId cannot be null");
            if (pGraph == null) throw new ApplicationException("pGraph cannot be null");

            if (pGraph[0] != '/')
                pGraph = string.Concat("/", pGraph);

            var fb = new Facebook.FacebookClient(this.GetAppAccessToken());
            return fb.Get(string.Concat(pUserId, pGraph));
        }
    }
}
