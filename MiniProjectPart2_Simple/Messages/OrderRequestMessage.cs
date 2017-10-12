namespace Messages
{
    public class EnvelopeRequestMessage
    {
        public string Country { get; set; }
        public CustomerOrderRequestMessage Message { get; set; }
    }

    public class CustomerOrderRequestMessage
    {
        public int ProductId { get; set; }
        public string Country { get; set; }
        public int CustomerId;
    }

    public class RetailerOrderRequestMessage : CustomerOrderRequestMessage
    {
        public int OrderId { get; set; }
        public string ReplyTo { get; set; }
    }

    public class OrderRequestMessageToLocalWarehouse : RetailerOrderRequestMessage
    {
    }

    public class OrderBroadcastRequestMessage : RetailerOrderRequestMessage
    {
    }
}
