using System.Text;

using Newtonsoft.Json;

using shared;
using shared.Models;
using shared.Managers;
using shared.Interfaces;

namespace web.Endpoints
{
    public static class SitemapEndpoint
    {
        public static void MapSitemapEndpoint(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/sitemap.xml", 
                async (
                    HttpContext context,
                    AzureBlobStorageManager azureBlobStorageManager
                    ) =>
            {
                var sitemap = await azureBlobStorageManager.DownloadFile(StaticStrings.DefaultSiteName, StaticStrings.SitemapDataXmlFilePath);

                context.Response.ContentType = "application/xml";
                await context.Response.WriteAsync(sitemap);
            });

            endpoints.MapPost("/sitemap.xml", 
                async (
                    HttpContext context,
                    IWebsiteService websiteService,
                    AzureBlobStorageManager azureBlobStorageManager
                    ) =>
            {
                var pagesWithoutSiteName = new List<string>() { StaticRoutesStrings.LoginPageUrl, StaticRoutesStrings.ForgotPasswordPageUrl };
                var duplicatePages = new List<string>() { StaticRoutesStrings.EmptyRoute, StaticRoutesStrings.AboutUsPageUrl, StaticRoutesStrings.ContactUsPageUrl, StaticRoutesStrings.ServicesPageUrl, StaticRoutesStrings.ServicePageUrl };
                var languages = new List<string>() { StaticStrings.DefaultEnLang, StaticStrings.DefaultUaLang };

                var websites = await websiteService.GetWebsitesNamesAsync();
                websites.Add(StaticStrings.DefaultSiteName);

                var sb = new StringBuilder();
                var domain = "https://myelegantpages.com";

                sb.AppendLine("""<?xml version="1.0" encoding="UTF-8"?>""");
                sb.AppendLine("""<urlset xmlns="https://www.sitemaps.org/schemas/sitemap/0.9">""");

                sb.AppendLine("<url>");
                sb.AppendLine($"<loc>{domain}</loc>");
                sb.AppendLine("</url>");

                foreach (var page in pagesWithoutSiteName)
                {
                    foreach (var lang in languages)
                    {
                        sb.AppendLine("<url>");
                        sb.AppendLine($"<loc>{domain}/{lang}/{page}</loc>");
                        sb.AppendLine("</url>");
                    }
                }

                foreach (var site in websites)
                {
                    var sitePages = duplicatePages;
                    if (site == StaticStrings.DefaultSiteName)
                    {
                        sitePages.Add(StaticRoutesStrings.PricingPageUrl);
                    }

                    foreach (var page in sitePages)
                    {
                        foreach (var lang in languages)
                        {
                            if (page == StaticRoutesStrings.ServicePageUrl)
                            {
                                var blobName = string.Format(StaticStrings.ServicesPageServicesListDataJsonFilePath, lang);
                                var jsonContent = await azureBlobStorageManager.DownloadFile(site, blobName);
                                var services = JsonConvert.DeserializeObject<List<ServiceItem>>(jsonContent);
                                var servicesUrls = services
                                    .SelectMany(x => x.LongDesc.Where(x => x.Key.Contains(StaticStrings.UrlKeyEnding)))
                                    .ToList();

                                foreach (var serviceUrl in servicesUrls)
                                {
                                    sb.AppendLine("<url>");
                                    sb.AppendLine($"<loc>{domain}/{site}/{lang}/{page}/{serviceUrl.Value}</loc>");
                                    sb.AppendLine("</url>");
                                }
                            }
                            else if (page == StaticRoutesStrings.EmptyRoute)
                            {
                                sb.AppendLine("<url>");
                                sb.AppendLine($"<loc>{domain}/{site}/{lang}</loc>");
                                sb.AppendLine("</url>");
                            }
                            else
                            {
                                sb.AppendLine("<url>");
                                sb.AppendLine($"<loc>{domain}/{site}/{lang}/{page}</loc>");
                                sb.AppendLine("</url>");
                            }
                        }
                    }
                }

                sb.AppendLine("</urlset>");

                var byteArray = Encoding.UTF8.GetBytes(sb.ToString());
                using (var stream = new MemoryStream(byteArray))
                {
                    await azureBlobStorageManager.UploadFile(StaticStrings.DefaultSiteName, StaticStrings.SitemapDataXmlFilePath, stream);
                }

                context.Response.StatusCode = StatusCodes.Status200OK;
            });
        }
    }
}
