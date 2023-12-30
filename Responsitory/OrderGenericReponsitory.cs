using demoAsp2.Data;
using demoAsp2.Models;
using Serilog;

namespace demoAsp2.Responsitory
{
    public class OrderGenericReponsitory : GenetricRepository<Order>
    {
        public OrderGenericReponsitory(DBAspDemo2Context context) : base(context)
        {
            Log.Information("day laf   OrderGenericReponsitory dc khoi tao");
        }
    }
}
