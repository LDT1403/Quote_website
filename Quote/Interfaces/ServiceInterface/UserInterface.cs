using Quote.Modal;
using Quote.Models;

namespace Quote.Interfaces.ServiceInterface
{
    public interface UserInterface
    {
        Task<User> GetUserIDAsync(int userId);
        Task<List<User>> GetUsersAsync();
        Task<User> RegisterAsync(RegisterModal user);

       

    }
}
