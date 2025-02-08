
using System.Threading.Tasks;
using BraviaControlLib;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace BraviaTestSuite
{
    [TestFixture]
    public class BraviaDisplayTest
    {
        private BraviaDisplay _display;

        [SetUp]
        public void Setup()
        { _display = new BraviaDisplay("8eba0691-b724-4f86-b68c-3d55c8298178.mock.pstmn.io", "12345") { IsTestMode = false };
            
        }

        [Test]
            public async Task GetPowerAsync_ReturnFalse_WhenServerUnavailable()
            {
                var powerStatus = await _display.GetPowerAsync();
                Assert.That(powerStatus, Is.EqualTo("active").Or.EqualTo("standby"),
                    "Expected power status to be either 'active' or 'standby'");
 

            }

        [Test]
        public Task SetInputAsync_InvalidInput_LogsErrorAndReturnGracefully()
        {
            const string invalidInput = "HDMI 99";
            Assert.DoesNotThrowAsync(async () => await _display.SetInputAsync(invalidInput));
            return Task.CompletedTask;
        }
        [Test]
        public Task SetPowerOnAsync_LogErrorAndReturnGracefully()
        {
            Assert.DoesNotThrowAsync(async () => await _display.SetPowerAsync(true));
            return Task.FromResult(Task.CompletedTask);
        }

    }

}