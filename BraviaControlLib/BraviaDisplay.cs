using System;
using System.Collections.Generic;
using Crestron.SimplSharp;
using Crestron.SimplSharp.Net.Http;
using Newtonsoft.Json;

namespace BraviaControlLib
{
    public class BraviaDisplay
    {
        public string IpAddress { get; set; }
        public string Psk { get; set; }

        public BraviaDisplay(string ipAddress, string psk)
        {
            IpAddress = ipAddress;
            Psk = psk;
        }

        //Power Commands
        public void PowerOn()
        {
            var url = $"http://{IpAddress}/sony/system";
            var payload =
                "{ \"id\": 1, \"method\": \"setPowerStatus\", \"version\": \"1.0\", \"params\": [{{ \"status\": true}]}";
            if (SendHttpCommand(url, payload))
                CrestronConsole.PrintLine("Power On command sent successfully to {0}", IpAddress);
        }

        public void PowerOff()
        {
            var url = $"http://{IpAddress}/sony/system";
            var payload =
                "{ \"id\": 1, \"method\": \"setPowerStatus\", \"version\": \"1.0\", \"params\": [{{ \"status\": false}]}";
            if (SendHttpCommand(url, payload))
                CrestronConsole.PrintLine("Power Off command sent successfully to {0}", IpAddress);
        }

        //Input
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


            var url = $"http://{IpAddress}/sony/avContent";
            var inputUri = _inputs[inputName];
            var payload =
                $"{{ \"id\": 1, \"method\": \"setPlayContent\", \"version\": \"1.0\", \"params\": [ {{ \"uri\": \"{inputUri}\" }} ] }}";
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
}