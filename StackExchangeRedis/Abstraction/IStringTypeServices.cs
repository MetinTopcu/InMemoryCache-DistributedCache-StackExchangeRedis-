using StackExchangeRedis.DTOs;

namespace StackExchangeRedis.Abstraction
{
    public interface IStringTypeServices
    {
        Task Add(StringTypeDTO stringTypeDTO);
        Task<string> GetCacheById(string id);
        Task<string> Example();
    }
}
