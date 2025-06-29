using Domain.Constants;

namespace ApplicationLayer.Utilities
{
    public class Message
    {
        public MessageCode code { get; }
        public object data { get; set; }
        public Message() { }

        public Message(MessageCode code, object data)
        {
            this.code = code;
            this.data = data;
        }
    }
}
