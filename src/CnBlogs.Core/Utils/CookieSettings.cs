using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CnBlogs.Core.Utils
{
    public class CookieSettings : ICookieSettings
    {
        public string CookiePublicKeyJson { get; private set; }
        public string CookiePrivateKeyJson { get; private set; }

        public double ExpiresValue { get; set; }

        public CookieSettings()
        {
            var configration = new ConfigurationBuilder().AddJsonFile("appGlobal.json").Build();
            ExpiresValue = double.Parse(configration["ExpiresValue"]);
            var section = configration.GetSection("CookiePrivateKey");
            CookiePrivateKeyJson = "{\"D\":\"" + section["D"]
                + "\",\"DP\":\"" + section["DP"]
                + "\",\"DQ\":\"" + section["DQ"]
                + "\",\"Exponent\":\"" + section["Exponent"]
                + "\",\"InverseQ\":\"" + section["InverseQ"]
                + "\",\"Modulus\":\"" + section["Modulus"]
                + "\",\"P\":\"" + section["P"]
                + "\",\"Q\":\"" + section["Q"]
                + "\"}";
            CookiePublicKeyJson = "{\"D\":null,\"DP\":null,\"DQ\":null,\"Exponent\":\"" + section["Exponent"]
                + "\",\"InverseQ\":null,\"Modulus\":\"" + section["Modulus"]
                + "\",\"P\":null,\"Q\":null}";
        }
    }
}
