using demoAsp2.Constacts;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace demoAsp2.Models
{
    public class ProductOrder : ISeedable, ICrudTable
    {


        public int productId { get; set; }
        public int orderId { get; set; }

        [Required]
        [MinLength(1)]

        public int quantity { get; set; }




        public virtual Product product { get; set; }

        public virtual Order order { get; set; }

        public IList Seed()
        {
            var pOrders = new List<ProductOrder>(
              Enumerable.Range(1, 5).Select(idx => new ProductOrder
              {
                  productId = idx,
                  orderId = idx,
                  quantity = new Random().Next(1, 50),


              })

              );
            return pOrders;
        }
    }
}
