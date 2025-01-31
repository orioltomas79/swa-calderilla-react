using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calderilla.Services
{
    public interface IGetMessageService
    {
        string GetMessage(string userName);
    }
}
