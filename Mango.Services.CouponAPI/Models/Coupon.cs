using System.ComponentModel.DataAnnotations;

namespace Mango.Services.CouponAPI.Models
{
    public class Coupon
    {
        [Key]
        public int CoupinId { get; set; }
        [Required]
        public string CouponCode { get; set; }
        [Required]
        public double DiscountAmount { get; set; }
        [Required]
        public int MinAmount { get; set; }
        
    }
}
