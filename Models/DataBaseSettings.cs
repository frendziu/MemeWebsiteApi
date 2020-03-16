using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemeWebsiteApi.Models
{
    public class DataBaseSettings : IDatabaseSettings
    {
        public string UsersCollectionName { get; set; }
        public string MemesCollectionName { get; set; }
        public string TagsCollectionName { get; set; }
        public string CommentsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IDatabaseSettings
    {
        string UsersCollectionName { get; set; }
        string MemesCollectionName { get; set; }
        string TagsCollectionName { get; set; }
        string CommentsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
