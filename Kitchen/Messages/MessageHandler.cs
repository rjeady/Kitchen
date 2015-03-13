using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kitchen.Messages
{
    public delegate void MessageHandler<in TMessage>(TMessage m) where TMessage : Message;
}
