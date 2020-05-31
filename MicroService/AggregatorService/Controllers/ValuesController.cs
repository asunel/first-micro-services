using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AggregatorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<ActionResult<string>> Get(int id)
        {
            string orderResponse = string.Empty;
            string userResponse = string.Empty;

            // Get data from User service
            string userServiceUrl = "http://localhost:7000/userservice/1";
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(userServiceUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(userServiceUrl);
                if (response.IsSuccessStatusCode)
                {
                    userResponse = await response.Content.ReadAsStringAsync();
                    //var table = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Data.DataTable>(data);
                }
            }

            // Get data from Order service
            string orderServiceUrl = "http://localhost:7000/orderservice/1";
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(orderServiceUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(orderServiceUrl);
                if (response.IsSuccessStatusCode)
                {
                    orderResponse = await response.Content.ReadAsStringAsync();
                    //var table = Newtonsoft.Json.JsonConvert.DeserializeObject<System.Data.DataTable>(data);
                }
            }

            var dict = new Dictionary<string, object>();
            dict.Add("userDetails", JsonConvert.DeserializeObject(userResponse));
            dict.Add("orders", JsonConvert.DeserializeObject(orderResponse));

            return JsonConvert.SerializeObject(dict);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
