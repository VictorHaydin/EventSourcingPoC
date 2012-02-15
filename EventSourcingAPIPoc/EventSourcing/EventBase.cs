namespace EventSourcingAPIPoc.EventSourcing
{
    abstract class EventBase
    {
        public VersionedEntityId Id { get; private set; }

        protected EventBase()
        {
            Id = new VersionedEntityId();
        }
    }

    abstract class EntityEvent<TEntity> : EventBase where TEntity : EntityBase
    {
        public abstract TEntity DoApply();

        public TEntity Apply()
        {
            var entity = DoApply();
            entity.Id.EntityId = Id.EntityId;
            entity.Id.Version = Id.Version;
            return entity;
        }
    }
}
