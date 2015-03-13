namespace Kitchen.Messages
{
    internal interface IMessageHandlerSet
    {
        void Invoke(Message m);
        int RemoveAllHandlers(object subscriber);
    }
}