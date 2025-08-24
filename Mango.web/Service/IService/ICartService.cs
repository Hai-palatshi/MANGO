using Mango.Services.web.Models;
using Mango.web.Models;

namespace Mango.web.Service.IService
{
    public interface ICartService
    {
        Task<ResponseDto?> GetCartByUserIdAsync(string userId);
        Task<ResponseDto?> UpsertAsync(CartDto cartDto);
        Task<ResponseDto?> DeleteFromCartAsync(int id);
        Task<ResponseDto?> DeleteFromByDetailCartAsync(CartDto cartDto);
        Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto);
        Task<ResponseDto?> EmailCart(CartDto cartDto);

        //Task<ResponseDto?> CreateCouponAsync(CouponDto couponDto);
        //Task<ResponseDto?> UpdateCouponAsync(CouponDto couponDto);
     }
}
