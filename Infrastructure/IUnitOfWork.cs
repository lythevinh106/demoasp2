using demoAsp2.Data;
using demoAsp2.Responsitory;

namespace demoAsp2.Infrastructure
{

    public interface IUnitOfWork
    {

        ProductGenericReponsitory ProductGenericReponsitory { get; }
        OrderGenericReponsitory OrderGenericReponsitory { get; }

        ProductOrderGenericRepository ProductOrderGenericRepository { get; }

        void SaveChanges();

    }

    public class UnitOfWork : IUnitOfWork
    {



        private DBAspDemo2Context context;

        public UnitOfWork(DBAspDemo2Context context)
        {
            this.context = context;
        }

        // ap dung lazy-loading chi có cái nào đc gọi thì mới khởi tạo
        private ProductGenericReponsitory productGenericReponsitory;

        public ProductGenericReponsitory ProductGenericReponsitory

        {
            get
            {
                if (productGenericReponsitory == null)
                {
                    productGenericReponsitory = new ProductGenericReponsitory(context);
                }

                return productGenericReponsitory;
            }

        }



        private OrderGenericReponsitory orderGenericReponsitory;

        public OrderGenericReponsitory OrderGenericReponsitory
        {
            get
            {
                return orderGenericReponsitory ?? (orderGenericReponsitory = new OrderGenericReponsitory(context));
            }
        }
        private ProductOrderGenericRepository productOrderGenericRepository;

        public ProductOrderGenericRepository ProductOrderGenericRepository
        {
            get
            {
                return productOrderGenericRepository ?? (productOrderGenericRepository = new ProductOrderGenericRepository(context));
            }
        }



        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }





    public interface IUnitOfWork2
    {

        ProductGenericReponsitory ProductGenericReponsitory { get; }
        OrderGenericReponsitory OrderGenericReponsitory { get; }

        void SaveChanges();

    }

    public class UnitOfWork2 : IUnitOfWork2
    {



        private DBAspDemo2Context context;

        public ProductGenericReponsitory ProductGenericReponsitory { get; }
        public OrderGenericReponsitory OrderGenericReponsitory { get; }


        public UnitOfWork2(DBAspDemo2Context context)
        {
            this.context = context;
            ProductGenericReponsitory = new ProductGenericReponsitory(context);
            OrderGenericReponsitory = new OrderGenericReponsitory(context);


        }






        void IUnitOfWork2.SaveChanges()
        {
            context.SaveChanges();
        }
    }



}
