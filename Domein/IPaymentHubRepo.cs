using DomainLayer.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer {
    public interface IPaymentHubRepo {
        // BANK SECTION
        public void AddBank(Bank bank);
        public List<Bank> GetBanks();
        public Bank GetBankById(int id);
        public Bank GetBankByName(string name);
        public int GetBankIdByName(string name);
        public void UpdateBankById(int Id, Bank bank);
        public void DeleteBank(int Id);


        // WEBSHOPS SECTION

        public void AddWebshop(Webshop webshop);
        public List<Webshop> GetAllWebshop();
        public Webshop GetWebshopById(int id);
        public Webshop GetWebshopByName(string name);
        public int GetWebshopIdByName(string name);
        public void UpdateWebshopById(int id, Webshop webshop);
        public void DeleteWebshopById(int id);


        // USERACCOUNT SECTION

        public void AddUserAccount(UserAccount ua);
        public List<UserAccount> GetUserAccounts();
        public UserAccount GetUserAccountById(int id);
        public UserAccount GetUserAccountByEmail(string email);
        public void UpdateUserAccountById(int id, UserAccount ua);
        public void DeleteUserAccountById(int id);


        // PAYMENT SECTION

        public void AddPayment(Payment payment);
        public List<Payment> GetPayments();
        public Payment GetPaymentById(int id);
        public List<Payment> GetPaymentsByUserId(int userId);
        public List<Payment> GetPaymentsByWebshopId(int webshopId);
        public void UpdatePayment(Payment payment);
        public void SetBetaaldToTrue(int id);
        public void DeletePayment(int id);
        public int GetPaymentIdByUserIdAndWebshopId(int userId, int webshopId);


        // USERS SECTION

        public void AddUser(User user);
        public List<User> GetUsers();
        public User GetUserById(int id);
        public User GetUserByEmail(string email);
        public int GetUserIdByEmail(string email);
        public void UpdateUserByID(int id, User u);
        
        public void DeleteUser(int id);


        // ACCOUNTNUMBER SECTION
        public List<AccountNumber> GetAccountNumbersByUserId(int id);
        public List<AccountNumber> GetAllAccountNumbers();
        public List<AccountNumber> GetAccountNumberByUserId(int id);
        public AccountNumber GetAccountNumberByBankId(int id);
        public AccountNumber GetAccountNumberByIBAN(string iban);
        public int GetAccountNumberIdByIBAN(string iban);
        public void AddAccountNumber(AccountNumber accountNumber);
        public void UpdateAccountNumber(int Id, AccountNumber accountNumber);
        public void DeleteAccountNumber(int Id);
        public List<AccountNumber> GetAccountNumbersByUserIdAndBankId(int UserId, int BankId);
    }
}
