using DistributedCacheRedisApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Text;

namespace DistributedCacheRedisApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private IDistributedCache _distributedCache;
        public ProductController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpPost]
        public IActionResult Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();

            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            _distributedCache.SetString("name", "Fatih", cacheEntryOptions);

            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> JsonConversion() //Json çevirme Serialize
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();

            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(30);

            Product product = new Product { Id = 2, Name = "silgi2", Price = 102 };

            string jsonProduct = JsonConvert.SerializeObject(product);

            await _distributedCache.SetStringAsync("product:2", jsonProduct, cacheEntryOptions);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> BinaryConversion() //Binary çevirme(binary çevirme içerisinde json çevirme de geçiyor)
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();

            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            Product product = new Product { Id = 3, Name = "silgi3", Price = 103 };

            string jsonProduct = JsonConvert.SerializeObject(product);

            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);

            await _distributedCache.SetAsync("product:3", byteProduct, cacheEntryOptions);

            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            string name = _distributedCache.GetString("name");

            return Ok(name);
        }
        [HttpGet]
        public IActionResult JsonGet()
        {
            string jsonProduct = _distributedCache.GetString("product:2");

            Product p = JsonConvert.DeserializeObject<Product>(jsonProduct);

            return Ok(p);
        }
        [HttpGet]
        public IActionResult ByteGet()
        {
            Byte[] byteProduct = _distributedCache.Get("product:3");

            string jsonProduct = Encoding.UTF8.GetString(byteProduct);

            Product p = JsonConvert.DeserializeObject<Product>(jsonProduct);

            return Ok(p);
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            _distributedCache.Remove("name");

            return Ok();
        }
    }
}
