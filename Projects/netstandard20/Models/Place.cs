using System;

namespace BC.Loop.Api.Client.Models {

    public class Place {

        public string ID { get; set; }
        public string GroupID { get; set; }
        public string ObjectType { get; set; }
        public string Name { get; set; }
        public string PlaceType { get; set; }
        public string FullName { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public string ParentPlace { get; set; }
        public string Zones { get; set; }

    }
    
}