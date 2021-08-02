using MongoDb.Monitoring;
using MongoDB.Driver;
using Prometheus.POC;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoDb
{
	public class MongoDbRepository<TEntity>
	{
		protected IMongoCollection<TEntity> _collection;
		protected IMongoDatabase _mongoDatabase;
		private readonly IMetricsRecorder<MetricsDefinitions> _metricsRecorder;

		public MongoDbRepository(IMetricsRecorder<MetricsDefinitions> metricsRecorder)
		{
			_metricsRecorder = metricsRecorder;
		}

		public Task ConnectAsync(string connection, string database, string collection, int taskId)
		{
			var settings = MongoClientSettings.FromConnectionString(connection);

			//settings.ConnectTimeout = TimeSpan.FromSeconds(5);
			settings.MaxConnectionIdleTime = TimeSpan.FromSeconds(1);
			settings.MaxConnectionPoolSize = 10;
			settings.MinConnectionPoolSize = 5;

			settings.ClusterConfigurator = cb =>
			{
				cb.Subscribe(new MongoDbDriverEventSubscriber(_metricsRecorder, taskId));
			};

			var client = new MongoClient(settings);

			_mongoDatabase = client.GetDatabase(database);

			_collection = _mongoDatabase.GetCollection<TEntity>(collection, new MongoCollectionSettings
			{
				AssignIdOnInsert = true
			});

			return Task.CompletedTask;
		}

		public async Task InsertAsync(TEntity[] item)
		{
			try
			{
				await _collection.InsertManyAsync(item);
			}
			catch (MongoWriteException ex)
			{
				if (ex.WriteConcernError?.CodeName != "WriteConcernFailed" || ex.WriteConcernError.Message != "waiting for replication timed out")
				{
					throw;
				}
			}
		}

		public async Task<IEnumerable<TEntity>> GetAsync(string key, string value)
		{
			var filter = Builders<TEntity>.Filter.Eq(key, value);

			var query = _collection.Find(filter);

			var entities = await query.ToListAsync();

			return entities;
		}
	}
}
