using System;
using System.Collections.Generic;
using System.Globalization;

namespace BC.Loop.Api.Client.Models
{

    public class Asset
    {

        public static List<object> Select {
            get {
                return new List<object>() {
                    new { value = "id" },
                    new { value = "groupID" },
                    new { value = "objectType" },
                    new { value = "type" },
                    new { value = "tagID" },
                    new { value = "assetIdentifier" },
                    new { value = "description" },
                    new { value = "damaged" },
                    new { value = "batterySoC" },
                    new { value = "batterySoCObservedAt" },
                    new { value = "temp" },
                    new { value = "tempObservedAt" },
                    new { value = "battV" },
                    new { value = "battVObservedAt" },
                    new { value = "long" },
                    new { value = "lat" },
                    new { value = "horizErr" },
                    new { value = "positionObservedAt" },
                    new { value = "positionChangedAt" },
                    new { value = "moving" },
                    new { value = "movingObservedAt" },
                    new { value = "movingChangedAt" },
                    new { value = "site" },
                    new { value = "distanceFromSite" },
                    new { value = "siteObservedAt" },
                    new { value = "siteChangedAt" },
                    new { value = "zone" },
                    new { value = "distanceFromZone" },
                    new { value = "zonePath" },
                    new { value = "zoneObservedAt" },
                    new { value = "zoneChangedAt" },
                    new { value = "locationSource" }
                };
            }
        }

        public string ID { get; set; }
        public string GroupID { get; set; }
        public string ObjectType { get; set; }
        public string Type { get; set; }
        public string TagID { get; set; }
        public string AssetIdentifier { get; set; }
        public string Description { get; set; }
        public bool Damaged { get; set; }
        public int BatterySoC { get; set; }
        public DateTime BatterySoCObservedAt { get; set; }
        public float Temp { get; set; }
        public DateTime TempObservedAt { get; set; }
        public float BattV { get; set; }
        public DateTime BattVObservedAt { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public int HorizontalError { get; set; }
        public DateTime PositionObservedAt { get; set; }
        public DateTime PositionChangeddAt { get; set; }
        public bool Moving { get; set; }
        public DateTime MovingObservedAt { get; set; }
        public DateTime MovingChangedAt { get; set; }
        public string Site { get; set; }
        public string SiteName { get; set; }
        public float DistanceFromSite { get; set; }
        public DateTime SiteObservedAt { get; set; }
        public DateTime SiteChangedAt { get; set; }
        public string Zone { get; set; }
        public string ZoneName { get; set; }
        public float DistanceFromZone { get; set; }
        public string ZonePath { get; set; }
        public DateTime ZoneObservedAt { get; set; }
        public DateTime ZoneChangedAt { get; set; }
        public string LocationSource { get; set; }
        public bool HasAssetAssociation { get; set; }
        public List<Asset> Associations { get; set; }

        public Asset() {
            Associations = new List<Asset>();
        }

    }

}