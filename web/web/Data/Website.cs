namespace web.Data
{
    public class Website
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
    }
}
