using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchangeRedis.Services;

namespace StackExchangeRedis.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StringTypeController : ControllerBase
    {
        private readonly RedisServices _redisService;
        public StringTypeController(RedisServices redisService)
        {
            _redisService = redisService;
        }
        [HttpPost]
        public IActionResult Post()
        {
            var db = _redisService.GetDb(1);

            db.StringSet("name", "Metocan");
            db.StringSet("sever", "Seversin");
            db.StringSet("sayi", 100);

            return Ok();
        }
        [HttpGet]
        public IActionResult Get()
        {
            var db = _redisService.GetDb(1);

            var value = db.StringGet("name");

            if (value.HasValue) // böyle bir data var mı kontrolü
            {
                return Ok(value.ToString());
            }
            return Ok("Not Found");
        }
        [HttpGet]
        public IActionResult DbMethodsTrying()
        {
            var db = _redisService.GetDb(1);

            db.StringIncrement("sayi", 15); // 100 dü sayi degeri sayi degerine 15 ekledik 115 oldu
            db.StringDecrementAsync("sayi", 10); // 10 düşür
            db.StringGetRange("name", 0,3); // ilk 4 karakteri aldık
            db.StringLength("name"); // length ini aldık


            var value = db.StringGet("sayi");

            if (value.HasValue) // böyle bir data var mı kontrolü
            {
                return Ok(value.ToString());
            }
            return Ok("Not Found");
        }
    }
}
