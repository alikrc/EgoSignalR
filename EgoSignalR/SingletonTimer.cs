using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace EgoSignalR
{
    public sealed class SingletonTimer
    {
        private static Timer _instance = null;
        private static readonly object _lock = new object();
        private static readonly TimeSpan _updateInterval = TimeSpan.FromSeconds(15);
        public static TimerCallback Callback = null;

        private SingletonTimer() { }

        public static Timer GetTimer()
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new Timer(Callback, null, new TimeSpan(), _updateInterval);
                }
                return _instance;
            }
        }
    }
}