using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.Interface
{
    public interface IUserRepository
    {
        User Add(User user);
        User GetById(int id);
        User GetByEmail(string email);
        List<User> GetAll();
        User Update(User user);
        bool Delete(int id);
    }
}
