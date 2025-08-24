namespace Mango.web.Models
{
    public class CartDto
    {
        public CartHeaderDto? CartHeader { get; set; }
        public IEnumerable<CardDetailDto>? CardDetail{ get; set; }
    }
}
