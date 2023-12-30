using MediatR;

namespace demoAsp2.Feature.ProductOrder.Queries
{
    public class GetProductOrderByIdQueries : IRequest<demoAsp2.Models.ProductOrder>
    {

        public int pruductId { get; }

        public GetProductOrderByIdQueries(int productId)
        {

            pruductId = productId;

        }
    }
}
