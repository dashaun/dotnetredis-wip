using System.Threading.Tasks;
using dotnetredis.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace dotnetredis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookRatingController
    {
        [HttpGet]
        [Route("empty")]
        public BookRating GetEmpty()
        {
            return new BookRating();
        }

        [HttpPost]
        [Route("create")]
        public void Create(BookRating book)
        {
        }

        [HttpPost]
        [Route("read")]
        public BookRating Get(string id)
        {
            return new BookRating();
        }
    }
}