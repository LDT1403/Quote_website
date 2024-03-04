using Quote.Models;

namespace Quote.Repositorys
{
    public class ProductRepository : RepoBase<Product>
    {
        private readonly DB_SWDContext _context;

        public ProductRepository(DB_SWDContext context)
        {
            _context = context;
        }
    }
}
