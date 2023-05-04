using DomainLayer.Objects;
using PersistenceLayer;

namespace WebAPI1.Controllers.Models.Bank1 {
    public class BankRepo : IBankRepo
    {
        private PaymentHubRepo _repo;

        public void AddBank(Bank bank)
        {
            _repo.AddBank(bank);
        }

        public IEnumerable<Bank> GetAll()
        {
            return _repo.GetBanks();
        }

        public Bank GetBank(int id)
        {
            return _repo.GetBankById(id);
        }

        public void RemoveBank(Bank bank)
        {
            _repo.DeleteBank(bank.Id);
        }

        public void UpdateBank(Bank bank)
        {
            _repo.UpdateBankById(bank.Id, bank);
        }
    }
}
