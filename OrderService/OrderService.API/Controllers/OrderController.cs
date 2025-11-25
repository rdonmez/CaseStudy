using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.API.Requests;

namespace OrderService.API.Controllers
{
    
    [ApiController]
    [Route("api/v1/order")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        // GET: api/v1/order
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var query = new GetAllOrderRequest();
            var result = await _mediator.Send(query);
            
            if (result == null)
            {
                return NotFound();
            }
            
            return Ok(result);
        }

        // GET: api/v1/order/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetOrderRequest { Id = id };
            var result = await _mediator.Send(query);
            
            if (result == null)
            {
                return NotFound();
            }
            
            return Ok(result);
        }
         
        // POST: api/v1/order
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateOrderRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }
  
            var result = await _mediator.Send(request);
           
            return Ok(result);
        }
    
        // PUT: api/v1/order/approve
        [HttpPut("approve")]
        public async Task<IActionResult> Put([FromBody] ApproveOrderRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }
  
            var result = await _mediator.Send(request);
            
            return Ok(result);
        }
        
        // PUT: api/v1/order/cancel
        [HttpPut("cancel")]
        public async Task<IActionResult> Put([FromBody] CancelOrderRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }
  
            var result = await _mediator.Send(request);
            
            return Ok(result);
        }
    }
}