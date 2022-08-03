using System;

namespace DataPiper
{
    public interface IEventService
    {
        void Invoke<T, U>(EventHandler<T> handler, U arg, string eventName) where T : EventArgs;
    }
}
