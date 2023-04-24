using DomainLayer.Exceptions;
using DomainLayer.Interfaces;
using DomainLayer.Objects;

namespace DomainLayer.Controllers
{
    public class DomainController {
        private User _activeUser;
        private IPaymentHubRepo _repo;
        public DomainController(IPaymentHubRepo _repo) {
            this._repo = _repo;
        }
        public void RegisterUser(string email, string password, string voornaam, string achternaam, string telefoonNummer) {
            if (_repo.GetUsers().Count != 0) { 
            List<User> ua = _repo.GetUsers().FindAll(ua => ua.Email == email);
            if (ua.Count != 0) {
                throw new DomainException("Email bestaat al");
            }
            }
            UserAccount userAccount = new UserAccount(password, email);
            _repo.AddUserAccount(userAccount);
            User user = new User(voornaam, achternaam, email, telefoonNummer);
            _repo.AddUser(user);
            _activeUser = _repo.GetUserByEmail(email);
        }
        public void LoginUser(string email, string password) {
            User a = _repo.GetUsers().Find(ua => ua.Email == email);
            if (a != null) {
                if (_repo.GetUserAccountByEmail(email).Password == password) {
                    _activeUser = a;
                } else {
                    throw new DomainException("Wachtwoord is niet correct");
                }
            } else {
                throw new DomainException("Account bestaat niet");
            }
        }
        public void LogoutUser() {
            _activeUser = null;
        }
        public List<Payment> ToonOpenPayments()
        {
            return _repo.GetPaymentsByUserId(_activeUser.Id).FindAll(open => open.Betaald == false);
        }
        public List<Payment> ToonBetaaldePayments()
        {
            return _repo.GetPaymentsByUserId(_activeUser.Id).FindAll(open => open.Betaald == true);
        }
        public void BetaalPayment(int paymentId)
        {
            Payment payment = _repo.GetPaymentById(paymentId);
            if (payment != null)
            {
                _repo.SetBetaaldToTrue(paymentId);
            } else
            {
                throw new DomainException("Payment bestaat niet");
            }
        }
        public void AddPayment(decimal amount, int begunstigdeWebShopId, int userId)
        {
            _repo.AddPayment(new Payment(amount, begunstigdeWebShopId, userId));
        }

        public void AddWebshop(string name, string telefoonNummer, string email,string IBAN, User u)
        {
            _repo.AddWebshop(new Webshop(name, telefoonNummer, email, IBAN, u));
        }
        public void MakeAccountNumber(int bankId, int userId, string iBAN) 
        {
            AccountNumber accountNumber = new AccountNumber(bankId, userId, iBAN);
            _repo.AddAccountNumber(accountNumber);
        }
        public void addBank(string naam, string telefoonNummer, string email, User contactPersoon) {
            _repo.AddBank(new Bank(naam, telefoonNummer, email, contactPersoon));
        }
        public List<Bank> getBanks() {
            return _repo.GetBanks();
        }
        public List<AccountNumber> GetAllAccountNumber() {
            return _repo.GetAllAccountNumbers();
        }
        public List<AccountNumber> GetAccountNumberOnBankId(int bankId) { 
            return GetAllAccountNumber().FindAll(accountNumber => accountNumber.BankId == bankId);
        }
        public UserAccount getUserAccountByEmail(string Email) {
            return _repo.GetUserAccountByEmail(Email);
        }
        public List<Webshop> getWebshops() {
            return _repo.GetAllWebshop();
        }
        public List<User> getUsers() {
            return _repo.GetUsers();
        }
        public List<UserAccount> getUserAccounts() {
            return _repo.GetUserAccounts();
        }
        public void AddUser(string voornaam, string achternaam, string email, string telefoonNummer) {
            _repo.AddUser(new User(voornaam, achternaam, email, telefoonNummer));
        }
        public void AddUserAccount(string email, string password) {
            _repo.AddUserAccount(new UserAccount(password, email));
        }
        public Webshop getWebshopById(int id) { 
            return _repo.GetWebshopById(id);
        }
        public User getUserById(int id) {
            return _repo.GetUserById(id);
        }
        public Webshop getWebshopByName(string naam) {
            return _repo.GetWebshopByName(naam);
        }
        public List<AccountNumber> getAccountNumberByUserIdAndBankId(int userId, int BankId) {
            return _repo.GetAccountNumbersByUserIdAndBankId(userId, BankId);
        }
        public Bank GetBankById(int id) {
            return _repo.GetBankById(id);
        }
        public UserAccount GetActiveUserAccount() {
            return _repo.GetUserAccountByEmail(_activeUser.Email);
        }
        public User GetActiveUser() {
            return _activeUser;
        }
        public User GetUserByEmail(string email) {
            return _repo.GetUsers().Find(user => user.Email == email);
        }
        public Bank GetBankByName(string name) {
            return _repo.GetBankByName(name);
        }
    }
}
