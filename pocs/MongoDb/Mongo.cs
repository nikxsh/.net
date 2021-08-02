using MongoDb.Models;
using MongoDb.Monitoring;
using Newtonsoft.Json;
using Prometheus.POC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDb
{
	class Program
	{
		static void Main(string[] args)
		{
			#region MongoDB with metrics collector
			IPrometheusFactory prometheusFactory = new PrometheusFactory();
			IMetricsRecorder<MetricsDefinitions> metricsCollector = new PrometheusMetricsRecorder<MetricsDefinitions>(prometheusFactory);
			_ = new MetricsServer(1234);
			var connection = "mongodb://localhost:27017/?readPreference=primary&appname=MongoDB%20Compass&ssl=false";
			new Mongo(metricsCollector, connection).Process();
			#endregion

			Console.ReadLine();
		}
	}

	class Mongo
	{
		private readonly string _connection;
		private readonly MongoDbRepository<Transaction> _mongoDbRepository;
		private readonly IMetricsRecorder<MetricsDefinitions> _metricsRecorder;

		public Mongo(IMetricsRecorder<MetricsDefinitions> metricsRecorder, string connection)
		{
			_metricsRecorder = metricsRecorder;
			_connection = connection;
			_mongoDbRepository = new MongoDbRepository<Transaction>(_metricsRecorder);
		}

		public void Process()
		{
			var tasks = new List<Task>();

			foreach (var item in Enumerable.Range(1, 10))
			{
				var task = new Task(async () =>
				{
					await _mongoDbRepository.ConnectAsync(_connection, "Testdb", "transactions", Task.CurrentId.Value);
					Console.WriteLine("** Executing Query/Command **");
					//_mongoDbRepository.InsertAsync(TransactionData).Wait();
					var result = await _mongoDbRepository.GetAsync("Code", "TRS001");
					Console.WriteLine("** Query/Command Executed **");
					Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
				});
				task.Start();
				tasks.Add(task);
			}
			Task.WaitAll(tasks.ToArray());
		}


		private Transaction[] TransactionData = new Transaction[]
		{
			new Transaction
			{
				Code = "TRS001",
				Amount = 100
			},
			new Transaction
			{
				Code = "TRS002",
				Amount = 200
			},
			new Transaction
			{
				Code = "TRS003",
				Amount = 300
			},
			new Transaction
			{
				Code = "TRS004",
				Amount = 400
			},
			new Transaction
			{
				Code = "TRS005",
				Amount = 400
			}
		};
	}
}
