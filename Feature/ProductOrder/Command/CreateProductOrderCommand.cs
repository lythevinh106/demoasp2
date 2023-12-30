using MediatR;

namespace demoAsp2.Feature.ProductOrder.Command
{
    public class CreateProductOrderCommand : IRequest<demoAsp2.Models.ProductOrder>
    {

        public demoAsp2.Models.ProductOrder _productOrderRequest;

        public CreateProductOrderCommand(demoAsp2.Models.ProductOrder productOrderRequest)
        {

            _productOrderRequest = productOrderRequest;

        }


    }
}
