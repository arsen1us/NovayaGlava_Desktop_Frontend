﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovayaGlava_Desktop_Frontend.Utilities
{
    public static class AppConfiguration
    {
        private static IConfiguration _configuration;

        static AppConfiguration()
        {
            //_configuration = new ConfigurationBuilder()
        }
        public static IConfiguration Configuration { get => _configuration; }


    }
}
