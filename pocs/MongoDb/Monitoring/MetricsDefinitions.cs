using Prometheus.POC.Common;

namespace MongoDb.Monitoring
{
	public class MetricsDefinitions
	{
		public MetricsCounterDefinition RequestsCounter { get; set; }
		public MetricsHistogramDefinition RequestHistogram { get; set; }

        public MetricsGaugeDefinition MongoDbDriverConnectionPoolSize { get; set; }
        public MetricsGaugeDefinition MongoDbDriverAvailableConnections { get; set; }
        public MetricsGaugeDefinition MongoDbDriverOpenDatabaseConnections { get; set; }
        public MetricsGaugeDefinition MongoDbDriverActiveConnections { get; set; }

        public MetricsDefinitions()
		{
			RequestsCounter = new MetricsCounterDefinition
			{
				Name = "requests_total",
				Description = "Requests that were received",
				Labels = new[] { "method", "protocol" }
			};

			RequestHistogram = new MetricsHistogramDefinition
			{
				Name = "request_duration_seconds",
				Description = "Successful requests and the time it took to complete",
				Buckets = new[] { 0.1, 0.3, 0.5, 1 },
				Labels = new[] { "method", "protocol" }
			};

            MongoDbDriverConnectionPoolSize = new MetricsGaugeDefinition
            {
                Name = "mongodb_driver_connection_pool_size",
                Description = "Mongodb driver connection pool size",
                Labels = new[] { "source" }
            };

            MongoDbDriverAvailableConnections = new MetricsGaugeDefinition
            {
                Name = "mongodb_driver_available_connections_total",
                Description = "Mongodb driver available connections",
                Labels = new[] { "source" }
            };

            MongoDbDriverOpenDatabaseConnections = new MetricsGaugeDefinition
            {
                Name = "mongodb_driver_open_database_connections_total",
                Description = "Mongodb driver open database connections",
                Labels = new[] { "source" }
            };

            MongoDbDriverActiveConnections = new MetricsGaugeDefinition
            {
                Name = "mongodb_driver_active_connections_total",
                Description = "Mongodb driver active connections",
                Labels = new[] { "source" }
            };
        }
	}
}
