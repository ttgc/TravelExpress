using System;
using System.Collections.Generic;

namespace Travel_Express.Database
{
    public partial class Travel
    {
        public Travel()
        {
            Booking = new HashSet<Booking>();
            Deviation = new HashSet<Deviation>();
        }

        public int IdTravel { get; set; }
        public string Driver { get; set; }
        public int? From { get; set; }
        public int? To { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public int? Seats { get; set; }

        public virtual Users DriverNavigation { get; set; }
        public virtual Address FromNavigation { get; set; }
        public virtual Address ToNavigation { get; set; }
        public virtual ICollection<Booking> Booking { get; set; }
        public virtual ICollection<Deviation> Deviation { get; set; }
    }
}
