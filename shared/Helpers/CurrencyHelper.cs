using System;

namespace shared.Helpers
{
    public static class CurrencyHelper
    {
        // Conversion factor: 1 euro = 100 cents
        private const long EurosConversionFactor = 100;

        public static decimal ConvertCentsToEuros(long cents)
        {
            return cents / (decimal)EurosConversionFactor;
        }

        public static string GetCurrencySymbol(string currencyCode)
        {
            switch (currencyCode.ToLower())
            {
                case "usd":
                    return "$";
                case "eur":
                    return "€";
                default:
                    return currencyCode;
            }
        }
    }
}
