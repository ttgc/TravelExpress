using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Travel_Express.Models
{
    public class Preferences
    {
        [Display(Name = "J'accepte les animaux de compagnie")]
        public bool AcceptPet { get; set; }

        [Display(Name = "J'accepte qu'on fume")]
        public bool AcceptSmoke { get; set; }

        [Display(Name = "J'accepte la musique")]
        public bool AcceptMusic { get; set; }

        [Display(Name = "Je suis bavard")]
        public bool AcceptTalking { get; set; }

        [Display(Name = "J'accepte les détours")]
        public bool AcceptDeviation { get; set; }

        [Display(Name = "J'accepte n'importe qui")]
        public bool AcceptEveryone { get; set; }
    }
}
