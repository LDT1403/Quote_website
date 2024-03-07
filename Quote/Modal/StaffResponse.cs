using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Quote.Modal
{
    public class StaffResponse
    {
        public int UserId { get; set; }

       
        public string UserName { get; set; }

      
        public string Password { get; set; }
               
        public bool? Status { get; set; }

     
        
    }
}
