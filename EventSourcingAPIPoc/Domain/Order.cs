using System;
using EventSourcingAPIPoc.EventSourcing;

namespace EventSourcingAPIPoc.Domain
{
    class Order : EntityBase
    {
        public VersionedEntityId CustomerRef { get; set; }
        public int Amount { get; set; }
        public bool Processed { get; set; }
    }

    class OrderCreated : EntityEvent<Order>
    {
        private readonly VersionedEntityId m_CustomerRef;

        public OrderCreated(VersionedEntityId customerRef)
        {
            Id.EntityId = Guid.NewGuid();
            Id.Version = 1;
            m_CustomerRef = customerRef;
        }

        public override Order DoApply()
        {
            var order = new Order
                   {
                       CustomerRef = m_CustomerRef
                   };
            return order;
        }
    }

    class OrderProcessed : EntityEvent<Order>
    {
        private readonly VersionedEntityId m_CustomerId;
        private readonly VersionedEntityId m_OrderId;

        public OrderProcessed(VersionedEntityId customerId, VersionedEntityId orderId)
        {
            Id.EntityId = orderId.EntityId;
            Id.Version = orderId.Version + 1;
            m_CustomerId = customerId;
            m_OrderId = orderId;
        }

        public override Order DoApply()
        {
            var customer = CustomerService.Repository.GetEntityWithVersion(m_CustomerId);
            var order = OrderService.Repository.GetEntityWithVersion(m_OrderId);
            order.Amount = customer.IsPremium ? 100 : 50;
            order.Processed = true;
            return order;
        }
    }
}
