namespace Prometheus.POC
{
	public interface IPrometheusFactory
    {
        Counter CreateCounter(string name, string help, string[] labelNames = null, bool suppressInitialValue = false);
        Histogram CreateHistogram(string name, string help, string[] labelNames = null, bool suppressInitialValue = false, double[] buckets = null);
        Gauge CreateGauge(string name, string help, string[] labelNames = null, bool suppressInitialValue = false);
    }

    public class PrometheusFactory : IPrometheusFactory
    {
        public Counter CreateCounter(string name, string help, string[] labelNames = null, bool suppressInitialValue = false)
        {
            return Metrics.CreateCounter(
                name,
                help,
                new CounterConfiguration
                {
                    LabelNames = labelNames,
                    SuppressInitialValue = suppressInitialValue
                });
        }

        public Histogram CreateHistogram(string name, string help, string[] labelNames = null, bool suppressInitialValue = false,
            double[] buckets = null)
        {
            return Metrics.CreateHistogram(
                name,
                help,
                new HistogramConfiguration
                {
                    Buckets = buckets,
                    LabelNames = labelNames,
                    SuppressInitialValue = suppressInitialValue
                });
        }

        public Gauge CreateGauge(string name, string help, string[] labelNames = null, bool suppressInitialValue = false)
        {
            return Metrics.CreateGauge(
                name,
                help,
                new GaugeConfiguration
                {
                    LabelNames = labelNames,
                    SuppressInitialValue = suppressInitialValue
                });
        }
    }
}
