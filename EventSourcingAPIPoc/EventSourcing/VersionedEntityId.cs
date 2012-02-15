using System;

namespace EventSourcingAPIPoc.EventSourcing
{
    class VersionedEntityId
    {
        public Guid EntityId { get; set; }
        public int Version { get; set; }
    }
}
