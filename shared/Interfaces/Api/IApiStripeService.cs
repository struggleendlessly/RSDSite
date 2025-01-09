using shared.Models.Stripe;
using System.Globalization;

namespace shared.Interfaces.Api
{
    public interface IApiStripeService
    {
        Task<List<StripeProductModel>> GetProductsWithPricesAsync(CultureInfo lang);
    }
}
