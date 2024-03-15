using Quote.Models;
using System.ComponentModel.DataAnnotations;

namespace Quote.Modal
{

    internal class ProductResponse
    {
        public int ProductId { get; set; }

        public int? CategoryId { get; set; }

        public string ProductName { get; set; }
        public string CateName { get; set; }
        public string Price { get; set; }

        public string Description { get; set; }
        public List<Option> Options { get; set; }
        public List<Image> Images { get; set; }
    }
    internal class ProductCateResponse
    {
        public int ProductId { get; set; }

        public int? CategoryId { get; set; }

        public string? ImagePath { get; set; }

    }
    internal class ProductAllResponse
    {
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public string? ImagePath { get; set; }

    }

}