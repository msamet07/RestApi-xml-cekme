using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace ConsoleApp2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // JSON verisini dinamik olarak oluştur
            JObject jsonContent = new JObject();
            jsonContent["startDate"] = "2024-05-01T00:00:00";
            jsonContent["endDate"] = "2024-05-05T00:00:00";
            jsonContent["pkAlias"] = "";
            jsonContent["isUseDocDate"] = true;
            jsonContent["cessionStatus"] = 0;
            jsonContent["afterValue"] = 0;
            jsonContent["limit"] = 100;
            jsonContent["tenantIdentifierNumber"] = "";

            // JSON verisini string'e dönüştür
            string jsonString = jsonContent.ToString();

            // HttpClient oluştur
            using (var client = new HttpClient())
            {
                // API adresini belirt
                client.BaseAddress = new Uri("https://edocumentapi.mysoft.com.tr/api/InvoiceInbox/getInvoiceInboxListForPeriod");

                // Bearer token'ı ekle
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "YtfHc7o8Cy4pjOFLc0xDF7DCqeQoF5aydtduqmS93hcv-NCRy2Z4PrwDvn-tOrW-Vy6nu_tESXePLYsYacrcZG5T4TAG7t_0rjdtw9Ww9Nf2x7paq8xR8THBVsqji8WJzGof3JRjpRQsGUg3mfyq8Leaigd0h6VWVYnhfwJfxA8_JZjtQ397orYw0gJ8oL2VRw1ay8Bkc4THwbkefQwhlki9HxFphp_D7wA5qjE2m2O_Hh8hOhVQ9GqIVmzlhvbaRIC6iX0wXtyVF_TiF4Uo05ROATEOv147WJn7lrUbctYIuwRiq_MU5_j4m_HffdlZeybNpP8ggGffVlH_di7CWk0hmmwsjhRjBrEQnOl5PVofPOa625mPj4oZZrRFRwNQ2AUQfAw4s6dZG0pZxfIdDk3QUqHu81aKz-AdNWhynmawngKYiP6uCiHlXQKBP-4Bcj6z0lx5I328MX4-MnPN1GIdHaaszFPlL5xddGbPQ1-ipkgcRJUPjeGSAfrNrq9vVUIuOWN3B7r6JF1197wA1ky8Wk_sIL-8PVE8ALxv6Xn3Yk8TT1LIRrTrOqqrVQ5vcLesbnPjzrc7RhWpgEQMjOVxSQTDnQIoaH6y_jp_ZArT-ntQ55OkEhWVUjqLH8mXABRzQb4mWiPxdDIA18CKrJaowoauq-j_H1_G0tdl6EncyZYkZb91JzNwyyC9w8h_nwsY1wnGykM9SV4pD09oPLz2ryFU_DU-XsuWNYXL2RZFUiuji7bCDXuI7yxSIozoAgBzE0y7XAET4SNek09MMGfsqIdEe7bfDZA5L1hkxbA");

                // JSON içeriğini HTTP içeriğine dönüştür
                var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");

                // PostAsync metodu ile isteği gönder ve cevabı al
                var response = await client.PostAsync("", content);

                // Sunucudan gelen yanıtı al
                string responseJson = await response.Content.ReadAsStringAsync();

                // JSON verisini XML'e dönüştür
                JObject jsonObject = JObject.Parse(responseJson);
                XDocument xmlData = JsonConvert.DeserializeXNode(responseJson, "Root");

                // Konsolda XML verisini göster
                Console.WriteLine(xmlData.ToString());
                Console.ReadLine();
            }
        }
    }
}

