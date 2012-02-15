using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using EventSourcingAPIPoc.Domain;
using EventSourcingAPIPoc.EventSourcing;

namespace EventSourcingAPIPoc
{
    class Program
    {
        private static Random ms_Random = new Random();
        private static readonly List<Thread> ms_Threads = new List<Thread>();

        static void Main(string[] args)
        {
            var customer = CustomerService.CreateCustomer("Test");
            StartCustomerModificationThreads(customer);
            StartOrderWorkflowsThreads(customer);
            Console.ReadKey();
            foreach (var thread in ms_Threads)
            {
                thread.Abort();
            }
            Console.ReadKey();
        }

        private static void ProcessOrderWorkflow(Customer customer)
        {
            while (true)
            {
                Thread.Sleep(ms_Random.Next(500));
                var order = OrderService.PlaceOrder(CustomerService.Repository.GetEntity(customer.Id.EntityId).Id);
                Console.WriteLine("Order placed.");
                Thread.Sleep(ms_Random.Next(500));
                order = OrderService.ProcessOrder(CustomerService.Repository.GetEntity(customer.Id.EntityId).Id,
                                                  order.Id);
                Console.WriteLine("Order processed. Amount: {0}", order.Amount);
            }
        }

        private static void StartOrderWorkflowsThreads(Customer customer)
        {
            for (int i = 0; i < 25; i++)
            {
                var processOrderWorkflowThread = new Thread(() => ProcessOrderWorkflow(customer));
                processOrderWorkflowThread.Start();
                ms_Threads.Add(processOrderWorkflowThread);
            }
        }

        private static void StartCustomerModificationThreads(Customer customer)
        {
            for (int i = 0; i < 5; i++)
            {
                var customerStatusChanges = new Thread(() => ChangeCustomerStatuses(customer.Id.EntityId));
                customerStatusChanges.Start();
                ms_Threads.Add(customerStatusChanges);
            }
        }

        static void ChangeCustomerStatuses(Guid customerId)
        {
            while (true)
            {
                Thread.Sleep(ms_Random.Next(200));
                try
                {
                    var customer = CustomerService.Repository.GetEntity(customerId);
                    Thread.Sleep(ms_Random.Next(100));
                    CustomerService.ChangeCustomerStatus(customer.Id, !customer.IsPremium);
                    Console.WriteLine("Customer premium status changed to: {0}", !customer.IsPremium);
                }
                catch(OptimisticLockException)
                {
                    Console.WriteLine("Optimistic lock exception!");
                }
            }
        }
    }
}
