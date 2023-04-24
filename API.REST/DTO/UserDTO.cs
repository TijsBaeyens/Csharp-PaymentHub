using System.ComponentModel.DataAnnotations;

#if ProducesConsumes
#endif

namespace API.REST.DTO {
    public class UserDTO {
        [Required]
        public int Id { get; set; }

        [MaxLength(10)]
        public string Aanspreking { get; set; }
        

        [Required]
        [MaxLength(50)]
        public string Voornaam { get; set; }
        


        [Required]
        [MaxLength(50)]
        public string Achternaam { get; set; }
        

        public string StraatNaam { get; set; }
        [MaxLength(10)]
        public string HuisNummer { get; set; }
        
        [MaxLength(50)]
        public string Gemeente { get; set; }
        

        [Required]
        [MaxLength(50)]
        public string TelefoonNummer { get; set; }
        
        [MaxLength(50)]
        public string Land { get; set; }
        
        [MaxLength(50)]
        public string Taal { get; set; }
        
        [MaxLength(50)]
        public DateOnly GeboorteDatum { get; set; }
        
        [MaxLength(10)]
        public string BusNummer { get; set; }
        

        [Required]
        public string Email { get; set; }
    }
}
