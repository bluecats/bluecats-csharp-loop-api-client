using System;
using System.Collections.Generic;
using System.Globalization;

namespace BC.Loop.Api.Client.Models {

    public class Asset {

        public String ID { get; set; }
        public String GroupID { get; set; }
        public String ObjectType { get; set; }
        public String TagID { get; set; }
        public String AssetIdentifier { get; set; }
        public String Description { get; set; }
        public String Type { get; set; }
        public Double Lat { get; set; }
        public Double Long { get; set; }
        public List<Association> Associations { get; set; }
        public DateTime PositionObservedAt { get; set; }
        public DateTime TempObservedAt { get; set; }
        public DateTime MovingObservedAt { get; set; }
        public DateTime BattVObservedAt { get; set; }
        public DateTime SiteChangedAt { get; set; }
        public DateTime ZoneChangedAt { get; set; }
        public DateTime BatterySoCObservedAt { get; set; }

        public Asset() {
            Associations = new List<Association>();
        }

        public string LastHeard {
            get {
                var lastHeard = PositionObservedAt;
                if (TempObservedAt.CompareTo( lastHeard ) > 0) {
                    lastHeard = TempObservedAt;
                }
                if (MovingObservedAt.CompareTo( lastHeard ) > 0) {
                    lastHeard = MovingObservedAt;
                }
                if (BattVObservedAt.CompareTo( lastHeard ) > 0) {
                    lastHeard = BattVObservedAt;
                }
                if (SiteChangedAt.CompareTo( lastHeard ) > 0) {
                    lastHeard = SiteChangedAt;
                }
                if (ZoneChangedAt.CompareTo( lastHeard ) > 0) {
                    lastHeard = ZoneChangedAt;
                }
                if (BatterySoCObservedAt.CompareTo( lastHeard ) > 0) {
                    lastHeard = BatterySoCObservedAt;
                }
                if (lastHeard == DateTime.MinValue) {
                    return "--";
                }
                return lastHeard.ToString( "HH:mm dd MMM yyyy", CultureInfo.CurrentCulture );
            }
        }

    }

}