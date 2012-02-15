using EventSourcingAPIPoc.EventSourcing;

namespace EventSourcingAPIPoc.Domain
{
    static class OrderService
    {
        private static readonly Repository<Order> ms_OrderRepository = new Repository<Order>(EventStorage.OrderEventRepository);

        public static Repository<Order> Repository
        {
            get { return ms_OrderRepository; }
        }

        public static Order PlaceOrder(VersionedEntityId customerId)
        {
            return EventStorage.OrderEventRepository.AppendEvent(new OrderCreated(customerId));
        }

        public static Order ProcessOrder(VersionedEntityId customerId, VersionedEntityId orderId)
        {
            return EventStorage.OrderEventRepository.AppendEvent(new OrderProcessed(customerId, orderId));
        }
    }
}
