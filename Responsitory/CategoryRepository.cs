
using demoAsp2.Data;
using demoAsp2.Interfaces;
using demoAsp2.Models.Category.Module;
using Microsoft.EntityFrameworkCore;

namespace demoAsp2.Responsitory
{
    public class CategoryRepository : ICategoryRepository
    {
        private DBAspDemo2Context _context;

        public CategoryRepository(DBAspDemo2Context context)
        {
            _context = context;
        }

        public bool CreateCategory(Category category)
        {
            _context.Add(category);
            return Save();
        }

        public bool DeleteCategory(int categoryId)
        {
            //_context.Remove(category);
            //return Save();

            var removeCategory = _context.categories.Where(c => c.id == categoryId).FirstOrDefault();

            if (removeCategory != null)
            {
                _context.Remove(removeCategory);
                return Save();
            }

            return false;



        }

        public List<Category> GetCategories()
        {
            var categorires = _context.categories.ToList();
            return categorires;
        }

        public Category GetCategory(int id)
        {


            var category = _context.categories.FirstOrDefault((c) => c.id == id);
            return category;
        }

        public bool GetCategoryExists(int categoryId)
        {
            bool isExist = _context.categories.FirstOrDefault(c => c.id == categoryId) != null ? true : false;
            return isExist;

        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateCategory(int categoryId, Category categoryUpdate)
        {
            //var entity = _context.categories.FirstOrDefault(c => c.id == categoryId);

            _context.Update(categoryUpdate);
            return Save();


            //if (entity != null)
            //{
            //    entity.name = categoryRequest.name;
            //    return Save();

            //}

            //return false;






        }

        public void getProductOfCategory(int idCategory)
        {


            var eangerLoading = _context.categories.Include(c => c.products).
               ThenInclude(p => p.productOrders);

            ;
            ///in ra chi

            Console.WriteLine("da thay");
            foreach (var item in eangerLoading)
            {
                Console.WriteLine(item.name);

                foreach (var product in item.products)
                {

                    product.showInfoProduct();

                    foreach (var productOrder in product.productOrders)
                    {

                        Console.Write("---product-Orderid" + productOrder.orderId);

                    }
                }


            }

        }




    }
}
