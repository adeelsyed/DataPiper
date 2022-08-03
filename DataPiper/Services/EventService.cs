using System;

namespace DataPiper
{
    public class EventService : IEventService
    {
        private readonly ILogService _logService;

        public EventService(ILogService logService)
        {
            _logService = logService;
        }

        /// <summary>
        /// Checks if event is subscribed to. If so, prepares EventArgs and invokes the delegates. Logs names of event handlers.
        /// </summary>
        public void Invoke<T, U>(EventHandler<T> handler, U arg, string eventName) where T : EventArgs
        {
            if (handler != null)
            {
                _logService.LogEventStarting(handler, eventName);
                Invoke(handler, arg);
                _logService.LogEventCompleted(handler, eventName);
            }
        }

        private static void Invoke<T, U>(EventHandler<T> handler, U arg) where T : EventArgs
        {
            //for example, T = FileEventArgs and U = FileInfo
            //Activator creates new FileEventArgs(arg)
            if (typeof(T) != typeof(EventArgs))
                handler.Invoke(handler.Method.DeclaringType, (T)Activator.CreateInstance(typeof(T), arg));

            //for example, T = EventArgs and U = EventArgs.Empty.
            //cannot use Activator when T = EventArgs because EventArgs does not have a constructor to call
            else
                handler.Invoke(handler.Method.DeclaringType, (T)EventArgs.Empty);
        }
    }
}
