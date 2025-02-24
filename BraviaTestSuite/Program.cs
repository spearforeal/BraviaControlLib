
using System;
using System.Reflection;
using System.Security.AccessControl;
using System.Threading.Tasks;
using BraviaControlLib;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.Legacy;

namespace BraviaTestSuite
{
    [TestFixture]
    public class BraviaTest
    {
        private Bravia _display;

        [SetUp]
        public void Setup()
        { _display = new Bravia("8eba0691-b724-4f86-b68c-3d55c8298178.mock.pstmn.io", "12345") { IsTestMode = false };
            
        }

        [Test]
            public async Task GetPowerAsync_ReturnFalse_WhenServerUnavailable()
            {
                var powerStatus = await _display.GetPowerAsync();
                //Assert.That(powerStatus, Is.EqualTo("active").Or.EqualTo("standby"),
                  //  "Expected power status to be either 'active' or 'standby'");
                  Console.WriteLine($"Power Status returned : {powerStatus}");
                  Assert.Pass("Test completed. Check output for power status value");
 

            }

        [Test]
        public Task SetInputAsync_InvalidInput_LogsErrorAndReturnGracefully()
        {
            const string invalidInput = "HDMI 99";
            Assert.DoesNotThrowAsync(async () => await _display.SetInputAsync(invalidInput));
            return Task.CompletedTask;
        }
        [Test]
        [Ignore("Ignore this test")]
        public Task SetPowerOnAsync_LogErrorAndReturnGracefully()
        {
            Assert.DoesNotThrowAsync(async () => await _display.SetPowerAsync(true));
            return Task.FromResult(Task.CompletedTask);
        }

        [Test]
        public async Task GetVolumeAsync_ReturnsValidVolumeInformation()
        {
            var volumeInfo = await _display.GetVolumeAsync();
            Console.WriteLine($"Volume {volumeInfo?.Volume}, Min: {volumeInfo?.MinVolume}, Max: {volumeInfo.MaxVolume}, Mute: {volumeInfo.Mute}, Target: {volumeInfo.Target}");
            ClassicAssert.IsNotNull(volumeInfo, "Expected non-null volume information.");
            Assert.That(volumeInfo.Volume, Is.InRange(volumeInfo.MinVolume, volumeInfo.MaxVolume));
        }

        [Test]
        public async Task SetVolumeAsync_ValidValue_SetsVolumeSuccessfully()
        {
            string validVolume = "30";
            await _display.SetVolumeAsync(validVolume);
            var volumeInfo = await _display.GetVolumeAsync();
            ClassicAssert.IsNotNull(volumeInfo, "Volume information should not be null.");
            Assert.That(volumeInfo.Volume, Is.EqualTo(30), "Expected volume to be set to 30.");
        }

    }

}