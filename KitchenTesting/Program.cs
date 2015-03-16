using System;
using System.Diagnostics;
using Kitchen;
using Kitchen.Messages;

namespace KitchenTesting
{
    static class Program
    {
        static void Main(string[] args)
        {
            // var test2 = new Test2();

            var testClass = new TestClass(true);
            var testClass2 = new TestClass(false);
        }
    }

    class Test2
    {
        public Test2()
        {
            var broker = new MessageBroker();
            broker.Subscribe<Message<int>>(HandleIntMsg, true);
            broker.Publish(new Message<int>(5));
            Console.WriteLine(broker.Unsubscribe<Message<int>>(HandleIntMsg));
            broker.Publish(new Message<int>(25));
           
            Console.ReadLine();
        }

        private static void HandleIntMsg(Message<int> m)
        {
            // Console.WriteLine("message said {0}", m.Data);
        }
    }

    class TestClass
    {
        public TestClass(bool weak)
        {
            var broker = new MessageBroker(true);
            var sw = new Stopwatch();

            Console.WriteLine("time taken for 1 registrations: {0}ms", sw.TimeAction(() =>
            {
                broker.Subscribe<Message<char>>(HandleMessage, weak);
            }, 1, StopwatchUnit.Milliseconds));

            Console.WriteLine("time taken for 9 different registrations: {0}ms", sw.TimeAction(() =>
            {
                broker.Subscribe<Message<int>>(HandleMessage, weak);
                broker.Subscribe<Message<double>>(HandleMessage, weak);
                broker.Subscribe<Message<bool>>(HandleMessage, weak);
                broker.Subscribe<Message<float>>(HandleMessage, weak);
                broker.Subscribe<Message<long>>(HandleMessage, weak);
                broker.Subscribe<Message<short>>(HandleMessage, weak);
                broker.Subscribe<Message<uint>>(HandleMessage, weak);
                broker.Subscribe<Message<ushort>>(HandleMessage, weak);
                broker.Subscribe<Message<ulong>>(HandleMessage, weak);
            }, 1, StopwatchUnit.Milliseconds));

            Console.WriteLine("time taken for 10000 {0} registrations: {1}ms", weak ? "weak" : "strong", sw.TimeAction(() =>
            {
                broker.Subscribe<Message<string>>(HandleMessage, weak);
                // broker.Subscribe<Message<int>>(HandleMessage, true);
            }, 10000, StopwatchUnit.Milliseconds));

            // broker.Subscribe<Message>(HandleAnyMessage, weak);

            Console.WriteLine("time taken to publish and handle 1000 messages: {0}ms", sw.TimeAction(() =>
            {
                broker.Publish(new Message<string>("wee"));
                // broker.Publish(new Message<int>(2));
            }, 1000, StopwatchUnit.Milliseconds));

            bool success = broker.Unsubscribe<Message>(HandleAnyMessage);

            broker.Publish(new Message<string>("this should not appear"));


            Console.ReadLine();
        }

        private void HandleMessage(Message<char> m)
        {
            throw new NotImplementedException();
        }

        private void HandleMessage(Message<ulong> m)
        {
            throw new NotImplementedException();
        }

        private void HandleMessage(Message<ushort> m)
        {
            throw new NotImplementedException();
        }

        private void HandleMessage(Message<uint> m)
        {
            throw new NotImplementedException();
        }

        private void HandleMessage(Message<short> m)
        {
            throw new NotImplementedException();
        }

        private void HandleMessage(Message<long> m)
        {
            throw new NotImplementedException();
        }

        private void HandleMessage(Message<string> m)
        {
            string test = m.Data.Substring(1);
            // Console.WriteLine("message was " + m.Data);
        }

        private void HandleMessage(Message<bool> m)
        {
            bool test = !m.Data;
        }

        private void HandleMessage(Message<float> m)
        {
            float test = 213.2f + m.Data;
        }

        private void HandleMessage(Message<double> m)
        {
            double test = 213.2 + m.Data;
        }

        private void HandleMessage(Message<int> m)
        {
            int test = 213 + m.Data;
        }

        private void HandleAnyMessage(Message m)
        {
            Console.WriteLine("anything goes!");
        }
    }
}
