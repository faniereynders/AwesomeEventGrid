using System.Threading.Tasks;
using AwesomeEventGrid.Abstractions.Models;

namespace AwesomeEventGrid.Abstractions
{
    public interface ISubscriberEventDispatcher
    {
        Task Dispatch(SubscriptionModel subscription, EventModel @event);
    }
}