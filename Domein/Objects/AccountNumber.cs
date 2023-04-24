namespace DomainLayer.Objects
{
    public class AccountNumber
    {
        public int Id { get; set; }
        public int BankId { get; set; }
        public int UserId { get; set; }
        public string IBAN { get; set; }

        public AccountNumber(int bankId, int userId, string iBAN)
        {
            BankId = bankId;
            UserId = userId;
            IBAN = iBAN;
        }

        public AccountNumber() {
        }
    }
}
