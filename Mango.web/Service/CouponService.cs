using Mango.Services.web.Models;
using Mango.web.Models;
using Mango.web.Service.IService;
using Mango.web.Models.Utility;

namespace Mango.web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService baseService;
        public CouponService(IBaseService _baseService)
        {
            baseService = _baseService;
        }

        public async Task<ResponseDto?> CreateCouponAsync(CouponDto couponDto)
        {
            var request = new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = couponDto,
                Url = SD.CouponAPIBase + "/api/coupon",
            };
            return await this.baseService.SendAsync(request);
        }

        public async Task<ResponseDto?> DeleteCouponAsync(int id)
        {
            var request = new RequestDto
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.CouponAPIBase + "/api/coupon/" + id,
            };
            return await this.baseService.SendAsync(request);
        }

        public async Task<ResponseDto?> GetAllCouponAsync()
        {
            var requestDto = new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + "/api/coupon",
            };
            return await baseService.SendAsync(requestDto);
        }
        public async Task<ResponseDto?> GetCouponAsync(string couponCode)
        {
            var request = new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + "/api/coupon/GetByCode" + couponCode,
            };
            return await this.baseService.SendAsync(request);
        }

        public async Task<ResponseDto?> GetByIdCouponAsync(int id)
        {
            var request = new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + "/api/coupon/" + id,
            };
            return await this.baseService.SendAsync(request);
        }

        public async Task<ResponseDto?> UpdateCouponAsync(CouponDto couponDto)
        {
            var request = new RequestDto
            {
                ApiType = SD.ApiType.PUT,
                Data = couponDto,   
                Url = SD.CouponAPIBase + "/api/coupon",
            };
            return await this.baseService.SendAsync(request);
        }
    }
}
