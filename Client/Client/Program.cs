using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        private static async Task MainAsync()
        {
            var identityServer = await DiscoveryClient.GetAsync("http://localhost:49381"); //discover the IdentityServer
            if (identityServer.IsError) 
            {
                Console.Write(identityServer.Error);
                return;
            }

            //Get the token
            var tokenClient = new TokenClient(identityServer.TokenEndpoint, "Client1", "secret");
            //var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("satya", "password", "api1");

            //Call the API

            HttpClient client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:52037/api/values");
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine(JArray.Parse(content));
            Console.ReadKey();
}
    }
}
