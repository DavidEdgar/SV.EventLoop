using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SV.EventLoop
{
    public class Timer<T> : IEventLoop<T>
    {
        private readonly int _sleepTime;
        public Func<T> _subjectGetter;

        public Timer(Func<T> subjectGetter, int sleepTime)
        {
            this._subjectGetter = subjectGetter;
            this._sleepTime = sleepTime;
        } 

        public void Subscribe(Action<T> processFunc)
        {
            while (true)
            {
                T data = _subjectGetter();
                processFunc(data);
                Thread.Sleep(this._sleepTime);
            }
        }
    }
}
