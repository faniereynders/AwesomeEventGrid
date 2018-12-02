using AwesomeEventGrid.Entities;
using System.Collections.Generic;

namespace AwesomeEventGrid
{
    public interface ISubscriptionsRepository
    {
        Subscription FindByName(string topic, string name);
        IEnumerable<Subscription> GetAll(string topic);
        Subscription AddOrUpdate(Subscription subscription);
        void Remove(string name);
    }
}