using System;

namespace shared.Data.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid? FacebookId { get; set; }
        public Guid? GoogleId { get; set; }
        public Guid? MicrosoftId { get; set; }

        public ICollection<Website> Websites { get; set; } = [];
    }
}
