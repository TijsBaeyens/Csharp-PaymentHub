namespace WebAPI1.Controllers.Models {
    public class PaymentException : Exception{
        public PaymentException(string message) : base(message) {
            
        }
    }
}
