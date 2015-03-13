using System;

namespace Kitchen.Messages
{
    internal class WeakMessageHandler<TSubscriber, TMessage> : IMessageHandler<TMessage> where TMessage : Message
    {
        private readonly WeakReference targetRef;
        private readonly Action<TSubscriber, TMessage> openHandler;

        private static readonly Type HandlerType = typeof(Action<TSubscriber, TMessage>);

        public WeakMessageHandler(MessageHandler<TMessage> handler)
        {
            targetRef = new WeakReference(handler.Target);
            openHandler = (Action<TSubscriber, TMessage>)Delegate.CreateDelegate(HandlerType, handler.Method);
        }

        /// <summary>
        /// Determines whether this WeakMessageHandler wraps the given message handler delegate,
        /// i.e. if it was constructed from this handler delegate.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public bool HandlerIs(MessageHandler<TMessage> handler)
        {
            return openHandler.Method == handler.Method && targetRef.Target == handler.Target;
        }
        public bool SubscriberIs(object subscriber)
        {
            return ReferenceEquals(targetRef.Target, subscriber);
        }

        public bool Invoke(TMessage m)
        {
            var target = (TSubscriber)targetRef.Target;

            if (target == null)
                return false;

            openHandler(target, m);
            return true;
        }
    }
}