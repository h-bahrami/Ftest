using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;

namespace Funda.Services
{
    public class BusinessLogic : IBusinessLogic
    {
        private readonly IDownloaderService downloader;
        private readonly int pageSize;

        public event OnProgress OnProgressEventHandler;

        private string GetPath(string city, int page, bool toHaveGarden)
        {
            return new StringBuilder()
                .Append("/feeds/Aanbod.svc/json/ac1b0b1572524640a0ecc54de453ea9f/?type=koop&zo=")
                .Append($"/{city}")
                .Append(toHaveGarden ? "/tuin" : string.Empty)
                .Append($"/&page={page}")
                .Append($"&pagesize={this.pageSize}")
                .ToString();
        }

        // the reason is that in real scenarios, services will be injected by DI
        public BusinessLogic(IDownloaderService downloader, int pageSize = 100)
        {
            this.downloader = downloader;
            this.pageSize = pageSize;
        }

        public IEnumerable<string> CountMakelaars(string city = "amsterdam", bool withGarden = false)
        {
            try
            {                
                var downloadTask = this.downloader.DownloadData(GetPath(city, 1, withGarden));
                downloadTask.Wait();
                var result = downloadTask.Result;

                var pages = (int)result["Paging"]["AantalPaginas"].Value;
                var dict = new Dictionary<string, int>();

                for (var i = 2; i <= pages; i++)
                {
                    foreach (var obj in result["Objects"])
                    {
                        var makelaarNaam = (string)obj["MakelaarNaam"].Value;
                        if (dict.ContainsKey(makelaarNaam))
                            dict[makelaarNaam] += 1;
                        else
                            dict.Add(makelaarNaam, 1);
                    }
                    
                    // for demo purpose, or even usefull for some cases ...
                    OnProgressEventHandler?.Invoke(i-1, pages);

                    Thread.Sleep(100);
                    downloadTask = this.downloader.DownloadData(GetPath(city, i, withGarden));
                    downloadTask.Wait();
                    result = downloadTask.Result;
                }

                var sorted = (from entry in dict orderby entry.Value descending select $"{entry.Key} ({entry.Value})").Take(10);
                return sorted;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Problem downloading data for [{ex.Message}]");
                return null;
            }
        }       

       
    }
}
