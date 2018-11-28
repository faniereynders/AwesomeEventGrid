using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Hangfire;
using Hangfire.Server;
using Hangfire.Storage;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication23.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly EventHandler eventHandler;
        private readonly ITopicsRepository topicsRepository;
        private readonly IMapper mapper;

        public TopicsController(EventHandler eventHandler, ITopicsRepository topicsRepository, IMapper mapper)
        {
            this.eventHandler = eventHandler;
            this.topicsRepository = topicsRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var topics = topicsRepository.GetAll();
            var model = mapper.Map<IEnumerable<TopicModel>>(topics);

            return Ok(model);
        }

        [HttpGet("{name}", Name = TopicModel.RouteName)]
        public IActionResult GetByName(string name)
        {
            var topic = topicsRepository.FindByName(name);

            if (topic == null)
            {
                return NotFound($"Topic '{name}' not found.");
            }
            var model = mapper.Map<TopicModel>(topic);

            return Ok(model);
        }

        [HttpPost("{name}")]
        public IActionResult Post([FromBody] EventModel[] events, string name)
        {
            if (topicsRepository.FindByName(name) == null)
            {
                ModelState.AddModelError("name", "Topic does not exists");
                return BadRequest(ModelState);
            }
            eventHandler.Handle(name, events);

            return Accepted();
        }
        [HttpPost]
        public IActionResult Create(TopicModel topicModel)
        {
            if (topicsRepository.FindByName(topicModel.Name) != null)
            {
                ModelState.AddModelError("name", "Topic already exists");
                return BadRequest(ModelState);
            }

            var topic = mapper.Map<Topic>(topicModel);
            var newTopic = topicsRepository.Add(topic);

            topicModel = mapper.Map<TopicModel>(newTopic);
            return CreatedAtAction(nameof(GetByName), new { name = topicModel.Name }, topicModel);
        }



    }



}
