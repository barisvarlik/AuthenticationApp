namespace AuthenticationApp.Core.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserCredentials Credentials { get; set; }
    }
}
