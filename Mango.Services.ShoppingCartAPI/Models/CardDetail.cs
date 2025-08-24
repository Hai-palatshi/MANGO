using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Mango.Services.ShoppingCartAPI.Models.Dto;

namespace Mango.Services.ShoppingCartAPI.Models
{
    public class CardDetail
    {
        [Key]
        public int CardDetailId { get; set; }
        public int CardHeaderId { get; set; }
        
        [ForeignKey("CardHeaderId")]
        public CartHeader? CartHeader { get; set; }
        
        public int ProductId { get; set; }
        [NotMapped]
        public ProductDto Product { get; set; }
        public int Count { get; set; }
    }
}
