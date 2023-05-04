using DomainLayer.Objects;

namespace WebAPI1.Controllers.Models.Bank1 {
    public interface IBankRepo {
        void AddBank(Bank bank);
        Bank GetBank(int id);
        IEnumerable<Bank> GetAll();
        void RemoveBank(Bank bank);
        void UpdateBank(Bank bank);
    }
}
