using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;

namespace FacebookEasyAccess.Interfaces
{
    public interface IFacebookEasyAccess
    {
        /// <summary>
        /// Generates the login URL.
        /// </summary>
        /// <param name="pCSRFstate">The CSRF web session state. <seealso cref="https://developers.facebook.com/docs/authentication/server-side/" /><seealso cref="http://en.wikipedia.org/wiki/Cross-site_request_forgery" /></param>
        /// <param name="pPermissions">comma separated list of permission names.<example>email,user_about_me</example></param>
        /// <param name="pRedirectUrl">The redirect URL.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">pCSRFstate or pRedirectUrl cannot be null</exception>
        Uri GenerateLoginUrl(string pCSRFstate, string pPermissions, string pRedirectUrl);

        /// <summary>
        /// Gets the authenticated user info.
        /// </summary>
        /// <param name="pAccessTokenRequest">The access token request.</param>
        /// <param name="pCSRFstateRequest">The CSRF state code request.</param>
        /// <param name="pSessionState">State of the IHttpSessionState.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">pAccessTokenRequest or pCSRFstateRequest cannot be null</exception>
        /// <exception cref="System.Security.SecurityException">invalid CSRF</exception>
        dynamic GetAuthenticatedUserInfo(string pAccessTokenRequest, string pCSRFstateRequest, IHttpSessionState pSessionState);

        /// <summary>
        /// Generates the CSRF state code.
        /// </summary>
        /// <param name="pSessionState">State of the p session.</param>
        /// <exception cref="System.ApplicationException">pSessionState cannot be null</exception>
        string GenerateCSRFStateCode(IHttpSessionState pSessionState);

        /// <summary>
        /// Gets the user info.
        /// </summary>
        /// <param name="pUserId">The user id.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">pUserId cannot be null</exception>
        dynamic GetUserInfo(string pUserId);

        /// <summary>
        /// Gets the friends.
        /// </summary>
        /// <param name="pUserId">The user id.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">pUserId cannot be null</exception>
        dynamic GetFriends(string pUserId);

        /// <summary>
        /// Gets the picture info.
        /// </summary>
        /// <param name="pUserId">The user id.</param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">pUserId cannot be null</exception>
        dynamic GetPictureInfo(string pUserId);

        /// <summary>
        /// Gets the custom graph data.
        /// </summary>
        /// <param name="pUserId">The user id.</param>
        /// <param name="pGraph">The graph term. <example>/picture</example><example>/books</example></param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">pUserId or pGraph cannot be null</exception>
        dynamic GetCustomGraphData(string pUserId, string pGraph);
    }
}
