using IdentityModel;
using MongoDB.Driver;
using System.Linq;

namespace IdentityRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _collection;

        public UserRepository(IMongoDatabase db)
        {
            _collection = db.GetCollection<User>(User.DocumentName);
        }

        public User GetUser(string email) =>
            _collection.Find(u => u.Email == email).FirstOrDefault();

        public void InsertUser(User user) =>
            _collection.InsertOne(user);
    }
}