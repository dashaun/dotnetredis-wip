using dotnetredis.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using NRediSearch;

namespace dotnetredis.Controllers
{
    [ApiController]
    [Route("/api/books")]
    public class BookController : ControllerBase
    {

        [HttpPost]
        [Route("create")]
        public void Create(Book book)
        {
            var db = Program.GetDatabase();
            var bookKey = $"Book:{book.Id}";
            var bookAuthorsKey = $"{bookKey}:authors";

            db.HashSet(bookKey, Program.ToHashEntries(book));
            foreach (var author in book.Authors)
            {
                db.SetAdd(bookAuthorsKey, author);
            }
        }

        [HttpGet]
        [Route("read")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(string id)
        {
            var db = Program.GetDatabase();
            var bookKey = $"Book:{id}";

            var bookHash = db.HashGetAll(bookKey);
            if (bookHash.Length == 0) return NotFound();

            var book = Program.ConvertFromRedis<Book>(bookHash);
            return Ok(book);
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
            Client client = new Client("books-idx", Program.GetDatabase());

            // drop the index, if it doesn't exists, that's fine
            try
            {
                client.DropIndex();
            }
            catch {}

            var schema = new Schema();
            schema.AddSortableTextField("Title");
            schema.AddTextField("Subtitle");
            schema.AddTextField("Description");
            schema.AddTextField("Authors");

            Client.ConfiguredIndexOptions options = new Client.ConfiguredIndexOptions(
                new Client.IndexDefinition( prefixes: new [] { "Book:" } )
            );

            client.CreateIndex(schema, options);
        }
    }
}