using Quote.Models;

namespace Quote.Modal
{
    public class CartResponse
    {
        public int CartDetailId { get; set; }
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public string ProductThumbnail { get; set; }

    }
}
