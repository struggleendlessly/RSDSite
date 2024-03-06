using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared
{
    public static class RoutesWithSiteName
    {
        private static string? _siteName = "";

        public static string? SiteName
        {
            get => _siteName;
            set
            {
                _siteName = value;

                MainPageRoute = $"{_siteName}/";
                MainPageRouteOld = $"{_siteName}/main-old";
                AboutUsPageRoute = $"{_siteName}/about-us";
                ContactUsPageRoute = $"{_siteName}/contact-us";
                ServicesPageRoute = $"{_siteName}/services";
                PricingPageRoute = $"{_siteName}/pricing";
            }
        }

        public static string MainPageRoute { get; private set; } = $"{_siteName}/";
        public static string MainPageRouteOld { get; private set; } = $"{_siteName}/main-old";
        public static string AboutUsPageRoute { get; private set; } = $"{_siteName}/about-us";
        public static string ContactUsPageRoute { get; private set; } = $"{_siteName}/contact-us";
        public static string ServicesPageRoute { get; private set; } = $"{_siteName}/services";
        public static string PricingPageRoute { get; private set; } = $"{_siteName}/pricing";

        public static string LoginPageRoute { get; private set; } = $"/Account/Login";
        public static string ForgotPasswordPageRoute { get; private set; } = $"/Account/ForgotPassword";
        public static string RegisterPageRoute { get; private set; } = $"/Account/Register";
    }
}
