using System.Diagnostics.Metrics;

namespace WebAPI1.Controllers.Models {
    public interface IPaymentRepo {
        void AddPayment(Payment payment);
        Payment GetPayment(int id);
        IEnumerable<Payment> GetAll();
        void RemovePayment(Payment Payment);
        void UpdatePayment(Payment Payment);
        IEnumerable<Payment> GetAll(string bedrag);
    }
}
