namespace Prometheus.POC.Common
{
	public abstract class MetricsDefinition
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string[] Labels { get; set; }
        public bool SuppressInitialValue { get; set; }
    }

    public class MetricsCounterDefinition : MetricsDefinition
    {
    }

    public class MetricsHistogramDefinition : MetricsDefinition
    {
        public double[] Buckets { get; set; }
    }

    public class MetricsGaugeDefinition : MetricsDefinition
    {
    }

    public class MetricsSummaryDefinition : MetricsDefinition
    {
    }
}
