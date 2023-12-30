using demoAsp2.Feature.ProductOrder.Queries;
using demoAsp2.Infrastructure;
using MediatR;

namespace demoAsp2.Feature.ProductOrder.Handler
{
    public class GetProductOrderHandlerById : IRequestHandler<GetProductOrderByIdQueries, demoAsp2.Models.ProductOrder>
    {
        private IUnitOfWork _unitOfWork;

        public GetProductOrderHandlerById(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        public Task<Models.ProductOrder> Handle(GetProductOrderByIdQueries request, CancellationToken cancellationToken)
        {
            var productOrder = _unitOfWork.ProductOrderGenericRepository.GetProductOrderByProductId(request.pruductId);

            return Task.FromResult(productOrder);
        }
    }
}
