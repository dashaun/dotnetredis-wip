using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace dotnetredis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IRedisCacheClient _redisCacheClient;

        public TestController(IRedisCacheClient redisCacheClient)
        {
            _redisCacheClient = redisCacheClient;
        }

        [HttpGet]
        [Route("get-set-cache")]
        public async Task<IActionResult> GetSetCache()
        {
            //The 'GetDbFromConfiguration' pics the Redis database value from our JSON configuration.

            //Saving string data to Redis store and specified the expiration for the record.
            await _redisCacheClient.GetDbFromConfiguration()
                .AddAsync("myName", "naveen", DateTimeOffset.Now.AddMinutes(2));
            //Fetching string data from Redis store
            return Ok(await _redisCacheClient.GetDbFromConfiguration().GetAsync<string>("myName"));
        }
    }
}