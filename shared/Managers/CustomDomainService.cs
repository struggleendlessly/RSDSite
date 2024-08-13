using shared.Data;
using shared.Models;
using shared.Interfaces;
using shared.Interfaces.Api;

using System.Text;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace shared.Managers
{
    public class CustomDomainService : ICustomDomainService
    {    
        private readonly IStateManager _stateManager;
        private readonly IDomainChecker _domainChecker;
        private readonly IPageDataService _pageDataService;
        private readonly IApiPageDataService _apiPageDataService;
        private readonly ApplicationDbContext _dbContext;
        private readonly AzureBlobStorageManager _blobStorageManager;

        public CustomDomainService(
            IStateManager stateManager,
            IDomainChecker domainChecker,
            IPageDataService pageDataService,
            IApiPageDataService apiPageDataService,
            ApplicationDbContext dbContext,
            AzureBlobStorageManager blobStorageManager
            )
        {    
            _stateManager = stateManager;
            _domainChecker = domainChecker;
            _pageDataService = pageDataService;
            _apiPageDataService = apiPageDataService;
            _dbContext = dbContext;
            _blobStorageManager = blobStorageManager;
        }

        public async Task SaveCustomDomainAsync(string customDomain)
        {
            var website = await _dbContext.Websites.FirstOrDefaultAsync(x => x.Name == _stateManager.SiteName);

            website.IsNewDomainInProcess = true;
            website.Domain = customDomain;

            await _dbContext.SaveChangesAsync();

            var newDomainsBlobName = string.Format(StaticStrings.NewDomainsDataJsonFilePath, DateTime.UtcNow.ToString("MM-dd-yyyy"));
            var newDomains = new List<CustomDomainValidationModel>();
            var customDomainValidationModel = new CustomDomainValidationModel 
            {
                WebsiteId = website.Id,
                Domain = customDomain,
                Status = nameof(CustomDomainValidationStatus.PendingVerification)
            };

            var jsonContent = await _blobStorageManager.DownloadFile(StaticStrings.NewDomainsContainerName, newDomainsBlobName);
            if (!string.IsNullOrWhiteSpace(jsonContent))
            {
                newDomains = JsonConvert.DeserializeObject<List<CustomDomainValidationModel>>(jsonContent);
            }

            newDomains.Add(customDomainValidationModel);

            var json = JsonConvert.SerializeObject(newDomains);
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            await _blobStorageManager.UploadFile(StaticStrings.NewDomainsContainerName, newDomainsBlobName, stream);
        }

        public async Task<string> CheckCustomDomainVerificationAsync()
        {
            var website = await _dbContext.Websites.FirstOrDefaultAsync(x => x.Name == _stateManager.SiteName);
            if (website == null || !website.IsNewDomainInProcess)
                return string.Empty;

            var localizationModel = await _apiPageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, _stateManager.SiteName, _stateManager.Lang, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);
            //var localizationModel = await _pageDataService.GetDataAsync<PageModel>(StaticStrings.LocalizationMemoryCacheKey, StaticStrings.LocalizationJsonFilePath, StaticStrings.LocalizationContainerName);

            var newDomainsBlobName = string.Format(StaticStrings.NewDomainsDataJsonFilePath, DateTime.UtcNow.ToString("MM-dd-yyyy"));
            var jsonContent = await _blobStorageManager.DownloadFile(StaticStrings.NewDomainsContainerName, newDomainsBlobName);
            if (!string.IsNullOrWhiteSpace(jsonContent))
            {
                var newDomains = JsonConvert.DeserializeObject<List<CustomDomainValidationModel>>(jsonContent);
                var domainValidationModel = newDomains?.FirstOrDefault(x => x.WebsiteId == website.Id);
                if (domainValidationModel != null)
                {
                    switch (domainValidationModel.Status)
                    {
                        case nameof(CustomDomainValidationStatus.Verified):
                            website.IsNewDomainInProcess = false;
                            await _dbContext.SaveChangesAsync();
                            return localizationModel.Data[StaticStrings.Localization_Admin_CustomDomain_Success_Message_Key];

                        case nameof(CustomDomainValidationStatus.PendingVerification):
                            return localizationModel.Data[StaticStrings.Localization_Admin_CustomDomain_InProgress_Message_Key];

                        case nameof(CustomDomainValidationStatus.VerificationFailed):
                            return localizationModel.Data[StaticStrings.Localization_Admin_CustomDomain_Failed_Message_Key];
                    }
                }
            }

            bool isDomainWorking = await _domainChecker.IsDomainWorkingAsync(website.Domain);
            if (isDomainWorking)
            {
                website.IsNewDomainInProcess = false;
                await _dbContext.SaveChangesAsync();
                return localizationModel.Data[StaticStrings.Localization_Admin_CustomDomain_Success_Message_Key];
            }

            return localizationModel.Data[StaticStrings.Localization_Admin_CustomDomain_Failed_Message_Key];
        }
    }
}
