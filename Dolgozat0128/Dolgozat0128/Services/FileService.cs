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
            
        }
        public void AllOfShipping(List<Orders> orders)
        {

        }
    }
}
