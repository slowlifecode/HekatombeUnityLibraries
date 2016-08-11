using System;
using System.Collections.Generic;

namespace Hekatombe.Services
{
    public class EventDispatcher
    {
		private static EventDispatcher _instance;
		
        public delegate void EventDelegate<T>(T e) where T : class;

        public delegate void DefaultEventDelegate(object e);

        readonly Dictionary<Type, List<Delegate>> _delegates = new Dictionary<Type, List<Delegate>>();

        List<Delegate> defaultDelegate = new List<Delegate>();

        readonly List<EventDispatcher> dispatchers = new List<EventDispatcher>();

		//Remember to initilize this anywhere, preferably on ServiceLocator
		public EventDispatcher()
		{
			//Creates the singleton
			if (_instance == null) {
				_instance = this;
			}
		}

		public static EventDispatcher GetInstance()
		{
			return _instance;
		}

        public void AddDispatcher(EventDispatcher dispatcher)
        {
            dispatchers.Add(dispatcher);
        }

        public bool RemoveDispatcher(EventDispatcher dispatcher)
        {
            return dispatchers.Remove(dispatcher);
        }

		public static void StAddListener<T>(EventDelegate<T> listener) where T : class
		{
			_instance.AddListener<T> (listener);
		}

        public void AddListener<T>(EventDelegate<T> listener) where T : class
        {
            List<Delegate> d;
			if (!_instance._delegates.TryGetValue(typeof(T), out d))
            {
                d = new List<Delegate>();
				_delegates[typeof(T)] = d;
            }
            if (!d.Contains(listener))
                d.Add(listener);
		}

		public static void StRemoveListener<T>(EventDelegate<T> listener) where T : class
		{
			_instance.RemoveListener<T> (listener);
		}

        public bool RemoveListener<T>(EventDelegate<T> listener) where T : class
        {
            List<Delegate> d;
            if (_delegates.TryGetValue(typeof(T), out d))
            {
                d.Remove(listener);
                return true;
            }
            return false;
        }

        public void AddDefaultListener(DefaultEventDelegate listener)
        {
            if (!defaultDelegate.Contains(listener))
                defaultDelegate.Add(listener);
        }

        public void RemoveDefaultListener(DefaultEventDelegate listener)
        {
            defaultDelegate.Remove(listener);
        }

		public static void StRaise<T>(T e) where T : class
		{
			_instance.Raise<T> (e);
		}

        public void Raise<T>(T e) where T : class
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }
            for (int i = 0; i < defaultDelegate.Count; ++i)
            {
                (defaultDelegate[i] as DefaultEventDelegate)(e);
            }

            List<Delegate> dlgList = GetDelegateListForEventType(e);

            for (int i = 0; i < dlgList.Count; ++i)
            {
                var callback = dlgList[i] as EventDelegate<T>;
                if (callback != null)
                {
                    callback(e);
                }
            }

            for (int k = 0; k < dispatchers.Count; k++)
            {
                EventDispatcher dispatcher = dispatchers[k];
                dispatcher.Raise<T>(e);
            }
        }

        public void UnregisterAll()
        {
            _delegates.Clear();
            defaultDelegate.Clear();
            dispatchers.Clear();
        }

        public void Raise<T>() where T : class, new()
        {
            Raise<T>(new T());
        }
			
        public void DebugPrintAllListeners(ref string debugStr)
        {
            var sb = new System.Text.StringBuilder();

            foreach (var key in _delegates.Keys)
            {
                var str = String.Empty;
                DebugPrintListenersForType(key, ref str);
                sb.Append(str);
            }

            debugStr = sb.ToString();
        }
			
        public void DebugPrintListenersForType<T>(ref string debugStr)
        {
            DebugPrintListenersForType(typeof(T), ref debugStr);
        }

        public void DebugPrintListenersForType(Type type, ref string debugStr)
        {
            var sb = new System.Text.StringBuilder();

            if (_delegates.ContainsKey(type))
            {
                sb.AppendFormat("{0} listener(s) for event {1}", _delegates[type].Count, type.ToString());
				sb.AppendLine();

                foreach (var dlg in _delegates[type])
                {
                    sb.Append("\tClass ").AppendLine(dlg.Target.GetType().ToString()).Append("\tInstance ").AppendLine(dlg.Target.ToString()).Append("\tMethod ").AppendLine(dlg.Method.ToString()).AppendLine();
                }
            }
            else
            {
                sb.AppendFormat("No listeners. Event {0}", type)
                  .AppendLine();
            }

            sb.AppendLine();

            debugStr = sb.ToString();
        }

		//Has to be done creating a copy of the delegates to avoid modify the list while you are iterating it, can happen if the event listener unregister itself, what you should never do
        List<Delegate> GetDelegateListForEventType<T>(T e)
        {
            List<Delegate> dlgList;
            if (_delegates.TryGetValue(typeof(T), out dlgList))
            {
                return new List<Delegate>(dlgList);
            }
            else
            {
                return new List<Delegate>();
            }
        }
    }   
}
