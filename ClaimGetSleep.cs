using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SV.EventLoop
{
    /// <summary>
    /// The looping mechanism that's asking "Are we there yet" every few seconds.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ClaimGetSleep<T> : IEventLoop<T>
    {
        private readonly Action _claimer;
        private readonly Func<T> _getter;
        private readonly int _loopSleepTime;

        public ClaimGetSleep(Action claimer, Func<T> getter, int loopSleepTime)
        {
            this._claimer = claimer;
            this._getter = getter;
            this._loopSleepTime = loopSleepTime;
        }

        public void Subscribe(Action<T> process)
        {
            while (true)
            {
                T items = this._getter();

                if (items != null)
                {
                    this._claimer();
                    items = this._getter();
                }

                process(items);

                Thread.Sleep(_loopSleepTime);
            }
        }

        public delegate void ClaimDelegate();

        public delegate T GetDelegate();

        public delegate void ProcessDelegate(T items);
    }
}
