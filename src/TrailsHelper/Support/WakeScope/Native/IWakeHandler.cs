using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrailsHelper.Support.WakeScope.Native
{
    internal interface IWakeHandler : IDisposable
    {
        ValueTask PreventSleep();
        void RestoreSleep();
    }
}
