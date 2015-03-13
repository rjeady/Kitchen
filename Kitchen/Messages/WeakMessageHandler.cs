using System;

namespace Kitchen.Messages
{
    internal class WeakMessageHandler<TSubscriber, TMessage> : IMessageHandler<TMessage>
        where TMessage : Message
        where TSubscriber : class
    {
        private readonly WeakReference<TSubscriber> weakRef;
        private readonly Action<TSubscriber, TMessage> openHandler;

        private static readonly Type HandlerType = typeof(Action<TSubscriber, TMessage>);

        public WeakMessageHandler(MessageHandler<TMessage> handler)
        {
            weakRef = new WeakReference<TSubscriber>((TSubscriber)handler.Target);
            openHandler = (Action<TSubscriber, TMessage>)Delegate.CreateDelegate(HandlerType, handler.Method);
        }

        private TSubscriber Target
        {
            get
            {
                TSubscriber target;
                weakRef.TryGetTarget(out target);
                return target;
            }
        }

        public bool HandlerIs(MessageHandler<TMessage> handler)
        {
            return openHandler.Method == handler.Method && Target == handler.Target;
        }

        public bool SubscriberIs(object subscriber)
        {
            return ReferenceEquals(Target, subscriber);
        }

        public bool Invoke(TMessage m)
        {
            TSubscriber target;
            if (weakRef.TryGetTarget(out target))
            {
                openHandler(target, m);
                return true;
            }
            return false;
        }
    }
}