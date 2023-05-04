using DomainLayer.Objects;
using PersistenceLayer;

namespace WebAPI1.Controllers.Models.Webshop1 {
    public class WebshopRepo : IWebshopRepo {

        private PaymentHubRepo _repo;

        public WebshopRepo() {
            string conn = System.Configuration.ConfigurationManager.ConnectionStrings["ADOconnSQL"].ConnectionString;
            _repo = new PaymentHubRepo(conn);
        }

        public void AddWebshop(Webshop webshop) {
            _repo.AddWebshop(webshop);
        }

        public IEnumerable<Webshop> GetAll() {
            return _repo.GetAllWebshop();
        }

        public Webshop GetWebshop(int id) {
            return _repo.GetWebshopById(id);
        }

        public void RemoveWebshop(Webshop webshop) {
            _repo.DeleteWebshopById(webshop.Id);
        }

        public void UpdateWebshop(Webshop webshop) {
            _repo.UpdateWebshopById(webshop.Id, webshop);
        }
    }
}
