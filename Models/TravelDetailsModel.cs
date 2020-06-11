using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Travel_Express.Models
{
    public class TravelDetailsModel
    {
        public TravelDetailsModel() { }
        public TravelDetailsModel(Preferences pref, TravelListModel travelModel)
        {
            TravelModel = travelModel;

            AcceptPet = pref.AcceptPet;
            AcceptSmoke = pref.AcceptSmoke;
            AcceptTalking = pref.AcceptTalking;
            AcceptEveryone = pref.AcceptEveryone;
            AcceptMusic = pref.AcceptMusic;
            AcceptDeviation = pref.AcceptDeviation;
        }

        public TravelListModel TravelModel { get; set; }

        [Display(Name = "Email")]
        public string Driver { get { return TravelModel.Driver; } }

        [Display(Name = "De")]
        public string From { get { return TravelModel.From; } }

        [Display(Name = "À")]
        public string To { get { return TravelModel.From; } }

        [Display(Name = "Départ")]
        public DateTime StartTime { get { return TravelModel.StartTime; } }

        [Display(Name = "Arrivée")]
        public DateTime EndTime { get { return TravelModel.EndTime; } }

        [Display(Name = "Sièges diponibles")]
        public int AvailableSeats { get { return TravelModel.AvailableSeats; } }

        [Display(Name = "Nombre de sièges")]
        public int TotalSeats { get { return TravelModel.TotalSeats; } }

        [Display(Name = "Animaux de compagnie")]
        public bool AcceptPet { get; set; }

        [Display(Name = "Fumer")]
        public bool AcceptSmoke { get; set; }

        [Display(Name = "Musique")]
        public bool AcceptMusic { get; set; }

        [Display(Name = "Bavard")]
        public bool AcceptTalking { get; set; }

        [Display(Name = "Détours")]
        public bool AcceptDeviation { get; set; }

        [Display(Name = "N'importe qui")]
        public bool AcceptEveryone { get; set; }


    }
}
