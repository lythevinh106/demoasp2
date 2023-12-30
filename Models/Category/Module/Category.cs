using demoAsp2.Constacts;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace demoAsp2.Models.Category.Module
{
    public class Category : ISeedable, ICrudTable
    {
        [Required]

        public int id { get; set; }

        [Required]
        [StringLength(500)]

        public string name { get; set; }
        public virtual List<Product> products { get; set; }

        public IList Seed()
        {

            var categories = new List<Category>(

               Enumerable.Range(1, 5).Select(idx => new Category
               {
                   id = idx,
                   name = $"category name {idx}",


               })

               );


            return categories;
        }


    }
}
