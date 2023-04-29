using DomainLayer.Objects;

namespace WebAPI1.Controllers.Models.Webshop1 {
    public interface IWebshopRepo {
        void AddWebshop(Webshop payment);
        Webshop GetWebshop(int id);
        IEnumerable<Webshop> GetAll();
        void RemoveWebshop(Webshop Payment);
        void UpdateWebshop(Webshop Payment);
    }
}
