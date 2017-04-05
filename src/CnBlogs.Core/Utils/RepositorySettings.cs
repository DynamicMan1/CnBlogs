using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.Utils
{
    public class RepositorySettings : IRepositorySettings
    {
        public string ConnStr { get; private set; }

        public RepositorySettings()
        {
            var configration = new ConfigurationBuilder().AddJsonFile("appGlobal.json").Build();
            ConnStr = configration["ConnectionString"];
        }
    }
}
