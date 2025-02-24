using Newtonsoft.Json;

namespace BraviaControlLib
{
    public class VolumeInformation
    {
         
        [JsonProperty("volume")] public int Volume { get; set; }
     
         [JsonProperty("mute")] public bool Mute { get; set; }
     
         [JsonProperty("minVolume")] public int MinVolume { get; set; }
     
         [JsonProperty("maxVolume")] public int MaxVolume { get; set; }
     
         [JsonProperty("Target")] public string Target { get; set; }
     
         [JsonConstructor]
         public VolumeInformation(int volume, bool mute, int minVolume, int maxVolume, string target)
         {
     
             Volume = volume;
             Mute = mute;
             MinVolume = minVolume;
             MaxVolume = maxVolume;
             Target = target;
         }
    }
}