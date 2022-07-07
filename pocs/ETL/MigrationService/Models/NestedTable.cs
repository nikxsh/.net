namespace MigrationService.Models
{
	public class NestedTable : BaseTable
	{
		public string ObjectIdentifier { get; set; }
		public string TargetKey { get; set; }
		public NestedTable Nest { get; set; } = null;
	}
}
