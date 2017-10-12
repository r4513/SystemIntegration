using Messages;
using System;

namespace EnvelopeWrapper
{
    public class EnvelopeWrapper
    {
        public EnvelopeRequestMessage WrapMessage(CustomerOrderRequestMessage message)
        {
            return new EnvelopeRequestMessage { Country = message.Country, Message = message };
        }
    }
}
