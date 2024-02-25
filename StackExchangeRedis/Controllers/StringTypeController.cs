using Microsoft.AspNetCore.Mvc;
using StackExchangeRedis.Abstraction;
using StackExchangeRedis.DTOs;

namespace StackExchangeRedis.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StringTypeController : ControllerBase
    {
        private readonly IStringTypeServices _stringTypeServices;
        public StringTypeController(IStringTypeServices stringTypeServices)
        {
            _stringTypeServices = stringTypeServices;
        }
        [HttpPost]
        public async Task<IActionResult> Post(StringTypeDTO stringTypeDTO)
        {
            await _stringTypeServices.Add(stringTypeDTO);

            return Ok();
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var value = await _stringTypeServices.GetCacheById(id);

            return Ok(value.ToString());
        }
        [HttpGet]
        public async Task<IActionResult> DbMethodsUsing()
        {
            var value = await _stringTypeServices.Example();

            return Ok(value.ToString());
        }
    }
}
