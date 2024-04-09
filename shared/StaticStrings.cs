using System;

namespace shared
{
    public static class StaticStrings
    {
        /* Paths to JSON files */

        public static string HomePageDataJsonFilePath = "{0}/data/home.json";
        public static string HomePageTestimonialsListDataJsonFilePath = "{0}/data/testimonials-list.json";
        public static string AboutUsPageDataJsonFilePath = "{0}/data/about-us.json";
        public static string AboutUsPageTeamListDataJsonFilePath = "{0}/data/team-list.json";
        public static string ContactUsPageDataJsonFilePath = "{0}/data/contact-us.json";
        public static string ServicesPageDataJsonFilePath = "{0}/data/services.json";
        public static string ServicesPageServicesListDataJsonFilePath = "{0}/data/services-list.json";
        public static string ServicesPageFAQListDataJsonFilePath = "{0}/data/faq-list.json";
        public static string AdminPageSettingsDataJsonFilePath = "{0}/data/settings.json";
        public static string AdminPageSocialNetworksDataJsonFilePath = "{0}/data/social-networks.json";
        public static string AdminPageSettingsMenuDataJsonFilePath = "{0}/data/settings-menu.json";

        /* Home Keys */

        public static string HomePageDataTitleKey = "Title";
        public static string HomePageDataSubtitleKey = "Subtitle";
        public static string HomePageDataImageKey = "Image";
        public static string HomePageDataFeaturedCaseStudiesTitleKey = "FeaturedCaseStudiesTitle";
        public static string HomePageDataFeaturedCaseStudiesSubtitleKey = "FeaturedCaseStudiesSubtitle";
        public static string HomePageDataViewAllWorksButtonTextKey = "ViewAllWorksButtonText";

        /* About Us Keys */

        public static string AboutUsPageDataTitleKey = "Title";
        public static string AboutUsPageDataSubtitleKey = "Subtitle";
        public static string AboutUsPageDataCarouselImage1Key = "CarouselImage1";
        public static string AboutUsPageDataCarouselImage2Key = "CarouselImage2";
        public static string AboutUsPageDataCarouselImage3Key = "CarouselImage3";
        public static string AboutUsPageDataFeatureStatsFirstTitleKey = "FeatureStatsFirstTitle";
        public static string AboutUsPageDataFeatureStatsFirstSubtitleKey = "FeatureStatsFirstSubtitle";
        public static string AboutUsPageDataFeatureStatsSecondTitleKey = "FeatureStatsSecondTitle";
        public static string AboutUsPageDataFeatureStatsSecondSubtitleKey = "FeatureStatsSecondSubtitle";
        public static string AboutUsPageDataFeatureStatsThirdTitleKey = "FeatureStatsThirdTitle";
        public static string AboutUsPageDataFeatureStatsThirdSubtitleKey = "FeatureStatsThirdSubtitle";
        public static string AboutUsPageDataInfoLeftKey = "InfoLeft";
        public static string AboutUsPageDataInfoRightFirstKey = "InfoRightFirst";
        public static string AboutUsPageDataInfoRightSecondKey = "InfoRightSecond";
        public static string AboutUsPageDataTeamTitleKey = "TeamTitle";
        public static string AboutUsPageDataTeamSubtitleKey = "TeamSubtitle";

        /* Contact Us Keys */

