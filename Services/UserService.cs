using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using MemeWebsiteApi.Models;
using MongoDB.Driver;
using MemeWebsiteApi.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MemeWebsiteApi.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
    }

    public class UserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly AppSettings _appSettings;
        private List<User> _users1 = new List<User>();
       

        public UserService(IDatabaseSettings settings, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _users = database.GetCollection<User>(settings.UsersCollectionName);
            
        }

        public List<User> Get() =>
            _users.Find(user => true).ToList();

        public User Get(string id) =>
            _users.Find<User>(user => user.Id == id).FirstOrDefault();

        public User GetByNickname(string nickname) =>
            _users.Find<User>(user => user.Nickname == nickname).FirstOrDefault();

        public User Create(User user)
        {
           
            _users.InsertOne(user);
            return user;
        }

        public bool CheckNickname(string nickname)
        {
            _users1 = _users.Find(user => true).ToList();
            var user = _users1.SingleOrDefault(x => x.Nickname == nickname);
            if (user == null)
            {
                return false;
            }
            else
            {
                return true ;
            }
            
        }

        public bool CheckEmail(string email)
        {
            _users1 = _users.Find(user => true).ToList();
            var user = _users1.SingleOrDefault(x => x.Email == email);
            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public void SetRank(string id, string rank, User user)
        {
            user.Rank = rank;
            _users.ReplaceOne(user => user.Id == id, user);
        }

       

        public User Authenticate(string username, string password)
        {
            _users1 = _users.Find(user => true).ToList(); 
            var user = _users1.SingleOrDefault(x => x.Nickname == username && x.Password == password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user.WithoutPassword();
        }

        public IEnumerable<User> GetAll()
        {
            return _users1.WithoutPasswords();
        }

        public void Update(string id, User userIn) =>
            _users.ReplaceOne(user => user.Id == id, userIn);

        public void Remove(User userIn) =>
            _users.DeleteOne(user => user.Id == userIn.Id);

        public void Remove(string id) =>
            _users.DeleteOne(user => user.Id == id);


    }
}
