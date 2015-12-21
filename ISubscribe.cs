using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV.EventLoop
{
    public interface IEventLoop<T>
    {
        void Subscribe(Action<T> processFunc);
    }
}
