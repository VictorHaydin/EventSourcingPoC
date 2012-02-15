using EventSourcingAPIPoc.Domain;

namespace EventSourcingAPIPoc.EventSourcing
{
    static class EventStorage
    {
        private static readonly EventRepository<Customer> ms_CustomerEventRepository = new EventRepository<Customer>();

        public static EventRepository<Customer> CustomerEventRepository
        {
            get { return ms_CustomerEventRepository; }
        }

        private static readonly EventRepository<Order> ms_OrderEventRepository = new EventRepository<Order>();


        public static EventRepository<Order> OrderEventRepository
        {
            get { return ms_OrderEventRepository; }
        }
    }
}
