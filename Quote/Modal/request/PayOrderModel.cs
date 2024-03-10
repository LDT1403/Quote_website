using System.ComponentModel.DataAnnotations;

namespace Quote.Modal.request
{
    public class PayRequestModel
    {
        [Required]
        public int ContractId { get; set; }
        [Required]
        public string Method { get; set; }

    }

}
