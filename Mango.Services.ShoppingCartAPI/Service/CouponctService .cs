using Mango.Services.ShoppingCardAPI.Models.DTO;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartAPI.Service
{
    public class CouponctService : ICouponctService
    {
        private readonly IHttpClientFactory clientFactory;
        public CouponctService(IHttpClientFactory _clientFactory)
        {
            clientFactory = _clientFactory;
        }

        public async Task<CouponDto> GetCoupon(string couponCode)
        {
            var client = clientFactory.CreateClient("Cuopon");
            var response = await client.GetAsync($"/api/coupon/GetByCode/{couponCode}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (resp != null && resp.IsSuccess)
            {
                var backToJason = Convert.ToString(resp.Result);
                return JsonConvert.DeserializeObject<CouponDto> (backToJason);
            }
            else
            {
                return new CouponDto();
            }
        }

       
    }
}
