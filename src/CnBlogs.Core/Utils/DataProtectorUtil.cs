using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CnBlogs.Core.Utils
{
    public class DataProtectorUtil : IDataProtectorUtil
    {
        private string _privateKeyJson;
        private string _publicKeyJson;

        public string PublicKeyJson
        {
            get
            {
                return _publicKeyJson;
            }
            set
            {
                _publicKeyJson = value;
                rsaParamsPublic = Newtonsoft.Json.JsonConvert.DeserializeObject<RSAParameters>(_publicKeyJson);
            }
        }
        public string PrivateKeyJson
        {
            get
            {
                return _privateKeyJson;
            }
            set
            {
                _privateKeyJson = value;
                rsaParamsPrivate = Newtonsoft.Json.JsonConvert.DeserializeObject<RSAParameters>(_privateKeyJson);
            }
        }

        private RSACryptoServiceProvider rsa;
        
        private RSAParameters rsaParamsPublic;
        private RSAParameters rsaParamsPrivate;

        public DataProtectorUtil()
        {
            rsa = new RSACryptoServiceProvider();
            rsaParamsPublic = rsa.ExportParameters(false);
            rsaParamsPrivate = rsa.ExportParameters(true);
            PublicKeyJson = Newtonsoft.Json.JsonConvert.SerializeObject(rsaParamsPublic);
            PrivateKeyJson = Newtonsoft.Json.JsonConvert.SerializeObject(rsaParamsPrivate);
        }

        public DataProtectorUtil(string publicKey, string privateKey)
        {
            rsa = new RSACryptoServiceProvider();
            rsaParamsPublic = Newtonsoft.Json.JsonConvert.DeserializeObject<RSAParameters>(publicKey);
            rsaParamsPublic = Newtonsoft.Json.JsonConvert.DeserializeObject<RSAParameters>(privateKey);
        }

        public string EncryptString(string str)
        {
            rsa.ImportParameters(rsaParamsPublic);
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            byte[] encryptBytes = rsa.Encrypt(bytes, false);
            return Convert.ToBase64String(encryptBytes);
        }

        public string DecryptString(string str)
        {
            rsa.ImportParameters(rsaParamsPrivate);
            byte[] encryptBytes = Convert.FromBase64String(str);
            byte[] decryptBytes = rsa.Decrypt(encryptBytes, false);
            return Encoding.UTF8.GetString(decryptBytes);
        }

        public string GetFingerprint(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            MD5 md5 = MD5.Create();
            byte[] result = md5.ComputeHash(bytes);
            return BitConverter.ToString(result).Replace("-", "");
        }
    }
}
