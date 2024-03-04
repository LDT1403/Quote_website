using Quote.Models;

namespace Quote.Modal
{
  
    internal class ProductResponse
    {
        public Product Product { get; set; }
        public List<Option> Options { get; set; }
    }
}
