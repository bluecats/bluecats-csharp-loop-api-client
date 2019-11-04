using System;

namespace BC.Loop.Api.Client.Models {

    public class Group {

        public string Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Type { get; set; }
        public string ParentId { get; set; }

    }

}