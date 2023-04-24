namespace DomainLayer.Objects
{
    public class Adress
    {
        public string StraatNaam { get; set; }
        public string HuisNummer { get; set; }
        public string Postcode { get; set; }
        public string Gemeente { get; set; }
        public string Land { get; set; }
        public string BusNummer { get; set; }

        public Adress(string straatNaam, string huisNummer, string postcode, string gemeente, string land, string busNummer)
        {
            StraatNaam = straatNaam;
            HuisNummer = huisNummer;
            Postcode = postcode;
            Gemeente = gemeente;
            Land = land;
            if (busNummer != null) {
                BusNummer = busNummer;
            } else {
                BusNummer = "";
            }
        }
    }
}
