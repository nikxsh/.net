using ETL.Models;
using MigrationService.Models;

namespace MigrationService.Helper
{
	public static class Parser
	{
		public static string ToSQLQuery(this SqlToMongoTemplate template, 
			bool isNestedQuery = false, 
			NestedTable nestedTable = null, 
			string nestedMatchingId = "", 
			int offset = 0, 
			int fetch = 0)
		{
			var selectQuery = string.Join(",", template.MainTable.Select);

			var conditions = string.Empty;
			if (!string.IsNullOrEmpty(template.MainTable.Conditions))
				conditions += $"Where {template.MainTable.Conditions} ";

			if(isNestedQuery && nestedTable != null)
			{
				var nestedCondition = $"{nestedTable.Key} = '{nestedMatchingId}'";
				if (!string.IsNullOrEmpty(conditions))
					conditions += $"AND {nestedCondition}'";
				else
					conditions += nestedCondition;
			}

			var limitQuery = string.Empty;
			if (fetch > 0)
				limitQuery = $"OFFSET {offset} ROWS FETCH NEXT {fetch} ROW ONLY";

			return $"{selectQuery} " +
				   $"FROM {template.MainTable.TableName} " +
				   $"{conditions} " +
				   $"ORDER BY {template.MainTable.Key} " +
				   $"{limitQuery}";
		}

		public static string ToSchemaQuery(this SchemaRequest schemaRequest)
		{
			return @$"USE [{schemaRequest.DbName}]
					SELECT CONCAT(isc.TABLE_SCHEMA, '.', isc.TABLE_NAME) TABLE_NAME, isc.COLUMN_NAME
					FROM sys.tables st INNER JOIN INFORMATION_SCHEMA.COLUMNS isc
					ON st.name = isc.TABLE_NAME
					WHERE isc.TABLE_NAME LIKE '%{schemaRequest.Filter}%' AND st.is_ms_shipped = 0";
		}
	}
}
