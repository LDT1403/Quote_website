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

        public async Task<bool> IsEmailExists(string email)
        {
            User existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return existingUser != null;
        }
    }
}
