using System;
using System.Collections.Generic;

namespace EventSourcingAPIPoc.EventSourcing
{
    class EventStream<TEvent> where TEvent : EventBase
    {
        private readonly object m_SyncRoot = new object();
        private readonly List<TEvent> m_InternalStorage = new List<TEvent>();
        private int m_Count = 0;

        public TEvent GetEvent(int version)
        {
            return m_InternalStorage[version - 1];
        }

        public TEvent GetLatestEvent()
        {
            return m_InternalStorage[m_Count - 1];
        }

        public void AppendEvent(TEvent e)
        {
            lock (m_SyncRoot)
            {
                if(e.Id.Version != m_InternalStorage.Count + 1)
                {
                    throw new OptimisticLockException();
                }
                m_InternalStorage.Add(e);
                m_Count++;
            }
        }

        public int GetEventCount()
        {
            return m_Count;
        }
    }

    public class OptimisticLockException : Exception
    {
    }
}
