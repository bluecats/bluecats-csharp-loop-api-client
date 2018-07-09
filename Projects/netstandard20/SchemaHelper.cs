using System;
using System.Collections.Generic;
using System.Linq;
using BlueCats.Loop.Api.Client.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlueCats.Loop.Api.Client {

    public static class SchemaHelper {

        public static string[] GetObjectTypes( string schemaJson ) {
            var schema = JObject.Parse( schemaJson );
            var objectTypes = schema[ "objectTypes" ].Children().Select( jToken => (string) jToken ).ToArray();
            return objectTypes;
        }

        public static ICollection< ObjectTypePropertyInfo > GetObjectTypeInfo( string schemaJson, string objectType ) {
            return null;
        }

    }

}