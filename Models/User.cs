using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MemeWebsiteApi.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("nickname")]
        [StringLength(255)]
        public string Nickname { get; set; }
        [BsonElement("email")]
        [EmailAddress]
        public string Email { get; set; }
        [BsonElement("password")]
        [StringLength(255)]
        public string Password { get; set; }
        [BsonElement("rank")]
        public string Rank { get; set; } = "User";
        public string Token { get; set; }

    }
}
