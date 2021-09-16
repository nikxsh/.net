using System;

namespace AwsRegionLatencies
{
	class Region
	{
		public string Service { get; set; }
		public string Name { get; set; }
		public string Code { get; set; }
		public TimeSpan Latency { get; set; }
	}
}
