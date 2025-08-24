using Mango.Services.web.Models;
using Mango.web.Models;

namespace Mango.web.Service.IService
{
    public interface IProductService
    {
        Task<ResponseDto?> GetProductAsync(string couponCode);
        Task<ResponseDto?> GetAllProductAsync();
        Task<ResponseDto?> GetByIdProductAsync(int id);
        Task<ResponseDto?> CreateProductAsync(ProductDto couponDto);
        Task<ResponseDto?> UpdateProductAsync(ProductDto couponDto);
        Task<ResponseDto?> DeleteProductAsync(int id);

    }
}
