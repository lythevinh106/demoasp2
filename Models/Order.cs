using demoAsp2.Constacts;
using demoAsp2.Enum;
using System.Collections;

namespace demoAsp2.Models
{
    public class Order : ISeedable, ICrudTable
    {
        public int id { get; set; }



        public StatusOrder Status { get; set; } = StatusOrder.pending;



        public virtual List<ProductOrder> productOrders { get; set; }

        public IList Seed()
        {
            var orders = new List<Order>(
              Enumerable.Range(1, 5).Select(idx => new Order
              {
                  id = idx,
                  Status = Enum.StatusOrder.pending,





              })

              );
            return orders;
        }
    }
}
