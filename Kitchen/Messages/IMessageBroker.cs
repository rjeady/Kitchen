namespace Kitchen.Messages
{
    public interface IMessageBroker {

        /// <summary>Publishes the specified message.</summary>
        /// <typeparam name="TMessage">The message type.</typeparam>
        /// <param name="message">The message.</param>
        void Publish<TMessage>(TMessage message) where TMessage : Message;

        /// <summary>
        /// Subscribes to the specified message type, in either a strong or weak fashion. The given handler delegate
        /// will be called whenever a message of this type, or a derrived type (if raising parent message types was
        /// set to true in the constructor), is published.
        /// </summary>
        /// <typeparam name="TMessage">The message type.</typeparam>
        /// <param name="handler">
        /// The message handler. This should be a static or instance method, an anonymous method, or a lambda that is
        /// not a closure.
        /// </param>
        /// <param name="subcribeWeakly">
        /// If set to <c>true</c> a weak subscription will be created. This means that the message broker will only
        /// hold a weak reference to the subscriber, allowing it to be garbage collected. If set to <c>false</c>, the
        /// message broker will hold a strong reference will be held to the subcriber, so it must later call
        /// Unsubscribe in order to be garbage collected.
        /// </param>
        void Subscribe<TMessage>(MessageHandler<TMessage> handler, bool subcribeWeakly) where TMessage : Message;

        /// <summary>
        /// Unsubscribes the specified handler from this type of message. Returns a boolean indicating whether the
        /// specified handler was found and successfully unsubscribed. A return value of <c>false</c> would indicate
        /// that the specified handler was not already subscribed to this type of message.
        /// </summary>
        /// <typeparam name="TMessage">The message type.</typeparam>
        /// <param name="handler">The message handler.</param>
        bool Unsubscribe<TMessage>(MessageHandler<TMessage> handler) where TMessage : Message;

        /// <summary>
        /// Unsubscribes all handlers belonging to the given subscriber object from this type of message.
        /// Returns the number of handlers that were unsubscribed.
        /// </summary>
        /// <typeparam name="TMessage">The message type.</typeparam>
        /// <param name="subscriber">The subscriber object, typically 'this' (if called from the subscriber).</param>
        /// <returns></returns>
        int UnsubscribeAll<TMessage>(object subscriber) where TMessage : Message;

        /// <summary>
        /// Unsubscribes all handlers belonging to the given subscriber object from any messages.
        /// Returns the number of handlers that were unsubscribed.
        /// </summary>
        /// <param name="subscriber">The subscriber object, typically 'this' (if called from the subscriber).</param>
        int UnsubscribeAll(object subscriber);
    }
}