using Quote.Interfaces.ServiceInterface;
using Quote.Modal;
using Quote.Models;
using Quote.Repositorys;

namespace Quote.Services
{
    public class OptionService : IOptionService
    {
        private readonly OptionRepository _repo;

        public OptionService(OptionRepository repo)
        {
            _repo = repo;
        }
        public async System.Threading.Tasks.Task AddOptions(Option opt)
        {
            await _repo.AddAsync(opt);
        }

        public async Task<List<Option>> GetOptionById(int productId)
        {
            var list = await _repo.GetAllAsync();
            if(list != null)
            {
                 list = list.Where(p=>p.ProductId == productId).ToList();
                if(list != null)
                {
                    return list;
                }
            }
            return null;

        }

        public Task<Option> Update(int productId, Options[] option)
        {
            var pro = _repo.GetAllAsync().Result.Where(p=>p.ProductId == productId).FirstOrDefault();
            if (pro != null)
            {
                foreach (var opt in option)
                {
                    pro.OptionName = opt.OptionName;
                    pro.Quantity = int.Parse(opt.OptionQuantity);
                }
                return _repo.UpdateAsync(pro);

            }
            return null;

           
        }
    }
}
