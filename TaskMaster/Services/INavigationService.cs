using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMaster.Services
{
    public interface INavigationService
    {
        Task NavigateToAsync(string route, IDictionary<string, object> parameters = null);
        Task GoBackAsync();
    }
}
