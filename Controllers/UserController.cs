using dotnetredis.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace dotnetredis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {

        [HttpGet]
        [Route("empty")]
        public User GetEmpty()
        {
            return new User();
        }
        
        [HttpPost]
        [Route("create")]
        public void Create(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            RedisKey userKey = new RedisKey(user.GetType().Name + ":" + user.Id);
            Program.GetDatabase().HashSet(userKey, Program.ToHashEntries(user));
        }
        
        [HttpGet]
        [Route("read")]
        public User Get(string id)
        {
            RedisKey userKey = new RedisKey(new User().GetType().Name + ":" + id);
            HashEntry[] userHashEntries = Program.GetDatabase().HashGetAll(userKey);
            return Program.ConvertFromRedis<User>(userHashEntries);
        }
        
        [HttpPost]
        [Route("load")]
        public void Load(User[] users)
        {
            foreach (var user in users)
            {
                Create(user);   
            }
        }
    }
}