using System.Web;
using System.Web.Mvc;
using SocialMediaDemo.Models;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Facebook;

namespace SocialMediaDemo.Controllers
{
    public class SocialMediaController : Controller
    {


        #region Private 
        private IAuthenticationManager AuthenticationManager
        {
            get{
                 return HttpContext.GetOwinContext().Authentication;
               }
        }
        #endregion
        
        #region Action Method
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel _loginViewModel,string provider)
        {     
            if (provider != null)
            {
                return new ChallangeResult(provider, Url.Action("LoginCallback", "SocialMedia"));                 
            }
            else
            {
                if (_loginViewModel.UserName == "Adit" && _loginViewModel.Password == "Adit@123")
                {

                    Session["logIn"] = _loginViewModel;                
                    return RedirectToAction("WelcomeUser", "SocialMedia", new { loginInfo = _loginViewModel.UserName });
                }
                else
                {
                    return View();
                }
            }
        }

        [AllowAnonymous]
        public async Task<ActionResult> LoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
           
            if (loginInfo.Login.LoginProvider == "Facebook")
            {
                var identity = AuthenticationManager.GetExternalIdentity(DefaultAuthenticationTypes.ExternalCookie);
                var access_token = identity.FindFirstValue("FacebookAccessToken");
                var fb = new FacebookClient(access_token);
                dynamic myInfo = fb.Get("/me?fields=email"); // specify the email field
                loginInfo.Email = myInfo.email;
            }

            Session["loginInfo"] = loginInfo.Email;
            var Info = Session["loginInfo"];

            if (Info == null)
            {
                return RedirectToAction("Login", "SocialMedia");
            }
            else
            {
                return RedirectToAction("WelcomeUser", "SocialMedia",new {loginInfo = loginInfo.DefaultUserName.ToString()});
            }            
        }

        public ActionResult WelcomeUser(string loginInfo)
        {
            ViewBag.Name = loginInfo;
            ViewBag.UserName = Session["loginInfo"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie); 
                
            var loginInfo = Session["loginInfo"];
           
            if(loginInfo != null)
            {
                Session["loginInfo"] = null;
            }
            else
            {
                Session["logIn"] = null;
            }          
            return RedirectToAction("Login", "SocialMedia");
        }

        public ActionResult Share() {
            return View();
        }

        public ActionResult FollowAndLike() {
            return View();
        }
        #endregion        
    }
}
