namespace Quote.Modal.request
{
    public class CreateRequestModel
    {
        public string? Status { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? ProductId { get; set; }
    }
}
