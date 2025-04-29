using Microsoft.AspNetCore.Mvc;
using InspectionStartAutomaticTraining.Messages.Dtos;
using InspectionStartAutomaticTraining.Controllers.DtoFactory;

namespace InspectionStartAutomaticTraining.Controllers
{
    [ApiController]
    [Route("Api/Controllers")]
    public class MyController : BaseController
    {
        public MyController(IMessageSession messageSession, IDtoFactory dtoFactory)
            : base(messageSession, dtoFactory) { }

        [HttpPost("Message")]
        public async Task<IActionResult> AddAccount([FromBody] MessageRequest dto)
        {
            var loginDto = (MessageRequest)_dtoFactory.UseDto("messagedto", dto);

            try
            {
                var response = await _messageSession.Request<MessageResponse>(loginDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while processing the request: {ex.Message}");
            }
        }
    }

}
