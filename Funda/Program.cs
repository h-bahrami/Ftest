using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Funda
{
    class Program
    {
        private const int PageRequestCount = 100;

        private static async Task<dynamic> DownloadData(string path)
        {
            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri("http://partnerapi.funda.nl");
                using var response = await client.GetAsync(path);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                dynamic parsed = JsonConvert.DeserializeObject(json);
                return parsed;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        private static IEnumerable<string> CountMakelaars(string urlPattern, string info)
        {
            try
            {
                var downloadTask = DownloadData(string.Format(urlPattern, 1, PageRequestCount));
                downloadTask.Wait();
                var result = downloadTask.Result;

                var pages = (int)result["Paging"]["AantalPaginas"].Value;
                var dict = new Dictionary<string, int>();

                for (var i = 2; i <= pages; i++)
                {
                    Console.Title = $"Processing {info} [Page {i} of {pages}]...";
                    foreach (var obj in result["Objects"])
                    {
                        var makelaarNaam = (string)obj["MakelaarNaam"].Value;
                        if (dict.ContainsKey(makelaarNaam))
                            dict[makelaarNaam] += 1;
                        else
                            dict.Add(makelaarNaam, 1);
                    }
                    Thread.Sleep(100);
                    downloadTask = DownloadData(string.Format(urlPattern, i, PageRequestCount));
                    downloadTask.Wait();
                    result = downloadTask.Result;
                }

                var sorted = (from entry in dict orderby entry.Value descending select $"{entry.Key} ({entry.Value})").Take(10);
                return sorted;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Problem downloading data for {info} [{ex.Message}]");
                return null;
            }
        }

        private static void print(IEnumerable<string> data, string info)
        {
            if (data != null && data.Any())
            {
                Console.WriteLine(info);
                Console.WriteLine("---------------------------------------------");
                data.ToList().ForEach(x => Console.WriteLine(x));
                Console.WriteLine("---------------------------------------------");
            }
        }

        static void Main(string[] args)
        {
            string path = "/feeds/Aanbod.svc/json/ac1b0b1572524640a0ecc54de453ea9f/?type=koop&zo=/amsterdam";
            string info = "Top Makelaars Amsterdam";
            var topMakelaarsAmsterdam = CountMakelaars($"{path}/&page={{0}}&pagesize={{1}}", info);
            print(topMakelaarsAmsterdam, info);

            info = "Top Makelaars Amsterdam (Tuin)";
            var topMakelaarsAmsterdamTuin = CountMakelaars($"{path}/tuin/&page={{0}}&pagesize={{1}}", info);
            print(topMakelaarsAmsterdamTuin, info);
        }
    }
}
