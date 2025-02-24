using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BraviaControlLib
{
    public partial class Bravia
    {
        public async Task SetPowerAsync(bool powerOn)
        {
            var command = Cmd(SysEnums.SetPowerStatus);


            if (await SendHttpCommand(ApiServicesEnum.System, command, "1.0", new { status = powerOn }))
            {
                var isPowerOn = await GetPowerAsync();
            }

            Console.WriteLine($"powerOn value: {powerOn}");
        }

        public async Task<string> GetPowerAsync()
        {
            var command = Cmd(SysEnums.GetPowerStatus);
            var response = await SendHttpCommandWithResponse(ApiServicesEnum.System, command, "1.0", new { });
            if (!string.IsNullOrEmpty(response))
            {
                try
                {
                    var powerResponse = ApiResponse<PowerStatus>.Parse(response);
                    if (powerResponse?.Result != null && powerResponse.Result.Length > 0)
                    {
                        Console.WriteLine($"PowerState = {powerResponse.Result[0][0].Status}");
                        return powerResponse.Result[0][0].Status;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}", ex.Message);
                }
            }

            return null;
        }
    }
}