        public static string ContactUsPageDataTitleKey = "Title";
        public static string ContactUsPageDataSubtitleKey = "Subtitle";
        public static string ContactUsPageDatMapCoordinatesKey = "MapCoordinates";
        public static string ContactUsPageDataMapMarkerTextKey = "MapMarkerText";
        public static string ContactUsPageDataFormTitleKey = "FormTitle";
        public static string ContactUsPageDataFormFirstNameFieldLabelKey = "FormFirstNameFieldLabel";
        public static string ContactUsPageDataFormFirstNameFieldPlaceholderKey = "FormFirstNameFieldPlaceholder";
        public static string ContactUsPageDataFormLastNameFieldLabelKey = "FormLastNameFieldLabel";
        public static string ContactUsPageDataFormLastNameFieldPlaceholderKey = "FormLastNameFieldPlaceholder";
        public static string ContactUsPageDataFormEmailAddressFieldLabelKey = "FormEmailAddressFieldLabel";
        public static string ContactUsPageDataFormEmailAddressFieldPlaceholderKey = "FormEmailAddressFieldPlaceholder";
        public static string ContactUsPageDataFormPhoneFieldLabelKey = "FormPhoneFieldLabel";
        public static string ContactUsPageDataFormPhoneFieldPlaceholderKey = "FormPhoneFieldPlaceholder";
        public static string ContactUsPageDataFormDetailsFieldLabelKey = "FormDetailsFieldLabel";
        public static string ContactUsPageDataFormDetailsFieldPlaceholderKey = "FormDetailsFieldPlaceholder";
        public static string ContactUsPageDataFormButtonTextKey = "FormButtonText";
        public static string ContactUsPageDataFormTextUnderButtonKey = "FormTextUnderButton";
        public static string ContactUsPageDataCallUsTitleKey = "CallUsTitle";
        public static string ContactUsPageDataCallUsSubtitleKey = "CallUsSubtitle";
        public static string ContactUsPageDataEmailUsTitleKey = "EmailUsTitle";
        public static string ContactUsPageDataEmailUsSubtitleKey = "EmailUsSubtitle";
        public static string ContactUsPageDataAddressTitleKey = "AddressTitle";
        public static string ContactUsPageDataAddressSubtitleKey = "AddressSubtitle";

        /* Services Keys */

        public static string ServicesPageDataTitleKey = "Title";
        public static string ServicesPageDataGalleryImage1Key = "GalleryImage1";
        public static string ServicesPageDataGalleryImage2Key = "GalleryImage2";
        public static string ServicesPageDataServicesListTitleKey = "ServicesListTitle";
        public static string ServicesPageDataServicesListSubtitleKey = "ServicesListSubtitle";
        public static string ServicesPageDataFAQTitleKey = "FAQTitle";

        /* Admin Keys */
        public static string AdminPageDataLogoKey = "Logo";
        public static string AdminPageDataFooterTextKey = "FooterText";
        public static string AdminPageDataServicesButtonTextKey = "ServicesButtonText";

        public static string AdminPageSettingsMenuDataMainKey = "MainPageText";
        public static string AdminPageSettingsMenuDataAboutUsKey = "AboutUsPageText";
        public static string AdminPageSettingsMenuDataContactUsKey = "ContactUsPageText";
        public static string AdminPageSettingsMenuDataServicesKey = "ServicesPageText";

        /* Key Endings */

        public static string AvatarKeyEnding = "_Avatar";
        public static string TitleKeyEnding = "_Title";
        public static string SubtitleKeyEnding = "_Subtitle";
        public static string UrlKeyEnding = "_Url";
        public static string TextKeyEnding = "_Text";
        public static string ImageKeyEnding = "_Image";

        /* Editor Content Formats */

        public static string EditorContentFormatText = "text";
        public static string EditorContentFormatRaw = "raw";
        public static string EditorContentFormatHtml = "html";

        /* Other */

        public static string WwwRootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot");

        /* Default Values */

        public static string DefaultSiteName = "main";
        public static string DefaultLang = "en";

        /* Memory Cache Keys */

        public static string HomePageDataJsonMemoryCacheKey = "{0}_{1}_HomePageModel";
        public static string HomePageTestimonialsListDataJsonMemoryCacheKey = "{0}_{1}_HomePageTestimonialsListModel";
        public static string AboutUsPageDataJsonMemoryCacheKey = "{0}_{1}_AboutUsPageModel";
        public static string ContactUsPageDataJsonMemoryCacheKey = "{0}_{1}_ContactUsPageModel";
        public static string AboutUsPageTeamListDataJsonMemoryCacheKey = "{0}_{1}_AboutUsPageTeamListModel";
        public static string ServicesPageDataJsonMemoryCacheKey = "{0}_{1}_ServicesPageModel";
        public static string ServicesPageServicesListDataJsonMemoryCacheKey = "{0}_{1}_ServicesPageServicesListModel";
        public static string ServicesPageFAQListDataJsonMemoryCacheKey = "{0}_{1}_ServicesPageFAQListModel";
        public static string AdminPageDataJsonMemoryCacheKey = "{0}_{1}_AdminPageModel";
        public static string AdminPageSettingsDataJsonMemoryCacheKey = "{0}_{1}_AdminPageSettingsModel";
        public static string AdminPageSocialNetworksDataJsonMemoryCacheKey = "{0}_{1}_AdminPageSocialNetworksModel";
        public static string AdminPageSettingsMenuDataJsonMemoryCacheKey = "{0}_{1}_AdminPageSettingsMenuModel";
    }
}
