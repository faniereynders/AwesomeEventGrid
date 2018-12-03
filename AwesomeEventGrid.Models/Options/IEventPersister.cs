using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeEventGrid.Abstractions.Options
{
    public interface IEventPersister
    {
        void ConfigureServices(IServiceCollection services);
        void ConfigureMiddleware(IApplicationBuilder applicationBuilder);
    }
}
