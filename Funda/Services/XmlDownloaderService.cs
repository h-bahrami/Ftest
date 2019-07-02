using System;
using System.Collections.Generic;
using System.Text;

namespace Funda.Services
{
    public class XmlDownloaderService : IDownloaderService
    {
        // in case we want to get xml then we implement this method 
        public System.Threading.Tasks.Task<dynamic> DownloadData(string path)
        {
            throw new NotImplementedException();
        }
    }
}
