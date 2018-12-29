using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HillPigeon.Core.Test.Infrastructure
{
    [Route("api/[controller]")]
    public class TestService
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public Task<string> Get(int id)
        {
            return Task.FromResult("value " + id);
        }

        [HttpPost]
        public Task<string> Post(string value)
        {
            return Task.FromResult("Post:value " + value);
        }

        [HttpPost("add")]
        public Task<string> Add([FromBody]List<string> values)
        {
            return Task.FromResult("Post:value " + values.Count);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, string value)
        {
        }


        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
