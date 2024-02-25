using StackExchangeRedis.Abstraction;
using StackExchangeRedis.DTOs;
using StackExchangeRedis.Services;
using System.Collections.Generic;
using System.Text.Json;
using System.Xml.Linq;

namespace StackExchangeRedis.AppServices
{
    public class StringTypeServices: IStringTypeServices
    {
        private readonly RedisServices _redisService;

        public StringTypeServices(RedisServices redisService)
        {
            _redisService = redisService;
        }

        public async Task Add(StringTypeDTO stringTypeDTO)
        {
            var db = _redisService.GetDb(1);

            await db.StringSetAsync(stringTypeDTO.Id.ToString(), JsonSerializer.Serialize(stringTypeDTO));

            if (!db.KeyExists(stringTypeDTO.Id.ToString())) // Key'e ait veri yoksa 5 dk olarak tanımlar. Hala cache de süresi dolmayan veri varsa buraya girmez.
            {
                db.KeyExpire(stringTypeDTO.Id.ToString(), DateTime.Now.AddMinutes(5)); //5 dk ekler
            }
        }

        public async Task<string> GetCacheById(string id)
        {
            var db = _redisService.GetDb(1);

            var value = await db.StringGetAsync(id);

            if (value.HasValue) // böyle bir data var mı kontrolü
            {
                return value.ToString();
            }
            return "Not Found";
        }

        public async Task<string> Example()
        {
            var db = _redisService.GetDb(1);

            await db.StringIncrementAsync("sayi", 15); // 100 dü sayi degeri sayi degerine 15 ekledik 115 oldu
            await db.StringDecrementAsync("sayi", 10); // 10 düşür
            await db.StringGetRangeAsync("name", 0, 3); // ilk 4 karakteri aldık
            await db.StringLengthAsync("name"); // length ini aldık


            var value = await db.StringGetAsync("sayi");

            if (value.HasValue) // böyle bir data var mı kontrolü
            {
                return value.ToString();
            }
            return "Not Found";
        }
    }
}
