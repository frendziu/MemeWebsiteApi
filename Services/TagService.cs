using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MemeWebsiteApi.Models;
using MongoDB.Driver;

namespace MemeWebsiteApi.Services
{
    public class TagService
    {
        private readonly IMongoCollection<TagModel> _tags;
        private List<TagModel> _tags1 = new List<TagModel>();

        public TagService(IDatabaseSettings settings)
        {

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _tags = database.GetCollection<TagModel>(settings.TagsCollectionName);
        }

        public bool CheckTag(string name)
        {
            _tags1 = _tags.Find(tagModel => true).ToList();
            var tagModel = _tags1.SingleOrDefault(x => x.Name == name);
            if (tagModel == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<TagModel> GetInOrder()
        {
            _tags1 = _tags.Find(tagModel => true).ToList();
            List<TagModel> SortedList = _tags1.OrderBy(x => x.Name).ToList();
            return SortedList;
        }
 
        public List<TagModel> Get() =>
            _tags.Find(tagModel => true).ToList();

        public TagModel Get(string id) =>
            _tags.Find<TagModel>(tagModel => tagModel.Id == id).FirstOrDefault();

        public TagModel Create(TagModel tagModel)
        {
            _tags.InsertOne(tagModel);
            return tagModel;
        }

        public void Update(string id, TagModel tagModelIn) =>
            _tags.ReplaceOne(tagModel => tagModel.Id == id, tagModelIn);

        public void Remove(TagModel tagModelIn) =>
            _tags.DeleteOne(tagModel => tagModel.Id == tagModelIn.Id);

        public void Remove(string id) =>
            _tags.DeleteOne(tagModel => tagModel.Id == id);
    }
}
