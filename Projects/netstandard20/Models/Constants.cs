namespace BC.Loop.Api.Client.Models {

    public class Constants {

        internal const string TIMESTAMP_FORMAT = "yyyy-MM-ddTHH:mm:ss.ffffffZ";

        internal const string GROUPS_QUERY = "{\"from\":\"groups\",\"select\":[{\"value\":\"id\"},{\"value\":\"type\"},{\"value\":\"name\"},{\"value\":\"parentID\"},{\"value\":\"abbreviation\"},{\"value\":\"placeParentID\"},{\"count\":\"all\"}]}";

    }

}