
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StockService.API.Requests;

namespace StockService.API.Controllers
{
    [ApiController]
    [Route("api/v1/stock")]
    public class StockController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public StockController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/v1/stock
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var query = new GetAllStockRequest() { };
            
            var result = await _mediator.Send(query);
 
            return Ok(result);
        }
        
        // GET: api/v1/stock/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetStockRequest() { ProductId = id };
            
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        
        // POST: api/v1/stock
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateStockRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            // TODO: validation

            var result = await _mediator.Send(request);

            return Created($"api/v1/stock/{request.ProductId}", result);
        }

        // PUT: api/v1/stock
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateStockRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }
            
            // TODO: validation
 
            var result = await _mediator.Send(request);

            return Created($"api/v1/stock/{request.ProductId}", result);
        }
    }
}