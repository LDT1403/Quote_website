using Microsoft.AspNetCore.Mvc;
using Quote.Models;

namespace Quote.Modal
{

    public class ProductModal
    {
        public int CategoryId { get; set; }

        public string Description { get; set; }

        public string ProductName { get; set; }

        public string Price { get; set; }

        public List<Imagess> Imagess { get; set; }

        public List<Options> Options { get; set; }
        public bool isDelete { get; set; }

    }
    public class Imagess
    {
        public string base24 { get; set; }

        public string description { get; set; }
    }
    public class Options
    {
        public string OptionName { get; set; }

        public string OptionQuantity { get; set; }
    }
}
