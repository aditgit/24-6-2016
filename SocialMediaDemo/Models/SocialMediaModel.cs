using Microsoft.Owin.Security;
using System.Web;
using System.Web.Mvc;

namespace SocialMediaDemo.Models
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public string Email { get; set; }
    }

    internal class ChallangeResult : HttpUnauthorizedResult
    {
        public string LoginProvider { get; set; }
        public string RedirectUri { get; set; }
        public string Email { get; set; }

        public ChallangeResult(string loginProvider, string redirectUri)
            : this(loginProvider, redirectUri, null)
        {
        }

        public ChallangeResult(string loginProvider, string redirectUri, string userId)
        {
            LoginProvider = loginProvider;
            RedirectUri = redirectUri;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
            context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
        }

    }
    
}