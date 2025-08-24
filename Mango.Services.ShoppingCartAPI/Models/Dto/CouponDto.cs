namespace Mango.Services.ShoppingCardAPI.Models.DTO
{
    public class CouponDto
    {
        public int CoupinId { get; set; }
        public string CouponCode { get; set; }
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
