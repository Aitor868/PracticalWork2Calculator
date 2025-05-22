// Services/UserManager.cs
using PracticalWork2Calculator.Models;
using System.Text.Json;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System; // Added for StringComparison

namespace PracticalWork2Calculator.Services
{
    public class UserManager
    {
        private static string _filePath = Path.Combine(FileSystem.AppDataDirectory, "users.json");
        private List<User> _users;

        public UserManager()
        {
            _users = LoadUsersFromFile();
        }

        private List<User> LoadUsersFromFile()
        {
            if (!File.Exists(_filePath))
            {
                return new List<User>();
            }
            try
            {
                string json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
            }
            catch
            {
                // Handle deserialization errors gracefully, e.g., log the error
                return new List<User>();
            }
        }
    
        private void SaveUsersToFile()
        {
            string json = JsonSerializer.Serialize(_users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        public async Task<(bool Success, string Message)> RegisterUserAsync(string name, string username, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return (false, "Todos los campos son obligatorios.");
            }

            if (username.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                return (false, "El nombre y el nombre de usuario deben ser diferentes.");
            }

            if (_users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)))
            {
                return (false, "El nombre de usuario ya existe.");
            }

            // Assuming PasswordValidator is a static class with a static method IsPasswordStrong
            if (!PasswordValidator.IsPasswordStrong(password))
            {
                return (false, "La contraseña no es lo suficientemente fuerte. Debe tener al menos 8 caracteres, incluir mayúsculas, minúsculas, números y un símbolo especial.");
            }

            var newUser = new User
            {
                Name = name,
                Username = username,
                Email = email,
                Password = password, // Direct password storage as per requirement
                OperationsExecuted = 0
            };
            _users.Add(newUser);
            SaveUsersToFile();
            return (true, "Usuario registrado exitosamente.");
        }

        public async Task<(bool Success, User User, string Message)> LoginUserAsync(string username, string password)
        {
            var user = _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && u.Password == password);
            if (user != null)
            {
                return (true, user, "Inicio de sesión exitoso.");
            }
            return (false, null, "Nombre de usuario o contraseña incorrectos.");
        }

        public User GetUser(string username)
        {
            return _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public void IncrementOperations(string username)
        {
            var user = _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            if (user != null)
            {
                user.OperationsExecuted++;
                SaveUsersToFile();
            }
        }
    }
}