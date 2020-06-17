using BooksScheduler.Model;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BooksScheduler
{
    public class BookContext
    {
        private readonly IMongoDatabase _database = null;
        public IMongoCollection<Book> BooksCollection { get; }

        public BookContext(
            string connectionString,
            string database,
            string collectionName = "books")
        {
            var client = new MongoClient(connectionString);
            if (client != null)
            {
                _database = client.GetDatabase(database);
                BooksCollection = _database.GetCollection<Book>(collectionName);
            }
        }

        public async Task InsertManyAsync(IEnumerable<Book> books)
        {
            await BooksCollection.InsertManyAsync(books);
        }
    }
}
