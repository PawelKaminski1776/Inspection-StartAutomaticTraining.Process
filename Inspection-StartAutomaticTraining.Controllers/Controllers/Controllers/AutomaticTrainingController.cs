using Microsoft.AspNetCore.Mvc;
using InspectionStartAutomaticTraining.Messages.Dtos;
using InspectionStartAutomaticTraining.Controllers.DtoFactory;

namespace InspectionStartAutomaticTraining.Controllers
{
    [ApiController]
    [Route("Api/AutomaticTraining")]
    public class AutomaticTrainingController : BaseController
    {
        public AutomaticTrainingController(IMessageSession messageSession, IDtoFactory dtoFactory)
            : base(messageSession, dtoFactory) { }

        [HttpGet("StartAutomaticTraining")]
        public async Task<IActionResult> InitalizeAutomaticTraining(string ModelUrl, int NumberOfImgs, string county, string inspection)
        {
            try
            {
                AutomaticTrainingRequest request = new AutomaticTrainingRequest
                {
                    ModelUrl = ModelUrl,
                    NumberofImgs = NumberOfImgs,
                    county = county,
                    inspection = inspection
                };
                Console.WriteLine(NumberOfImgs + ModelUrl + county + inspection);
                var response = await _messageSession.Request<AutomaticTrainingResponse>(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error while processing the request: {ex.Message}");
            }
        }
    }

}
