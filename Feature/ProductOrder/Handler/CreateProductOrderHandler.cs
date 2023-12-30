using demoAsp2.Feature.ProductOrder.Command;
using demoAsp2.Infrastructure;
using MediatR;

namespace demoAsp2.Feature.ProductOrder.Handler
{
    public class CreateProductOrderHandler : IRequestHandler<CreateProductOrderCommand, demoAsp2.Models.ProductOrder>
    {

        private IUnitOfWork _unitOfWork;

        public CreateProductOrderHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public Task<Models.ProductOrder> Handle(CreateProductOrderCommand request, CancellationToken cancellationToken)
        {





            var productOrder = _unitOfWork.ProductOrderGenericRepository.Add(request._productOrderRequest);

            _unitOfWork.SaveChanges();
            return Task.FromResult(productOrder);
        }
    }
}
