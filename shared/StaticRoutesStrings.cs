using System;

namespace shared
{
    public static class StaticRoutesStrings
    {
        public const string EmptyRoute = "";

        public const string MainPageRouteWithLang = "/{Lang}";
        public const string MainPageRouteWithSitenameAndLang = "/{SiteName}/{Lang}"; 
        public const string AboutUsPageRouteWithLang = "/{Lang}/about-us";
        public const string AboutUsPageRouteWithSitenameAndLang = "/{SiteName}/{Lang}/about-us";
        public const string ContactUsPageRouteWithLang = "/{Lang}/contact-us";
        public const string ContactUsPageRouteWithSitenameAndLang = "/{SiteName}/{Lang}/contact-us";
        public const string ServicesPageRouteWithLang = "/{Lang}/services";
        public const string ServicesPageRouteWithSitenameAndLang = "/{SiteName}/{Lang}/services";
        public const string ServicePageRouteWithLang = "/{Lang}/service/{UrlKey}";
        public const string ServicePageRouteWithSitenameAndLang = "/{SiteName}/{Lang}/service/{UrlKey}";
        public const string PricingPageRouteWithLang = "/{Lang}/pricing";
        public const string PricingPageRouteWithSitenameAndLang = "/{SiteName}/{Lang}/pricing";
        public const string AdminPageRouteWithLang = "/{Lang}/admin";
        public const string AdminPageRouteWithSitenameAndLang = "/{SiteName}/{Lang}/admin";
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
