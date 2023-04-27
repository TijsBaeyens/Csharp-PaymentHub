using System.Diagnostics.Metrics;

namespace WebAPI1.Controllers.Models.Payment
{
    public class PaymentRepo : IPaymentRepo
    {
        private readonly Dictionary<int, Payment> _data = new();

        public PaymentRepo()
        {
            _data.Add(1, new Payment(1, 90.32m, 1, 1, DateTime.Now));
            _data.Add(2, new Payment(2, 91.32m, 2, 2, DateTime.Now));
            _data.Add(3, new Payment(3, 92.32m, 3, 3, DateTime.Now));
            _data.Add(4, new Payment(4, 93.32m, 4, 4, DateTime.Now));
            _data.Add(5, new Payment(5, 94.32m, 5, 5, DateTime.Now));
        }

        public void AddPayment(Payment payment)
        {
            if (!_data.ContainsKey(payment.Id))
            {
                _data.Add(payment.Id, payment);
            }
            else
            {
                throw new PaymentException("Payment already exists");
            }
        }

        public IEnumerable<Payment> GetAll()
        {
            return _data.Values;
        }

        public Payment GetPayment(int id)
        {
            if (_data.ContainsKey(id))
            {
                return _data[id];
            }
            else
            {
                throw new PaymentException("Payment not found");
            }
        }

        public void RemovePayment(Payment Payment)
        {
            if (_data.ContainsKey(Payment.Id))
            {
                _data.Remove(Payment.Id);
            }
            else
            {
                throw new PaymentException("Payment not found");
            }
        }

        public void UpdatePayment(Payment Payment)
        {
            if (_data.ContainsKey(Payment.Id))
            {
                _data[Payment.Id] = Payment;
            }
            else
            {
                throw new PaymentException("Payment not found");
            }
        }
        public IEnumerable<Payment> GetAll(string bedrag)
        {
            return _data.Values.Where(p => p.Bedrag.ToString().Contains(bedrag));
        }
    }
}
