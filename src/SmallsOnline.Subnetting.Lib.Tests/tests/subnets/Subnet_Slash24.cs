using System.Net;

using NUnit.Framework;
using SmallsOnline.Subnetting.Lib.Models;

namespace SmallsOnline.Subnetting.Lib.Tests
{
    [TestFixture]
    public class Subnet_Test_Slash24
    {
        private Subnet subnet;

        [SetUp]
        public void Setup()
        {
            subnet = new(
                netAddress: IPAddress.Parse("192.168.0.0"),
                cidrNotation: 24
            );
        }

        [Test]
        public void Test_TotalAddresses()
        {
            Assert.AreEqual(
                (double)256,
                subnet.TotalAddresses
            );
        }

        [Test]
        public void Test_UsableAddresses()
        {
            Assert.AreEqual(
                (double)254,
                subnet.UsableAddresses
            );
        }

        [Test]
        public void Test_SubnetMask()
        {
            Assert.AreEqual(
                IPAddress.Parse("255.255.255.0"),
                subnet.SubnetMask
            );
        }
    }
}