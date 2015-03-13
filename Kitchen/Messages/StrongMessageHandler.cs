namespace Kitchen.Messages
{
    internal class StrongMessageHandler<TMessage> : IMessageHandler<TMessage> where TMessage : Message
    {
        private readonly MessageHandler<TMessage> handler;

        public StrongMessageHandler(MessageHandler<TMessage> handler)
        {
            this.handler = handler;
        }

        public bool HandlerIs(MessageHandler<TMessage> handler)
        {
            return this.handler == handler;
        }

        public bool SubscriberIs(object subscriber)
        {
            return ReferenceEquals(handler.Target, subscriber);
        }

        public bool Invoke(TMessage m)
        {
            handler(m);
            return true;
        }
    }
}