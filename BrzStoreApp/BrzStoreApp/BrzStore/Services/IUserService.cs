namespace BrzStore.Services
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(string email, string hashedPassword);
        Task<Models.User> GetUserByEmailAsync(string email);
        Task<bool> LoginAsync(string email, string password);
    }
}
