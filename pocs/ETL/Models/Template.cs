namespace ETL.Models
{
	public class Template
	{
		public Settings Settings { get; set; }
		public Base Main { get; set; }
		public Nested[] Nested { get; set; }
	}

	public class Settings
	{
		public DbSettings Sql { get; set; }
		public DbSettings Mongo { get; set; }
	}

	public class DbSettings
	{
		public string Connection { get; set; }
		public string Database { get; set; }
		public string Collection { get; set; }
	}

	public class Nested
	{
		public string TableName { get; set; }
		public string ObjectName { get; set; }
		public string SKey { get; set; }
		public string TKey { get; set; }
		public string[] Select { get; set; }
		public string Conditions { get; set; }

		public string ToString(string matchingId)
		{
			return $"SELECT {string.Join(",", Select)} FROM {TableName} WHERE {TKey} = {matchingId}";
		}
	}

	public class Base
	{
		public string TableName { get; set; }
		public string[] Select { get; set; }
		public string Conditions { get; set; }

		public override string ToString()
		{
			return $"SELECT {string.Join(",", Select)} FROM {TableName}";
		}
	}
}
