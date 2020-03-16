using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemeWebsiteApi.Models;
using MongoDB.Driver;

namespace MemeWebsiteApi.Services
{
    public class MemeService
    {
        private readonly IMongoCollection<Meme> _memes;

        public MemeService(IDatabaseSettings settings)
        {

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _memes = database.GetCollection<Meme>(settings.MemesCollectionName);
        }

        public List<Meme> Get() =>
            _memes.Find(Meme => true).ToList();

        public Meme Get(string id) =>
            _memes.Find<Meme>(meme => meme.Id == id).FirstOrDefault();

        public Meme Create(Meme meme)
        {
            _memes.InsertOne(meme);
            return meme;
        }

        public void Update(string id, Meme memeIn) =>
            _memes.ReplaceOne(meme => meme.Id == id, memeIn);

        public void Remove(Meme memeIn) =>
            _memes.DeleteOne(meme => meme.Id == memeIn.Id);

        public void Remove(string id) =>
            _memes.DeleteOne(meme => meme.Id == id);
    }
}
