using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace Kitchen.Messages
{
    public class MessageBroker : IMessageBroker
    {
        private readonly Dictionary<Type, IMessageHandlerSet> handlers = new Dictionary<Type, IMessageHandlerSet>();
        private static readonly Type MessageBaseType = typeof(object);

        private readonly bool publishParentMessages;

        /// <summary>Initializes a new instance of the <see cref="MessageBroker"/> class.</summary>
        /// <param name="publishParentMessages">
        /// If set to <c>true</c>, when a message of a certain type is published, messages of all parent types (up to
        /// Message) will also be published. This allows you to implement handlers of inherited message types, which
        /// are also invoked when a message of any derrived type is published. If set to <c>false</c>, only handlers
        /// for the exact message type will be invoked. This is <c>true</c> by default.
        /// </param>
        public MessageBroker(bool publishParentMessages = true)
        {
            this.publishParentMessages = publishParentMessages;
        }

        public void Publish<TMessage>(TMessage message) where TMessage : Message
        {
            Type messageType = MessageBrokerHelper<TMessage>.MessageType;

            if (publishParentMessages)
            {
                do
                {
                    var handlerSet = GetHandlerSet(messageType);
                    if (handlerSet != null)
                        handlerSet.Invoke(message);

                    messageType = messageType.BaseType;
                } while (messageType != MessageBaseType);
            }
            else
            {
                var handlerSet = GetHandlerSet<TMessage>();
                if (handlerSet != null)
                    handlerSet.Invoke(message);
            }
        }

        public void Subscribe<TMessage>(MessageHandler<TMessage> handler, bool subcribeWeakly) where TMessage : Message
        {
            Contract.Requires(handler != null);

            // can create a strong handler for static methods (where the handler target is null).
            if (subcribeWeakly && handler.Target != null)
            {
                Type subscriberType = handler.Target.GetType();
                var weakHandler = MessageBrokerHelper<TMessage>.GetWeakHandler(subscriberType, handler);
                AddHandler(weakHandler);
            }
            else
            {
                AddHandler(new StrongMessageHandler<TMessage>(handler));
            }
        }

        public bool Unsubscribe<TMessage>(MessageHandler<TMessage> handler) where TMessage : Message
        {
            var handlerSet = GetHandlerSet<TMessage>();
            if (handlerSet == null)
                return false;

            return handlerSet.RemoveHandler(handler);
        }

        public int UnsubscribeAll<TMessage>(object subscriber) where TMessage : Message
        {
            var handlerSet = GetHandlerSet<TMessage>();
            if (handlerSet == null)
                return 0;

            return handlerSet.RemoveAllHandlers(subscriber);
        }

        public int UnsubscribeAll(object subscriber)
        {
            // For each handler set (1 for each message type), remove all handlers that pointed to the given subscriber.
            // The result of each removal is the number of handlers removed. Return the sum of all these removal counts.
            return handlers.Values.Sum(hs => hs.RemoveAllHandlers(subscriber));
        }

        private void AddHandler<TMessage>(IMessageHandler<TMessage> handler) where TMessage : Message
        {
            var handlerSet = GetHandlerSet<TMessage>();
            if (handlerSet == null)
            {
                handlerSet = new MessageHandlerSet<TMessage>();
                handlers[MessageBrokerHelper<TMessage>.MessageType] = handlerSet;
            }
            handlerSet.Add(handler);
        }

        private MessageHandlerSet<TMessage> GetHandlerSet<TMessage>() where TMessage : Message
        {
            IMessageHandlerSet handlerSet;
            return handlers.TryGetValue(MessageBrokerHelper<TMessage>.MessageType, out handlerSet)
                ? (MessageHandlerSet<TMessage>)handlerSet
                : null;
        }

        private IMessageHandlerSet GetHandlerSet(Type messageType)
        {
            IMessageHandlerSet handlerSet;
            return handlers.TryGetValue(messageType, out handlerSet)
                ? handlerSet
                : null;
        }

        private static class MessageBrokerHelper<TMessage> where TMessage : Message
        {
            public static readonly Type MessageType = typeof(TMessage);

            private static readonly MethodInfo weakHandlerGetter = typeof(MessageBrokerHelper<TMessage>)
                .GetMethod("GetWeakHandlerInternal", BindingFlags.NonPublic | BindingFlags.Static);

            // cache, mapping subscriber type to the generic method for getting a weak handler.
            private static readonly Dictionary<Type, MethodInfo> weakHandlerGetters = new Dictionary<Type, MethodInfo>();

            public static IMessageHandler<TMessage> GetWeakHandler(Type subscriberType, MessageHandler<TMessage> handler)
            {
                MethodInfo genericMethod;
                if (!weakHandlerGetters.TryGetValue(subscriberType, out genericMethod))
                {
                    genericMethod = weakHandlerGetter.MakeGenericMethod(subscriberType);
                    weakHandlerGetters[subscriberType] = genericMethod;
                }
                
                return (IMessageHandler<TMessage>)genericMethod.Invoke(null, new object[] { handler });
            }

            private static IMessageHandler<TMessage> GetWeakHandlerInternal<TSubscriber>(MessageHandler<TMessage> handler)
                where TSubscriber : class
            {
                return new WeakMessageHandler<TSubscriber, TMessage>(handler);
            }
        }
    }
}
