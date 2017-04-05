using CnBlogs.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CnBlogs.test
{
    public class EmailUtilSettingsTest
    {
        [Fact]
        public void GetHost()
        {
            // Arrange
            var eus = new EmailUtilSettings();

            // Act

            // Assert
            Assert.Equal(eus.Host, "smtp.test.com");
        }

        [Fact]
        public void GetSender()
        {
            // Arrange
            var eus = new EmailUtilSettings();

            // Act

            // Assert
            Assert.Equal(eus.Sender, "hello@test.com");
        }

        [Fact]
        public void GetPassword()
        {
            // Arrange
            var eus = new EmailUtilSettings();

            // Act

            // Assert
            Assert.Equal(eus.Password, "test");
        }
    }
}
