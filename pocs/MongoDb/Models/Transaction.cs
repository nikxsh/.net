using MongoDB.Bson.Serialization.Attributes;
using System.Runtime.Serialization;

namespace MongoDb.Models
{
	[DataContract]
	//This will instruct the driver to ignore any elements that it cannot deserialize into a corresponding property
	[BsonIgnoreExtraElements]
	public class Transaction
	{
		[DataMember(Order = 1)]
		public string Code { get; set; }
		[DataMember(Order = 2)]
		public double Amount  { get; set; }
	}
}
