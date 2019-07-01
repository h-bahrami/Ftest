using Funda.Services;
using System;
using Xunit;

namespace FundaUnittests
{
    public class JsonDownloaderUnitTest
    {
        [Fact]
        public void Returned_Data_Must_Be_Not_Null()
        {
            var downloader = new JsonDownloaderService ("http://partnerapi.funda.n");
            var query = "feeds/Aanbod.svc/json/ac1b0b1572524640a0ecc54de453ea9f/?type=koop&zo=/amsterdam/tuin/&page=1&pagesize=2";
            var result = downloader.DownloadData(query);
            Assert.NotNull(result);
        }
    }
}
