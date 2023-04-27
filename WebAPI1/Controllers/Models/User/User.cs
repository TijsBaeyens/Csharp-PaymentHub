namespace WebAPI1.Controllers.Models.User {
    public class User
    {
        public int Id { get; set; }
        public string Aanspreking { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        //public Adress Adress { get; set; }
        public string TelefoonNummer { get; set; }
        public string Taal { get; set; }
        public string Email { get; set; }
        public DateTime GeboorteDatum { get; set; }
        //public List<AccountNumber> RekeningNummers { get; set; }

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
    }
}
