﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MemeWebsiteApi.Models
{
    public class Meme
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("type"), StringLength(255), Required]
        public string Type { set; get; }
        [BsonElement("title"), StringLength(255), Required]
        public string Title { set; get; }
        [BsonElement("tags"), Required]
        public string[] Tags { set; get; }
        [BsonElement("author"), StringLength(255), Required]
        public string Author { set; get; }
        [BsonElement("rating")]
        public Rating Rating { set; get; } = new Rating();
        [BsonElement("date"), BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Date { set; get; } = DateTime.Now;
        [BsonElement("FilePath")]
        public string FilePath { get; set; }
        [BsonElement("FileName"), Required]
        public string FileName { get; set; }
       
    }
    public class Rating
    {
        [BsonElement("value")]
        public int Value { set; get; } = 0;
        [BsonElement("voted")]
        public string[] Voted { set; get; }
    }

}
