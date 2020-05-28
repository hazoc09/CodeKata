using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CodeKata.Models
{
    public class Trip
    {
        public string driverName { get; set; }

        public TimeSpan startTime { get; set; }

        public TimeSpan endTime { get; set; }

        public double milesDriven { get; set; }

        public double hoursDriven { get; set; }

        public double speed { get; set; }
    }
}