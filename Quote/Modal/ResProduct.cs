using Quote.Models;

namespace Quote.Modal
{
    public class ResProduct
    {
        public Product Product { get; set; }

        public List<Image> Images { get; set; }

        public List<Option> options { get; set; }
    }
}
