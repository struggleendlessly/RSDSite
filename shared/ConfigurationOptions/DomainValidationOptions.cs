using System;

namespace shared.ConfigurationOptions
{
    public class DomainValidationOptions
    {
        public static string SectionName { get; } = "DomainValidation";
        public List<DomainValidationRecord> Records { get; set; } = new List<DomainValidationRecord>();
    }

    public class DomainValidationRecord
    {
        public string Type { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
