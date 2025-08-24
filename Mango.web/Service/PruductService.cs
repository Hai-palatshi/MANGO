using Mango.Services.web.Models;
using Mango.web.Models;
using Mango.web.Service.IService;
using Mango.web.Models.Utility;


namespace Mango.web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService baseService;
        public ProductService(IBaseService _baseService)
        {
            baseService = _baseService;
        }

        public async Task<ResponseDto?> CreateProductAsync(ProductDto ProductDto)
        {
            var request = new RequestDto
            {
                ApiType = SD.ApiType.POST,
                Data = ProductDto,
                Url = SD.ProductAPIBase + "/api/Product",
            };
            return await this.baseService.SendAsync(request);
        }

        public async Task<ResponseDto?> DeleteProductAsync(int id)
        {
            var request = new RequestDto
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.ProductAPIBase + "/api/Product/" + id,
            };
            return await this.baseService.SendAsync(request);
        }

        public async Task<ResponseDto?> GetAllProductAsync()
        {
            var requestDto = new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductAPIBase + "/api/Product",
            };
            return await baseService.SendAsync(requestDto);
        }
        public async Task<ResponseDto?> GetProductAsync(string ProductCode)
        {
            var request = new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductAPIBase + "/api/Product/GetByCode" + ProductCode,
            };
            return await this.baseService.SendAsync(request);
        }

        public async Task<ResponseDto?> GetByIdProductAsync(int id)
        {
            var request = new RequestDto
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProductAPIBase + "/api/Product/" + id,
            };
            return await this.baseService.SendAsync(request);
        }

        public async Task<ResponseDto?> UpdateProductAsync(ProductDto ProductDto)
        {
            var request = new RequestDto
            {
                ApiType = SD.ApiType.PUT,
                Data = ProductDto,
                Url = SD.ProductAPIBase + "/api/Product",
            };
            return await this.baseService.SendAsync(request);
        }
    }
}
