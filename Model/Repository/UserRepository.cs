using BookStoreApp.Model.Entities;
using BookStoreApp.Model.Interface;
using System.Runtime.CompilerServices;

namespace BookStoreApp.Model.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new();

        public async Task<User> AddAsync(User user)
        {
            await Task.Run(() =>
            {
                user.Id = _users.Count + 1; // Geçici ID ataması
                _users.Add(user);
            });
            return user;
        }



        public async Task<bool> DeleteAsync(int id)
        {
            var user =_users.FirstOrDefault(x => x.Id == id);
            if (user == null) return false;
            _users.Remove(user);

            await Task.CompletedTask;
            return true;
        }



        public async Task <List<User>> GetAllAsync()
        {
            return await Task.Run (() => _users);
        }



        public async Task<User?> GetByEmailAsync(string email)
        {

            return await Task.Run (() => _users.FirstOrDefault(u => u.Email == email));
        }



        public async Task<User?> GetByIdAsync(int id)
        {
           return await Task.Run(() =>_users.FirstOrDefault( u => u.Id == id));
        }



        public async Task<User> UpdateAsync(User user)
        {
            var existingUser = await GetByIdAsync(user.Id);
            if (existingUser == null) throw new Exception("Kullanıcı bulunamadı.");

            existingUser.FullName = user.FullName;
            existingUser.Email = user.Email;

            return existingUser;
        }
    }
}
