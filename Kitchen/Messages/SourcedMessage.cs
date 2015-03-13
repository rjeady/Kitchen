namespace Kitchen.Messages
{
    public class SourcedMessage<TSource> : Message
    {
        public SourcedMessage(TSource source)
        {
            Source = source;
        }

        public TSource Source { get; private set; }
    }

    public class SourcedMessage<TSource, TData> : SourcedMessage<TSource>
    {
        public SourcedMessage(TSource source, TData data)
            : base(source)
        {
            Data = data;
        }

        public TData Data { get; private set; }
    }
}