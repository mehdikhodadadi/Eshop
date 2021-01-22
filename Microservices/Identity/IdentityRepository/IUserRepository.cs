using IdentityModel;

namespace IdentityRepository
{
    public interface IUserRepository
    {
        User GetUser(string email);

        void InsertUser(User user);
    }
}