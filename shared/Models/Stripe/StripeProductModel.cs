using System;

namespace shared.Models.Stripe
{
    public class StripeProductModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<StripePriceModel> Prices { get; set; } = new List<StripePriceModel>();
    }
}
