namespace AuthenticationApp.Core.Entities
{
    public class UserCredentials : BaseEntity
    {
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
