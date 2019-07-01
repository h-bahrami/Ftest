using Funda.BL;
using Funda.Services;
using System;
using System.Linq;
using Xunit;

namespace FundaUnittests
{
    public class BusinessLogicUnitTest
    {
        [Fact]
        public void Returned_Data_Must_Be_Not_Null()
        {
            var downloader = new JsonDownloaderService ("http://partnerapi.funda.nl");
            var bl = new BusinessLogic(downloader);

            var result = bl.GetTopMakelaars();
            Assert.NotNull(result);
            Assert.Equal(10, result.ToList().Count);
        }

        [Fact]
        public void Returned_Data_Must_Be_Not_Null_Garden()
        {
            var downloader = new JsonDownloaderService("http://partnerapi.funda.nl");
            var bl = new BusinessLogic(downloader);

            var result = bl.GetTopMakelaars(withGarden:true);
            Assert.NotNull(result);
            Assert.Equal(10, result.ToList().Count);
        }

    }
}
