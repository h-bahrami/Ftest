using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using Funda.Services;

namespace Funda.BL
{
    public class BusinessLogic : IBusinessLogic
    {
        private readonly IDownloaderService downloader;
        private int pageSize = 100;

        public event OnProgress OnProgressEventHandler;

        private string GetPath(string city, int page, int pageSize, bool toHaveGarden)
        {
            return new StringBuilder()
                .Append("/feeds/Aanbod.svc/json/ac1b0b1572524640a0ecc54de453ea9f/?type=koop&zo=")
                .Append($"/{city}")
                .Append(toHaveGarden ? "/tuin" : string.Empty)
                .Append($"/&page={page}")
                .Append($"&pagesize={pageSize}")
                .ToString();
        }

        // the reason is that in real scenarios, services will be injected by DI
        public BusinessLogic(IDownloaderService downloader)
        {
            this.downloader = downloader;
        }

        /// <summary>
        /// Downloads and processes the data to extract top makelaars
        /// </summary>
        /// <param name="city"></param>
        /// <param name="withGarden"></param>
        /// <param name="pageSize">this parameter can be assigned by </param>
        /// <returns></returns>
        public IEnumerable<string> GetTopMakelaars(string city, bool withGarden = false, int take = 10)
        {
            try
            {                
                var downloadTask = this.downloader.DownloadData(GetPath(city, 1, this.pageSize, withGarden));
                downloadTask.Wait();
                var result = downloadTask.Result;

                var totalPages = (int)result["Paging"]["AantalPaginas"].Value;
                var dict = new Dictionary<string, int>();

                for (var i = 2; i <= totalPages; i++)
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
                    OnProgressEventHandler?.Invoke(i-1, totalPages);

                    Thread.Sleep(100);
                    downloadTask = this.downloader.DownloadData(GetPath(city, i, this.pageSize, withGarden));
                    downloadTask.Wait();
                    result = downloadTask.Result;
                }

                var sorted = (from entry in dict orderby entry.Value descending select $"{entry.Key} ({entry.Value})").Take(take);
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
