using System.ComponentModel.DataAnnotations;

namespace demoAsp2.Models.CategoryDto.Category.Request
{
    public class CategoryRequestDto
    {


        [Required]

        public int id { get; set; }

        [Required]
        [StringLength(500)]

        public string name { get; set; }
    }
}
