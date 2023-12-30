using System.ComponentModel.DataAnnotations;

namespace demoAsp2.Dto
{
    public class ProductOrderRequest
    {
        public int productId { get; set; }
        public int orderId { get; set; }

        [Required]


        public int quantity { get; set; }
    }
}
