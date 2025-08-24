using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Mango.Services.web.Models;

namespace Mango.web.Models
{
    public class CardDetailDto
    {
        public int CardDetailId { get; set; }
        public int CardHeaderId { get; set; }
        public CartHeaderDto? CartHeader { get; set; }
        public int ProductId { get; set; }
        public ProductDto? Product { get; set; }
        public int Count { get; set; }
    }
}
