using Quote.Modal;
using Quote.Models;

namespace Quote.Interfaces.ServiceInterface
{
    public interface UserInterface
    {
        Task<User> GetUserIDAsync(int userId);
        Task<List<User>> GetUsersAsync();
        Task<User> RegisterAsync(RegisterModal user);
        Task<User> RegisterStaffAsync(User user);

        Task<User> UserById(int userId);

        Task<List<User>> GetStaffByStatus(int staffid);

        Task<List<User>> GetStaffById(int staffid);

        Task<User> UpdateStatusStaff(User user);

    }
}
