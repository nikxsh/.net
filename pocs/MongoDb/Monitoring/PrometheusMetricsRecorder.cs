using Prometheus.POC;

namespace MongoDb.Monitoring
{
	public class PrometheusMetricsRecorder<T> : MetricsRecorder<T> where T: class
	{
		public PrometheusMetricsRecorder(IPrometheusFactory prometheusFactory)
		: base(prometheusFactory)
		{
		}

		protected override void CreateMetrics()
		{
			var definitions = Definitions as MetricsDefinitions;
			CreateCounter(definitions.RequestsCounter);
			CreateHistogram(definitions.RequestHistogram);
			CreateGauge(definitions.MongoDbDriverConnectionPoolSize);
			CreateGauge(definitions.MongoDbDriverAvailableConnections);
			CreateGauge(definitions.MongoDbDriverOpenDatabaseConnections);
			CreateGauge(definitions.MongoDbDriverActiveConnections);
		}
	}
}
