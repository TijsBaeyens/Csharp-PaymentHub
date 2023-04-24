using DomainLayer.Interfaces;

namespace DomainLayer.Objects
{
    public class Webshop : IWebshop
    {
        public int Id { get; set; }
        public string Naam { get; set; }
        public string TelefoonNummer { get; set; }
        public string Email { get; set; }
        public string IBAN { get; set; }
        public User ContactPersoon { get; set; }

        public Webshop(string naam, string telefoonNummer, string email, string iban, User contactPersoon)
        {
            Naam = naam;
            TelefoonNummer = telefoonNummer;
            Email = email;
            IBAN = iban;
            ContactPersoon = contactPersoon;
        }

        public Webshop() {
        }
    }
}
