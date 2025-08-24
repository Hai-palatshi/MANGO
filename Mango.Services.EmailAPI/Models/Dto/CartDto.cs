namespace Mango.Services.EmailAPI.Models
{
    public class CartDto
    {
        public CartHeaderDto? CartHeader { get; set; }
        public IEnumerable<CardDetailDto>? CardDetail{ get; set; }
    }
}
