using System;
using System.Threading;

namespace BlueCats.Loop.Api.Client.Models {

    public class PaginatedEvents {

        public Event[] Events { get; set; }
        public int Count { get; set; }
        public string LastKeyID { get; set; }
        public DateTime? LastKeyTS { get; set; }

    }

}