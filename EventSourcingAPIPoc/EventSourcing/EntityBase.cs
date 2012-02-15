namespace EventSourcingAPIPoc.EventSourcing
{
    class EntityBase
    {
        private readonly VersionedEntityId m_Id = new VersionedEntityId();

        public VersionedEntityId Id
        {
            get { return m_Id; }
        }
    }
}
