namespace WebAPI1.Controllers.Models.Bank1 {
    public class BankException : Exception
    {
        public BankException(string? message) : base(message)
        {
        }

        public BankException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
