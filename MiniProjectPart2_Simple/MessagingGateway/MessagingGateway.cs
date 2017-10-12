using System;
using EasyNetQ;
using Messages;
using System.Threading;
using System.Text;

namespace MessagingGateway
{
    public class MessagingGateway
    {
        IBus bus = RabbitHutch.CreateBus("host=localhost");
        OrderReplyMessage replyMessage = null;
        int timeout;
        private int customerID;

        public MessagingGateway(int customerID, int timeout)
        {
            this.customerID = customerID;
            // Listen to reply messages from the Retailer
            bus.Subscribe<OrderReplyMessage>("customer" + customerID,
            HandleOrderEvent, x => x.WithTopic(customerID.ToString()));
            this.timeout = timeout;
        }

        public MessagingGateway(int customerID) : this(customerID, 10000)
        {

        }

        public OrderReplyMessage SendRequest(EnvelopeRequestMessage request)
        {
            // Send an order request message to the Retailer
            bus.Send<EnvelopeRequestMessage>("retailerQueue", request);

            lock (this)
            {
                // Block this thread so that the Customer program will not exit.
                bool gotReply = Monitor.Wait(this, timeout);
                if (!gotReply)
                {
                    SynchronizedWriteLine("Timeout. The requested product is out of stock!");
                    return null;
                }
                else
                {
                    return replyMessage;
                }
            }
        }

        public EnvelopeRequestMessage CreateRequestMessage(int customerID, int productID, string country)
        {
            return new EnvelopeRequestMessage
            {
                Message = new CustomerOrderRequestMessage{
                    CustomerId = customerID,
                    ProductId = productID,
                    Country = country
                },
                Country = country
            };
        }

        private void HandleOrderEvent(OrderReplyMessage message)
        {
            replyMessage = message;

            lock (this)
            {
                // Wake up the blocked Customer thread
                Monitor.Pulse(this);
            }
        }

        private void SynchronizedWriteLine(string s)
        {
            lock (this)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(s);
                Console.ResetColor();
            }
        }
    }
}
