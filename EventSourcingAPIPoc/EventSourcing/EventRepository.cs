using System;
using System.Collections.Generic;
using System.Linq;

namespace EventSourcingAPIPoc.EventSourcing
{
    class EventRepository<TEntity> where TEntity : EntityBase
    {
        private readonly object m_SyncRoot = new object();
        private readonly Dictionary<Guid, EventStream<EntityEvent<TEntity>>> m_Storage = new Dictionary<Guid, EventStream<EntityEvent<TEntity>>>();

        public EntityEvent<TEntity> GetEvent(VersionedEntityId versionedEntityId)
        {
            return m_Storage[versionedEntityId.EntityId].GetEvent(versionedEntityId.Version);
        }

        public EntityEvent<TEntity> GetLatestVersion(Guid entityId)
        {
            return m_Storage[entityId].GetLatestEvent();
        }

        public TEntity AppendEvent(EntityEvent<TEntity> e)
        {
            lock (m_SyncRoot)
            {
                if (!m_Storage.ContainsKey(e.Id.EntityId))
                {
                    m_Storage.Add(e.Id.EntityId, new EventStream<EntityEvent<TEntity>>());
                }
            }
            m_Storage[e.Id.EntityId].AppendEvent(e);
            return e.Apply();
        }

        public int GetEventCount()
        {
            return m_Storage.Values.Sum(s => s.GetEventCount());
        }
    }
}
