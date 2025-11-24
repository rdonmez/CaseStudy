using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NotificationService.API.Requests;

namespace NotificationService.API.Controllers
{
    [ApiController]
    [Route("api/v1/notification")]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }
 
        // POST: api/v1/notification
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SendNotificationRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }
  
            var result = await _mediator.Send(request);
            
            return Ok(result);
        }
    
        // GET: api/v1/notification/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetNotificationRequest() { Id = id };
            var result = await _mediator.Send(query);
            
            if (result == null)
            {
                return NotFound();
            }
            
            return Ok(result);
        }
        
        // GET: api/v1/notification
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var query = new GetAllNotificationRequest() { };
            
            var result = await _mediator.Send(query);
             
            return Ok(result);
        }
    }
}