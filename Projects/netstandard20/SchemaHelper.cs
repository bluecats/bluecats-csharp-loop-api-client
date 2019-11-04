using System;
using System.Collections.Generic;
using System.Linq;
using BC.Loop.Api.Client.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BC.Loop.Api.Client {

    public static class SchemaHelper {

        public static string[] GetObjectTypes( string schemaJson ) {
            var schema = JObject.Parse( schemaJson );
            var objectTypes = schema[ "objectTypes" ].Children().Select( jToken => (string) jToken ).ToArray();
            return objectTypes;
        }

        public static ObjectTypeInfo GetObjectTypeInfo( string schemaJson, string objectType ) { 
            var schema = JObject.Parse( schemaJson ); 
            var objectTypeProperties = schema[ "objects" ][ objectType ]; 
            var objectTypeInfo = objectTypeProperties.ToObject< ObjectTypeInfo >(); 
            return objectTypeInfo; 
        } 

    }

}