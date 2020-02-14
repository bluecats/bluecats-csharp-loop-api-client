using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BC.Loop.Api.Client.Models;
using BC.Loop.Api.Client.netstandard20;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BC.Loop.Api.Client {

    /// <summary>
    /// Provides an async interface to the BlueCats Loop web API 
    /// </summary>
    public class LoopClient: ILoopClient {

        public bool IsAuthenticated() => !string.IsNullOrEmpty( _authString );

        private readonly HttpClient _client;
        private string _authString;


        /// <summary>
        /// Initializes a new instance of the <see cref="LoopClient"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        public LoopClient( string baseUrl ) {
            if ( baseUrl == null ) throw new ArgumentNullException( nameof(baseUrl) );
            var baseUrl1 = new Uri( baseUrl );
            _client = new HttpClient { BaseAddress = baseUrl1 };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Add( "X-API-HEADER", "1" );
        }

        /// <summary>
        /// Authenticates with the Loop API asynchronously. This is required before making any other API calls.
        /// </summary>
        /// <param name="email">Loop user email.</param>
        /// <param name="password">Loop user password.</param>
        /// <returns>The authenticated user info</returns>
        /// <exception cref="System.Exception">Received an empty auth token from API</exception>
        public async Task< User > LoginAsync( string email, string password ) {
            if ( email == null ) throw new ArgumentNullException( nameof(email) );
            if ( password == null ) throw new ArgumentNullException( nameof(password) );

            if ( IsAuthenticated() ) {
                _client.DefaultRequestHeaders.Remove( "Authorization" );
                _authString = null;
            }

            // Request
            // Create json string from a C# Anonymous Class and JObject to serialize
            var json = JObject.FromObject( new { email, password } ).ToString();
            var route = new Uri( "login", UriKind.Relative );
            var request = _client.PostAsync( route, new StringContent( json, Encoding.ASCII, "application/json" ) );

            // Response
            var response = await UnwrapResponseStringAsync( request ).ConfigureAwait( false );
            try {
                var jsonObj = JObject.Parse( response );
                _authString = (string)jsonObj[ "auth" ];
                if (string.IsNullOrEmpty( _authString )) {
                    throw new Exception( "Received an empty auth token from API" );
                }
                _client.DefaultRequestHeaders.Add( "Authorization", _authString );
                var user = JsonConvert.DeserializeObject<User>( response );
                return user;
            } catch {
                throw new Exception( response );
            }
        }

		/// <summary>
		/// Logs Out and clears the current session and auth details.
		/// </summary>
		public void Logout() {

			_client.DefaultRequestHeaders.Remove( "Authorization" );
			_authString = string.Empty;

			// Ultimtely we should send an event to the cloud if required?

		}

		/// <summary>
		/// Gets the paginated Loop Events asynchronously.
		/// </summary>
		/// <param name="objectType">Type of the Loop Object.</param>
		/// <param name="objectID">The Loop Object identifier.</param>
		/// <param name="eventType">Type of the Loop Event.</param>
		/// <param name="lastKeyID">The identifier for the key of the page to start on.</param>
		/// <param name="lastKeyTimestamp">The timestamp of the page to start on.</param>
		/// <param name="limit">The Page limit.</param>
		/// <param name="startTime">The window start time.</param>
		/// <param name="endTime">The window end time.</param>
		/// <returns>The paginated events</returns>
		public async Task<string> GetEventsAsync( string objectType, string objectID, string eventType = null, string lastKeyID = null, DateTime? lastKeyTimestamp = null, int? limit = null, DateTime? startTime = null, DateTime? endTime = null) {
            if (objectType == null) throw new ArgumentNullException(nameof(objectType));
            if (objectID == null) throw new ArgumentNullException(nameof(objectID));
            EnsureAuthenticated();

            // Request
            const string ROUTE = "events";
            var queryStr = QueryHelpers.AddQueryString(ROUTE, nameof(objectType), objectType);
            queryStr = QueryHelpers.AddQueryString(queryStr, nameof(objectID), objectID);
            if (!string.IsNullOrEmpty(eventType)) {
                queryStr = QueryHelpers.AddQueryString(queryStr, nameof(eventType), eventType);
            }
            if (!string.IsNullOrEmpty(lastKeyID)) {
                queryStr = QueryHelpers.AddQueryString(queryStr, nameof(lastKeyID), lastKeyID);
            }
            if (lastKeyTimestamp.HasValue) {
                var timestamp = lastKeyTimestamp.Value.ToUniversalTime().ToString(Constants.TIMESTAMP_FORMAT);
                queryStr = QueryHelpers.AddQueryString(queryStr, "lastKeyTS", timestamp);
            }
            if (limit.HasValue) {
                queryStr = QueryHelpers.AddQueryString(queryStr, nameof(limit), limit.ToString());
            }
            if (startTime.HasValue && endTime.HasValue) {
                var formattedTimestamp = startTime.Value.ToUniversalTime().ToString(Constants.TIMESTAMP_FORMAT);
                queryStr = QueryHelpers.AddQueryString(queryStr, "tsStart", formattedTimestamp);
                formattedTimestamp = endTime.Value.ToUniversalTime().ToString(Constants.TIMESTAMP_FORMAT);
                queryStr = QueryHelpers.AddQueryString(queryStr, "tsEnd", formattedTimestamp);
            }
            var uri = new Uri(queryStr, UriKind.Relative);
            var request = _client.GetAsync(uri);

            return await UnwrapResponseStringAsync( request ).ConfigureAwait( false );

            /*
            // Response
            var jsonContent = await UnwrapResponseStringAsync(request).ConfigureAwait(false);
            var loopEvents = JsonConvert.DeserializeObject<PaginatedEvents>(jsonContent);
            return loopEvents;
            */
        }

        /// <summary>
        /// Posts Loop Events asynchronously.
        /// </summary>
        /// <param name="edgeMac">The MAC Address of the Edge Relay that generated the events.</param>
        /// <param name="eventInfos">The events to post.</param>
        /// <returns>The response string from the request</returns>
        public async Task< string > PostEventsAsync( string jsonEvents ) {
            if (jsonEvents == null ) throw new ArgumentNullException( nameof( jsonEvents ) );
            EnsureAuthenticated();

            // Request
            const string ROUTE = "events";
            var uri = new Uri( ROUTE, UriKind.Relative );
            var request = _client.PostAsync( uri, new StringContent( jsonEvents, Encoding.ASCII, "application/json" ) );

            // Response
            var response = await UnwrapResponseStringAsync( request ).ConfigureAwait( false );
            return response;
        }

        /// <summary> 
        /// Allows for a custom JSON search query to pull specific data from the Loop API. This is mainly used under-the-hood by other methods in this class. 
        /// </summary> 
        /// <param name="queryJson">The JSON for the Loop API query. See the Loop docs online for how to format this query.</param> 
        /// <returns>The result of the query as a JSON string</returns> 
        public async Task< string > PostSearchAsync( string queryJson ) {
            if ( queryJson == null ) throw new ArgumentNullException( nameof(queryJson) );
            EnsureAuthenticated();

            // Request
            const string ROUTE = "search";
            var uri = new Uri( ROUTE, UriKind.Relative );
            var request = _client.PostAsync( uri, new StringContent( queryJson, Encoding.ASCII, "application/json" ) );

            var response = await UnwrapResponseStringAsync( request ).ConfigureAwait( false );
            return response;
        }

        /// <summary> 
        /// Creates a new Loop Object
        /// </summary> 
        /// <param name="jsonObject">The JSON for the Loop Object. See the Loop docs online for how to format this.</param> 
        /// <returns>The result of the request as a JSON string</returns> 
        public async Task<string> CreateObjectAsync(string jsonObject) {
            if (jsonObject == null) throw new ArgumentNullException( nameof( jsonObject ) );
            EnsureAuthenticated();

            // Request
            const string ROUTE = "objects";
            var uri = new Uri( ROUTE, UriKind.Relative );
            var request = _client.PostAsync( uri, new StringContent( jsonObject, Encoding.ASCII, "application/json" ) );

            var response = await UnwrapResponseStringAsync( request ).ConfigureAwait( false );
            return response;
        }

        /// <summary> 
        /// Creates a new Loop Object
        /// </summary> 
        /// <param name="jsonObject">The JSON for the Loop Object. See the Loop docs online for how to format this.</param> 
        /// <returns>The result of the request as a JSON string</returns> 
        public async Task<string> UpdateObjectAsync(string jsonObject) {
            if (jsonObject == null) throw new ArgumentNullException( nameof( jsonObject ) );
            EnsureAuthenticated();

            // Request
            const string ROUTE = "objects";
            var uri = new Uri( ROUTE, UriKind.Relative );
            var request = _client.PatchAsync( uri, new StringContent( jsonObject, Encoding.ASCII, "application/json" ) );

            var response = await UnwrapResponseStringAsync( request ).ConfigureAwait( false );
            return response;
        }

        /// <summary> 
        /// Delete Object Link
        /// </summary> 
        /// <param name="jsonObject">The JSON for the Loop Object. See the Loop docs online for how to format this.</param> 
        /// <returns>The result of the request as a JSON string</returns> 
        public async Task<string> PutObjectLinksAsync(string jsonObject) {
            if (jsonObject == null) throw new ArgumentNullException( nameof( jsonObject ) );
            EnsureAuthenticated();

            // Request
            const string ROUTE = "objects/link";
            var uri = new Uri( ROUTE, UriKind.Relative );
            var request = _client.PutAsync( uri, new StringContent( jsonObject, Encoding.ASCII, "application/json" ) );

            var response = await UnwrapResponseStringAsync( request ).ConfigureAwait( false );
            return response;
        }

        /// <summary>
        /// Gets the schema asynchronous.
        /// </summary>
        /// <returns>The Loop API Schema as a JSON string</returns>
        public async Task< string > GetSchemaAsync() {
            EnsureAuthenticated();

            // Request
            const string ROUTE = "schema";
            var uri = new Uri( ROUTE, UriKind.Relative );
            var request = _client.GetAsync( uri );
            
            // Response
            return await UnwrapResponseStringAsync( request ).ConfigureAwait( false );
        }

        public async Task< List< Dictionary< string, string > > > GetObjectsAsync( string objectType, string schemaJson, string groupId = null ) {
            EnsureAuthenticated();
            // TODO: include group ID
            // Lookup all property keys that I need to include in search query for this particular objectType
            var schema = JObject.Parse( schemaJson );
            var objectFields =
                from key in schema[ "objects" ][ objectType ][ "keys" ]
                select key[ "name" ];

            // Create search query
            var selectList = (
                from field in objectFields
                select new JObject( new JProperty( "value", field ) )
            ).ToList();
            selectList.Add( new JObject( new JProperty( "count", "all" ) ) );

            var query = 
                new JObject( 
                    new JProperty( "from", objectType ),
                    new JProperty( "select", 
                        new JArray( selectList )));

            // Add "where" clause for GroupID if supplied
            if ( groupId != null ) {
                query.Add( "where", 
                    new JArray(
                        new JObject( 
                            new JProperty( "groupID",
                                new JObject( 
                                    new JProperty( "EQ", groupId.ToLower() ))))));
            }

            var queryJson = query.ToString();

            // API call
            var objectsJson = await PostSearchAsync( queryJson ).ConfigureAwait( false );

            var objects = JObject.Parse( objectsJson )[ objectType ];
            var deserializedObjects = objects.ToObject< List< Dictionary< string, string > > >();

            return deserializedObjects;
        }

        public async Task< int > GetObjectCountAsync( string objectType, string groupID = null ) {
            EnsureAuthenticated();
            // TODO: include group ID
            // create search query
            var query = 
                new JObject( 
                    new JProperty( "from", objectType ),
                    new JProperty( "select", 
                        new JArray( 
                            new JObject( 
                                new JProperty( "count", "all" )))));

            // Add "where" clause for GroupID if supplied
            if ( groupID != null ) {
                query.Add( "where", 
                    new JArray(
                        new JObject( 
                            new JProperty( "groupID",
                                new JObject( 
                                    new JProperty( "EQ", groupID.ToLower() ))))));
            }
            var queryJson = query.ToString();

            // send and receive
            var countJson = await PostSearchAsync( queryJson ).ConfigureAwait( false );

            // parse count
            var count = (int)JObject.Parse( countJson )[ "count" ];
            return count;
        }

        public async Task< ICollection< Group > > GetGroupsAsync() {
            var groupsJson = await PostSearchAsync( Constants.GROUPS_QUERY ).ConfigureAwait( false );
            var groupsJObject = JObject.Parse( groupsJson )[ "groups" ];
            var groups = groupsJObject.ToObject< ICollection< Group > >();
            return groups;
        }

        private async Task< string > UnwrapResponseStringAsync( Task< HttpResponseMessage > request ) {
            HttpResponseMessage response = null;
            try {
                response = await request.ConfigureAwait( false );
                //response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait( false );
                return content;
            }
            catch ( HttpRequestException ) {
                var builder = new StringBuilder();
                if ( response == null ) throw;
                builder.AppendLine( "[ Response ]" );
                builder.AppendLine( response.ToString() );
                throw new HttpRequestException( builder.ToString() );  
            }
            finally {
                response?.Dispose();
            }
        }

        private void EnsureAuthenticated() {
            if ( !IsAuthenticated() ) throw new Exception( "Must login before API request" );
        }

    }

}