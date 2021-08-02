using MongoDB.Driver.Core.Events;
using MongoDB.Driver.Core.Servers;
using Prometheus.POC;
using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace MongoDb.Monitoring
{
	public class MongoDbDriverEventSubscriber : IEventSubscriber
	{
		private readonly IEventSubscriber _subscriber;
		private readonly IMetricsRecorder<MetricsDefinitions> _metricsRecorder;
		private readonly int _taskId;

		private readonly ConcurrentDictionary<ServerId, ConnectionPerformanceRecorder> _connectionPoolRecorders;

		public MongoDbDriverEventSubscriber(IMetricsRecorder<MetricsDefinitions> metricsRecorder, int taskId)
		{
			_taskId = taskId;
			_metricsRecorder = metricsRecorder;
			_connectionPoolRecorders = new ConcurrentDictionary<ServerId, ConnectionPerformanceRecorder>();
			_subscriber = new ReflectionEventSubscriber(this, bindingFlags: BindingFlags.Instance | BindingFlags.NonPublic);
		}

		public bool TryGetEventHandler<TEvent>(out Action<TEvent> handler)
		{
			return _subscriber.TryGetEventHandler(out handler);
		}

		private void Handle(ConnectionPoolOpenedEvent e)
		{
			Display(MethodBase.GetCurrentMethod().GetParameters()[0], "");
			var recorder = new ConnectionPerformanceRecorder(_metricsRecorder, e.ConnectionPoolSettings.MaxConnections, e.ConnectionPoolSettings.MinConnections);
			if (_connectionPoolRecorders.TryAdd(e.ServerId, recorder))
			{
				recorder.ConnectionPoolOpened();
			}
		}

		private void Handle(ConnectionPoolClosedEvent @event)
		{
			Display(MethodBase.GetCurrentMethod().GetParameters()[0], "");
			if (_connectionPoolRecorders.TryRemove(@event.ServerId, out ConnectionPerformanceRecorder recorder))
			{
				recorder.ConnectionPoolClosed();
			}
		}

		private void Handle(ConnectionPoolAddedConnectionEvent e)
		{
			Display(MethodBase.GetCurrentMethod().GetParameters()[0], e.ConnectionId.ServerValue.ToString());
			if (_connectionPoolRecorders.TryGetValue(e.ServerId, out ConnectionPerformanceRecorder recorder))
			{
				recorder.ConnectionAddedToPool(_taskId.ToString());
			}
		}

		private void Handle(ConnectionPoolRemovedConnectionEvent e)
		{
			Display(MethodBase.GetCurrentMethod().GetParameters()[0], e.ConnectionId.ServerValue.ToString());
			if (_connectionPoolRecorders.TryGetValue(e.ServerId, out ConnectionPerformanceRecorder recorder))
			{
				recorder.ConnectionRemovedFromPool(_taskId.ToString());
			}
		}

		private void Handle(ConnectionPoolCheckedOutConnectionEvent e)
		{
			Display(MethodBase.GetCurrentMethod().GetParameters()[0], e.ConnectionId.ServerValue.ToString());
			if (_connectionPoolRecorders.TryGetValue(e.ServerId, out ConnectionPerformanceRecorder recorder))
			{
				recorder.ConnectionCheckedOutFromPool(_taskId.ToString());
			}
		}

		private void Handle(ConnectionPoolCheckingOutConnectionFailedEvent e)
		{
			Display(MethodBase.GetCurrentMethod().GetParameters()[0], e.Reason.ToString());
			if (_connectionPoolRecorders.TryGetValue(e.ServerId, out ConnectionPerformanceRecorder recorder))
			{
				recorder.ConnectionCheckedOutFromPoolFailed(e.OperationId.ToString());
			}
		}

		private void Handle(ConnectionPoolCheckedInConnectionEvent e)
		{
			Display(MethodBase.GetCurrentMethod().GetParameters()[0], e.ConnectionId.ServerValue.ToString());
			if (_connectionPoolRecorders.TryGetValue(e.ServerId, out ConnectionPerformanceRecorder recorder))
			{
				recorder.ConnectionCheckedIntoPool(_taskId.ToString());
			}
		}

		private void Handle(ConnectionOpenedEvent e)
		{
			Display(MethodBase.GetCurrentMethod().GetParameters()[0], e.ConnectionId.ServerValue.ToString());
			if (_connectionPoolRecorders.TryGetValue(e.ServerId, out ConnectionPerformanceRecorder recorder))
			{
				recorder.ConnectionOpened(_taskId.ToString());
			}
		}


		private void Handle(ConnectionClosedEvent e)
		{
			Display(MethodBase.GetCurrentMethod().GetParameters()[0], e.ConnectionId.ServerValue.ToString());
			if (_connectionPoolRecorders.TryGetValue(e.ServerId, out ConnectionPerformanceRecorder recorder))
			{
				recorder.ConnectionClosed(_taskId.ToString());
			}
		}

		Action<ParameterInfo, string> Display = (e, severId) => Console.WriteLine($"--- {e.ParameterType.Name}({severId}) ----");
	}
}
