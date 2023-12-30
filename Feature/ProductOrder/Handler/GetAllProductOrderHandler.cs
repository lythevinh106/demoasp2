using demoAsp2.Feature.ProductOrder.Queries;
using demoAsp2.Infrastructure;
using MediatR;

namespace demoAsp2.Feature.ProductOrder.Handler
{
    public class GetAllProductOrderHandler : IRequestHandler<GetAllProductsOrderQuery, IEnumerable<demoAsp2.Models.ProductOrder>>

    {
        private IUnitOfWork _unitOfWork;

        public GetAllProductOrderHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }


        public Task<IEnumerable<demoAsp2.Models.ProductOrder>> Handle(GetAllProductsOrderQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<demoAsp2.Models.ProductOrder> productOrders = _unitOfWork.ProductOrderGenericRepository.All();

            return Task.FromResult(productOrders);
        }
    }
}
