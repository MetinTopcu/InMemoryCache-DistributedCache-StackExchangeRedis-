using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchangeRedis.Services;

namespace StackExchangeRedis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HashTypeController : ControllerBase // Veriler Dictionary olarak tutulur
    {
        private readonly RedisServices _redisService;

        public string hashKey { get; set; } = "sozluk";
        public HashTypeController(RedisServices redisService)
        {
            _redisService = redisService;
        }

        [HttpPost]
        public IActionResult Add(string name, string value)
        {
            var db = _redisService.GetDb(1);

            db.HashSet(hashKey, name, value);

            if (!db.KeyExists(hashKey)) // daha vakit yoksa 5 dk eklicek ama diyelim daha önceden 5 dk ekledik ve o 5 dk dan 3 dk kaldı ozaman buraya girip 5 dk daha eklemez ama her girdiğimizde 5 dk eklemek istersek bu if i kaldırırız ve her method çalıştığında aşağıdaki kod 5 dk ekler.
            {
                db.KeyExpire(hashKey, DateTime.Now.AddMinutes(5)); //5 dk ekler
            }
            return Ok();
        }
        [HttpGet]
        public IActionResult Get()
        {
            var db = _redisService.GetDb(1);

            Dictionary<string?,string?> namesList = new Dictionary<string?, string?>();
            if (db.KeyExists(hashKey))
            {
                //db.HashGet(hashKey, "ad"); // tek data almak için kullanılan method.
                db.HashGetAll(hashKey).ToList().ForEach(x =>
                {
                    namesList.Add(x.Name, x.Value);
                });
            }

            return Ok(namesList);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string name)
        {
            var db = _redisService.GetDb(1);

            await db.HashDeleteAsync(hashKey, name);

            return Ok();
        }
    }
}
