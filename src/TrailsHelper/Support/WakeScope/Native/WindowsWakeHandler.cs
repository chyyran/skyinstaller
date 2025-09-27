using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TrailsHelper.Support.WakeScope.Native
{
    internal partial class WindowsWakeHandler : IWakeHandler
    {
        [FlagsAttribute]
        public enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
            // Legacy flag, should not be used.
            // ES_USER_PRESENT = 0x00000004
        }

        [LibraryImport("kernel32.dll", SetLastError = true)]
        internal static partial EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);


        public ValueTask PreventSleep()
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_SYSTEM_REQUIRED | EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
            Debug.WriteLine("Setting ThreadExecutionState SYSTEM_REQUIRED | DISPLAY_REQUIRED");
            return new ValueTask();
        }

        public void RestoreSleep() 
        {
            SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
            Debug.WriteLine("Setting ThreadExecutionState CONTINUOUS");
        }

        public void Dispose()
        {
            
        }
    }
}
