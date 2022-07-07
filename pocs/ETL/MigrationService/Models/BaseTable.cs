namespace MigrationService.Models
{
	public class BaseTable
	{
		public string Key { get; set; }
		public string TableName { get; set; }
		public string[] Select { get; set; }
		public string Conditions { get; set; }
	}
}
