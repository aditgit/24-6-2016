using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Google;
using Owin;
using Microsoft.Owin.Security;
using Owin.Security.Providers.LinkedIn;
using Microsoft.Owin.Security.Facebook;
using System.Threading.Tasks;

namespace SocialMediaDemo
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {   
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            
            var facebookAuthenticationOptions = new FacebookAuthenticationOptions()
            {
                AppId = "1752061708340745",
                AppSecret = "d37d77705053ca5430f8c03aa735b08f",
                Scope = { "email" },
                Provider = new FacebookAuthenticationProvider
                {
                    OnAuthenticated = context =>
                    {
                        context.Identity.AddClaim(new System.Security.Claims.Claim("FacebookAccessToken", context.AccessToken));
                        return Task.FromResult(true);
                    }
                }
            };
            app.UseFacebookAuthentication(facebookAuthenticationOptions);
         
            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "481648268302-e51a2cputcrcuf1v1644nk9gurpmq8ot.apps.googleusercontent.com",
                ClientSecret = "ozZdXINwTzZ0WEgNsvAMC5Nv"
            });

            app.UseTwitterAuthentication(new Microsoft.Owin.Security.Twitter.TwitterAuthenticationOptions
            {
                ConsumerKey = "q9TKEtTNuucyxrWrZ7YCu0Hwg",
                ConsumerSecret = "CmozSWLFruRBf4B0FJdzS4JyWpNa9FXfI7x67trygzphPQn2Mo",

                BackchannelCertificateValidator = new CertificateSubjectKeyIdentifierValidator(new[]
                {
                        "A5EF0B11CEC04103A34A659048B21CE0572D7D47", // VeriSign Class 3 Secure Server CA - G2
                        "0D445C165344C1827E1D20AB25F40163D8BE79A5", // VeriSign Class 3 Secure Server CA - G3
                        "7FD365A7C2DDECBBF03009F34339FA02AF333133", // VeriSign Class 3 Public Primary Certification Authority - G5
                        "39A55D933676616E73A761DFA16A7E59CDE66FAD", // Symantec Class 3 Secure Server CA - G4
                        "5168FF90AF0207753CCCD9656462A212B859723B", //DigiCert SHA2 High Assurance Server C‎A 
                        "B13EC36903F8BF4701D498261A0802EF63642BC3" //DigiCert High Assurance EV Root CA
                })
            });

            app.UseLinkedInAuthentication(
                clientId: "75rrlg3r7t0e9y",
                clientSecret: "LW0DxWQ86aYggW93");
        }
    }
}