using StackExchangeRedis.DTOs;
using StackExchangeRedis.Services;

namespace StackExchangeRedis.AppServices
{
    public class StringTypeServices
    {
        private readonly RedisServices _redisService;

        public StringTypeServices(RedisServices redisService)
        {
            _redisService = redisService;
        }

        public async Task<object> Add(StringTypeDTO stringTypeDTO)
        {
            var db = _redisService.GetDb(1);

            db.StringSet("name", stringTypeDTO.Name);
            db.StringSet("number", stringTypeDTO.Number);

            return true;
        }
    }
}
