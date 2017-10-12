using System;
using System.Threading.Tasks;

namespace Customer
{
    class CustomerProgram
    {
        static void Main(string[] args)
        {
            // I have no warehouse in Sweeden (SE), so this request will eventually,
            // after a timeout, be forwarded to all warehouses.
            Task.Factory.StartNew(() => new Customer(2, 1, "SE").Start());

            // Because I have a warehouse in Denmark (DK) with product number 1 in
            // stock, this order request will be processed immediately by the local
            // warehouse. This also means, that the customer program will receive
            // a reply for this request before it receives a reply for the request
            // from Sweeden above.
            Task.Factory.StartNew(() => new Customer(1, 1, "DK").Start());

            // A customer placing an order for a product which is not in stock.
            Task.Factory.StartNew(() => new Customer(3, 100, "DK").Start());
            Console.ReadLine();
        }

    }
}
