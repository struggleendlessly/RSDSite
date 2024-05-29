using System;

namespace shared.Models
{
    public class CustomDomainValidationModel
    {
        public Guid WebsiteId { get; set; }
        public string Domain { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Error { get; set; }
    }

    public enum CustomDomainValidationStatus
    {
        PendingVerification,
        Verified,
        VerificationFailed
    }
}
