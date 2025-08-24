using Mango.Services.ShoppingCardAPI.Models.DTO;

namespace Mango.Services.ShoppingCartAPI.Service.IService
{
    public interface ICouponctService
    {
        public Task<CouponDto> GetCoupon(string couponCode);
    }
}
