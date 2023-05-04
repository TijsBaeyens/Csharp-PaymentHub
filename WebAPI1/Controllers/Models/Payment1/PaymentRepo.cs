using PersistenceLayer;
using System.Diagnostics.Metrics;
using DomainLayer;
using DomainLayer.Objects;
using DomainLayer.Interfaces;

namespace WebAPI1.Controllers.Models.Payment1
{
    public class PaymentRepo : IPaymentRepo
    {
        private PaymentHubRepo _repo;
        
        public PaymentRepo()
        {
            string conn = System.Configuration.ConfigurationManager.ConnectionStrings["ADOconnSQL"].ConnectionString;
            _repo = new PaymentHubRepo(conn);
        }

        public void AddPayment(Payment payment)
        {
            if (!_repo.GetAllPayments().Contains(payment))
            {
                _repo.AddPayment(payment);
            }
            else
            {
                throw new PaymentException("Payment already exists");
            }
        }

        public IEnumerable<Payment> GetAll()
        {
            return _repo.GetAllPayments();
        }

        public Payment GetPayment(int id)
        {
            if (_repo.GetPaymentById(id) != null)
            {
                return _repo.GetPaymentById(id);
            }
            else
            {
                throw new PaymentException("Payment not found");
            }
        }

        public void RemovePayment(Payment Payment)
        {
            if (_repo.GetPaymentById(Payment.Id) != null)
            {
                _repo.DeletePayment(Payment.Id);
            }
            else
            {
                throw new PaymentException("Payment not found");
            }
        }

        public void UpdatePayment(Payment Payment)
        {
            if (_repo.GetPaymentById(Payment.Id) != null)
            {
                _repo.UpdatePayment(Payment);
            }
            else
            {
                throw new PaymentException("Payment not found");
            }
        }
        public IEnumerable<Payment> GetAll(string bedrag)
        {
            return _repo.GetAllPayments().Where(p => p.Bedrag.ToString().Contains(bedrag));
        }
    }
}
