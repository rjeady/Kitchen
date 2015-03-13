using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitchen.Messages
{
    public abstract class Message { }

    public class Message<TData> : Message
    {
        public Message(TData data)
        {
            Data = data;
        }

        public TData Data { get; private set; }
    }
}
