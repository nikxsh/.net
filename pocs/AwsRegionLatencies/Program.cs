using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;

namespace AwsRegionLatencies
{
	class Program
	{
		static List<Region> _regions;
		static List<string> _lookupRegions = new List<string> { "us-west-1", "us-west-2", "ap-south-1" };

		static void Main(string[] args)
		{
			Console.WriteLine("Get nearest Aws region.");
			InitRegions("ec2");
			DisplayLatencies();
		}

		static void DisplayLatencies()
		{
			var client = new HttpClient();
			Stopwatch sw = new Stopwatch();

			foreach (var region in _regions.Where(x => _lookupRegions.Contains(x.Code)).ToList())
			{
				sw.Reset();
				sw.Start();
				var response = client.SendAsync(new HttpRequestMessage
				{
					RequestUri = new Uri($"http://{region.Service}.{region.Code}.amazonaws.com/ping"),
					Method = HttpMethod.Head
				}).Result;
				sw.Stop();
				region.Latency = sw.Elapsed;
				Console.WriteLine($"{region.Name} - {region.Code} : {region.Latency.TotalMilliseconds}");
			}

			var nearestRegion = _regions.OrderBy(x => x.Latency).Where(x => x.Latency != TimeSpan.FromSeconds(0)).ToList().First();

			Console.WriteLine($"Nearest Aws region {nearestRegion.Code}({nearestRegion.Latency.TotalMilliseconds})");
		}

		static void InitRegions(string service)
		{
			_regions = new List<Region>
			{
				new Region { Service = service, Name = "US-East (Virginia)", Code = "us-east-1"},
				new Region { Service = service, Name = "US-East (Ohio)", Code = "us-east-2"},
				new Region { Service = service, Name = "US-West (California)", Code = "us-west-1"},
				new Region { Service = service, Name = "US-West (Oregon)", Code = "us-west-2"},
				new Region { Service = service, Name = "Canada (Central)", Code = "ca-central-1"},
				new Region { Service = service, Name = "Europe (Ireland)", Code = "eu-west-1"},
				new Region { Service = service, Name = "Europe (Frankfurt)", Code = "eu-central-1"},
				new Region { Service = service, Name = "Europe (London)", Code = "eu-west-2"},
				new Region { Service = service, Name = "Europe (Milan)", Code = "eu-south-1"},
				new Region { Service = service, Name = "Europe (Paris)", Code = "eu-west-3"},
				new Region { Service = service, Name = "Europe (Stockholm)", Code = "eu-north-1"},
				new Region { Service = service, Name = "Africa (Cape Town)", Code = "af-south-1"},
				new Region { Service = service, Name = "Asia Pacific (Osaka-Local)", Code = "ap-northeast-3"},
				new Region { Service = service, Name = "Asia Pacific (Hong Kong)", Code = "ap-east-1"},
				new Region { Service = service, Name = "Asia Pacific (Tokyo)", Code = "ap-northeast-1"},
				new Region { Service = service, Name = "Asia Pacific (Seoul)", Code = "ap-northeast-2"},
				new Region { Service = service, Name = "Asia Pacific (Singapore)", Code = "ap-southeast-1"},
				new Region { Service = service, Name = "Asia Pacific (Mumbai)", Code = "ap-south-1"},
				new Region { Service = service, Name = "Asia Pacific (Sydney)", Code = "ap-southeast-2"},
				new Region { Service = service, Name = "South America (São Paulo)", Code = "sa-east-1"},
				new Region { Service = service, Name = "Middle East (Bahrain)", Code = "me-south-1"},
				new Region { Service = service, Name = "South America (São Paulo)", Code = "sa-east-1"}
			};
		}
	}
}
