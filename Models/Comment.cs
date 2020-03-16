using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MemeWebsiteApi.Models
{
    public class Comment
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("author"), StringLength(255), Required]
        public string Author { set; get; }
        [BsonElement("content"), StringLength(255), Required]
        public string Content { set; get; }
        [BsonElement("date"), BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Date { set; get; } = DateTime.Now;
    }
}
