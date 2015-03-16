using System.Collections.Generic;

namespace Kitchen.Messages
{
    internal class MessageHandlerSet<TMessage> : IMessageHandlerSet where TMessage : Message
    {
        private readonly List<IMessageHandler<TMessage>> handlers = new List<IMessageHandler<TMessage>>();
        private readonly object listLock = new object();


        public void Add(IMessageHandler<TMessage> handler)
        {
            lock (listLock)
            {
                handlers.Add(handler);
            }
        }

        private void Remove(IMessageHandler<TMessage> handler)
        {
            lock (listLock)
            {
                handlers.Remove(handler);
            }
        }

        public bool RemoveHandler(MessageHandler<TMessage> handler)
        {
            lock (listLock)
            {
                for (int i = 0; i < handlers.Count; i++)
                {
                    if (handlers[i].HandlerIs(handler))
                    {
                        handlers.RemoveAt(i);
                        return true;
                    }
                }
            }
            return false;
        }

        public int RemoveAllHandlers(object subscriber)
        {
            lock (listLock)
            {
                return handlers.RemoveAll(h => h.SubscriberIs(subscriber));
            }
        }

        public void Invoke(Message m)
        {
            lock (listLock)
            {
                foreach (var handler in handlers)
                {
                    if (!handler.Invoke((TMessage)m))
                        Remove(handler);
                }
            }
        }
    }
}