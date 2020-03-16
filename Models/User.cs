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
        [StringLength(255), Required]
        public string Nickname { get; set; }
        [BsonElement("email")]
        [EmailAddress, Required]
        public string Email { get; set; }
        [BsonElement("password")]
        [StringLength(255), Required]
        public string Password { get; set; }
        [BsonElement("rank")]
        public string Rank { get; set; } = "Guest";


    }
}
