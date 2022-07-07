using ETL.Models;
using Microsoft.AspNetCore.Mvc;
using MigrationService.Helper;
using MigrationService.Models;
using MigrationService.Repository;
using MongoDB.Bson;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MigrationService.Controllers
{
	[ApiController]
	[Route("api/sqltomongo")]
	public class SqlToMongoController : ControllerBase
	{
		private ISqlRepository _sqlRepository;
		private IMongoRepository _mongoRepository;

		public SqlToMongoController(ISqlRepository sqlRepository, IMongoRepository mongoRepository)
		{
			_sqlRepository = sqlRepository;
			_mongoRepository = mongoRepository;
		}

		[HttpPost("schema")]
		public async Task<IActionResult> GetSchema([FromBody] SchemaRequest request)
		{
			if (request == null || string.IsNullOrEmpty(request.URL) || string.IsNullOrEmpty(request.DbName))
				return BadRequest();

			var schemaDs = _sqlRepository.GetTablesSchema(request.URL, request.ToSchemaQuery());

			var schema = LoadDbSchema(schemaDs);

			return await Task.FromResult(new ContentResult
			{
				ContentType = "application/json",
				Content = JsonConvert.SerializeObject(schema, Formatting.Indented)
			});
		}

		[HttpPost("sample")]
		public async Task<IActionResult> GetSampleDocument([FromBody] SqlToMongoTemplate template)
		{
			if (template == null && template.MainTable == null && template.NestedTables == null && template.Settings == null)
				return BadRequest();

			// Transform table
			var mainTableDataset = _sqlRepository.ExtractSourceData(template.Settings.Sql.Connection, template.ToSQLQuery(offset: 0, fetch: 1));

			var outputList = Transform(mainTableDataset, template);

			var dotNetObj = outputList.ConvertAll(BsonTypeMapper.MapToDotNetValue);

			return await Task.FromResult(new ContentResult
			{
				ContentType = "application/json",
				Content = JsonConvert.SerializeObject(dotNetObj, Formatting.Indented)
			});
		}

		[HttpPost("migrate")]
		public async Task<IActionResult> MigrateData([FromBody] SqlToMongoTemplate template)
		{
			if (template == null)
				return BadRequest();

			var mainData = _sqlRepository.ExtractSourceData(template.Settings.Sql.Connection, template.ToSQLQuery());

			var outputList = Transform(mainData, template);

			await _mongoRepository.Save(template.Settings.Mongo, outputList);

			return new ContentResult
			{
				ContentType = "application/json",
				Content = JsonConvert.SerializeObject(new MigrationResponse { Message = "Migration Completed!" }, Formatting.Indented)
			};
		}

		private List<BsonDocument> Transform(DataSet mainData, SqlToMongoTemplate template)
		{
			// Transform table
			var outputList = LoadTableData(mainData);

			foreach (var item in outputList)
			{
				// Transform nested tables
				foreach (var nest in template.NestedTables)
				{
					var mappingKey = item[nest.TargetKey].ToString();
					var nestedData = _sqlRepository.ExtractSourceData(template.Settings.Sql.Connection, template.ToSQLQuery(isNestedQuery: true, nestedTable: nest, nestedMatchingId: mappingKey));
					var transformedData = LoadTableData(nestedData);
					item.Add(nest.ObjectIdentifier, BsonValue.Create(transformedData));
				}
			}

			return outputList;
		}

		private List<BsonDocument> TransformNestedTables(DataSet mainData, SqlToMongoTemplate template)
		{

		}

		private List<BsonDocument> LoadTableData(DataSet data)
		{
			var outputList = new List<BsonDocument>();

			foreach (DataRow row in data.Tables[0].Rows)
			{
				var pair = new BsonDocument();

				foreach (DataColumn col in data.Tables[0].Columns)
				{
					var fieldValue = row[$"{col.ColumnName}"].ToString();
					pair.Add(col.ColumnName, fieldValue);
				}

				outputList.Add(pair);
			}

			return outputList;
		}

		private List<SchemaResponse> LoadDbSchema(DataSet data)
		{
			var dbSchemaList = new List<SchemaResponse>();

			var dbSchema = from r in data.Tables[0].Rows.OfType<DataRow>()
						   group r by r["TABLE_NAME"] into g
						   select new { Table = g.Key, Data = g.Select(x => x.ItemArray) };

			foreach (var row in dbSchema)
			{
				var schemaResponse = new SchemaResponse
				{
					Table = row.Table.ToString()
				};

				foreach (var r in row.Data)
				{
					schemaResponse.Columns.Add(r[1].ToString());
				}

				dbSchemaList.Add(schemaResponse);
			}

			return dbSchemaList;
		}
	}
}
