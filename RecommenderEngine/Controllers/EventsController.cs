using Microsoft.AspNetCore.Mvc;
using Recommender.Models;
using RecommenderUtils.DAL;

namespace RecommenderEngine.Controllers
{
    public class EventsController : ControllerBase
    {
        [HttpPost]
        [Route("api/registerEvent")]
        public IActionResult RegisterEvent(Event evt)
        {
            EventsRepository.SaveEvent(evt);
            RecommenderUtils.Engine.RecommenderEngine.UpdateUser(evt.UserId);

            return Ok();
        }
    }
}