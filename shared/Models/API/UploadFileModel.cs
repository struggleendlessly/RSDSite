using System;

namespace shared.Models.API
{
    public class UploadFileModel
    {
        public string SiteName { get; set; } = string.Empty;
        public string BlobName { get; set; } = string.Empty;
        public byte[] FileData { get; set; } = [];
    }
}
