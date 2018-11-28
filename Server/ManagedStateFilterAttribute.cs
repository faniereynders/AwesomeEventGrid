using Hangfire.Common;
using Hangfire.States;
using System;
using System.Net.Http;

namespace WebApplication23
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
                    var handler = (EventHandler)activator.ActivateJob(typeof(EventHandler));
                   // var httpClientFactory = (IHttpClientFactory)serviceProvider.GetService(typeof(IHttpClientFactory));
                    var subscriber = (Subscription)context.BackgroundJob.Job.Args[0];
                    var @event = (EventModel)context.BackgroundJob.Job.Args[1];

                    var errorEvent = new EventModel
                    {
                        Data = @event,
                        EventType = "nl.t-mobilethuis.events.failed",
                        Source = @event.Source
                    };

                   handler.Handle(subscriber.Topic, errorEvent);
                }


                //job has failed, stop application here
            }
        }

        
    }



}
