using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
namespace LntegratedMiddleware.ClearCache
{
  public   class ClearCache
    {
        public static Timer timer = null;
        public static void init()
        {
            timer = new Timer(18000000);
            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Elapsed += Timer_Elapsed;
        }


        [DllImport("kernel32.dll")]
        public static extern bool SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);

        public static void GarbageCollect()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        public static void FlushMemory()

        {
            GarbageCollect();

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }


        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            
            FlushMemory();
        }
    }
}
