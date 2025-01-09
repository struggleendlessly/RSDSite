using shared.Models.Stripe;

namespace shared.Interfaces
{
    public interface IStripeService
    {
        Task<List<StripeProductModel>> GetProductsWithPricesAsync(string lang);
    }
}
