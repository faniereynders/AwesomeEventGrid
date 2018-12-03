using AutoMapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System;
using AwesomeEventGrid.Entities;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Microsoft.AspNetCore.Routing;
using AwesomeEventGrid.Abstractions.Options;
using AwesomeEventGrid.Abstractions.Models;
using AwesomeEventGrid.Abstractions;

namespace AwesomeEventGrid.Endpoints
{
    public class SubscriptionsCreateOrUpdateEndpoint : EndpointBase
    {

        public SubscriptionsCreateOrUpdateEndpoint(RequestDelegate next)
        {

        }

        

        public async Task InvokeAsync(HttpContext context, IHttpClientFactory httpClientFactory, ISubscriptionsRepository subscriptionsRepository, ITopicsRepository topicsRepository, IMapper mapper, IEventGridEventHandler eventGridEventHandler, IOptions<AwesomeEventGridOptions> options)
        {
            try
            {
                ModelState.Reset();

                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                {
                    var routeData = context.GetRouteData();
                    var topic = (string)routeData.Values["topic"];

                    if (topicsRepository.FindByName(topic) == null)
                    {
                        ModelState.AddError("name", "Topic with this name not found");
                        await NotFound(context);
                        return;
                    }

                    var subscriptionModel = JsonConvert.DeserializeObject<SubscriptionModel>(await reader.ReadToEndAsync(), options.Value.SerializerSettings);

                    var subscription = mapper.Map<Subscription>(subscriptionModel);

                    subscription.Topic = topic;
                    var client = httpClientFactory.CreateClient();

                    var validation = new SubscriptionValidationEventDataModel() { ValidationCode = Guid.NewGuid().ToString() };

                    var source = $"{options.Value.TopicsPath}/{topic}#validation";

                    var validationEvent = new EventModel
                    {
                        EventType = "subscriptions.validate",
                        Source = new Uri(source,UriKind.Relative ),
                        Data = validation,
                    };
                    var response = await client.PostAsJsonAsync(subscription.EndpointUrl, validationEvent);
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsAsync<SubscriptionValidationResponse>();
                    if (content.ValidationResponse != validation.ValidationCode)
                    {
                        ModelState.AddError("", "Endpoint responded with invalid validation code.");
                        await BadRequest(context);
                        return;
                    }


                    var newSubscription = subscriptionsRepository.AddOrUpdate(subscription);
                    var model = mapper.Map<SubscriptionModel>(newSubscription);

                    var link = "http://link"; // todo
                    await CreatedAt(context,link, model);
                    //    }
                    //    catch (HttpRequestException ex)
                    //    {
                    //        return BadRequest(ex.Message);
                    //    }

                }
            }
            catch (JsonException ex)
            {

                ModelState.AddError("", ex.Message);
                await BadRequest(context);

            }
            catch (HttpRequestException ex)
            {
                ModelState.AddError("", ex.Message);
                await BadRequest(context);
            }



        }


        }
}
