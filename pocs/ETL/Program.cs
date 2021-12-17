using ETL.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Threading.Tasks;

namespace ETL
{
	class Program
	{
		static string _baseUrl = @"C:\Users\nikhilesh.shinde\work\playground\dotnet\pocs\ETL\";
		static Template _template;

		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			_template = ReadTemplate($"{_baseUrl}template.json");
			var transformedlist = Tranform();
			SendToMongoDb(transformedlist);
		}

		static Template ReadTemplate(string url)
		{
			return JsonConvert.DeserializeObject<Template>(File.ReadAllText(url));
		}

		static List<BsonDocument> Tranform()
		{
			// Transform table
			var mainData = ExtractSourceData(_template.Main.ToString());

			var outputList = Load(mainData);

			foreach (var item in outputList)
			{
				// Transform nested tables
				foreach (var nest in _template.Nested)
				{
					var mappingKey = item[nest.SKey].ToString();
					var nestedData = ExtractSourceData(nest.ToString(mappingKey));
					var transformedData = Load(nestedData);
					item.Add(nest.ObjectName, BsonValue.Create(transformedData));
				}
			}

			var dotNetObj = outputList.ConvertAll(BsonTypeMapper.MapToDotNetValue);

			Console.WriteLine(JsonConvert.SerializeObject(dotNetObj, Formatting.Indented));

			return outputList;
		}

		static List<BsonDocument> Load(DataSet data)
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

		static DataSet ExtractSourceData(string query)
		{
			DataSet ds = new DataSet();

			using (SqlConnection connection = new SqlConnection(_template.Settings.Sql.Connection))
			{
				using (SqlCommand cmd = new SqlCommand(query))
				{
					cmd.Connection = connection;
					using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
					{
						sda.Fill(ds);
					}
				}
			}
			return ds;
		}

		static Task SendToMongoDb(List<BsonDocument> payload)
		{
			var settings = MongoClientSettings.FromConnectionString(_template.Settings.Mongo.Connection);
			var client = new MongoClient(settings);
			IMongoDatabase _mongoDatabase = client.GetDatabase(_template.Settings.Mongo.Database);

			var _collection = _mongoDatabase.GetCollection<BsonDocument>(_template.Settings.Mongo.Collection, new MongoCollectionSettings
			{
				AssignIdOnInsert = true
			});

			try
			{
				_collection.InsertMany(payload);
			}
			catch (MongoWriteException ex)
			{
				throw;
			}

			return Task.CompletedTask;
		}

		private static dynamic DictionaryToObject(IDictionary<String, Object> dictionary)
		{
			var expandoObj = new ExpandoObject();
			var expandoObjCollection = (ICollection<KeyValuePair<String, Object>>)expandoObj;

			foreach (var keyValuePair in dictionary)
			{
				expandoObjCollection.Add(keyValuePair);
			}
			dynamic eoDynamic = expandoObj;
			return eoDynamic;
		}
	}
}
