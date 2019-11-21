using System;
using System.Collections.Generic;

namespace BC.Loop.Api.Client.Models {

    public class Asset {

        public String ID { get; set; }
        public String GroupID { get; set; }
        public String ObjectType { get; set; }
        public String TagID { get; set; }
        public String EddystoneUID { get; set; }
        public String AssetIdentifier { get; set; }
        public String Description { get; set; }
        public List<Association> Associations { get; set; }

        public Asset() {
            Associations = new List<Association>();
        }

    }

}