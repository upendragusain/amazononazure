using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Infrastructure.DataContexts
{
    public class CatalogBooksReadDataContext
    {
        private readonly IMongoDatabase _database = null;
        private readonly IMongoCollection<Dto.Book> _bookCollection = null;

        public CatalogBooksReadDataContext(IOptions<CatalogSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);

            if (client != null)
            {
                _database = client.GetDatabase(settings.Value.Database);
                _bookCollection = _database.GetCollection<Dto.Book>(settings.Value.Collection);
            }
        }

        public async Task<Dto.Book> GetSingleOrDefaultAsync(string documentId)
        {
            ObjectId parsedObjectId;
            ObjectId.TryParse(documentId, out parsedObjectId);

            var filter = Builders<Dto.Book>.Filter
                .Eq("Id", parsedObjectId);

            return await _bookCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<long> GetTotalCountAsync(string searchTerm = null)
        {
            FilterDefinition<Dto.Book> filter = !string.IsNullOrWhiteSpace(searchTerm)
                ? Builders<Dto.Book>.Filter
                    .Where(p => p.Name.ToLower()
                    .Contains(searchTerm.ToLower()))
                : new BsonDocument();

            return await _bookCollection.CountDocumentsAsync(filter);
        }

        public async Task<IEnumerable<Dto.Book>> GetPageDocumentsAsync(
            int pageSize, int pageIndex, string searchTerm = null)
        {
            FilterDefinition<Dto.Book> filter = !string.IsNullOrWhiteSpace(searchTerm)
                ? Builders<Dto.Book>.Filter
                    .Where(p => p.Name.ToLower()
                    .Contains(searchTerm.ToLower()))
                : new BsonDocument();

            return await _bookCollection.Find(filter)
                    .Skip(pageSize * (pageIndex - 1))
                    .Limit(pageSize)
                    .ToListAsync();
        }
    }
}
