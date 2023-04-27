namespace WebAPI1.Controllers.Models.Payment
{
    public class PaymentException : Exception
    {
        public PaymentException(string message) : base(message)
        {

        }
    }
}
