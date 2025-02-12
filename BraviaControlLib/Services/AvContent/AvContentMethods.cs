using System;
using System.Threading.Tasks;

namespace BraviaControlLib
{
    public partial class Bravia
    {
        public async Task<InputInformation> GetInputAsync()
        {
            var command = Cmd(AvEnums.GetPlayingContentInfo);
            var response = await SendHttpCommandWithResponse(ApiServicesEnum.AvContent, command, "1.0", new { });
            if (!string.IsNullOrEmpty(response))
            {
                try
                {
                    var inputInformation = ApiResponse<InputInformation>.Parse(response);
                    if (inputInformation != null)
                    {
                        return inputInformation;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error parsing input information: {0}", ex.Message);
                }
            }

            Console.WriteLine("Failed to retrieve input information.");
            return null;
        }

        public async Task SetInputAsync(InputSrcEnums inputSource)
        {
            if (!InputSrcDict.TryGetValue(inputSource, out var inputUri))
            {
                Console.WriteLine("Input '{0}' is not valid.", inputSource);
                return;
            }


            var success = await SendHttpCommand(ApiServicesEnum.AvContent, Cmd(AvEnums.SetPlayContent), "1.0",
                new { uri = inputUri });
            Console.WriteLine(success ? "Input switched to {0} on {1}" : "Failed to switch input to {0} on {1}",
                inputSource, IpAddress);
        }

        public async Task SetInputAsync(string inputSource)
        {
            if (!Enum.TryParse<InputSrcEnums>(inputSource.Replace(" ", ""), true, out var parsedInput))
            {
                Console.WriteLine("Input '{0}' is not valid.", inputSource);
                return;
            }

            await SetInputAsync(parsedInput);
        }
    }
}