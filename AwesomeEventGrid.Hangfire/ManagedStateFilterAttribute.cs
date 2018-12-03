using AwesomeEventGrid.Abstractions;
using AwesomeEventGrid.Abstractions.Models;
using AwesomeEventGrid.Entities;
using Hangfire.Common;
using Hangfire.States;

namespace AwesomeEventGrid.Hangfire
{
    public class ManagedStateFilterAttribute : JobFilterAttribute, IElectStateFilter
    {
        private readonly HangfireActivator activator;

        public ManagedStateFilterAttribute(HangfireActivator activator)
        {
            this.activator = activator;
        }

        public void OnStateElection(ElectStateContext context)
        {
            var failedState = context.CandidateState as FailedState;
            if (failedState != null)
            {
                int retryCount = context.GetJobParameter<int>("RetryCount");

                if (retryCount == 3)
                {
                    var eventGridEventHandler = (IEventGridEventHandler)activator.ActivateJob(typeof(IEventGridEventHandler));
                   // var httpClientFactory = (IHttpClientFactory)serviceProvider.GetService(typeof(IHttpClientFactory));
                    var subscriber = (Subscription)context.BackgroundJob.Job.Args[0];
                    var @event = (EventModel)context.BackgroundJob.Job.Args[1];

                    var errorEvent = new EventModel
                    {
                        Data = @event,
                        EventType = "event.failed",
                        Source = @event.Source
                    };

                   eventGridEventHandler.HandleAsync(subscriber.Topic, errorEvent).Wait();
                }


                //job has failed, stop application here
            }
        }

        
    }



}
