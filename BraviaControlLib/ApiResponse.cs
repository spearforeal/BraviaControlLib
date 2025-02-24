using System;
using Newtonsoft.Json;

namespace BraviaControlLib
{
    public partial class Bravia
    {
        public  class ApiResponse<T>
        {
            [JsonProperty("result")] public T[][] Result { get; set; }
            [JsonProperty("id")] public int Id { get; set; }

            public static ApiResponse<T> Parse(string jsonResponse)
            {
                try
                {
                    return JsonConvert.DeserializeObject<ApiResponse<T>>(jsonResponse);
                }
                catch(JsonException ex)
                {
                    Console.WriteLine("Error parsing JSON {0}", ex.Message);
                    throw;
                }
            }

        }
    }
}