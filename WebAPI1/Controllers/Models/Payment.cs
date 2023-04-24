namespace WebAPI1.Controllers.Models {
    public class Payment {
        public int Id { get; set; }
        public decimal Bedrag { get; set; }
        public int BegunstigeWebshopId { get; set; }
        public int UserId { get; set; }
        public bool Betaald { get; set; }
        public DateTime Datum { get; set; }


        public Payment(int id, decimal bedrag, int begunstigeWebshopId, int userId, DateTime datum) {
            Id = id;
            Bedrag = bedrag;
            BegunstigeWebshopId = begunstigeWebshopId;
            UserId = userId;
            Betaald = false;
            Datum = datum;

        }

        public Payment(decimal bedrag, int begunstigeWebshopId, int userId) {
            Bedrag = bedrag;
            BegunstigeWebshopId = begunstigeWebshopId;
            UserId = userId;
            Betaald = false;
            Datum = DateTime.Now;
        }

        public Payment() {
        }
    }
}
