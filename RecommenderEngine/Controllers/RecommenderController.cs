using Microsoft.AspNetCore.Mvc;

namespace RecommenderApi.Controllers
{
    public class RecommenderController : Controller
    {
        [HttpGet]
        [Route("api/getRecommendations")]
        public IEnumerable<int> GetRecommendations(int userId)
        {
            return RecommenderUtils.Engine.RecommenderEngine.GetRecommendations(userId);
        }
    }
}
