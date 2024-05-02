using System;

namespace shared
{
    public static class StaticRoutesStrings
    {
        public const string EmptyRoute = "";
        
        public const string MainPageRoute = "/{SiteName}/{Lang}";
        public const string AboutUsPageRoute = "/{SiteName}/{Lang}/about-us";
        public const string ContactUsPageRoute = "/{SiteName}/{Lang}/contact-us";
        public const string ServicesPageRoute = "/{SiteName}/{Lang}/services";
        public const string ServicePageRoute = "/{SiteName}/{Lang}/service/{UrlKey}";
        public const string PricingPageRoute = "/{SiteName}/{Lang}/pricing";
        public const string AdminPageRoute = "/{SiteName}/{Lang}/admin";
        public const string SubscriptionErrorPageRoute = "/subscription-error";

        public const string LoginPageRoute = "/{SiteName}/{Lang}/account/login";
        public const string RegisterPageRoute = "/{SiteName}/{Lang}/account/register";
        public const string RegisterConfirmationPageRoute = "/{SiteName}/{Lang}/account/register-confirmation";
        public const string LogoutPageRoute = "/{SiteName}/{Lang}/account/logout";
        public const string AccountConfirmEmailPageRoute = "/{SiteName}/{Lang}/account/confirm-email";
        public const string ForgotPasswordPageRoute = "/{SiteName}/{Lang}/account/forgot-password";
        public const string AccountForgotPasswordConfirmationPageRoute = "/{SiteName}/{Lang}/account/forgot-password-confirmation";
        public const string AccountResetPasswordPageRoute = "/{SiteName}/{Lang}/account/reset-password";
        public const string AccountResetPasswordConfirmationPageRoute = "/{SiteName}/{Lang}/account/reset-password-confirmation";
        public const string AccountInvalidPasswordResetPageRoute = "/{SiteName}/{Lang}/account/invalid-password-reset";

        public const string AboutUsPageUrl = "about-us";
        public const string ContactUsPageUrl = "contact-us";
        public const string ServicesPageUrl = "services";
        public const string ServicePageUrl = "service";
        public const string PricingPageUrl = "pricing";
        public const string AdminPageUrl = "admin";

        public const string SubscriptionErrorUrl = "subscription-error";

        public const string LoginPageUrl = "account/login";    
        public const string RegisterPageUrl = "account/register";
        public const string RegisterConfirmationPageUrl = "account/register-confirmation";
        public const string LogoutPageUrl = "account/logout";
        public const string AccountConfirmEmailPageUrl = "account/confirm-email";
        public const string AccountForgotPasswordConfirmationPageUrl = "account/forgot-password-confirmation";
        public const string ForgotPasswordPageUrl = "account/forgot-password";
        public const string AccountResetPasswordPageUrl = "account/reset-password";
        public const string AccountResetPasswordConfirmationPageUrl = "account/reset-password-confirmation";
        public const string AccountInvalidPasswordResetPageUrl = "account/invalid-password-reset";
    }
}
