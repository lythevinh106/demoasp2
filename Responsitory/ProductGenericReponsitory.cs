

using demoAsp2.Data;
using demoAsp2.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace demoAsp2.Responsitory
{
    public class ProductGenericReponsitory : GenetricRepository<Product>
    {
        public ProductGenericReponsitory(DBAspDemo2Context context) : base(context)
        {
            Log.Information("day laf  ProductGenericReponsitory   dc khoi tao");
        }


        //co the ghi de lop abtract đe custom lai. vd như nếu có nhiều bảng chẳng hạn nhu bảng order
        public override IEnumerable<Product> All()
        {
            return _context.products.Include(po => po.productOrders);
        }
    }

}
