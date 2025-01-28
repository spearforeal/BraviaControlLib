using System;
using System.Collections.Generic;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Net.Http;
using Independentsoft.Exchange;
using Newtonsoft.Json;

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
        public void PowerOn()
        {
            var url = BuildUrl(SystemUrl);
            var payload = BuildPayload(SetPowerCommand, PowerCommandVersion, new { status = PowerOnConst });
            if (SendHttpCommand(url, payload))
                CrestronConsole.PrintLine("Power On command sent successfully to {0}", IpAddress);
        }

        public void PowerOff()
        {
            var url = BuildUrl(SystemUrl);
            var payload = BuildPayload(SetPowerCommand, PowerCommandVersion, new { status = PowerOffConst });
            if (SendHttpCommand(url, payload))
                CrestronConsole.PrintLine("Power Off command sent successfully to {0}", IpAddress);
        }

        public bool GetPower()
        {
            var url = BuildUrl(SystemUrl);
            var payload = BuildPayload(GetPowerCommand, PowerCommandVersion, new{});
            var response = SendHttpCommandWithResponse(url, payload);
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
                    CrestronConsole.PrintLine("{0}", ex.Message);
                }
            }

            return false;
        }

        //Input
        public InputInformation GetInput()
        {
            var url = BuildUrl(AvContentUrl);
            var payload = BuildPayload(GetInputCommand, InputCommandVersion, new { });
            var response = SendHttpCommandWithResponse(url, payload);
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
                    CrestronConsole.PrintLine("Error parsing input information: {0}", ex.Message);

                }
            }
            CrestronConsole.PrintLine("Failed to retrieve input information.");
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

        public void SetInput(string inputName)
        {
            if (!_inputs.ContainsKey(inputName))
            {
                CrestronConsole.PrintLine("Input '{0}' is not valid.", inputName);
                return;
            }
            var url = BuildUrl(AvContentUrl);
            var inputUri = _inputs[inputName];
            var payload = BuildPayload(SetInputCommand, InputCommandVersion, new { uri = inputUri });
            if (SendHttpCommand(url, payload))
                CrestronConsole.PrintLine("Input switched to {0} on {1}", inputName, IpAddress);
            else
                CrestronConsole.PrintLine("Failed to switch input to {0} on {1}", inputName, IpAddress);
        }
        //Audio


        //HTTP
        private bool SendHttpCommand(string url, string payload)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpClientRequest()
                {
                    Url = new UrlParser(url),
                    RequestType = RequestType.Post,
                    ContentString = payload
                };
                //Headers
                request.Header.AddHeader(new HttpHeader("X-Auth-PSK", Psk));
                request.Header.ContentType = "application/json";

                //Send
                var response = client.Dispatch(request);

                //Success?
                return response.Code == 200;
            }
            catch (Exception ex)
            {
                CrestronConsole.PrintLine("HTTP Request Error: {0}", ex.Message);
                return false;
            }
            
        }


        private string SendHttpCommandWithResponse(string url, string payload)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpClientRequest()
                {
                    Url = new UrlParser(url),
                    RequestType = RequestType.Post,
                    ContentString = payload
                };
                //Headers
                request.Header.AddHeader(new HttpHeader("X-Auth-PSK", Psk));
                request.Header.ContentType = "application/json";

                //Send
                var response = client.Dispatch(request);

                //Success?
                if (response.Code == 200)
                {
                    return response.ContentString;
                }
                CrestronConsole.PrintLine("HTTP Request failed. Code:{0}", response.Code);
                return null;
            }
            catch (Exception ex)
            {
                CrestronConsole.PrintLine("HTTP Request Error: {0}", ex.Message);
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
                    CrestronConsole.PrintLine("Error parsing JSON: {0}", ex.Message);
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
}