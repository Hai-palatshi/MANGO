using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mango.Services.ShoppingCartAPI.Models.Dto
{
    public class CardHeaderDto
    {
        public int CartHeaderId { get; set; }
        public string? UserId { get; set; }
        public string? CoupobCode { get; set; }
        public double Discount { get; set; }
        public double CardTotal { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
