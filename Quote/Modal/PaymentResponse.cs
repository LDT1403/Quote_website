namespace Quote.Modal
{
    public class PaymentResponse
    {
        public int PaymentId { get; set; }

        public int UserId { get; set; }

        public string? Method { get; set; }

        public DateTime? DatePay { get; set; }

        public decimal? PricePay { get; set; }

        public string? PaymentUrl { get; set; }

    }
}
