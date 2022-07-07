using MigrationService.Models;

namespace ETL.Models
{
	public class SqlToMongoTemplate
	{
		public Settings Settings { get; set; }
		public BaseTable MainTable { get; set; }
		public NestedTable[] NestedTables { get; set; }
		public int TransferSize { get; set; }
	}

	public class Settings
	{
		public DbSettings Sql { get; set; }
		public DbSettings Mongo { get; set; }
	}
}
