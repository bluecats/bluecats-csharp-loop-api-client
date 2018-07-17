namespace BlueCats.Loop.Api.Client.Models {

    public class ObjectTypePropertyInfo {

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Type { get; set; }
        public bool Required { get; set; }
        public bool? Editable { get; set; }
        public bool? Populated { get; set; }
        public string TimeCol { get; set; }
        public int? Length { get; set; }
        public string Link { get; set; }

    }

}