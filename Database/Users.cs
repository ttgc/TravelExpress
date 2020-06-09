using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;

namespace Travel_Express.Database
{
    public partial class Users
    {
        public Users()
        {
            Booking = new HashSet<Booking>();
            Travel = new HashSet<Travel>();
        }

        public string Mail { get; set; }
        public string PasswordHash { get; set; }
        public bool? AcceptPet { get; set; }
        public bool? AcceptSmoke { get; set; }
        public bool? AcceptMusic { get; set; }
        public bool? AcceptTalking { get; set; }
        public bool? AcceptDeviation { get; set; }
        public bool? AcceptEveryone { get; set; }

        public virtual ICollection<Booking> Booking { get; set; }
        public virtual ICollection<Travel> Travel { get; set; }
    }
}
