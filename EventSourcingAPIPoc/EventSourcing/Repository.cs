using System;

namespace EventSourcingAPIPoc.EventSourcing
{
    class Repository<TEntity> where TEntity : EntityBase
    {
        private readonly EventRepository<TEntity> m_EventRepository;

        public Repository(EventRepository<TEntity> eventRepository)
        {
            m_EventRepository = eventRepository;
        }

        public TEntity GetEntity(Guid id)
        {
            var e = m_EventRepository.GetLatestVersion(id);
            return e.Apply();
        }

        public TEntity GetEntityWithVersion(VersionedEntityId id)
        {
            var e = m_EventRepository.GetEvent(id);
            return e.Apply();
        }
    }
}
