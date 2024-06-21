using System;
using System.Globalization;

namespace shared
{
    public static partial class StaticStrings
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
        public static string PortfolioPageDataJsonFilePath = "{0}/data/portfolio.json";
        public static string PortfolioPageServicesListDataJsonFilePath = "{0}/data/portfolio-list.json";
        public static string PortfolioPageFAQListDataJsonFilePath = "{0}/data/portfolio-faq-list.json";
        public static string DocumentsPageDataJsonFilePath = "{0}/data/documents.json";
        public static string DocumentsPageServicesListDataJsonFilePath = "{0}/data/documents-list.json";
        public static string DocumentsPageFAQListDataJsonFilePath = "{0}/data/documents-faq-list.json";

        public static string PopoversDataJsonFilePath = "{0}/popovers.json";
        public static string SitemapDataXmlFilePath = "sitemap.xml";

        public static string NewDomainsContainerName = "new-domains-dev";
        public static string NewDomainsDataJsonFilePath = "{0}.json";

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
        public static string ServicesPageDataImageKey = "Image";
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
        public static string AdminPageSettingsMenuDataPortfolioKey = "PortfolioPageText";
        public static string AdminPageSettingsMenuDataDocumentsKey = "DocumentsPageText";

        /* Popovers Keys */

        public static string Popover_Main_1_Title = "Main_1_Title";
        public static string Popover_Main_1_Subtitle = "Main_1_Subtitle";
        public static string Popover_Main_1_ImageEditor = "Main_1_ImageEditor";
        public static string Popover_Main_2_Title = "Main_2_Title";
        public static string Popover_Main_2_Subtitle = "Main_2_Subtitle";
        public static string Popover_Main_1_Services = "Main_1_Services";
        public static string Popover_Main_1_Button = "Main_1_Button";
        public static string Popover_Main_1_Card = "Main_1_Card";

        public static string Popover_AboutUs_1_Title = "AboutUs_1_Title";
        public static string Popover_AboutUs_1_Subtitle = "AboutUs_1_Subtitle";
        public static string Popover_AboutUs_1_ImageEditor = "AboutUs_1_ImageEditor";
        public static string Popover_AboutUs_2_1_Title = "AboutUs_2_1_Title";
        public static string Popover_AboutUs_2_1_Subtitle = "AboutUs_2_1_Subtitle";
        public static string Popover_AboutUs_2_2_Title = "AboutUs_2_2_Title";
        public static string Popover_AboutUs_2_2_Subtitle = "AboutUs_2_2_Subtitle";
        public static string Popover_AboutUs_2_3_Title = "AboutUs_2_3_Title";
        public static string Popover_AboutUs_2_3_Subtitle = "AboutUs_2_3_Subtitle";
        public static string Popover_AboutUs_3_Title = "AboutUs_3_Title";
        public static string Popover_AboutUs_3_1_Subtitle = "AboutUs_3_1_Subtitle";
        public static string Popover_AboutUs_3_2_Subtitle = "AboutUs_3_2_Subtitle";
        public static string Popover_AboutUs_4_Title = "AboutUs_4_Title";
        public static string Popover_AboutUs_4_Subtitle = "AboutUs_4_Subtitle";
        public static string Popover_AboutUs_1_Card = "AboutUs_1_Card";

        public static string Popover_Services_1_Title = "Services_1_Title";
        public static string Popover_Services_1_ImageEditor = "Services_1_ImageEditor";
        public static string Popover_Services_2_Title = "Services_2_Title";
        public static string Popover_Services_2_Subtitle = "Services_2_Subtitle";
        public static string Popover_Services_1_Card = "Services_1_Card";
        public static string Popover_Services_3_Title = "Services_3_Title";
        public static string Popover_Services_2_Card = "Services_2_Card";

        public static string Popover_Service_1_Title = "Service_1_Title";
        public static string Popover_Service_1_Subtitle = "Service_1_Subtitle";
        public static string Popover_Service_1_ImageEditor = "Service_1_ImageEditor";
        public static string Popover_Service_1_Content = "Service_1_Content";

        public static string Popover_ContactUs_1_Title = "ContactUs_1_Title";
        public static string Popover_ContactUs_1_Subtitle = "ContactUs_1_Subtitle";
        public static string Popover_ContactUs_1_1_Contact = "ContactUs_1_1_Contact";
        public static string Popover_ContactUs_1_2_Contact = "ContactUs_1_2_Contact";
        public static string Popover_ContactUs_1_3_Contact = "ContactUs_1_3_Contact";
        public static string Popover_ContactUs_1_1_Map = "ContactUs_1_1_Map";
        public static string Popover_ContactUs_1_2_Map = "ContactUs_1_2_Map";
        public static string Popover_ContactUs_1_1_Form = "ContactUs_1_1_Form";
        public static string Popover_ContactUs_1_2_Form = "ContactUs_1_2_Form";
        public static string Popover_ContactUs_1_3_Form = "ContactUs_1_3_Form";

