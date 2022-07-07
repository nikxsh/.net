using MigrationService.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MigrationService.Repository
{
	public interface IMongoRepository
	{
		Task Save(DbSettings dbSettings, List<BsonDocument> payload);
	}

	public class MongoRepository : IMongoRepository
	{
		public async Task Save(DbSettings dbSettings, List<BsonDocument> payload)
		{
			var settings = MongoClientSettings.FromConnectionString(dbSettings.Connection);
			var client = new MongoClient(settings);
			IMongoDatabase _mongoDatabase = client.GetDatabase(dbSettings.Database);

			var _collection = _mongoDatabase.GetCollection<BsonDocument>(dbSettings.Collection, new MongoCollectionSettings
			{
				AssignIdOnInsert = true
			});
			try
			{
				await _collection.InsertManyAsync(payload);
			}
			catch (MongoWriteException ex)
			{
				throw ex;
			}
		}
	}
}
