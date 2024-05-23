using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovayaGlava_Desktop_Frontend.CacheHandlers
{
    public class UserIdHandler
    {
        private IDistributedCache _cache;

        //public static string KeyWord { get; set; }

        public UserIdHandler()
        {

            var serviceCollection = new ServiceCollection()
                    .AddStackExchangeRedisCache(options =>
                    {
                        options.Configuration = "localhost";
                    })
                    .BuildServiceProvider();
            _cache = serviceCollection.GetService<IDistributedCache>();
        }

        public IDistributedCache Cache { get { return _cache; } set { _cache = value; } }

        public string GetFromCache()
        {

            string userId = _cache.GetString("userId");
            if (string.IsNullOrEmpty(userId))
                return null;
            return userId;
        }

        public void SetToCache(string userId)
        {
            _cache.SetString("userId", userId, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
            });
        }

        public void DeleteFromCache()
        {
            _cache.Remove("userId");
        }
    }
}
