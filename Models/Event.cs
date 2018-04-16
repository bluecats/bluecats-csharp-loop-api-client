using System;
using System.Collections.Generic;

namespace BlueCats.Loop.Events.Api.Client.Models {

    public class Event {

        public DateTime Timestamp { get; set; }
        public string ObjectID { get; set; }
        public string ObjectType { get; set; }
        public string EventID { get; set; }
        public string EventIDType { get; set; }
        public string EventType { get; set; }
        public Dictionary< string, string > EventBody { get; set; }
        public string EdgeMAC { get; set; }
        public string GroupID { get; set; }
    }
}