using DomainLayer.Interfaces;
namespace DomainLayer.Objects
{
    public class UserAccount : IUserAccounts
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public UserAccount(int id, string password, string email)
        {
            Id = id;
            Password = password;
            Email = email;
        }

        public UserAccount(string password, string email) {
            Password = password;
            Email = email;
        }

        public UserAccount() {
        }

        public void setEmail(string email) {
            Email = email;
        }
    }
}
