using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BC.Loop.Api.Client.netstandard20
{
    public static class Extensions
    {
        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, Uri uri, StringContent content) {
            if (client == null) throw new Exception( "HttpClient is null" );
            if (uri == null) throw new Exception( "Uri is null" );
            if (content == null) throw new Exception( "StringContent is null" );

            var request = new HttpRequestMessage( new HttpMethod( "PATCH" ), uri ) { Content = content };

            return await client.SendAsync( request );
        }
    }
}
