using System;
using Newtonsoft.Json;

namespace BraviaControlLib
{
    public partial class Bravia
    {
        public abstract partial class ApiResponse<T>
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
}