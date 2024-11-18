using BookStoreApp.Model.Entities;
using BookStoreApp.Model.Interface;
using System.Runtime.CompilerServices;

namespace BookStoreApp.Model.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new();

        public User Add(User user)
        {
            user.Id = _users.Count + 1; // Geçici ID ataması
            _users.Add(user);
            return user;
        }



        public bool Delete(int id)
        {
            var user =_users.FirstOrDefault(x => x.Id == id);
            if (user == null) return false;
            _users.Remove(user);
            return true;
        }



        public List<User> GetAll()
        {
            return _users;
        }



        public User GetByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.Email == email);
        }



        public User GetById(int id)
        {
           return _users.FirstOrDefault( u => u.Id == id);
        }



        public User Update(User user)
        {
            var existingUser = GetById(user.Id);
            if (existingUser == null) Console.WriteLine("Kullanıcı bulunamadı! ");

            existingUser.FullName = user.FullName;
            existingUser.Email = user.Email;

            return existingUser;
        }
    }
}
