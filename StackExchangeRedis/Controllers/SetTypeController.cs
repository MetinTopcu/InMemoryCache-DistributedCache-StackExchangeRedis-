using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchangeRedis.Services;

namespace StackExchangeRedis.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SetTypeController : ControllerBase //SetType ın ListType dan farkı unique dir aynı değeri kaydetmez
    {
        private readonly RedisServices _redisService;
        private string listKey = "setnames";
        public SetTypeController(RedisServices redisService)
        {
            _redisService = redisService;
        }
        [HttpPost]
        public IActionResult Add(string name)
        {
            var db = _redisService.GetDb(1);

            db.SetAdd(listKey, name);

            if (!db.KeyExists(listKey)) // daha vakit yoksa 5 dk eklicek ama diyelim daha önceden 5 dk ekledik ve o 5 dk dan 3 dk kaldı ozaman buraya girip 5 dk daha eklemez ama her girdiğimizde 5 dk eklemek istersek bu if i kaldırırız ve her method çalıştığında aşağıdaki kod 5 dk ekler.
            {
                db.KeyExpire(listKey, DateTime.Now.AddMinutes(5)); //5 dk ekler
            }

            return Ok();
        }
        [HttpGet]
        public IActionResult Get()
        {
            var db = _redisService.GetDb(1);

            HashSet<string> namesList = new HashSet<string>();
            if (db.KeyExists(listKey))
            {
                db.SetMembers(listKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.ToString());
                });
            }

            return Ok(namesList);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string name)
        {
            var db = _redisService.GetDb(1);

            await db.SetRemoveAsync(listKey, name);

            return Ok();
        }
    }
}
