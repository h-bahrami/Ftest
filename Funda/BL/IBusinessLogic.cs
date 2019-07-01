using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Funda.BL
{
    public delegate void OnProgress(int page, int totalPages);

    public interface IBusinessLogic
    {
        IEnumerable<string> GetTopMakelaars(string city = "amsterdam", bool withGarden = false, int pageSize = 100);
        public event OnProgress OnProgressEventHandler;
    }
}
