using System;

namespace shared
{
    public static class StaticStrings
    {
        public static string HomePageDataJsonFilePath = "data/home.json";
        public static string AboutUsPageDataJsonFilePath = "data/about-us.json";
        public static string ContactUsPageDataJsonFilePath = "data/contact-us.json";
        public static string ServicesPageDataJsonFilePath = "data/services.json";
        public static string ServicesPageServicesListDataJsonFilePath = "data/services-list.json";

        public static string HomePageDataTitleKey = "Title";
        public static string HomePageDataSubtitleKey = "Subtitle";

        public static string AboutUsPageDataTitleKey = "Title";
        public static string AboutUsPageDataSubtitleKey = "Subtitle";

        public static string ContactUsPageDataTitleKey = "Title";
        public static string ContactUsPageDataSubtitleKey = "Subtitle";

        public static string ServicesPageDataTitleKey = "Title";
        public static string ServicesPageDataServicesListTitleKey = "ServicesListTitle";
        public static string ServicesPageDataServicesListSubtitleKey = "ServicesListSubtitle";

        public static string ServicesUrlKeyEnding = "_Url";

        public static string TinyContentFormatText = "text";
        public static string TinyContentFormatRaw = "raw";
        public static string TinyContentFormatHtml = "html";

        public static string WwwRootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot");
    }
}
