namespace Prometheus.POC
{
	public class MetricsServer
	{
		public MetricsServer(int port)
		{
			new MetricServer(port: port).Start();
		}
	}
}
