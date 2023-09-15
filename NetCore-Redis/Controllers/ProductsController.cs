using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using NetCore_Redis.Models;
using Newtonsoft.Json;
using System.Text;
using System.Xml.Linq;

namespace NetCore_Redis.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(IDistributedCache distributedCache, ILogger<ProductsController> logger)
        {
            _distributedCache = distributedCache;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheOptions = new DistributedCacheEntryOptions();
            cacheOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(5);

            Product product = new Product()
            {
                Id = 1,
                Name = "Bilgisayar",
                Price = 15000
            };
            string jsonProduct = JsonConvert.SerializeObject(product);
            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);
            await _distributedCache.SetAsync("Product:1", byteProduct, cacheOptions);
            //await _distributedCache.SetStringAsync("Product:1", jsonProduct,cacheOptions);

            if (_distributedCache is not null)
            {
                _logger.LogInformation($"{product.Name} Adlı Ürün Redis Sunucusuna Eklenmiştir.");
            }
            else
            {
                _logger.LogInformation("Ürün Eklenemedi");
            }

            return View();
        }

        public IActionResult Show()
        {
            Byte[] byteProduct = _distributedCache.Get("Product:1");
            string name = Encoding.UTF8.GetString(byteProduct);
            Product p =JsonConvert.DeserializeObject<Product>(name);
            
            if (name is not null)
            {
                _logger.LogInformation($"{p.Name} Adlı Ürün Gösterildi");
                ViewBag.Name = p.Name;
            }
            else
            {
                _logger.LogInformation("Redis Sunucusunda gösterilecek Ürün bulunamadı");
            }
            return View();
        }
        public IActionResult Remove() 
        {
            _distributedCache.Remove("Product:1");
            _logger.LogInformation("Ürün Kaldırıldı");
            return View();
        }
        public async Task<IActionResult> ImageCache()
        {
            DistributedCacheEntryOptions entryOptions = new DistributedCacheEntryOptions();

            entryOptions.AbsoluteExpiration = DateTime.UtcNow.AddMinutes(5);
            
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/272f2a98b4ce3f4214605d01be019696.jpg");
            byte[] byteImage = System.IO.File.ReadAllBytes(path);
            await _distributedCache.SetAsync("Image", byteImage,entryOptions);

            if (_distributedCache is not null)
            {
                _logger.LogInformation("Resim Eklendi");
            }
            else
            {
                _logger.LogInformation("Resim Eklenemedi");
            }
            return View();
        }
    }
}
