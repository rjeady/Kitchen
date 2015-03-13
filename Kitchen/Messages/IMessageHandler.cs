using System;

namespace Kitchen.Messages
{
    interface IMessageHandler<TMessage> where TMessage : Message
    {
        bool Invoke(TMessage m);
        bool HandlerIs(MessageHandler<TMessage> handler);
        bool SubscriberIs(object subscriber);
    }
}