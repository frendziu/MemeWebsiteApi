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
        private List<Meme> _memes1 = new List<Meme>();

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

        public int GetCount()
        {
            int count = 0;
            _memes1 = _memes.Find(meme => true).ToList();
            count = _memes1.Count();

            return count;
        }

        public List<Meme>GetMemesPage(int pagenumber, int limit)
        {

            _memes1 = _memes.Find(meme => true).ToList();
            List<Meme> SortedList = _memes1.OrderByDescending(x => x.Date).Skip((pagenumber-1)*limit).Take(limit).ToList();
            return SortedList;
        }
            
        public List<Meme>GetByTags(string[] tags, int page, int limit)
        {
            _memes1 = _memes.Find(meme => meme.Tags == tags).ToList();
            List<Meme> SortedList = _memes1
                .OrderByDescending(x => x.Date)
                .Skip((page - 1) * limit)
                .Take(limit).ToList();
            return SortedList;
        }
    

        public Meme Create(Meme meme)
        {
            _memes.InsertOne(meme);
            return meme;
        }
        
        public void RatingPlus(string id, Meme memeIn)
        {
            memeIn.Rating.Value++;
            _memes.ReplaceOne(meme => meme.Id == id, memeIn);
        }

        public void RatingMinus(string id, Meme memeIn)
        {
            memeIn.Rating.Value--;
            _memes.ReplaceOne(meme => meme.Id == id, memeIn);
        }

        public void Update(string id, Meme memeIn) =>
            _memes.ReplaceOne(meme => meme.Id == id, memeIn);

        public void Remove(Meme memeIn) =>
            _memes.DeleteOne(meme => meme.Id == memeIn.Id);

        public void Remove(string id) =>
            _memes.DeleteOne(meme => meme.Id == id);
    }
}
