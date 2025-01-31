using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Task = System.Threading.Tasks.Task;

namespace BraviaControlLib
{
    public class BraviaDisplay
    {
        private string IpAddress { get; set; }
        private string Psk { get; set; }
        private const string PowerCommandVersion = "1.0";
        private const string SetPowerCommand = "setPowerStatus";
        private const bool PowerOnConst = true;
        private const bool PowerOffConst = false;
        private const string SystemUrl = "system";
        private const string AvContentUrl = "avContent";
        private const string SetInputCommand = "setPlayContent";
        private const string InputCommandVersion = "1.0";
        private const string GetPowerCommand = "getPowerStatus";
        private const string GetInputCommand = "getPlayingContentInfo";


        public BraviaDisplay(string ipAddress, string psk)
        {

            IpAddress = ipAddress;
            Psk = psk;
        }

        private string BuildUrl(string endpoint) => $"http://{IpAddress}/sony/{endpoint}";

        private string BuildPayload(string method, string version, object parameters)
        {
            var payload = new { id = 1, method = method, version = version, @params = new[] { parameters } };
            return JsonConvert.SerializeObject(payload);
        }

        //Power Commands
        public async Task SetPowerAsync(bool powerOn)
        {
            var payload = BuildPayload(SetPowerCommand, PowerCommandVersion, new { status = powerOn });
            if (await SendHttpCommand(SystemUrl, payload))
            {
                var isPowerOn = await GetPowerAsync();
            }
        }


        public async Task<bool> GetPowerAsync()
        {
            var payload = BuildPayload(GetPowerCommand, PowerCommandVersion, new { });
            var response = await SendHttpCommandWithResponse(SystemUrl, payload);
            if (!string.IsNullOrEmpty(response))
            {
                try
                {
                    var powerStatus = ApiResponse<PowerStatus>.Parse(response);
                    if (powerStatus != null)
                    {
                        return powerStatus.Status;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0}", ex.Message);
                }
            }

            return false;
        }

        //Input
        public async Task<InputInformation> GetInputAsync()
        {
            var payload = BuildPayload(GetInputCommand, InputCommandVersion, new { });
            var response = await SendHttpCommandWithResponse(AvContentUrl, payload);
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

        public async Task SetInputAsync(string inputName)
        {
            if (!_inputs.TryGetValue(inputName, out var inputUri))
            {
                Console.WriteLine("Input '{0}' is not valid.", inputName);
                return;
            }

            var payload = BuildPayload(SetInputCommand, InputCommandVersion, new { uri = inputUri });
            var success = await SendHttpCommand(AvContentUrl, payload);
            Console.WriteLine(success ? "Input switched to {0} on {1}" : "Failed to switch input to {0} on {1}",
                inputName, IpAddress);
        }
        //Audio


        //HTTP
        private async Task<HttpWebRequest> CreateRequestAsync(string endpoint, string body)
        {
            try
            {
                var uri = BuildUrl(endpoint);
                var request = WebRequest.CreateHttp(uri);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("X-Auth-PSK", Psk);
                request.ContentLength = body.Length;
                var bodyBytes = System.Text.Encoding.UTF8.GetBytes(body);
                request.ContentLength = bodyBytes.Length;
                using (var streamWriter = new StreamWriter(await request.GetRequestStreamAsync()))
                {
                    await streamWriter.WriteAsync(body);
                }

                return request;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"$Error: {ex.Message}");
                throw;
            }
        }

        private async Task<bool> SendHttpCommand(string url, string payload)
        {
            try
            {
                var request = await CreateRequestAsync(url, payload);
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

        private async Task<string> SendHttpCommandWithResponse(string url, string payload)
        {
            try
            {
                var request = await CreateRequestAsync(url, payload);
                using (var response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
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
        [JsonProperty("status")] public bool Status { get; set; }
    }

    public class InputInformation
    {
        [JsonProperty("active")] public bool Active { get; set; }
        [JsonProperty("uri")] public string Uri { get; set; }
        [JsonProperty("title")] public string Title { get; set; }
    }

    public enum EnumTesting
    {
        Debug = 1
    }
}