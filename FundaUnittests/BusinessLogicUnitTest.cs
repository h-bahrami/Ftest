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

            var result = bl.GetTopMakelaars(city: "amsterdam", take: 15);
            Assert.NotNull(result);
            Assert.Equal(15, result.ToList().Count);
        }

        [Fact]
        public void Returned_Data_Must_Be_Not_Null_Garden()
        {
            var downloader = new JsonDownloaderService("http://partnerapi.funda.nl");
            var bl = new BusinessLogic(downloader);

            var result = bl.GetTopMakelaars(city: "amsterdam", withGarden:true, take:12);
            Assert.NotNull(result);
            Assert.Equal(12, result.ToList().Count);
        }

    }
}
