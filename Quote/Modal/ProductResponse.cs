using Quote.Models;

namespace Quote.Modal
{

    internal class ProductResponse
    {
        public Product Product { get; set; }
        public List<Option> Options { get; set; }
        public List<Images> Images { get; set; }
    }
    public class Images
    {
        public string Src { get; set; }
        public string Description { get; set; }
    }
}