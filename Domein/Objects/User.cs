using DomainLayer.Exceptions;
using DomainLayer.Interfaces;

namespace DomainLayer.Objects
{
    public class User : IUser
    {

        public int Id { get; set; }
        public string Aanspreking { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public Adress Adress { get; set; }
        public string TelefoonNummer { get; set; }
        public string Taal { get; set; }
        public string Email { get; set; }
        public DateTime GeboorteDatum { get; set; }
        public List<AccountNumber> RekeningNummers { get; set; }

        public User(int id, string voornaam, string achternaam)
        {
            Id = id;
            Voornaam = voornaam;
            Achternaam = achternaam;
        }

        public User() {
        }

        public User(string voornaam, string achternaam, string email, string telefoonNummer) {
            Voornaam = voornaam;
            Achternaam = achternaam;
            Email = email;
            TelefoonNummer = telefoonNummer;
        }

        public void SetId(int id)
        {
            if (id <= 0) throw new DomainException("User-setId");
            Id = id;
        }
        public void SetTelefoonNummer(string telefoonNummer)
        {
            TelefoonNummer = telefoonNummer;
        }
        public void SetTaal(string taal)
        {
            List<string> talen = new List<string>();
            talen.Add("NL");
            talen.Add("FR");
            talen.Add("DE");
            talen.Add("EN");

            if (talen.Contains(taal) == false) throw new DomainException("User-setTaal");
            Taal = taal;
        }
        public void SetEmail(string email)
        {
            if (email.Contains("@") == false) throw new DomainException("User-setEmail");
            Email = email;
        }
        public void SetBirthday(DateTime gb)
        {
            DateTime minDate = new DateTime(1900, 1, 1);
            DateTime maxDate = DateTime.Today.AddYears(-18);

            if (gb < minDate || gb > maxDate)
            {
                throw new DomainException("User-setBirthday");
            }

            GeboorteDatum = gb;
        }
        public void addRekeningNumber(AccountNumber an)
        {
            RekeningNummers.Add(an);
        }
        public List<AccountNumber> getRekeningNumbers() { return RekeningNummers; }

        public void AddAccountNumberToUser(AccountNumber an) {
            this.RekeningNummers = new List<AccountNumber>();
            RekeningNummers.Add(an);
        }
        public void AddAdressToUser(Adress a) {
            this.Adress = a;
        }
    }
}
