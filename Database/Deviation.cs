using System;
using System.Collections.Generic;

namespace Travel_Express.Database
{
    public partial class Deviation
    {
        public int IdTravel { get; set; }
        public int? Addr { get; set; }
        public int DeviationOrder { get; set; }

        public virtual Travel IdTravelNavigation { get; set; }
    }
}
