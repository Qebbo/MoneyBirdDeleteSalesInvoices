using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoneyBirdDeleteSalesInvoices
{
    class Program
    {

        private static readonly HttpClient client = new HttpClient();
        private static string baseurl = "https://moneybird.com/api/v2/270301429150778709";

        public class SalesInvoice
        {
            public string id { get; set; }
        }

        static async Task Main(string[] args)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "d1200bfaf8058a207fd4d9d0a3676929039d23243daf27d3959d7e6e5e25ebf4");
            for (int i = 0; i < 50; i++)
            {
                await GetSalesInvoices();
            }
        }

        private static void DeleteSalesInvoice(string id)
        {
            var response = client.DeleteAsync($"{baseurl}/sales_invoices/{id}.json").Result;
            if (response.IsSuccessStatusCode)
            {
                Console.Write("Success");
            }
            else
                Console.Write("Error");
        }

        private static async Task GetSalesInvoices()
        {

            var streamTask = client.GetStreamAsync($"{baseurl}/sales_invoices.json?filter=state:draft,contact_id:270935613549578232");
            var salesinvoices = await JsonSerializer.DeserializeAsync<List<SalesInvoice>>(await streamTask);
            foreach (var salesinvoice in salesinvoices)
            { 
                Console.WriteLine(salesinvoice.id);
                DeleteSalesInvoice(salesinvoice.id);
            }
        }
    }
    
}
