using demoAsp2.Models.Category.Module;

namespace demoAsp2.Interfaces
{
    public interface ICategoryRepository
    {
        List<Category> GetCategories();

        bool CreateCategory(Category category);
        Category GetCategory(int id);

        bool GetCategoryExists(int categoryId);

        bool UpdateCategory(int categoryId, Category categoryRequest);

        bool DeleteCategory(int categoryId);
        bool Save();



    }
}
