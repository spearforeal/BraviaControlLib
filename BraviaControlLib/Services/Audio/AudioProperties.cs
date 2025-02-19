using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraviaControlLib
{
    public partial class Bravia
    {
        private const int GetSoundSettingsId = 26;
        private const int GetSpeakerSettingsId = 27;
        private const int GetVolumeInformationId = 28;
        private const int SetAudioMuteId = 29;
        private const int SetAudioVolumeId = 30;
        private const int SetSoundSettingsId = 31;
        private const int SetSpeakerSettingsId = 32;

        public enum AudioEnum
        {
            GetSoundSettings = GetSoundSettingsId,
            GetSpeakerSettings = GetSpeakerSettingsId,
            GetVolumeInformation = GetVolumeInformationId,
            SetAudioMute = SetAudioMuteId,
            SetAudioVolume = SetAudioVolumeId,
            SetSoundSettings = SetSoundSettingsId,
            SetSpeakerSettings = SetSpeakerSettingsId,
        }

        private static readonly Dictionary<AudioEnum, string> AudCmdDict = new Dictionary<AudioEnum, string>
        {
            { AudioEnum.GetSoundSettings, "getSoundSettings" },
            { AudioEnum.GetSpeakerSettings, "getSpeakerSettings" },
            { AudioEnum.GetVolumeInformation, "getVolumeInformation" },
            { AudioEnum.SetAudioMute, "setAudioMute" },
            { AudioEnum.SetAudioVolume, "setAudioVolume" },
            { AudioEnum.SetSoundSettings, "setSoundSettings" },
            { AudioEnum.SetSpeakerSettings, "setSpeakerSettings" },
        };

        private static KeyValuePair<int, string> Cmd(AudioEnum t) =>
            new KeyValuePair<int, string>((int)t, AudCmdDict[t]);

        public async Task SetVolumeAsync(ApiServicesEnum apiServicesEnum, string value)
        {
            if (!int.TryParse(value, out var newVolume))
            {
                throw new ArgumentException("Volume value must be an integer", nameof(value));
            }
            var volInfo = await GetVolumeAsync(); 
            var command = Cmd(AudioEnum.SetAudioVolume);
            if(newVolume < volInfo.MinVolume || newVolume > volInfo.MaxVolume )
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    $"Volume must be between {volInfo.MinVolume} and {volInfo.MaxVolume}.");
            }

            var success = await SendHttpCommand( ApiServicesEnum.Audio, command, "1.0",
                new { volume = newVolume });
            Console.WriteLine(success
                ? $"Volume successfully set to: {newVolume}"
                : $"Failed to set volume to: {newVolume}");
        }

        private async Task<VolumeInformation> GetVolumeAsync()
        {
            var command = Cmd(AudioEnum.GetVolumeInformation);
            var response = await SendHttpCommandWithResponse(ApiServicesEnum.Audio, command, "1.0", new { });
            if (string.IsNullOrEmpty(response)) return null;
            try
            {
                var apiResponse = ApiResponse<VolumeInformation>.Parse(response);
                if (apiResponse?.Result != null && apiResponse.Result.Length > 0)
                {
                    return apiResponse.Result[0];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing volume information {0}", ex.Message);
                    
            }

            return null;

        }
    }
}