using Microsoft.AspNetCore.Mvc;

namespace Quote.Modal
{
    
    public class ProductModal
    {
        public int CategoryId { get; set; }

        public string Description { get; set; }

        public string ProductName { get; set; }

        public string Price { get; set; }

            
        public IFormFile[] formFiles { get; set; }
      

    }

    public class Options
    {
        public string OptionName { get; set; }

        public string OptionQuantity { get; set; }
    }
}
