using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DemoSendJson
{
    class Program
    {
        private readonly string apiUrl;
        private readonly string apiToken;

        public Program()
        {
            apiUrl = ConfigurationManager.AppSettings["ApiUrl"];
            apiToken = ConfigurationManager.AppSettings["ApiToken"];
        }

        static async Task Main()
        {
            Program program = new Program();

            List<MyData> dataList = new List<MyData>
            {
                new MyData { Id = 1, Name = "Item 1" },
                new MyData { Id = 2, Name = "Item 2" },
                new MyData { Id = 3, Name = "Item 3" },
                new MyData { Id = 4, Name = "Item 4" },
                new MyData { Id = 5, Name = "Item 5" }
            };

            using (HttpClient httpClient = new HttpClient())
            {
                foreach (var data in dataList)
                {
                    string jsonData = JsonConvert.SerializeObject(data);

                    httpClient.DefaultRequestHeaders.Clear();
                    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {program.apiToken}");
                    HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await httpClient.PostAsync(program.apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Data {data.Id} berhasil dikirim ke server.");
                    }
                    else
                    {
                        Console.WriteLine($"Gagal mengirim data {data.Id}. Status Code: {response.StatusCode}");
                    }

                    httpClient.DefaultRequestHeaders.Remove("Authorization");
                }
            }

            Console.WriteLine("Pengolahan data selesai.");
        }
    }

    class MyData
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