        public static string Popover_Admin_1_1_General = "Admin_1_1_General";
        public static string Popover_Admin_1_2_General = "Admin_1_2_General";
        public static string Popover_Admin_1_3_General = "Admin_1_3_General";
        public static string Popover_Admin_1_4_General = "Admin_1_4_General";
        public static string Popover_Admin_2_1_General = "Admin_2_1_General";
        public static string Popover_Admin_2_2_General = "Admin_2_2_General";
        public static string Popover_Admin_2_3_General = "Admin_2_3_General";
        public static string Popover_Admin_1_1_Settings = "Admin_1_1_Settings";
        public static string Popover_Admin_1_2_Settings = "Admin_1_2_Settings";
        public static string Popover_Admin_1_3_Settings = "Admin_1_3_Settings";
        public static string Popover_Admin_1_4_Settings = "Admin_1_4_Settings";
        public static string Popover_Admin_1_Social_ImageEditor = "Admin_1_Social_ImageEditor";
        public static string Popover_Admin_1_Social_Url = "Admin_1_Social_Url";
        public static string Popover_Admin_1_Social_Card = "Admin_1_Social_Card";
        public static string Popover_Admin_1_1_CustomDomain = "Admin_1_1_CustomDomain";
        public static string Popover_Admin_1_2_CustomDomain = "Admin_1_2_CustomDomain";
        public static string Popover_Admin_1_3_CustomDomain = "Admin_1_3_CustomDomain";

        /* Key Endings */

        public static string AvatarKeyEnding = "_Avatar";
        public static string TitleKeyEnding = "_Title";
        public static string SubtitleKeyEnding = "_Subtitle";
        public static string UrlKeyEnding = "_Url";
        public static string TextKeyEnding = "_Text";
        public static string ImageKeyEnding = "_Image";
        public static string LinkKeyEnding = "_Link";

        public static string PortfolioPageKeyEnding = "_Portfolio";
        public static string DocumentsPageKeyEnding = "_Documents";

        /* Editor Content Formats */

        public static string EditorContentFormatText = "text";
        public static string EditorContentFormatRaw = "raw";
        public static string EditorContentFormatHtml = "html";

        /* Text Editor Types */

        public static string TextEditorTypeText = "text";
        public static string TextEditorTypeEmail = "email";
        public static string TextEditorTypePhone = "tel";
        public static string TextEditorTypeUrl = "url";

        /* Other */

        public static string WwwRootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot");
        public static string PowerShellScriptsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PowerShellScripts");

        /* Default Values */

        public static string MainSiteName = "main";
        public static string EnLanguageCode = "en";
        public static string UaLanguageCode = "ua";
        public static string ExampleContainerName = "example";
        public static string ProdDomain = "myelegantpages";
        public static string DevDomain = "dev.myelegantpages";
        public static string LocalDomain = "localhost";

        public static List<string> Domains = new List<string>() { ProdDomain, DevDomain, LocalDomain };

        /* Containers */
        public static string PopoversContainerName = "popovers";

        /* Subscription modules names */
        public static string SubscriptionModuleWebsite = "Website";
        public static string SubscriptionModuleCustomDomain = "Custom domain";

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
        public static string PricingPageDataJsonMemoryCacheKey = "{0}_PricingPageModel";

        public static string ActiveWebsiteSubscription = "ActiveWebsiteSubscription_{0}";
        public static string ActiveCustomDomainSubscription = "ActiveCustomDomainSubscription_{0}";

        /* Azure */

        public static string PowerShellAzureAddCustomDomainScript = "azure-add-custom-domain.ps1";
        public static string AzureWebAppName = "WebAppName";
        public static string AzureWebAppResourceGroup = "WebAppResourceGroup";
        public static string AzureCustomDomain = "CustomDomain";

        /* Languages */

        public static readonly List<CultureInfo> Languages = new List<CultureInfo>
        {
            new CultureInfo("en"),
            new CultureInfo("ua")
        };

        public static readonly CultureInfo DefaultLanguage = new CultureInfo("en");

        /* Emails */

        public static string EmailTemplatesContainerName = "email-templates";
        public static string EmailTemplatesConfirmEmailDataHtmlFilePath = "{0}/confirm-email-template.html";
        public static string EmailTemplatesResetPasswordDataHtmlFilePath = "{0}/reset-password-template.html";
    }
}
