using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlueCats.Loop.Api.Client {

    public static class SchemaHelper {

        public static string[] GetObjectTypes( string schemaJson ) {
            var schema = JObject.Parse( schemaJson );
            var objectTypes = schema[ "objectTypes" ].Children().Select( jToken => (string) jToken ).ToArray();
            return objectTypes;
        }

    }

}