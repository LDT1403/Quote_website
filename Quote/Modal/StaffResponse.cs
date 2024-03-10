using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Quote.Modal
{
    public class StaffResponse
    {
        public int UserId { get; set; }
    
        public string UserName { get; set; }
             
        public string? Status { get; set; }
    
        public string Phone { get; set; }

        [StringLength(250)]
        public string Role { get; set; }
 
        public byte[] Image { get; set; }
    
        public DateTime? Dob { get; set; }

        public string Position { get; set; }
   
        public string Email { get; set; }

    }
}
