using System.Threading.Tasks;
using dotnetredis.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace dotnetredis.Controllers
{
    [ApiController]
    [Route("/api/ratings")]
    public class BookRatingController
    {
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