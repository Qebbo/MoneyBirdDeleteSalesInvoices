using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoneyBirdDeleteSalesInvoices
{
    // this program deletes all draft invoices for a certain contact
    class Program
    {

        private static readonly HttpClient client = new HttpClient();
        private static string baseurl = "https://moneybird.com/api/v2/xxxxxx"; // replace xxxxxx with the ID of your administration

        public class SalesInvoice
        {
            public string id { get; set; }
        }

        static async Task Main(string[] args)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "yyy"); // replace yyyy with the token you created at https://moneybird.com/user/applications/new

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

            var streamTask = client.GetStreamAsync($"{baseurl}/sales_invoices.json?filter=state:draft,contact_id:zzzzz"); // replace zzzz with the ID of the contact that you want to delete the draft invoices for
            var salesinvoices = await JsonSerializer.DeserializeAsync<List<SalesInvoice>>(await streamTask);
            foreach (var salesinvoice in salesinvoices)
            { 
                Console.WriteLine(salesinvoice.id);
                DeleteSalesInvoice(salesinvoice.id);
            }
        }
    }
    
}
