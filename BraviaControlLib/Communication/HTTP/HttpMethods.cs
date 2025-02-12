using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace BraviaControlLib
{
    public partial class Bravia
    {
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
                        using (var stream = response.GetResponseStream())
                        {
                            if (stream == null)
                            {
                                Console.WriteLine("Response is null");
                                return null;
                            }

                            using (var streamReader = new StreamReader(stream))
                            {
                                var content = await streamReader.ReadToEndAsync();
                                Console.WriteLine($"[SendHttpCommandWithResponse] Response content: {content}");
                                return content;
                            }
                        }
                    }

                    Console.WriteLine("HTTP Request failed with status code: {0}", response.StatusCode);
                    return null; // Return null if the status code is not OK
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("HTTP Request Error: {0}", ex.Message);
                return null;
            }
        }
    }
}