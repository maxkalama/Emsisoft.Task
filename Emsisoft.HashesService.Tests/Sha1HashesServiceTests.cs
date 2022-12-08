using Emsisoft.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace Emsisoft.HashesService.Tests
{
    [TestClass]
    public class Sha1HashesServiceTests : HashServiceTests
    {
        public Sha1HashesServiceTests()
        {
            base._service = new Sha1HashesService();
        }
    }
}