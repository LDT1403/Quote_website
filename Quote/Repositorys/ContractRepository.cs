using Quote.Models;

namespace Quote.Repositorys
{
    public class ContractRepository : RepoBase<Contract>
    {
        private readonly DB_SWDContext _context;

        public ContractRepository(DB_SWDContext context)
        {
            _context = context;
        }

    }
}
