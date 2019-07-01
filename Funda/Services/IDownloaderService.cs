using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Funda.Services
{
    public interface IDownloaderService
    {
        Task<dynamic> DownloadData(string path);
    }
}
