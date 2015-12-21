using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Burrow;

namespace SV.EventLoop
{
    public class RabbitQueue<T> : IEventLoop<T>
    {
        private readonly RabbitTunnel _tunnel;
        private readonly ushort _batchSize = 1;
        private readonly ushort _queuePrefetchSize;
        private readonly string _subscriptionName;
        private readonly IRouteFinder _routeFinder;
        
        public RabbitQueue(RabbitTunnel tunnel)
        {
            this._tunnel = tunnel;
        }

        public RabbitQueue(RabbitTunnel tunnel, ushort batchSize, ushort queuePrefetchSize, string subscriptionName)
            : this(tunnel)
        {
            this._batchSize = batchSize;
            this._queuePrefetchSize = queuePrefetchSize;
            this._subscriptionName = subscriptionName;
        }

        public RabbitQueue(RabbitTunnel tunnel, ushort batchSize, ushort queuePrefetchSize, string subscriptionName, IRouteFinder routeFinder)
            : this(tunnel, batchSize, queuePrefetchSize, subscriptionName)
        {
            this._routeFinder = routeFinder;
        }

        public void Subscribe(Action<T> processFunc)
        {
            SubscriptionOption<T> options = new SubscriptionOption<T>{MessageHandler = processFunc, BatchSize = this._batchSize};
            if (this._queuePrefetchSize > 0)
            {
                options.QueuePrefetchSize = this._queuePrefetchSize;
            }

            if (!string.IsNullOrEmpty(this._subscriptionName))
            {
                options.SubscriptionName = this._subscriptionName;
            }

            if (this._routeFinder != null)
            {
                options.RouteFinder = this._routeFinder;
            }
            
            
            this._tunnel.Subscribe(options);
        }
    }
}
