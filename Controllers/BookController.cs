using dotnetredis.Models;
using Microsoft.AspNetCore.Mvc;
using NRediSearch;
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
            RedisKey bookKey = new RedisKey(new Book().GetType().Name + ":" + id);
            HashEntry[] bookHash = Program.GetDatabase().HashGetAll(bookKey);
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
        
        [HttpGet]
        [Route("createIndex")]
        public void CreateIndex()
        {
            RedisKey booksSearchIndexName = "books-idx";
            Client client = new Client(booksSearchIndexName, Program.GetDatabase());
            try
            {
                client.DropIndex();
            }
            catch
            {
                // OK if the index didn't already exist.
            }
            Schema sch = new Schema();
            sch.AddSortableTextField("Title");
            sch.AddTextField("Subtitle");
            sch.AddTextField("Description");
            sch.AddTextField("Authors");
            Client.ConfiguredIndexOptions options = new Client.ConfiguredIndexOptions(
                new Client.IndexDefinition( 
                    prefixes: new []{new Book().GetType().Name + ":" })
                );
            client.CreateIndex(sch, options);
        }
    }
}