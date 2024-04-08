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
        public const string ServicePageRoute = "/{SiteName}/{Lang}/service/{Key}";
        public const string PricingPageRoute = "/{SiteName}/{Lang}/pricing";
        public const string AdminPageRoute = "/{SiteName}/{Lang}/admin";

        public const string LoginPageRoute = "/{SiteName}/{Lang}/account/login";
        public const string ForgotPasswordPageRoute = "/{SiteName}/{Lang}/account/forgot-password";
        public const string RegisterPageRoute = "/{SiteName}/{Lang}/account/register";
        public const string RegisterConfirmationPageRoute = "/{SiteName}/{Lang}/account/register-confirmation";
        public const string LogoutPageRoute = "/{SiteName}/{Lang}/account/logout";
        public const string AccountConfirmEmailPageRoute = "/{SiteName}/{Lang}/account/confirm-email";

        public const string AboutUsPageUrl = "about-us";
        public const string ContactUsPageUrl = "contact-us";
        public const string ServicesPageUrl = "services";
        public const string ServicePageUrl = "service";
        public const string PricingPageUrl = "pricing";
        public const string AdminPageUrl = "admin";

        public const string LoginPageUrl = "account/login";
        public const string ForgotPasswordPageUrl = "account/forgot-password";
        public const string RegisterPageUrl = "account/register";
        public const string RegisterConfirmationPageUrl = "account/register-confirmation";
        public const string LogoutPageUrl = "account/logout";
        public const string AccountConfirmEmailPageUrl = "account/confirm-email";
    }
}
