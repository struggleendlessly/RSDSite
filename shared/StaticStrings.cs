using System;

namespace shared
{
    public static class StaticStrings
    {
        public static string HomePageDataJsonFilePath = "data/home.json";
        public static string AboutUsPageDataJsonFilePath = "data/about-us.json";
        public static string AboutUsPageTeamListDataJsonFilePath = "data/team-list.json";
        public static string ContactUsPageDataJsonFilePath = "data/contact-us.json";
        public static string ServicesPageDataJsonFilePath = "data/services.json";
        public static string ServicesPageServicesListDataJsonFilePath = "data/services-list.json";
        public static string ServicesPageFAQListDataJsonFilePath = "data/faq-list.json";

        public static string HomePageDataTitleKey = "Title";
        public static string HomePageDataSubtitleKey = "Subtitle";
        public static string HomePageDataFeaturedCaseStudiesTitleKey = "FeaturedCaseStudiesTitle";
        public static string HomePageDataFeaturedCaseStudiesSubtitleKey = "FeaturedCaseStudiesSubtitle";
        public static string HomePageDataViewAllWorksButtonTextKey = "ViewAllWorksButtonText";

        public static string AboutUsPageDataTitleKey = "Title";
        public static string AboutUsPageDataSubtitleKey = "Subtitle";
        public static string AboutUsPageDataInfoLeftKey = "InfoLeft";
        public static string AboutUsPageDataInfoRightFirstKey = "InfoRightFirst";
        public static string AboutUsPageDataInfoRightSecondKey = "InfoRightSecond";
        public static string AboutUsPageDataTeamTitleKey = "TeamTitle";
        public static string AboutUsPageDataTeamSubtitleKey = "TeamSubtitle";

        public static string ContactUsPageDataTitleKey = "Title";
        public static string ContactUsPageDataSubtitleKey = "Subtitle";
        public static string ContactUsPageDatMapCoordinatesKey = "MapCoordinates";
        public static string ContactUsPageDataMapMarkerTextKey = "MapMarkerText";
        public static string ContactUsPageDataFormTitleKey = "FormTitle";
        public static string ContactUsPageDataFormButtonTextKey = "FormButtonText";
        public static string ContactUsPageDataFormTextUnderButtonKey = "FormTextUnderButton";
        public static string ContactUsPageDataCallUsTitleKey = "CallUsTitle";
        public static string ContactUsPageDataCallUsSubtitleKey = "CallUsSubtitle";
        public static string ContactUsPageDataEmailUsTitleKey = "EmailUsTitle";
        public static string ContactUsPageDataEmailUsSubtitleKey = "EmailUsSubtitle";
        public static string ContactUsPageDataAddressTitleKey = "AddressTitle";
        public static string ContactUsPageDataAddressSubtitleKey = "AddressSubtitle";

        public static string ServicesPageDataTitleKey = "Title";
        public static string ServicesPageDataServicesListTitleKey = "ServicesListTitle";
        public static string ServicesPageDataServicesListSubtitleKey = "ServicesListSubtitle";
        public static string ServicesPageDataFAQTitleKey = "FAQTitle";

        public static string ServicesImageKeyEnding = "_Image";
        public static string ServicesTitleKeyEnding = "_Title";
        public static string ServicesSubtitleKeyEnding = "_Subtitle";
        public static string ServicesUrlKeyEnding = "_Url";
        public static string ServicesTextKeyEnding = "_Text";

        public static string TinyContentFormatText = "text";
        public static string TinyContentFormatRaw = "raw";
        public static string TinyContentFormatHtml = "html";

        public static string WwwRootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot");

        public static string DefaultSiteName = "main";

        /* Memory Cache */

        public static string HomePageDataJsonMemoryCacheKey = "{0}_HomePageModel";
        public static string AboutUsPageDataJsonMemoryCacheKey = "{0}_AboutUsPageModel";
        public static string ContactUsPageDataJsonMemoryCacheKey = "{0}_ContactUsPageModel";
        public static string ContactUsPageTeamListDataJsonMemoryCacheKey = "{0}_ContactUsPageTeamListModel";
        public static string ServicesPageDataJsonMemoryCacheKey = "{0}_ServicesPageModel";
        public static string ServicesPageServicesListDataJsonMemoryCacheKey = "{0}_ServicesPageServicesListModel";
        public static string ServicesPageFAQListDataJsonMemoryCacheKey = "{0}_ServicesPageFAQListModel";
    }
}
