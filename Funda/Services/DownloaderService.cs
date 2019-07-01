using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Funda.Services
{
    public class DownloaderService: IDownloaderService
    {
        private readonly string baseAdress;

        public DownloaderService(string baseAdress)
        {
            this.baseAdress = baseAdress;
        }

        public async Task<dynamic> DownloadData(string path)
        {
            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(this.baseAdress);
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

       
    }
}
