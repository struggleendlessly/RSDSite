using System;

namespace shared.Models.Stripe
{
    public class StripePriceModel
    {
        public string Currency { get; set; }
        public long UnitAmount { get; set; }
        public string? Interval { get; set; }
    }
}
