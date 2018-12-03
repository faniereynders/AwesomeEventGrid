using AwesomeEventGrid.Abstractions.Models;
using System.Threading.Tasks;

namespace AwesomeEventGrid.Abstractions
{
    public interface IEventGridEventHandler
    {
        Task HandleAsync(string topic, params EventModel[] events);
    }
}