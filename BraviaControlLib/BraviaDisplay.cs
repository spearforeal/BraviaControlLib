using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Task = System.Threading.Tasks.Task;

namespace BraviaControlLib
{
    public partial class BraviaDisplay
    {
        public bool IsTestMode { get; set; }
        private string IpAddress { get; set; }

        private string Psk { get; set; }


        public BraviaDisplay(string ipAddress, string psk)
        {
            IpAddress = ipAddress;
            Psk = psk;
        }

        private static readonly Dictionary<ApiServicesEnum, string> apiServiceDict =
            new Dictionary<ApiServicesEnum, string>
            {
                { ApiServicesEnum.System, "system" },
                { ApiServicesEnum.AvContent, "avContent" },
                { ApiServicesEnum.Audio, "audio" },
            };

        private string BuildUrl(ApiServicesEnum service)
        {
            var serviceStr = apiServiceDict[service];
            var url = IsTestMode ? $"http://{IpAddress}/{serviceStr}" : $"http://{IpAddress}/sony/{serviceStr}";

            //Console.WriteLine($"[BuildUrl] Generated URL: {url}");
            return url;
        }

        private string BuildPayload(KeyValuePair<int, string> command, string version, object parameters)
        {
            var payload = new
                { id = command.Key, method = command.Value, version = version, @params = new[] { parameters } };
            var jsonPayload = JsonConvert.SerializeObject(payload);
            //Console.WriteLine($"[BuildPayload] Payload: {jsonPayload}");
            return JsonConvert.SerializeObject(payload);
        }

