using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Event
    {
        public string EventID { get; set; }

        public string EventName { get; set; }

        public string Room { get; set; }

        public string Class { get; set; }

        public DateTime EventTime { get; set; }
    }
}
