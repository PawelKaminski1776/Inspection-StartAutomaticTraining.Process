using InspectionStartAutomaticTraining.Channel;
using InspectionStartAutomaticTraining.Channel.Services;
using InspectionStartAutomaticTraining.Messages.Dtos;
using MongoDB.Bson.IO;
using Newtonsoft.Json;

namespace InspectionStartAutomaticTraining.Handlers
{
    public class MyHandler : IHandleMessages<AutomaticTrainingRequest>
    {
        private PythonAPI _pythonApi;
        private readonly InspectionService inspectionService;
        private string Inspection { get; set; }
        public MyHandler(PythonAPI pythonApi, InspectionService _inspectionService)
        {
            this._pythonApi = pythonApi;
            this.inspectionService = _inspectionService;
        }

        public async Task Handle(AutomaticTrainingRequest message, IMessageHandlerContext context)
        {
            try
            {
                this.Inspection = message.inspection;

                var response = await _pythonApi.SendToImageTrainingAPI("/AutomaticTraining", message);

                var automaticTrainingResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<AutomaticTrainingResponse>(response);

                automaticTrainingResponse.Inspectionname = this.Inspection;

                await inspectionService.InsertOnFinishedImageScanning(automaticTrainingResponse);

                await context.Reply(new MessageResponse { Message = "Successful Upload" });

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while processing the message: {ex.Message}");

                throw;
            }
        }
    }
}
