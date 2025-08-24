using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Mango.Services.ShoppingCartAPI.Models.Dto;

namespace Mango.Services.ShoppingCartAPI.Models.Dto
{
    public class CardDetailDto
    {
        public int CardDetailId { get; set; }
        public int CardHeaderId { get; set; }
        public CardHeaderDto? CartHeader { get; set; }
        public int ProductId { get; set; }
        public ProductDto? Product { get; set; }
        public int Count { get; set; }
    }
}
