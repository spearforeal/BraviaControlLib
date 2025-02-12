using System.Collections.Generic;
using Newtonsoft.Json;

namespace BraviaControlLib
{
    public partial class Bravia
    {
        private string BuildUrl(ApiServicesEnum service)
        {
            var serviceStr = ApiServiceDict[service];
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
    }
}