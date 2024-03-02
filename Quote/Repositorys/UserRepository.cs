using Microsoft.EntityFrameworkCore;
using Quote.Models;

namespace Quote.Repositorys
{
    public class UserRepository : RepoBase<User>
    {
        private readonly DB_SWDContext _context;

        public UserRepository(DB_SWDContext context)
        {
            _context = context;
        }

        public bool IsEmailExists(string email)
        {
            // Kiểm tra xem địa chỉ email đã tồn tại trong danh sách người dùng hay chưa
            User exixst = _context.Users.FirstOrDefault(u => u.Email== email);
            if (exixst != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
