using InMemoryCaching.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryCaching.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private IMemoryCache _memoryCache;
        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpPost]
        public IActionResult CreateTimeAbsoluteWithSliding()
        {
            //1. yol memoryda cache var mı yok mu diye
            if (string.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            {
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions(); //sadece absolute

                options.AbsoluteExpiration = DateTime.Now.AddSeconds(30); //30 sn lik cacheledik

                _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options); // cache oluşturuyor
            }
            // ****************************************
            //2. yol memoryda cache var mı yok mu diye (best pratic asıl yapı burada )
            if (!_memoryCache.TryGetValue("zaman", out string zamancache)) // out tüm verileri alabilmek için
            {
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions(); // absolute with sliding

                options.AbsoluteExpiration = DateTime.Now.AddMinutes(30); //30 dk lık cacheledik

                options.SlidingExpiration = TimeSpan.FromSeconds(10); // 10 sn lik cacheledik

                options.Priority = CacheItemPriority.High; // bu data benim için önemli cache dolarsa en son sil

                options.RegisterPostEvictionCallback((key, value, reason, state) =>
                {
                    _memoryCache.Set("callback", $"{key}->{value} => sebep: {reason}");
                }); //cachenin neden silindğini görüyoruz. absolute time mi doldu yoksa cache doldu diye mi

                _memoryCache.Set<string>("zaman", DateTime.Now.ToString(),options); // cache oluşturuyor

                Product p = new Product { Id = 1, Name = "Kalem", Price = 200 };
                _memoryCache.Set<Product>("product:1", p); // seriliaz işlemi gerçekleştirmeden direk gönderiyoruz kendisi otomatik gerçekleştiriyorz. Redis de biz manuel olarak yapacağız
            }
            
            return Ok();
        }
        [HttpPost]
        public IActionResult CreateTimeSliding()
        {
            //1. yol memoryda cache var mı yok mu diye
            if (string.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            {
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions(); // sadece sliding

                options.SlidingExpiration = TimeSpan.FromSeconds(10); // 10 sn lik cacheledik

                _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options); // cache oluşturuyor
            }

            //2. yol memoryda cache var mı yok mu diye (best pratic asıl yapı burada )
            if (!_memoryCache.TryGetValue("zaman", out string zamancache)) // out tüme verileri alabilmek için
            {
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();

                options.SlidingExpiration = TimeSpan.FromSeconds(10); // 10 sn lik cacheledik

                _memoryCache.Set<string>("zaman", DateTime.Now.ToString(), options); // cache oluşturuyor
            }

            return Ok();
        }

        [HttpGet]
        public IActionResult Get() // cacheyi alıyoruz
        {
            _memoryCache.Get<string>("zaman");

            return Ok();
        }
        [HttpGet]
        public IActionResult TryGetValue() // cacheyi alıyoruz cache varsa TryGetValue metodu ile zamanCache isimli değişkene değerler atıldı.
        {
            _memoryCache.TryGetValue("zaman", out string zamanCache);
            _memoryCache.TryGetValue("callback", out string callback);
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete() 
        {
            _memoryCache.Remove("zaman");

            return Ok();
        }
        [HttpPost]
        public IActionResult GetIfNoCreate() // almaya çalış yoksa oluştur
        {
            _memoryCache.GetOrCreate<string>("zaman",entry =>
            {
                return DateTime.Now.ToString();
            });

            return Ok();
        }
    }
}
