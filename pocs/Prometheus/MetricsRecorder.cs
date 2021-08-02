using Prometheus.POC.Common;
using System;
using System.Collections.Generic;

namespace Prometheus.POC
{
	public interface IMetricsRecorder<T>
	{
		T Definitions { get; set; }
		void IncrementCounter(string counterName, int increment = 1, params string[] labels);
		void ObserveHistogram(string histogramName, double seconds, params string[] labels);
		void SetGaugeValue(string gaugeName, double value, params string[] labels);
		void IncrementGauge(string gaugeName, double value, bool isBulkIncrement = false, params string[] labels);
		void DecrementGauge(string gaugeName, double value, bool isBulkDecrement = false, params string[] labels);
	}

	public abstract class MetricsRecorder<T> : IMetricsRecorder<T> where T : class
	{
		protected readonly IPrometheusFactory _prometheusFactory;
		protected readonly Dictionary<string, Counter> _counters;
		protected readonly Dictionary<string, Histogram> _histograms;
		protected readonly Dictionary<string, Gauge> _gauges;

		public T Definitions { get; set; }

		public MetricsRecorder(IPrometheusFactory prometheusFactory)
		{
			_prometheusFactory = prometheusFactory;

			Definitions = Activator.CreateInstance<T>();

			_counters = new Dictionary<string, Counter>();
			_histograms = new Dictionary<string, Histogram>();
			_gauges = new Dictionary<string, Gauge>();

			CreateMetrics();
		}

		abstract protected void CreateMetrics();

		public void CreateCounter(MetricsCounterDefinition definition)
		{
			var counter = _prometheusFactory.CreateCounter(definition.Name, definition.Description, definition.Labels, definition.SuppressInitialValue);

			_counters.Add(definition.Name, counter);
		}

		public void CreateHistogram(MetricsHistogramDefinition definition)
		{
			var histogram = _prometheusFactory.CreateHistogram(definition.Name, definition.Description, definition.Labels, definition.SuppressInitialValue, definition.Buckets);

			_histograms.Add(definition.Name, histogram);
		}

		public void CreateGauge(MetricsGaugeDefinition definition)
		{
			var gauge = _prometheusFactory.CreateGauge(definition.Name, definition.Description, definition.Labels, definition.SuppressInitialValue);
			_gauges.Add(definition.Name, gauge);
		}

		public void IncrementCounter(string counterName, int increment = 1, params string[] labels)
		{
			if (_counters.ContainsKey(counterName))
			{
				_counters[counterName]
					.WithLabels(labels)
					.Inc(increment);
			}
		}

		public void ObserveHistogram(string histogramName, double seconds, params string[] labels)
		{
			if (_histograms.ContainsKey(histogramName))
			{
				_histograms[histogramName]
					.WithLabels(labels)
					.Observe(seconds);
			}
		}

		public void SetGaugeValue(string gaugeName, double value, params string[] labels)
		{
			if (_gauges.ContainsKey(gaugeName))
			{
				_gauges[gaugeName]
					.WithLabels(labels)
					.Set(value);
			}
		}

		public void IncrementGauge(string gaugeName, double value, bool isBulkIncrement = false, params string[] labels)
		{
			if (_gauges.ContainsKey(gaugeName))
			{
				if (isBulkIncrement)
				{
					_gauges[gaugeName]
						.WithLabels(labels)
						.IncTo(value);
				}
				else
				{
					_gauges[gaugeName]
						.WithLabels(labels)
						.Inc(value);
				}
			}
		}

		public void DecrementGauge(string gaugeName, double value, bool isBulkDecrement = false, params string[] labels)
		{
			if (_gauges.ContainsKey(gaugeName))
			{
				if (isBulkDecrement)
				{
					_gauges[gaugeName]
						.WithLabels(labels)
						.DecTo(value);
				}
				else
				{
					_gauges[gaugeName]
						.WithLabels(labels)
						.Dec(value);
				}
			}
		}
	}
}
