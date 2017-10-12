using System;
using Messages;
using MessagingGateway;
using System.Threading;
using EasyNetQ;
using System.Text;

namespace Customer
{
    public class Customer
    {
        private int customerID;
        private int productID;
        private string country;
        private MessagingGateway.MessagingGateway gateway;

        public Customer(int customerID, int productID, string country)
        {
            this.customerID = customerID;
            this.productID = productID;
            this.country = country;
            gateway = new MessagingGateway.MessagingGateway(this.customerID);
        }

        public void Start()
        {
            SynchronizedWriteLine("Customer running. Waiting for a reply.\n");

            OrderReplyMessage message = this.gateway.SendRequest(this.gateway.CreateRequestMessage(this.customerID, this.productID, this.country));

            StringBuilder reply = new StringBuilder();
            reply.Append("Order reply received by customer:" + customerID + "\n");
            reply.Append("Warehouse Id: " + message.WarehouseId + "\n");
            reply.Append("Order Id: " + message.OrderId + "\n");
            reply.Append("Items in stock: " + message.ItemsInStock + "\n");
            reply.Append("Shipping charge: " + message.ShippingCharge + "\n");
            reply.Append("Days for delivery: " + message.DaysForDelivery + "\n");

            SynchronizedWriteLine(reply.ToString());
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
