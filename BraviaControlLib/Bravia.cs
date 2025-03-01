﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Task = System.Threading.Tasks.Task;

namespace BraviaControlLib
{
    public partial class Bravia
    {
        public bool IsTestMode { get; set; }
        private string IpAddress { get; set; }

        private string Psk { get; set; }


        public Bravia(string ipAddress, string psk)
        {
            IpAddress = ipAddress;
            Psk = psk;
        }

        private static readonly Dictionary<ApiServicesEnum, string> ApiServiceDict =
            new Dictionary<ApiServicesEnum, string>
            {
                { ApiServicesEnum.System, "system" },
                { ApiServicesEnum.AvContent, "avContent" },
                { ApiServicesEnum.Audio, "audio" },
            };


    }


    public class PowerStatusResponse
    {
        [JsonProperty("result")] public PowerStatus[] Result { get; set; }
        [JsonProperty("id")] public int Id { get; set; }
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


}