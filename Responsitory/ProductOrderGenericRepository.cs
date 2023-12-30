using demoAsp2.Data;
using demoAsp2.Models;

namespace demoAsp2.Responsitory
{
    public class ProductOrderGenericRepository : GenetricRepository<ProductOrder>
    {


        public ProductOrderGenericRepository(DBAspDemo2Context context) : base(context)
        {

        }


        public ProductOrder GetProductOrderByProductId(int id)
        {
            var productOrder = _context.productOrders.Where(po => po.productId == id).FirstOrDefault();


            return productOrder;

        }
    }
}
