using System;

namespace shared.Models.API
{
    public class RenameContainerModel
    {
        public string SourceContainerName { get; set; } = default!;
        public string DestinationContainerName { get; set; } = default!;
    }
}
