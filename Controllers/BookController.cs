using System;
using dotnetredis.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace dotnetredis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {

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
            Program.GetDatabase().HashSet(bookKey, Program.ToHashEntries(book));
            RedisKey bookAuthors = new RedisKey(book.GetType().Name + ":" + book.Id + ":authors");
            foreach (var author in book.Authors)
            {
                Program.GetDatabase().SetAdd(bookAuthors,author);
            }
        }

        [HttpGet]
        [Route("read")]
        public Book Get(string id)
        {
            Book book = new Book();
            RedisKey bookKey = new RedisKey(book.GetType().Name + ":" + id);
            HashEntry[] bookHash = Program.GetDatabase().HashGetAll(bookKey);
            Console.WriteLine("BookHash  =" + string.Join(",",bookHash));
            return Program.ConvertFromRedis<Book>(bookHash);
        }
        
        [HttpPost]
        [Route("load")]
        public void Load(Book[] books)
        {
            foreach (var book in books)
            {
                Create(book);   
            }
        }
    }
}