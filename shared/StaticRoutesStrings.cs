using System;

namespace shared
{
    public static class StaticRoutesStrings
    {
        public const string MainPageRoute = "";
        public const string MainWebsitePageRoute = "/{SiteName}";
        public const string AboutUsPageRoute = "/about-us";
        public const string AboutUsWebsitePageRoute = "/{SiteName}/about-us";
        public const string ContactUsPageRoute = "/contact-us";
        public const string ContactUsWebsitePageRoute = "/{SiteName}/contact-us";
        public const string ServicesPageRoute = "/services";
        public const string ServicesWebsitePageRoute = "/{SiteName}/services";
        public const string ServicePageRoute = "/service/{Key}";
        public const string ServiceWebsitePageRoute = "/{SiteName}/service/{Key}";
        public const string PricingPageRoute = "/pricing";
        public const string PricingWebsitePageRoute = "/{SiteName}/pricing";

        public const string LoginPageRoute = "/Account/Login";
        public const string ForgotPasswordPageRoute = "/Account/ForgotPassword";
        public const string RegisterPageRoute = "/Account/Register";

        public static string[] GetPagesRoutes()
        {
            return
            [
                MainPageRoute,
                AboutUsPageRoute.Substring(1),
                ContactUsPageRoute.Substring(1),
                ServicesPageRoute.Substring(1),
                PricingPageRoute.Substring(1),
                LoginPageRoute.Split("/")[1]
            ];
        }
    }
}
