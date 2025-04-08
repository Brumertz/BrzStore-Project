using MongoDB.Driver;
using BrzStore.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BrzStore.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserService(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("BrzStoreDb"); // Replace with your actual database name
            _userCollection = database.GetCollection<User>("Users"); // Replace with your actual collection name
        }

        public async Task<bool> RegisterAsync(string email, string password)
        {
            var user = await _userCollection.Find(u => u.Email == email).FirstOrDefaultAsync();
            if (user != null)
            {
                return false;  // User already exists
            }

            // Hash the password before storing it
            var passwordHasher = new PasswordHasher<User>();
            var hashedPassword = passwordHasher.HashPassword(null, password);

            var newUser = new User
            {
                Email = email,
                Password = hashedPassword
            };

            await _userCollection.InsertOneAsync(newUser);  // Save the new user to MongoDB
            return true;
        }
        public async Task<bool> LoginAsync(string email, string password)
        {
            var user = await _userCollection.Find(u => u.Email == email).FirstOrDefaultAsync();
            if (user == null)
            {
                return false;  // User not found
            }

            // Use PasswordHasher to verify the password against the hash stored in MongoDB
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(null, user.Password, password);

            return result == PasswordVerificationResult.Success;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userCollection.Find(u => u.Email == email).FirstOrDefaultAsync();
        }
    }
}
