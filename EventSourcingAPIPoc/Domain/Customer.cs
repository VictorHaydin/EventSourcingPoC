using System;
using EventSourcingAPIPoc.EventSourcing;

namespace EventSourcingAPIPoc.Domain
{
    class Customer : EntityBase
    {
        public string Name { get; set; }
        public bool IsPremium { get; set; }
    }

    class CustomerCreatedEvent : EntityEvent<Customer>
    {
        private readonly string m_Name;

        public CustomerCreatedEvent(string name)
        {
            Id.EntityId = Guid.NewGuid();
            Id.Version = 1;
            m_Name = name;
        }

        public override Customer DoApply()
        {
            var customer = new Customer
                   {
                       Name = m_Name
                   };
            return customer;
        }
    }

    class CustomerStatusChanged : EntityEvent<Customer>
    {
        private readonly VersionedEntityId m_SourceCustomer;
        private readonly bool m_NewStatus;

        public CustomerStatusChanged(VersionedEntityId sourceCustomer, bool newStatus)
        {
            Id.EntityId = sourceCustomer.EntityId;
            Id.Version = sourceCustomer.Version + 1;
            m_SourceCustomer = sourceCustomer;
            m_NewStatus = newStatus;
        }

        public override Customer DoApply()
        {
            var previousVersion = EventStorage.CustomerEventRepository.GetEvent(m_SourceCustomer);
            var customer = previousVersion.Apply();
            customer.IsPremium = m_NewStatus;
            return customer;
        }
    }
}
