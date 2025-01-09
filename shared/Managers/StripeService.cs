using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Memory;

using shared.Interfaces;
using shared.Models.Stripe;
using shared.ConfigurationOptions;

using Stripe;

namespace shared.Managers
{
    public class StripeService : IStripeService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly StripeOptions _stripeOptions;

        public StripeService(IMemoryCache memoryCache, IOptions<StripeOptions> stripeOptions)
        {
            _memoryCache = memoryCache;
            _stripeOptions = stripeOptions.Value;

            StripeConfiguration.ApiKey = _stripeOptions.ApiSecretKey;
        }

        public async Task<List<StripeProductModel>> GetProductsWithPricesAsync(string lang)
        {
            var key = string.Format(StaticStrings.PricingPageDataJsonMemoryCacheKey, lang);
            if (_memoryCache.TryGetValue(key, out List<StripeProductModel> cachedProductsWithPrices))
            {
                return cachedProductsWithPrices;
            }

            var productsWithPrices = new List<StripeProductModel>();

            var productsOptions = new ProductListOptions { Active = true };
            var productService = new ProductService();
            StripeList<Product> products = await productService.ListAsync(productsOptions);

            foreach (var product in products.OrderByDescending(x => x.Name))
            {
                var stripeProductModel = new StripeProductModel()
                {
                    Id = product.Id,
                    Name = product.Name
                };

                var pricesOptions = new PriceListOptions { Active = true, Product = product.Id };
                var priceService = new PriceService();
                StripeList<Price> prices = await priceService.ListAsync(pricesOptions);

                foreach (var price in prices.OrderBy(x => x.UnitAmount))
                {
                    var stripePriceModel = new StripePriceModel()
                    {
                        Currency = price.Currency,
                        UnitAmount = price.UnitAmount.Value,
                        Interval = price.Recurring?.Interval
                    };

                    stripeProductModel.Prices.Add(stripePriceModel);
                }

                productsWithPrices.Add(stripeProductModel);
            }

            _memoryCache.Set(key, productsWithPrices, TimeSpan.FromMinutes(30));

            return productsWithPrices;
        }
    }
}
