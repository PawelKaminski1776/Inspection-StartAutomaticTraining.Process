using InspectionStartAutomaticTraining.Channel;
using InspectionStartAutomaticTraining.Messages.Dtos;
using MongoDB.Bson.IO;
using Newtonsoft.Json;

namespace InspectionStartAutomaticTraining.Handlers
{
    public class MyHandler : IHandleMessages<AutomaticTrainingRequest>
    {
        private PythonAPI _pythonApi;
        public MyHandler(PythonAPI pythonApi)
        {
            this._pythonApi = pythonApi;
        }

        public async Task Handle(AutomaticTrainingRequest message, IMessageHandlerContext context)
        {
            try
            {

                var response = await _pythonApi.SendToImageTrainingAPI("/AutomaticTraining", message);

                var automaticTrainingResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<AutomaticTrainingResponse>(response);

                await context.Reply(automaticTrainingResponse);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while processing the message: {ex.Message}");

                throw;
            }
        }
    }
}
