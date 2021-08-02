using MongoDb.Monitoring;
using Prometheus.POC;
using System;
using System.Threading;

namespace MongoDb
{
	internal class ConnectionPerformanceRecorder
	{
		private readonly IMetricsRecorder<MetricsDefinitions> _metricsRecorder;
		private readonly int _maxSize;
		private readonly int _minSize;
		private int _mongoDbDriverConnectionPoolSize;
		private int _mongoDbDriverAvailableConnections;
		private int _mongoDbDriverOpenConnections;
		private int _mongoDbDriverActiveConnections;

		public ConnectionPerformanceRecorder(IMetricsRecorder<MetricsDefinitions> metricsRecorder, int maxSize, int minSize)
		{
			_metricsRecorder = metricsRecorder;
			_maxSize = maxSize;
			_minSize = minSize;
		}

		/// <summary>
		/// Connection pool opened
		/// </summary>
		public void ConnectionPoolOpened()
		{
			_mongoDbDriverConnectionPoolSize = _maxSize;
			_metricsRecorder.SetGaugeValue(_metricsRecorder.Definitions.MongoDbDriverConnectionPoolSize.Name, _maxSize, "");
			Console.WriteLine($"MaxPoolSize : {_maxSize}, MinPoolSize: {_minSize}");
		}

		/// <summary>
		/// New connection created and added into the pool
		/// </summary>
		public void ConnectionAddedToPool(string serverId)
		{
			_mongoDbDriverAvailableConnections++;
			_metricsRecorder.IncrementGauge(_metricsRecorder.Definitions.MongoDbDriverAvailableConnections.Name, 1, false, serverId);
			Console.WriteLine($"AvailableConnections : {_mongoDbDriverAvailableConnections}");
		}

		/// <summary>
		/// Connection pool opened connection with database
		/// </summary>
		public void ConnectionOpened(string serverId)
		{
			_mongoDbDriverOpenConnections++;
			_metricsRecorder.IncrementGauge(_metricsRecorder.Definitions.MongoDbDriverOpenDatabaseConnections.Name, 1, false, serverId);
			Console.WriteLine($"OpenedConnections(DB) : {_mongoDbDriverOpenConnections}");
		}

		/// <summary>
		/// Connection checked in into the pool
		/// </summary>
		public void ConnectionCheckedIntoPool(string serverId)
		{
			_mongoDbDriverAvailableConnections++;
			_metricsRecorder.IncrementGauge(_metricsRecorder.Definitions.MongoDbDriverAvailableConnections.Name, 1, false, serverId);
			Console.WriteLine($"AvailableConnections : {_mongoDbDriverAvailableConnections}");
			_mongoDbDriverActiveConnections--;
			_metricsRecorder.DecrementGauge(_metricsRecorder.Definitions.MongoDbDriverActiveConnections.Name, 1, false, serverId);
			Console.WriteLine($"ActiveConnections : {_mongoDbDriverActiveConnections}");
		}

		/// <summary>
		/// Connection checked out from the pool
		/// </summary>
		public void ConnectionCheckedOutFromPool(string serverId)
		{
			_mongoDbDriverAvailableConnections--;
			_metricsRecorder.DecrementGauge(_metricsRecorder.Definitions.MongoDbDriverAvailableConnections.Name, 1, false, serverId);
			Console.WriteLine($"AvailableConnections : {_mongoDbDriverAvailableConnections}");
			_mongoDbDriverActiveConnections++;
			_metricsRecorder.IncrementGauge(_metricsRecorder.Definitions.MongoDbDriverActiveConnections.Name, 1, false, serverId);
			Console.WriteLine($"ActiveConnections : {_mongoDbDriverActiveConnections}");

			Thread.Sleep(2000);
		}

		/// <summary>
		/// Connection checked out from the pool
		/// </summary>
		public void ConnectionCheckedOutFromPoolFailed(string reason)
		{
			_mongoDbDriverAvailableConnections++;
			_metricsRecorder.IncrementGauge(_metricsRecorder.Definitions.MongoDbDriverAvailableConnections.Name, 1, false, reason);
			Console.WriteLine($"AvailableConnections : {_mongoDbDriverAvailableConnections}");
			_metricsRecorder.DecrementGauge(_metricsRecorder.Definitions.MongoDbDriverActiveConnections.Name, 1, false, reason);
			Console.WriteLine($"ActiveConnections : {_mongoDbDriverActiveConnections}");

			Thread.Sleep(2000);
		}

		/// <summary>
		/// Connection pool closed connection with database
		/// </summary>
		public void ConnectionClosed(string serverId)
		{
			_mongoDbDriverOpenConnections--;
			_metricsRecorder.DecrementGauge(_metricsRecorder.Definitions.MongoDbDriverOpenDatabaseConnections.Name, 1, false, serverId);
			Console.WriteLine($"OpenedConnections(DB) : {_mongoDbDriverOpenConnections}");
		}


		/// <summary>
		/// Connection removed from the pool
		/// </summary>
		public void ConnectionRemovedFromPool(string serverId)
		{
			_mongoDbDriverAvailableConnections--;
			_metricsRecorder.DecrementGauge(_metricsRecorder.Definitions.MongoDbDriverAvailableConnections.Name, 1, false, serverId);
			Console.WriteLine($"AvailableConnections : {_mongoDbDriverAvailableConnections}");
		}

		/// <summary>
		/// Connection pool closed
		/// </summary>
		public void ConnectionPoolClosed()
		{
			_mongoDbDriverConnectionPoolSize = 0;
			_metricsRecorder.SetGaugeValue(_metricsRecorder.Definitions.MongoDbDriverConnectionPoolSize.Name, 0, "");
			Console.WriteLine($"ConnectionPoolSize : {_mongoDbDriverConnectionPoolSize}");
		}
	}
}
