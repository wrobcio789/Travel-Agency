using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pg.Rsww.RedTeam.DataStorage.Models
{
	public abstract class Entity
	{
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string? Id { get; set; }
	}
}
