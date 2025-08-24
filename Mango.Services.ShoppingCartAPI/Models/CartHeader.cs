using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Services.ShoppingCartAPI.Models
{
    public class CartHeader
    {
        [Key]
        public int CartHeaderId { get; set; }
        public string? UserId { get; set; }
        public string? CoupobCode { get; set; }

        [NotMapped]//this is not goin in to the database    
        public double Discount { get; set; }
        [NotMapped]// this is not going in to the database
        public double CardTotal { get; set; }
   
    }
}
