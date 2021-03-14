using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UniprotService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniprotController : ControllerBase 
    {
        private readonly IHttpClientFactory m_ClientFactory;

        public UniprotController(IHttpClientFactory httpClientFactory)
        {
            m_ClientFactory = httpClientFactory;
        }
        
        [HttpGet("{id}")]
        public async Task<string> GetUniprotEntry(string id)
        {
            var response = await Entry(id);
            var jsonResonse = new JObject(
                new JProperty("uniProtId", response.UniprotId),
                new JProperty("function", response.Function),
                new JProperty("sequence", response.Sequence));

            return $"{jsonResonse}";
        }

        public async Task<UniprotEntry> Entry(string id)
        {
            string uri = $"proteins/{id}";
            var client = m_ClientFactory.CreateClient("UniprotService");
            var request = new HttpRequestMessage(method: HttpMethod.Get, requestUri: uri);
            HttpResponseMessage response = await client.SendAsync(request);
            string jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UniprotEntry>(jsonString) ?? new UniprotEntry();
        }
    }
}