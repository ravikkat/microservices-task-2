using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssesmentTask2.Models
{
    public class Booking
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime? Date { get; set; }

        

        public string Venue { get; set; }
        public int NoofTickets { get; set; }

        public double? Amount { get; set; }

        public string Currency { get; set; }

        public String SGDAmount { get; set; }
    }
}
