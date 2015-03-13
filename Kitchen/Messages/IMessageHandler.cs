namespace Kitchen.Messages
{
    public delegate void MessageHandler<in TMessage>(TMessage m) where TMessage : Message;

    internal interface IMessageHandler<TMessage> where TMessage : Message
    {
        bool Invoke(TMessage m);
        bool HandlerIs(MessageHandler<TMessage> handler);
        bool SubscriberIs(object subscriber);
    }
}