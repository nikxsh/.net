using System.Collections.Generic;

namespace MigrationService.Models
{
	public class SchemaRequest
	{
		public string URL { get; set; }
		public string DbName { get; set; }
		public string Filter { get; set; }
	}

	public class SchemaResponse
	{
		public string Table { get; set; }
		public List<string> Columns { get; set; } = new List<string>();
	}
}
