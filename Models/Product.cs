using demoAsp2.Constacts;
using System.Collections;
using System.ComponentModel.DataAnnotations;
namespace demoAsp2.Models
{
    public class Product : ISeedable, ICrudTable
    {


        public int id { get; set; }
        [Required]
        [StringLength(50)]
        public string name { get; set; }

        [Required]
        [Range(0, double.MaxValue)]

        public decimal price { get; set; }


        public int categoryId { get; set; }

        public virtual Category.Module.Category category { get; set; }

        public virtual List<ProductOrder> productOrders { get; set; }

        public IList Seed()
        {
            var products = new List<Product>(
               Enumerable.Range(1, 5).Select(idx => new Product
               {
                   id = idx,
                   name = $"ProductName {idx}",
                   price = 2536 + idx,
                   categoryId = idx,

               })

               );
            return products;
        }

        public void showInfoProduct()
        {
            Console.WriteLine($" {name} -- {price}--categoryId{category.id} ");
        }
    }
}
