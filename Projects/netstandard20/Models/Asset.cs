using System;
using System.Collections.Generic;
using System.Globalization;

namespace BC.Loop.Api.Client.Models {

    public class Asset {

        public string ID { get; set; }
        public string GroupID { get; set; }
        public string ObjectType { get; set; }
        public string TagID { get; set; }
        public string AssetIdentifier { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public float Temp { get; set; }
        public int BatterySoC { get; set; }
        public List<Asset> Associations { get; set; }
        public DateTime PositionObservedAt { get; set; }
        public DateTime TempObservedAt { get; set; }
        public DateTime MovingObservedAt { get; set; }
        public DateTime BattVObservedAt { get; set; }
        public DateTime SiteChangedAt { get; set; }
        public DateTime ZoneChangedAt { get; set; }
        public DateTime BatterySoCObservedAt { get; set; }

        public Asset() {
            Associations = new List<Asset>();
        }

    }

}