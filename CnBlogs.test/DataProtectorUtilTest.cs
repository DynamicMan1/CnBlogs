using CnBlogs.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CnBlogs.test
{
    public class DataProtectorUtilTest
    {
        [Fact]
        public void EncryptAndDecryptString()
        {
            // Arrange
            var eus = new DataProtectorUtil();
            string str = "FE37713BC06BE69E9B56B0B0F23183178A6F3F93F3758059C892A4B85EA305C9";

            // Act
            string enStr = eus.EncryptString(str);
            string deStr = eus.DecryptString(enStr);

            // Assert
            Assert.Equal(deStr, str);
        }

        
        public void SetPrivateKeyDecryptString()
        {
            // Arrange
            var eus = new DataProtectorUtil();
            string planStr = "FE37713BC06BE69E9B56B0B0F23183178A6F3F93F3758059C892A4B85EA305C9";
            string str = "QMMDD94xcvyJ8xKtoFDwOhtMY/CO0GFIgFCGzj79nEhpACcXqilVkeQeKJ5qJKxC9Joam6WWilKeRZunPoSgUGsf1IbP7Ty4FuG+8Sd2do8Qv178cTx2zkrbk93VB4hEl1YrxZL9tYun5qkYbnTtsTD8J4IZNa8q0fnqKQD4hI8=";
            eus.PrivateKeyJson = "{\"D\":\"K8h25PY9XvhcMWosIAhN6iaNZDGzj/jzrpOAZgNW0mnTmW1mzpk9h8XyeQyLZc1CPH961wU5Oi8EjpUwd4Tb9eN5HjHPi3W8jK7fNaC6cCdvOhEVJNkXL4YuvpCX1Lubp3iAyd1ZCg+5XbdYEELIbrHs+0ryzyd75A5Apld8qiU=\",\"DP\":\"kgCoGhJVWUBafyVh7L1ulejzLosuJovrr6PIL5cErHFZuw1C6UdhQkbWkEEnq4McILxYhbWVnNolhDmNsGxl/Q ==\",\"DQ\":\"UWU+pR4Nlp1UkvYRFFirD9ycVlcdabdWx+GVljcLWvt5obXCux4NnoelZ40C1trxUSzBXAUq+kmNKAeZNdmplQ==\",\"Exponent\":\"AQAB\",\"InverseQ\":\"V9OdeyAejzF1yntZOz/MqMTozw5XQzFriF8pb06qtxj4PmU5xRiqkc1Fx+IhlRlQVsW5VHoWf/QvtdY/Vg9rzg==\",\"Modulus\":\"mrbjH3BCN3GKPgXG9FXa1Zwu4sJi5SAfPmnXl2x7yH4iH/fU7ydnIWAtQq0jlRcEa6kdJvlbcc2rj4dKzVcOYOm10L9+6JptFREikQx0SfF10z45BN1nHEs5BhV4L/H8RB+OtAQsEuExXe0gsr0OWnng+34sN7rYUgxjbdzTA7M=\",\"P\":\"xdc+aiOj+5rsBBXrRd7fT80mWzZbvQAiMOUKmUXdZBZI6LiR08swPqDXEFPiZ5RF89w0SQ5C4r5tChwzevw6bQ==\",\"Q\":\"yDIc4ip9q/trR4/6Qjs28MJrDif28EUtqpL4N054MHy15kjd+pbEYb01Wh8whC1ZSvBTZmuh14AGAp7XmuFinw==\"}";

            // Act
            string deStr = eus.DecryptString(str);

            // Assert
            Assert.Equal(deStr, planStr);
        }

        [Fact]
        public void Get()
        {
            // Arrange
            var eus = new DataProtectorUtil();
            string str1 = "hello,world!";
            string str2 = "hello,world!";

            // Act
            string fingerprint1 = eus.GetFingerprint(str1);
            string fingerprint2 = eus.GetFingerprint(str2);

            // Assert
            Assert.Equal(fingerprint1, fingerprint2);
        }
    }
}
