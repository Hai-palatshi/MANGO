using Mango.Services.web.Models;
using Mango.web.Models;
using Mango.web.Models.Utility;
using Mango.web.Service.IService;
using static Mango.web.Models.Utility.SD;

namespace Mango.web.Service
{
    public class CartService : ICartService
    {
        private readonly IBaseService baseService;  
        public CartService(IBaseService _baseService)
        {
            baseService = _baseService;
        }

        public async Task<ResponseDto?> ApplyCouponAsync(CartDto cartDto)
        {
            var request = new RequestDto
            {
                ApiType = ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartAPIBase + "/api/cart/ApplyCoupon",
            };
            var response = await baseService.SendAsync(request);
            return response;
        }

        public async Task<ResponseDto?> DeleteFromByDetailCartAsync(CartDto cartDto)
        {
            var request = new RequestDto
            {
                ApiType = ApiType.DELETE,
                Data = cartDto,
                Url = SD.ShoppingCartAPIBase + "/api/cart/removeCoupon"
            };
            var response = await baseService.SendAsync(request);
            return response;    
        }

        public async Task<ResponseDto?> DeleteFromCartAsync(int id)
        {
            var request = new RequestDto
            {
                ApiType = ApiType.DELETE,
                Url = SD.ShoppingCartAPIBase + "/api/cart/DeleteCart",
                Data = id
            };
            var response = await baseService.SendAsync(request);
            return response;    
        }

        public async Task<ResponseDto?> EmailCart(CartDto cartDto)
        {
            RequestDto request = new RequestDto
            {
                ApiType = ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartAPIBase + "/api/cart/EmailCartRequest"
            };
            return await baseService.SendAsync(request);
        }

        public async Task<ResponseDto?> GetCartByUserIdAsync(string userId)
        {
            var request = new RequestDto
            {
                ApiType = ApiType.GET,
                Url = SD.ShoppingCartAPIBase + $"/api/cart/GetCart/{userId}"
            };
            var response = await baseService.SendAsync(request);
            return response;    
        }

        public Task<ResponseDto?> UpsertAsync(CartDto cartDto)
        {
           var request = new RequestDto
           {
               ApiType = ApiType.POST,
               Data = cartDto,
               Url = SD.ShoppingCartAPIBase + "/api/cart/CartUpsert"
           };
            return baseService.SendAsync(request);  
        }
    }
}
