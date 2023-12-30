using demoAsp2.Dto;
using demoAsp2.Feature.ProductOrder.Command;
using demoAsp2.Feature.ProductOrder.Queries;
using demoAsp2.Notification;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace demoAsp2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductOrderController : ControllerBase
    {
        private IMediator _imediator;

        public ProductOrderController(IMediator mediator)
            => (_imediator) = mediator;



        [HttpGet]
        public async Task<IActionResult> GetProductOrders()
        {

            var productOrders = await _imediator.Send(new GetAllProductsOrderQuery());


            return Ok(productOrders);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductOrder(int id)
        {

            var productOrder = await _imediator.Send(new GetProductOrderByIdQueries(id));
            ///test notify
            var cancellationTokenSource = new CancellationTokenSource();
            // var cancellationToken = cancellationTokenSource.Token;
            var cancellationToken2 = HttpContext.RequestAborted;

            try
            {


                await _imediator.Publish(new SendMailNofication { Message = "Day laf test nofi" }, cancellationToken2);

            }
            catch (OperationCanceledException)
            {
                return BadRequest("nguoi dung da huy");
            }





            return Ok(productOrder);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductOrder([FromBody] ProductOrderRequest productOrderRequest)
        {


            var productRequest = new demoAsp2.Models.ProductOrder
            {
                productId = productOrderRequest.productId,
                orderId = productOrderRequest.orderId,
                quantity = productOrderRequest.quantity

            };


            var productOrderResult = await _imediator.Send(new CreateProductOrderCommand(productRequest));


            return Ok(productOrderResult);

        }

    }
}
