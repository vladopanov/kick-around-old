﻿using System;
using KickAround.Data;
using KickAround.Models.EntityModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Owin;

namespace KickAround.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(KickAroundContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, User>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //Microsoft.Owin.Security.Facebook.FacebookAuthenticationOptions fbOptions = new Microsoft.Owin.Security.Facebook.FacebookAuthenticationOptions()
            //{
            //    AppId = "290673761360558",
            //    AppSecret = "d1e8e2e8c01e431ee377387ca7772189"
            //};
            //fbOptions.Scope.Add("email");
            //fbOptions.Provider = new Microsoft.Owin.Security.Facebook.FacebookAuthenticationProvider()
            //{
            //    OnAuthenticated = (context) =>
            //    {
            //        context.Identity.AddClaim(new System.Security.Claims.Claim("FacebookAccessToken", context.AccessToken));
            //        foreach (var claim in context.User)
            //        {
            //            var claimType = string.Format("urn:facebook:{0}", claim.Key);
            //            string claimValue = claim.Value.ToString();
            //            if (!context.Identity.HasClaim(claimType, claimValue))
            //                context.Identity.AddClaim(new System.Security.Claims.Claim(claimType, claimValue, "XmlSchemaString", "Facebook"));

            //        }
            //        return System.Threading.Tasks.Task.FromResult(0);
            //    }
            //};
            //fbOptions.SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie;
            //fbOptions.UserInformationEndpoint = "https://graph.facebook.com/v2.4/me?fields=id,name,email";
            //app.UseFacebookAuthentication(fbOptions);

            var facebookOptions = new FacebookAuthenticationOptions()
            {
                AppId = "290673761360558",
                AppSecret = "d1e8e2e8c01e431ee377387ca7772189",
                UserInformationEndpoint = "https://graph.facebook.com/v2.4/me?fields=id,name,email",
                Scope = { "email" }
            };
            app.UseFacebookAuthentication(facebookOptions);

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "60457657793-vf002cr86a5d58opfo9n3kg6drf0dl45.apps.googleusercontent.com",
                ClientSecret = "TNu6i4fYcTFwRZYM0oYbGHxl"
            });
        }
    }
}