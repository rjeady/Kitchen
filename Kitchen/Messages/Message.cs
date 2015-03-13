namespace Kitchen.Messages
{
    public abstract class Message { }

    public class Message<TData> : Message
    {
        public Message(TData data)
        {
            Data = data;
        }

        public TData Data { get; private set; }
    }
}
