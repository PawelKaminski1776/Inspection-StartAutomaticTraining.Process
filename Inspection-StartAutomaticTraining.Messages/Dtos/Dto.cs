using NServiceBus;

namespace InspectionStartAutomaticTraining.Messages.Dtos
{
    public class MessageRequest : IMessage
    {
        public string Message { get; set; }
    }

    public class MessageResponse : IMessage
    {
        public string Message { get; set; }
    }

    public class AutomaticTrainingRequest : IMessage
    {
        public string ModelUrl { get; set; }
        public int NumberofImgs { get; set; }
        public string county { get; set; }
        public string inspection { get; set; }

    }
    public class AutomaticTrainingResponse : IMessage
    {
        public string id { get; set; }
        public string Inspectionname { get; set; }

        public string county { get; set; }
        public string NumOfImages { get; set; }

        public string overalllossrate { get; set; }
    }

}
