using System.Text.Json;
using UserAuthApp.Models;

namespace UserAuthApp.Services
{
    public class UserService
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "users.json");

        public UserService()
        {
            if (!Directory.Exists(Path.GetDirectoryName(_filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(_filePath));

            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath, "[]");
        }

        public List<UserModel> GetAllUsers()
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<UserModel>>(json) ?? new List<UserModel>();
        }

        public UserModel GetUser(string username)
        {
            var users = GetAllUsers();
            return users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public void AddUser(UserModel user)
        {
            var users = GetAllUsers();
            if (users.Any(u => u.Username.Equals(user.Username, StringComparison.OrdinalIgnoreCase)))
                return;
            users.Add(user);
            SaveAll(users);
        }

        public bool UpdateUser(string username, UserModel updatedUser)
        {
            var users = GetAllUsers();
            var existing = users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (existing == null) return false;

            existing.Password = updatedUser.Password;
            existing.Phone = updatedUser.Phone;
            existing.Verified = updatedUser.Verified;
            SaveAll(users);
            return true;
        }

        public bool DeleteUser(string username)
        {
            var users = GetAllUsers();
            var target = users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (target == null) return false;

            users.Remove(target);
            SaveAll(users);
            return true;
        }

        private void SaveAll(List<UserModel> users)
        {
            var json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
    }
}
