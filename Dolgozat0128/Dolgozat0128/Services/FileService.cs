using Dolgozat0128.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Dolgozat0128.Services
{
    /*Ez alapján riportáljuk, hogy:
    1.	ki volt a legtöbbet vásárolt ügyfelünk

    2.	melyik termékből fogyott a legtöbb

    3.	összegezzük, hogy melyik fizetési móddal mennyien fizettek,
    és mekkora értékben (fizetési mód, tranzakciók száma, összes érték)

    4.	összegezzük, hogy melyik szállítási móddal mennyien rendeltek, 
    és mekkora értékben (szállítási mód, tranzakciók száma, összes érték)
    */
    public class FileService
    {
        public Data loadData()
        {
            var jsonDataRaw = File.ReadAllText("autoparts_orders.json");
            var jsonData = JsonSerializer.Deserialize<Data>(jsonDataRaw);
            return jsonData;
        }
        public void MostSoldCustomer(List<Orders> orders, List<Customers> customers)
        {
            int mostCommon = orders
                       .GroupBy(o => o.customer_id)
                       .OrderByDescending(g => g.Count())
                       .First()
                       .Key;

            var desiredCustomer = customers.Where(c => c.customer_id == mostCommon).FirstOrDefault();
            Console.WriteLine($"Legtöbbet vásárolt ügyfelünk: {desiredCustomer.name}");
            var orderSummaries = new Dictionary<int, int>();
            foreach (var items in orders)
            {
                if (orderSummaries.ContainsKey(items.customer_id))
                {
                    orderSummaries.TryGetValue(items.customer_id, out var oldTotal);
                    var newTotal = oldTotal + items.total;
                    orderSummaries[items.customer_id] = newTotal;
                }
                else
                {
                    orderSummaries.Add(items.customer_id, items.total);
                }
            }
            //foreach (var orderSummary in orderSummaries)
            //{
            //    Console.WriteLine($"Customer Id: {orderSummaries.Keys}, total: {orderSummaries.Values}");
            //}
            var biggestCustomer = orderSummaries.MaxBy(o => o.Value);
            var customer = customers.Single(c => c.customer_id == biggestCustomer.Key);
            Console.WriteLine($"Biggest customer Id: {biggestCustomer.Key}, total: {biggestCustomer.Value}");
            Console.WriteLine($"Biggest customer name: {customer.name}, total: {biggestCustomer.Value}");

        }

        public void MostSoldProduct(List<Orders> orders, List<Parts> parts)
        {

                var mostCommon = orders.SelectMany(o => o.items)
                   .GroupBy(o => o.part_id, o => o.quantity).Select(g => new { part_id = g.Key, total = g.Sum() })
                   .OrderByDescending(g => g.total)
                   .First().part_id;

                var desiredParts = parts.Where(c => c.part_id == mostCommon).FirstOrDefault();
                Console.WriteLine($"Legtöbbet vásárolt alkatrészünk: {desiredParts.name}");
            

        }
        public void AllOfPayment(List<Orders> orders, Summary summary)
        {
            //cash, credit card, bank transfer, cash on delivery
            int cashTotal = 0;
            int creditCardTotal = 0;
            int bankTransfer = 0;
            int cashOnDelivery = 0;

            foreach (var order in orders)
            {
                if (order.payment_method.ToLower() == "cash")
                {
                    cashTotal+=order.total;
                }
                else if (order.payment_method.ToLower() == "credit card")
                {
                    creditCardTotal += order.total;
                }
                else if (order.payment_method.ToLower() == "bank transfer")
                {
                    bankTransfer += order.total;
                }
                else if (order.payment_method.ToLower() == "cash on delivery")
                {
                    cashOnDelivery += order.total;
                }
            }

            Console.WriteLine($"Payments of each payment method:" +
                                $"\ncash: {cashTotal}" +
                                $"\ncredit card: {creditCardTotal}" +
                                $"\nbank transfer: {bankTransfer}" +
                                $"\ncash on delivery: {cashOnDelivery}");
        }
        public void AllOfShipping(List<Orders> orders)
        {
            int foxpost = 0;
            int glsCourier = 0;
            int mpl = 0;
            int storePickup = 0;

            foreach (var order in orders)
            {
                if (order.shipping_method.ToLower() == "foxpost locker")
                {
                    foxpost += order.total;
                }
                else if (order.shipping_method.ToLower() == "gls courier")
                {
                    glsCourier += order.total;
                }
                else if (order.shipping_method.ToLower() == "mpl")
                {
                    mpl += order.total;
                }
                else if (order.shipping_method.ToLower() == "store pickup")
                {
                    storePickup += order.total;
                }
            }

            Console.WriteLine($"Payments of each shipping method:" +
                                $"\nFoxpost locker: {foxpost}" +
                                $"\nGLS courier: {glsCourier}" +
                                $"\nMPL: {mpl}" +
                                $"\nStore pickup: {storePickup}");
        }
    }
}
