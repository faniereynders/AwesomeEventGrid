using AwesomeEventGrid.Entities;
using AwesomeEventGrid.Stubs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AwesomeEventGrid.Infrastructure
{

    public class SubscriptionsRepository : ISubscriptionsRepository
    {
        //private static readonly List<Subscription> subscriptions;
        private readonly Data data;

        public SubscriptionsRepository(Data data)
        {
            // subscriptions = new List<Subscription>();

            this.data = data;

        }
        public IEnumerable<Subscription> GetAll(string topic = null)
        {
            var result = data.Subscriptions;
            if (topic != null)
            {
                result = result.Where(t => t.Topic.Equals(topic, StringComparison.InvariantCultureIgnoreCase)).ToList();

            }

            return result;
        }
        public Subscription AddOrUpdate(Subscription subscription)
        {
            var current = data.Subscriptions.FirstOrDefault(s => s.Topic == subscription.Topic && s.EndpointUrl == subscription.EndpointUrl);
            if (current != null)
            {

                current.EventTypes = subscription.EventTypes;
                current.SubjectBeginsWith = subscription.SubjectBeginsWith;
                current.SubjectEndsWith = subscription.SubjectEndsWith;
                return current;
            }
            else
            {
                //  subscription.Id = Guid.NewGuid().ToString();
                data.Subscriptions.Add(subscription);
                return subscription;
            }

        }
        public void Remove(string name)
        {
            data.Subscriptions.Remove(data.Subscriptions.FirstOrDefault(s => s.Name == name));
        }

        public Subscription FindByName(string topic, string name)
        {
            return data.Subscriptions.FirstOrDefault(s => s.Topic.Equals(topic, StringComparison.InvariantCultureIgnoreCase) && 
                s.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
