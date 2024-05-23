using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.StackExchangeRedis;


namespace NovayaGlava_Desktop_Frontend.CacheHandlers
{
    public class JwtTokenHandler
    {
        IDistributedCache _cache;

        public JwtTokenHandler()
        {
            var serviceCollection = new ServiceCollection()
            .AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "localhost";
            }).BuildServiceProvider();

            _cache = serviceCollection.GetService<IDistributedCache>();
        }

        //Функция для получения токена из кэша
        public string GetFromCache()
        {
            string token = _cache.GetString("token");
            if (string.IsNullOrEmpty(token))
                return null;
            return token;
        }

        //Функция для занесения токена юзера в кэш
        //Сохраняю на 1 минуту токен юзера в кэше
        public void SetToCache(string jwtToken)
        {
            _cache.SetString("token", jwtToken, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
            });
        }

    }
}
