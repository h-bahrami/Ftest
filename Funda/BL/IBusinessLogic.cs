using System.Collections.Generic;

namespace Funda.BL
{
    public delegate void OnProgress(int page, int totalPages);

    public interface IBusinessLogic
    {
        IEnumerable<string> GetTopMakelaars(string city, bool withGarden = false, int take = 10);
        public event OnProgress OnProgressEventHandler;
    }
}
