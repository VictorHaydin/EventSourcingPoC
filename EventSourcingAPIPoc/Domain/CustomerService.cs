using EventSourcingAPIPoc.EventSourcing;

namespace EventSourcingAPIPoc.Domain
{
    static class CustomerService
    {
        private static readonly Repository<Customer> ms_Repository = new Repository<Customer>(EventStorage.CustomerEventRepository);

        public static Repository<Customer> Repository
        {
            get { return ms_Repository; }
        }

        public static void ChangeCustomerStatus(VersionedEntityId id, bool status)
        {
            EventStorage.CustomerEventRepository.AppendEvent(new CustomerStatusChanged(id, status));
        }

        public static Customer CreateCustomer(string name)
        {
            return EventStorage.CustomerEventRepository.AppendEvent(new CustomerCreatedEvent(name));
        }
    }
}
