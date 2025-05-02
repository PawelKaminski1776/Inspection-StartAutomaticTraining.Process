using InspectionStartAutomaticTraining.Messages;
using InspectionStartAutomaticTraining.Messages.Dtos;
using MongoDB.Bson;
using MongoDB.Driver;

namespace InspectionStartAutomaticTraining.Channel.Services
{
    public class InspectionService : MongoConnect
    {

        public InspectionService(string ConnectionString) : base(ConnectionString)
        {
        }

        public async Task<string> InsertOnFinishedImageScanning(AutomaticTrainingResponse request)
        {
            var database = dbClient.GetDatabase("InspectionAppDatabase");
            var collection = database.GetCollection<BsonDocument>("InspectionDetails");

            Console.WriteLine(request.id + "Num Of Images" + request.NumOfImages + request.county + request.Inspectionname + request.overalllossrate);

            var document = new BsonDocument {
                {"id",  request.id.ToString() },
                { "Inspectionname", request.Inspectionname },
                { "NumberOfImages", request.NumOfImages },
                { "County", request.county },
                { "Overall Training Loss Rate", request.overalllossrate },
                { "Status", "Success" }
            };

            try
            {
                await collection.InsertOneAsync(document);
                return "Request Successful";
            }
            catch (Exception e)
            {
                return "Failed" + e.Message;
            }
        }


    }
}