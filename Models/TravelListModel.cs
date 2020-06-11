using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Travel_Express.Models
{
    public class TravelListModel
    { 
        public int ID { get; set; }

        [Display(Name = "Email")]
        public string Driver { get; set; }

        [Display(Name = "De")]
        public string From { get; set; }

        [Display(Name = "À")]
        public string To { get; set; }

        [Display(Name = "Départ")]
        public DateTime StartTime { get; set; }

        [Display(Name = "Arrivée")]
        public DateTime EndTime { get; set; }

        [Display(Name = "Sièges diponibles")]
        public int AvailableSeats { get; set; }

        [Display(Name = "Nombre de sièges")]
        public int TotalSeats { get; set; }
    }
}
