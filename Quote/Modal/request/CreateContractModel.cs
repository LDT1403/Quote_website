using System.ComponentModel.DataAnnotations;

namespace Quote.Modal.request
{
    public class CreateContractModel
    {
        public int? RequestId { get; set; }
        public int taskId { get; set; }

        [StringLength(50)]
        public string FinalPrice { get; set; }

        [StringLength(50)]
        public string ConPrice { get; set; }
        public IFormFile ContractFile { get; set; }
        public string Status { get; set; }
    }
}
