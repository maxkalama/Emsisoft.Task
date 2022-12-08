using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Emsisoft.HashesService.Tests
{
    [TestClass]
    public class Sha1HashesServiceTests : HashServiceTests
    {
        public Sha1HashesServiceTests()
        {
            base._service = new Sha1HashesService();
        }

        [TestMethod]
        public void ShouldReturnRandomDayOfCurrentMonth()
        {
            var hash = _service.GetRandomHash();
            Assert.AreEqual(hash.Date.Month, DateTime.UtcNow.Month);
        }
    }
}