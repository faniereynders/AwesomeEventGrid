//using System.Collections.Generic;
//using AutoMapper;
//using AwesomeEventGrid.Entities;
//using AwesomeEventGrid.Infrastructure;
//using AwesomeEventGrid.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace AwesomeEventGrid.Controllers
//{


//    [Route("api/[controller]")]
//    [ApiController]
//    public class TopicsController : ControllerBase
//    {
//        private readonly DefaultEventGridEventHandler eventHandler;
//        private readonly ITopicsRepository topicsRepository;
//        private readonly IMapper mapper;

//        public TopicsController(DefaultEventGridEventHandler eventHandler, ITopicsRepository topicsRepository, IMapper mapper)
//        {
//            this.eventHandler = eventHandler;
//            this.topicsRepository = topicsRepository;
//            this.mapper = mapper;
//        }

//        [HttpGet]
//        public IActionResult GetAll()
//        {
//            var topics = topicsRepository.GetAll();
//            var model = mapper.Map<IEnumerable<TopicModel>>(topics);

//            return Ok(model);
//        }

//        [HttpGet("{name}", Name = Constants.Topics.RouteName)]
//        public IActionResult GetByName(string name)
//        {
//            var topic = topicsRepository.FindByName(name);

//            if (topic == null)
//            {
//                return NotFound($"Topic '{name}' not found.");
//            }
//            var model = mapper.Map<TopicModel>(topic);

//            return Ok(model);
//        }

//        [HttpPost("{name}")]
//        public IActionResult Post([FromBody] EventModel[] events, string name)
//        {
//            if (topicsRepository.FindByName(name) == null)
//            {
//                ModelState.AddModelError("name", "Topic does not exists");
//                return BadRequest(ModelState);
//            }
//            eventHandler.Handle(name, events);

//            return Accepted();
//        }
//        [HttpPost]
//        public IActionResult Create(TopicModel topicModel)
//        {
//            if (topicsRepository.FindByName(topicModel.Name) != null)
//            {
//                ModelState.AddModelError("name", "Topic already exists");
//                return BadRequest(ModelState);
//            }

//            var topic = mapper.Map<Topic>(topicModel);
//            var newTopic = topicsRepository.Add(topic);

//            topicModel = mapper.Map<TopicModel>(newTopic);
//            return CreatedAtAction(nameof(GetByName), new { name = topicModel.Name }, topicModel);
//        }



//    }



//}
