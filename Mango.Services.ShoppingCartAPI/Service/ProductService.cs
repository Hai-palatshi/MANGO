using Mango.Services.ShoppingCardAPI.Models.DTO;
using Mango.Services.ShoppingCartAPI.Models.Dto;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartAPI.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory clientFactory;
        public ProductService(IHttpClientFactory _clientFactory)
        {
            clientFactory = _clientFactory;
      
        }
        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var client = clientFactory.CreateClient("Product");
            var response = await client.GetAsync("/api/product");
            var apiContent = await response.Content.ReadAsStringAsync();    
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (resp != null && resp.IsSuccess)
            {
                var backToJason = Convert.ToString(resp.Result);
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(backToJason);
            }
            else
            {
                return new List<ProductDto>();
            }   
        }
    }
}
