using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchangeRedis.Services;

namespace StackExchangeRedis.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ListTypeController : ControllerBase
    {

        private readonly RedisServices _redisService;
        private string listKey = "names";
        public ListTypeController(RedisServices redisService)
        {
            _redisService = redisService;
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            var db = _redisService.GetDb(1);

            db.ListRightPush(listKey, name);

            return Ok();
        }
        [HttpGet]
        public IActionResult Get()
        {
            var db = _redisService.GetDb(1);

            List<string> namesList = new List<string>();
            if (db.KeyExists(listKey))
            {
                db.ListRange(listKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString());
                });
            }

            return Ok(namesList);
        }
        [HttpDelete]
        public IActionResult Delete(string name)
        {
            var db = _redisService.GetDb(1);

            db.ListRemoveAsync(listKey, name).Wait();

            return Ok();
        }
    }
}
