using Microsoft.EntityFrameworkCore;
using Quote.Models;

namespace Quote.Repositorys
{
    public class UserRepository : RepoBase<User>
    {
        private readonly DbSwdContext _context;

        public UserRepository(DbSwdContext context)
        {
            _context = context;
        }

        public bool IsEmailExists(string email)
        {
            // Kiểm tra xem địa chỉ email đã tồn tại trong danh sách người dùng hay chưa
            User exixst = _context.Users.FirstOrDefault(u => u.Email == email);
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
