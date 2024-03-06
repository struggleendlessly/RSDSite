using System;

namespace shared
{
    public static class StaticRoutesStrings
    { 
        public const string MainPageRoute = "/";
        public const string MainPageRouteWithParameter = "/{siteName}";

        public const string MainPageRouteOld = "/main-old";
        public const string MainPageRouteOldWithParameter = "{siteName}/main-old";

        public const string AboutUsPageRoute = "/about-us";
        public const string AboutUsPageRouteWithParameter = "{siteName}/about-us";

        public const string ContactUsPageRoute = "/contact-us";
        public const string ContactUsPageRouteWithParameter = "{siteName}/contact-us";

        public const string ServicesPageRoute = "/services";
        public const string ServicesPageRouteWithParameter = "{siteName}/services";


        public const string PricingPageRoute = "/pricing";
        public const string PricingPageRouteWithParameter = "{siteName}/pricing";

        public const string LoginPageRoute = "Account/Login";
        public const string ForgotPasswordPageRoute = "/Account/ForgotPassword";
        public const string RegisterPageRoute = "/Account/Register";
    }
}
