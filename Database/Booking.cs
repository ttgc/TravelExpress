using System;
using System.Collections.Generic;

namespace Travel_Express.Database
{
    public partial class Booking
    {
        public int IdTravel { get; set; }
        public string Author { get; set; }
        public bool? Pending { get; set; }
        public int? Seats { get; set; }

        public virtual Users AuthorNavigation { get; set; }
        public virtual Travel IdTravelNavigation { get; set; }
    }
}
