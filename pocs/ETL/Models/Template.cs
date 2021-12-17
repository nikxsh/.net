namespace ETL.Models
{
	public class Template
	{
		public Base Main { get; set; }
		public Nested[] Nested { get; set; }
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
