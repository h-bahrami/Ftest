using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Funda.Services
{
    public delegate void OnProgress(int page, int totalPages);

    public interface IBusinessLogic
    {
        IEnumerable<string> CountMakelaars(string city = "amsterdam", bool withGarden = false);
        public event OnProgress OnProgressEventHandler;
    }
}
