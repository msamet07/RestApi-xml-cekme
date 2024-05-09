using System;
using System.Collections.Generic;
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
                // istek atacağım API adresini belirt
                client.BaseAddress = new Uri("https://edocumentapi.mysoft.com.tr/api/InvoiceInbox/getNewInvoiceInboxWithHeaderInfoList");

                // Bearer token'ı ekle
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue
                    ("Bearer", "TUrMbfHt9uxJvfBvrsLAjdlWoe4pJl4_1vtuZJ6UMnd5EB1_kqdWeHjp8roOhIvpf008YHqxYCATImUtu6z_t_Pd_9KAO6Bxn808QR9S4Gu2LwFe2J1kPFtuOgDkSf433GtKBSqzl_bk_hfFiTKLqMnFt1HcUUdT6R9zf4gu1A6TAaBBEChfHLIFNaTRqRja-cOfGRCHUPgVIy_n-nUmiiQzCFOsuzuuXTpQdYQpou3S3_gOQF0NWICWvvOef2HKM80F_trqbZt2fSc8kqQHURxDVJOsZ7lUjBP6snBm5HNHA6fmA-dgczQaRbWwZoCuyBSO1OwvyWAADe8qIBBdkL0vzC_2tlh1kJwkuSZfA58fYAZ_Z32_O7qKSt2XQX97S0WNLTNiwNLEXhaJlALBHqFnKR8ntZ-8PtRP85F725ffPpnnanlaYV_9m75hXgCvRCDEmUYiIYMlGJGFVTdW0HxXif40WXFQqEWYX3avOYrgR-myWuAADa7YRm2wgDWk6VKaOWRH0w7MijizbthsKxjzcfjS_EskCQeX1f7LTdCLesscXxH7l0STvPpvwcM2xPRMjFNhZVQ6jaOWwjqsxQ4hmw49vhhzuXHQNRtBbVJ2QxV3VanN_RKkn6pXbEOkjhP4i2nFztoplFcJseR_V5pnE3-J84viDgmC3bYs01xQoiTgA3faDKOclYQRCOxYxxJNrIE3weEoYJFeYUmPptY7ql4SZdta4pWPLbR0nb6uBxrNzKWv8rokps10fRheFVejh1et8wzvnAWS7HU5t9jhGNY1lsxkWCdZeJzpBj4");

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
               // Console.WriteLine(xmlData.ToString());

                // XML verisini faturalar listesine dönüştür
                List<Invoice> invoices = InvoiceHelper.ParseInvoices(xmlData);

                // Faturaları işle veya görüntüle
                foreach (var invoice in invoices)
                {
                    Console.WriteLine($"Fatura No: {invoice.DocNo}");
                    Console.WriteLine($"Profil: {invoice.Profile}");
                    Console.WriteLine($"Fatura Durumu: {invoice.InvoiceStatusText}");
                    Console.WriteLine($"Fatura Tipi: {invoice.InvoiceType}");
                    Console.WriteLine($"ETTN: {invoice.Ettn}");
                    Console.WriteLine($"Fatura Tarihi: {invoice.DocDate}");
                    Console.WriteLine($"PK Alias: {invoice.PkAlias}");
                    Console.WriteLine($"GB Alias: {invoice.GbAlias}");
                    Console.WriteLine($"VKN/TCKN: {invoice.VknTckn}");
                    Console.WriteLine($"Hesap Adı: {invoice.AccountName}");
                    Console.WriteLine($"Satır Uzunluğu Tutarı: {invoice.LineExtensionAmount}");
                    Console.WriteLine($"Vergi Hariç Tutar: {invoice.TaxExclusiveAmount}");
                    Console.WriteLine($"Vergi Dahil Tutar: {invoice.TaxInclusiveAmount}");
                    Console.WriteLine($"Ödenecek Yuvarlama Tutarı: {invoice.PayableRoundingAmount}");
                    Console.WriteLine($"Ödenecek Tutar: {invoice.PayableAmount}");
                    Console.WriteLine($"Toplam İndirim Tutarı: {invoice.AllowanceTotalAmount}");
                    Console.WriteLine($"Toplam Vergi: {invoice.TaxTotalTra}");
                    Console.WriteLine($"Para Birimi Kodu: {invoice.CurrencyCode}");
                    Console.WriteLine($"Para Birimi Kuru: {invoice.CurrencyRate}");
                    Console.WriteLine($"Oluşturulma Tarihi: {invoice.CreateDate}");
                    Console.WriteLine($"Referans Anahtarı: {invoice.ReferenceKey}");
                    Console.WriteLine();
                }

                Console.ReadLine();
            }
        }
    }

    public class Invoice
    {
        public string Id { get; set; }
        public string Profile { get; set; }
        public string InvoiceStatusText { get; set; }
        public string InvoiceType { get; set; }
        public string Ettn { get; set; }
        public string DocNo { get; set; }
        public DateTime DocDate { get; set; }
        public string PkAlias { get; set; }
        public string GbAlias { get; set; }
        public string VknTckn { get; set; }
        public string AccountName { get; set; }
        public decimal LineExtensionAmount { get; set; }
        public decimal TaxExclusiveAmount { get; set; }
        public decimal TaxInclusiveAmount { get; set; }
        public decimal PayableRoundingAmount { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal AllowanceTotalAmount { get; set; }
        public decimal TaxTotalTra { get; set; }
        public string CurrencyCode { get; set; }
        public decimal CurrencyRate { get; set; }
        public DateTime CreateDate { get; set; }
        public string ReferenceKey { get; set; }
    }

    public static class InvoiceHelper
    {
        public static List<Invoice> ParseInvoices(XDocument xmlData)
        {
            List<Invoice> invoices = new List<Invoice>();

            foreach (var dataElement in xmlData.Root.Elements("data"))
            {
                Invoice invoice = new Invoice
                {
                    Id = dataElement.Element("id")?.Value,
                    Profile = dataElement.Element("profile")?.Value,
                    InvoiceStatusText = dataElement.Element("invoiceStatusText")?.Value,
                    InvoiceType = dataElement.Element("invoiceType")?.Value,
                    Ettn = dataElement.Element("ettn")?.Value,
                    DocNo = dataElement.Element("docNo")?.Value,
                    DocDate = DateTime.Parse(dataElement.Element("docDate")?.Value),
                    PkAlias = dataElement.Element("pkAlias")?.Value,
                    GbAlias = dataElement.Element("gbAlias")?.Value,
                    VknTckn = dataElement.Element("vknTckn")?.Value,
                    AccountName = dataElement.Element("accountName")?.Value,
                    LineExtensionAmount = decimal.Parse(dataElement.Element("lineExtensionAmount")?.Value),
                    TaxExclusiveAmount = decimal.Parse(dataElement.Element("taxExclusiveAmount")?.Value),
                    TaxInclusiveAmount = decimal.Parse(dataElement.Element("taxInclusiveAmount")?.Value),
                    PayableRoundingAmount = decimal.Parse(dataElement.Element("payableRoundingAmount")?.Value),
                    PayableAmount = decimal.Parse(dataElement.Element("payableAmount")?.Value),
                    AllowanceTotalAmount = decimal.Parse(dataElement.Element("allowanceTotalAmount")?.Value),
                    TaxTotalTra = decimal.Parse(dataElement.Element("taxTotalTra")?.Value),
                    CurrencyCode = dataElement.Element("currencyCode")?.Value,
                    CurrencyRate = decimal.Parse(dataElement.Element("currencyRate")?.Value),
                    CreateDate = DateTime.Parse(dataElement.Element("createDate")?.Value),
                    ReferenceKey = dataElement.Element("referanceKey")?.Value
                };

                invoices.Add(invoice);
            }

            return invoices;
        }
    }
}

