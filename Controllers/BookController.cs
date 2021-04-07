using System.Threading.Tasks;
using dotnetredis.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace dotnetredis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private static RedisValue Title = "Title";
        private static RedisValue Subtitle = "Subtitle";
        private static RedisValue Description = "Description";
        private static RedisValue Language = "Language";
        private static RedisValue PageCount = "PageCount";
        private static RedisValue Thumbnail = "Thumbnail";
        private static RedisValue Price = "Price";
        private static RedisValue Currency = "Currency";
        private static RedisValue InfoLink = "InfoLink";
            
        [HttpGet]
        [Route("empty")]
        public Book GetEmpty()
        {
            return new();
        }

        [HttpPost]
        [Route("create")]
        public void Create(Book book)
        {
            RedisKey bookKey = new RedisKey(book.GetType().Name + ":" + book.Id);
            HashEntry[] bookHash = {
                new HashEntry(Title, book.Title),
                new HashEntry(Subtitle, book.Subtitle),
                new HashEntry(Description, book.Description),
                new HashEntry(Language, book.Language),
                new HashEntry(PageCount, book.PageCount),
                new HashEntry(Thumbnail, book.Thumbnail),
                new HashEntry(Price, book.Price),
                new HashEntry(Currency, book.Currency),
                new HashEntry(InfoLink, book.InfoLink)
            };
            Program.GetDatabase().HashSet(bookKey,bookHash);
        }

        [HttpPost]
        [Route("read")]
        public Book Get(string id)
        {
            Book book = new Book();
            RedisKey bookKey = new RedisKey(book.GetType().Name + ":" + id);
            HashEntry[] bookHash = Program.GetDatabase().HashGetAll(bookKey);
            return Program.ConvertFromRedis<Book>(bookHash);
        }
    }
}