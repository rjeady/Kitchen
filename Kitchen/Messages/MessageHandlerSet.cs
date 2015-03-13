using System.Collections.Generic;

namespace Kitchen.Messages
{
    internal interface IMessageHandlerSet
    {
        void Invoke(Message m);
        int RemoveAllHandlers(object subscriber);
    }

    class MessageHandlerSet<TMessage> : IMessageHandlerSet where TMessage : Message
    {
        private readonly List<IMessageHandler<TMessage>> handlers = new List<IMessageHandler<TMessage>>();

        public void Add(IMessageHandler<TMessage> handler)
        {
            handlers.Add(handler);
        }

        private void Remove(IMessageHandler<TMessage> handler)
        {
            handlers.Remove(handler);
        }

        public bool RemoveHandler(MessageHandler<TMessage> handler)
        {
            for (int i = 0; i < handlers.Count; i++)
            {
                if (handlers[i].HandlerIs(handler))
                {
                    handlers.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public int RemoveAllHandlers(object subscriber)
        {
            return handlers.RemoveAll(h => h.SubscriberIs(subscriber));
        }

        public void Invoke(Message m)
        {
            foreach (var handler in handlers)
            {
                if (!handler.Invoke((TMessage)m))
                    Remove(handler);
            }
        }
    }
}