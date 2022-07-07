using System.Data;
using System.Data.SqlClient;

namespace MigrationService.Repository
{
	public interface ISqlRepository
	{
		DataSet ExtractSourceData(string connection, string query);
		DataSet GetTablesSchema(string connection, string query);
	}

	public class SqlRepository : ISqlRepository
	{
		public DataSet ExtractSourceData(string connection, string query)
		{
			DataSet ds = new DataSet();

			using (SqlConnection conn = new SqlConnection(connection))
			{
				using (SqlCommand cmd = new SqlCommand(query))
				{
					cmd.Connection = conn;
					using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
					{
						sda.Fill(ds);
					}
				}
			}
			return ds;
		}

		public DataSet GetTablesSchema(string connection, string query)
		{
			DataSet ds = new DataSet();

			using (SqlConnection conn = new SqlConnection(connection))
			{
				using (SqlCommand cmd = new SqlCommand(query))
				{
					cmd.Connection = conn;
					using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
					{
						sda.Fill(ds);
					}
				}
			}
			return ds;
		}
	}
}
