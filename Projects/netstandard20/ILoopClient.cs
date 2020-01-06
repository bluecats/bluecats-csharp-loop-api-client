using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BC.Loop.Api.Client.Models;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BC.Loop.Api.Client {

    /// <summary>
    /// Provides an async interface to the BlueCats Loop web API 
    /// </summary>
    public interface ILoopClient {

        bool IsAuthenticated();

        /// <summary>
        /// Authenticates with the Loop API asynchronously. This is required before making any other API calls.
        /// </summary>
        /// <param name="email">Loop user email.</param>
        /// <param name="password">Loop user password.</param>
        /// <returns>The authenticated user info</returns>
        /// <exception cref="System.Exception">Received an empty auth token from API</exception>
        Task<User> LoginAsync(string email, string password);

        /// <summary>
        /// Logs Out and clears the current session and auth details.
        /// </summary>
        void Logout();

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
        Task<PaginatedEvents> GetEventsAsync(string objectType, string objectID, string eventType = null, string lastKeyID = null, DateTime? lastKeyTimestamp = null, int? limit = null, DateTime? startTime = null, DateTime? endTime = null);

        /// <summary>
        /// Posts Loop Events asynchronously.
        /// </summary>
        /// <param name="edgeMac">The MAC Address of the Edge Relay that generated the events.</param>
        /// <param name="eventInfos">The events to post.</param>
        /// <returns>The response string from the request</returns>
        Task<string> PostEventsAsync(string jsonEvents);

        /// <summary> 
        /// Allows for a custom JSON search query to pull specific data from the Loop API. This is mainly used under-the-hood by other methods in this class. 
        /// </summary> 
        /// <param name="queryJson">The JSON for the Loop API query. See the Loop docs online for how to format this query.</param> 
        /// <returns>The result of the query as a JSON string</returns> 
        Task<string> PostSearchAsync(string queryJson);

        /// <summary> 
        /// Creates a new Loop Object
        /// </summary> 
        /// <param name="jsonObject">The JSON for the Loop Object. See the Loop docs online for how to format this.</param> 
        /// <returns>ThThe result of the query as a JSON string</returns> 
        Task<string> CreateObjectAsync(string jsonObject);

        /// <summary> 
        /// Updates an existing Loop Object
        /// </summary> 
        /// <param name="jsonObject">The JSON for the Loop Object. See the Loop docs online for how to format this.</param> 
        /// <returns>ThThe result of the query as a JSON string</returns> 
        Task<string> UpdateObjectAsync(string jsonObject);

        /// <summary>
        /// Gets the schema asynchronous.
        /// </summary>
        /// <returns>The Loop API Schema as a JSON string</returns>
        Task<string> GetSchemaAsync();

        Task<List<Dictionary<string, string>>> GetObjectsAsync(string objectType, string schemaJson, string groupId = null);

        Task<int> GetObjectCountAsync(string objectType, string groupID = null);

        Task<ICollection<Group>> GetGroupsAsync();

    }

}