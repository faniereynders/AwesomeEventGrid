using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication23.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionsRepository subscriptionsRepository;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMapper mapper;

        public SubscriptionsController(ISubscriptionsRepository subscriptionsRepository, IHttpClientFactory httpClientFactory, IMapper mapper )
        {
            this.subscriptionsRepository = subscriptionsRepository;
            this.httpClientFactory = httpClientFactory;
            this.mapper = mapper;
        }
        [HttpGet("{topic}/{name}",Name = nameof(GetById))]
        public IActionResult GetById(string topic, string name)
        {
            var sub = this.subscriptionsRepository.GetAll().FirstOrDefault(s=>s.Name == name);
            if (sub == null)
            {
                return NotFound();
            }

            var model = mapper.Map<SubscriptionModel>(sub);

            return Ok(model);
        }
        [HttpGet("{topic}", Name = nameof(GetAllForTopic))]
        public IActionResult GetAllForTopic(string topic)
        {
            var subs = this.subscriptionsRepository.GetAll().Where(s => s.Topic == topic);
            var model = mapper.Map<IEnumerable<SubscriptionModel>>(subs);

            return Ok(model);
        }
        [HttpGet(Name = nameof(GetAll))]
        public IActionResult GetAll()
        {
            var subs = this.subscriptionsRepository.GetAll();
            var model = mapper.Map<IEnumerable<SubscriptionModel>>(subs);

            return Ok(model);
        }
        [HttpPut("{topic}")]
        public async Task<IActionResult> CreateOrUpdate(string topic, [FromBody]SubscriptionModel subscriptionModel)
        {
            try
            {
                //validate
                //...todo

                var subscription = mapper.Map<Subscription>(subscriptionModel);

                subscription.Topic = topic;
                var client = httpClientFactory.CreateClient();

                var validation = new SubscriptionValidationEventData() { ValidationCode = Guid.NewGuid().ToString() };

                var source = $"{this.Url.Link(TopicModel.RouteName,new { name = topic })}#validation";

                var validationEvent = new EventModel
                {
                    EventType = "nl.t-mobilethuis.subscriptions.validate",
                    Source = new Uri(source),
                    Data = validation,
                };
                var response = await client.PostAsJsonAsync(subscription.EndpointUrl, validationEvent);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsAsync<SubscriptionValidationResponse>();
                if (content.ValidationResponse != validation.ValidationCode)
                {
                    return BadRequest("Endpoint responded with invalid validation code.");
                }


                var newSubscription = this.subscriptionsRepository.AddOrUpdate(subscription);
                var model = mapper.Map<SubscriptionModel>(newSubscription);

                var link = Url.Link(nameof(GetById), new { name = model.Name, topic });
                return Created(link, model);
            }
            catch (HttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            

        }
    }
}