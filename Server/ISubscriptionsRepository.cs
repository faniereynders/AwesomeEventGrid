using System.Collections.Generic;

namespace WebApplication23
{
    public interface ISubscriptionsRepository
    {
        IEnumerable<Subscription> GetAll();
        Subscription AddOrUpdate(Subscription subscription);
        void Remove(string name);
    }
}