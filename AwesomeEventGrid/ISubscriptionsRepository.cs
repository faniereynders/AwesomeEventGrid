using AwesomeEventGrid.Entities;
using System.Collections.Generic;

namespace AwesomeEventGrid
{
    public interface ISubscriptionsRepository
    {
        IEnumerable<Subscription> GetAll();
        Subscription AddOrUpdate(Subscription subscription);
        void Remove(string name);
    }
}