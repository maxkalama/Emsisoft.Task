using Emsisoft.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Emsisoft.HashesService.Tests
{
    public class HashServiceTests
    {
        public IHashesService _service;

        [TestMethod]
        public void ShouldNotReturnNullHash()
        {
            EmsisoftHash hash = _service.GetRandomHash();
            Assert.IsNotNull(hash.Hash);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("2022-08-12")]
        public void ShouldReturnHashWithCorrectDate(string dateString)
        {
            DateOnly.TryParse(dateString, out DateOnly date);
            EmsisoftHash hash = _service.GetRandomHash(date);

            Assert.AreEqual(date, hash.Date);
        }

        [TestMethod]
        [DataRow(null)]
        [DataRow("2022-08-12")]
        public void ShouldReturnDifferentHash(string dateString)
        {
            DateOnly.TryParse(dateString, out DateOnly date);
            EmsisoftHash hash1 = _service.GetRandomHash(date);
            EmsisoftHash hash2 = _service.GetRandomHash(date.AddDays(1));

            Assert.IsFalse(hash1.Hash.SequenceEqual(hash2.Hash));
        }

        [TestMethod]
        public void ShouldNotChangeWhenConverted()
        {
            var hash = _service.GetRandomHash();
            var hashBytes = _service.ToBinary(hash);
            var convertedHash = _service.FromBinary(hashBytes);
            Assert.IsTrue(hash.Hash.SequenceEqual(convertedHash.Hash));
            Assert.AreEqual(hash.Date, convertedHash.Date);
        }
    }
}
