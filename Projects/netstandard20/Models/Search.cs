using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BC.Loop.Api.Client.Models {

    public class Search {

        private ILoopClient _loopClient;

        public string ObjectType { get; set; }
        public List<object> Select { get; set; }
        public List<object> Where { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }

        public Search(ILoopClient loopClient) {
            _loopClient = loopClient;

            Select = new List<object>();
            Where = new List<object>();
            Offset = 0;
            Limit = 100;
        }

        public async Task<JArray> SearchAsync() {
            var response = await this.PageResults();

            var count = (int)JObject.Parse( response )[ "count" ];
            var results = (JArray)JObject.Parse( response )[ "Place" ];
            while (results.Count < count) {
                this.Offset += this.Limit;

                response = await this.PageResults();
                results.Merge( (JArray)JObject.Parse( response )[ "Place" ] );
            }
            return results;
        }

        private async Task<string> PageResults() {
            var request = JsonConvert.SerializeObject( this.GetSearchObject() );
            var response = await _loopClient.PostSearchAsync( request ).ConfigureAwait( false );

            return response;
        }

        private object GetSearchObject() {
            return new {
                from = this.ObjectType,
                select = this.Select,
                offset = this.Offset,
                limit = this.Limit,
                where = this.Where
            };
        }

    }
    
}