        //Power Commands
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
                    var powerStatus = ApiResponse<PowerStatus>.Parse(response);
                    Console.WriteLine($"Powerstatus response: {powerStatus}");
                    if (powerStatus != null)
                    {
                        Console.WriteLine($"Powerstate = {powerStatus.Status}");
                        return powerStatus.Status;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}", ex.Message);
                }
            }

            return null;
        }

        //Input
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

        private readonly Dictionary<string, string> _inputs = new Dictionary<string, string>
        {
            { "HDMI 1", "extInput:hdmi?port=1" }, { "HDMI 2", "extInput:hdmi?port=2" },
            { "HDMI 3", "extInput:hdmi?port=3" }, { "HDMI 4", "extInput:hdmi?port=4" },
            { "Comp 1", "extInput:component?port=1" }, { "Comp 2", "extInput:component?port=2" },
            { "Comp 3", "extInput:component?port=3" }, { "Comp 4", "extInput:component?port=4" },
            { "Wifi", "extInput:widi?port=1" }, { "CEC 1", "extInput:cec?type=player@port=1" },
            { "CEC 2", "extInput:cec?type=player@port=2" }, { "CEC 3", "extInput:cec?type=player@port=3" }
        };

        public async Task SetInputAsync()
        {
            if (!_inputs.TryGetValue(inputName, out var inputUri))
            {
                Console.WriteLine("Input '{0}' is not valid.", inputName);
                return;
            }

            var success = await SendHttpCommand(ApiServicesEnum.AvContent, Cmd(AvEnums.SetPlayContent), "1.0",
                new { uri = inputUri });
            Console.WriteLine(success ? "Input switched to {0} on {1}" : "Failed to switch input to {0} on {1}",
                inputName, IpAddress);
        }
        //Audio


        //HTTP
        private async Task<HttpWebRequest> CreateRequestAsync(ApiServicesEnum service,
            KeyValuePair<int, string> command, string version, object parameters)
        {
            try
            {
                var uri = BuildUrl(service);
                var payload = BuildPayload(command, version, parameters);
                //Console.WriteLine($"[CreateRequestAsync] Requested URL: {uri}");
                //Console.WriteLine($"[CreateRequestAsync] Requested Payload: {payload}");
                var request = WebRequest.CreateHttp(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("X-Auth-PSK", Psk);
                var bodyBytes = System.Text.Encoding.UTF8.GetBytes(payload);
                request.ContentLength = bodyBytes.Length;
                using (var streamWriter = new StreamWriter(await request.GetRequestStreamAsync()))
                {
                    await streamWriter.WriteAsync(payload);
                }

                return request;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"$Error: {ex.Message}");
                throw;
            }
        }

        private async Task<bool> SendHttpCommand(ApiServicesEnum service, KeyValuePair<int, string> command,
            string version, object parameters)
        {
            try
            {
                var request = await CreateRequestAsync(service, command, version, parameters);
                using (var response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("HTTP Request Error: {0}", ex.Message);
                return false;
            }
        }

        private async Task<string> SendHttpCommandWithResponse(ApiServicesEnum service,
            KeyValuePair<int, string> command, string version, object parameters)
        {
            try
            {
                var request = await CreateRequestAsync(service, command, version, parameters);
                using (var response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Accepted)
                    {
                        Console.WriteLine(response.StatusCode);
                        using (var streamReader = new StreamReader(response.GetResponseStream()))
                        {
                            return await streamReader.ReadToEndAsync();
                        }
                    }
                    else
                    {
                        Console.WriteLine("HTTP Request failed with status code: {0}", response.StatusCode);
                        return null; // Return null if the status code is not OK
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("HTTP Request Error: {0}", ex.Message);
                return null;
            }
        }


        public abstract class ApiResponse<T>
        {
            public T Data { get; set; }

            public static T Parse(string jsonResponse)
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(jsonResponse);
                }

                catch (JsonException ex)
                {
                    Console.WriteLine("Error parsing JSON: {0}", ex.Message);
                    throw;
                }
            }
        }

        public static readonly Dictionary<InputSrcEnums, string> InputSrcDict = new Dictionary<InputSrcEnums, string>
        {
            { InputSrcEnums.Hdmi1, Hdmi1Uri },
            { InputSrcEnums.Hdmi2, Hdmi2Uri },
            { InputSrcEnums.Hdmi3, Hdmi3Uri },
            { InputSrcEnums.Hdmi4, Hdmi4Uri },
            { InputSrcEnums.Component1, Comp1Uri },
            { InputSrcEnums.Component2, Comp2Uri },
            { InputSrcEnums.Component3, Comp3Uri },
            { InputSrcEnums.Component4, Comp4Uri },
            { InputSrcEnums.Wifi, WifiUri },
            { InputSrcEnums.Cec1, Cec1Uri },
            { InputSrcEnums.Cec2, Cec2Uri },
            { InputSrcEnums.Cec3, Cec3Uri },
        };

        private const string Hdmi1Uri = "extInput:hdmi?port=1";
        private const string Hdmi2Uri = "extInput:hdmi?port=2";
        private const string Hdmi3Uri = "extInput:hdmi?port=3";
        private const string Hdmi4Uri = "extInput:hdmi?port=4";
        private const string Comp1Uri = "extInput:component?port1;";
        private const string Comp2Uri = "extInput:component?port2;";
    private const string Comp3Uri = "extInput:component?port3;";
    private const string Comp4Uri = "extInput:component?port4;";
    private const string WifiUri = "extInput:widi?port=1";
    private const string Cec1Uri = "extInput:cec?type=player&port=1";
    private const string Cec2Uri = "extInput:cec?type=player&port=2";
    private const string Cec3Uri = "extInput:cec?type=player&port=3";
    }

    public class VolumeInformation
    {
        [JsonProperty("volume")] public int Volume { get; set; }

        [JsonProperty("mute")] public bool Mute { get; set; }

        [JsonProperty("minVolume")] public int MinVolume { get; set; }

        [JsonProperty("maxVolume")] public int MaxVolume { get; set; }

        [JsonProperty("Target")] public string Target { get; set; }
    }

    public class PowerStatus
    {
        [JsonProperty("status")] public string Status { get; set; }
    }

    public class InputInformation
    {
        [JsonProperty("active")] public bool Active { get; set; }
        [JsonProperty("uri")] public string Uri { get; set; }
        [JsonProperty("title")] public string Title { get; set; }
    }

    public enum ApiServicesEnum
    {
        System = 10,
        AvContent = 20,
        Audio = 30
    }


    public enum InputSrcEnums
    {
        Hdmi1,
        Hdmi2,
        Hdmi3,
        Hdmi4,
        Component1,
        Component2,
        Component3,
        Component4,
        Wifi,
        Cec1,
        Cec2,
        Cec3
    }
}