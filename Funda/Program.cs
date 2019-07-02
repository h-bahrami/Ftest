using System;
using System.Linq;
using System.Collections.Generic;
using Funda.Services;
using Funda.BL;

namespace Funda
{
    class Program
    { 
        static void Main(string[] args)
        {
            // in real-world scenario the downloader service will be injected by DI, also the BL also will be injected
            var business = new BusinessLogic(new JsonDownloaderService ("http://partnerapi.funda.nl"));
            business.OnProgressEventHandler += (int page, int totalPages) =>
            {
                Console.Title = $"{page}/{totalPages} Downloaded and Processed.";
            };

            var topMakelaarsAmsterdam = business.GetTopMakelaars(city: "amsterdam");
            print(topMakelaarsAmsterdam, "Amsterdam top makelaar");
            var topMakelaarsAmsterdamTuin = business.GetTopMakelaars(city: "amsterdam", withGarden: true);
            print(topMakelaarsAmsterdam, "Amsterdam top makelaar (tuin)");
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

    }
}
