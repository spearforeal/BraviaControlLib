using System;
using System.Threading.Tasks;

namespace BraviaControlLib
{
    public partial class Bravia
    {
        public async Task SetVolumeAsync(ApiServicesEnum apiServicesEnum, string value)
        {
            if (!int.TryParse(value, out var newVolume))
            {
                throw new ArgumentException("Volume value must be an integer", nameof(value));
            }

            var volInfo = await GetVolumeAsync();
            var command = Cmd(AudioEnum.SetAudioVolume);
            if (newVolume < volInfo.MinVolume || newVolume > volInfo.MaxVolume)
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    $"Volume must be between {volInfo.MinVolume} and {volInfo.MaxVolume}.");
            }

            var success = await SendHttpCommand(ApiServicesEnum.Audio, command, "1.0",
                new { volume = newVolume, target = "speaker" });
            Console.WriteLine(success
                ? $"Volume successfully set to: {newVolume}"
                : $"Failed to set volume to: {newVolume}");
        }

        public async Task SetVolumeAsync(string value)
        {
            await SetVolumeAsync(ApiServicesEnum.Audio, value);
        }

        private async Task ChangeVolumeAsync(string volumeChange)
        {
            var command = Cmd(AudioEnum.SetAudioVolume);
            await SendHttpCommand(ApiServicesEnum.Audio, command, "1.0",
                new { volume = volumeChange, target = "speaker" });
        }

        public async Task IncreaseVolumeAsync(int step = 1)
        {
            var vol = step > 0 ? $"+{step}" : step.ToString();
            await ChangeVolumeAsync(vol);
        }

        public async Task DecreaseVolumeAsync(int step = 1)
        {
            var vol = step > 0 ? $"-{step}" : step.ToString();
            await ChangeVolumeAsync(vol);
        }

        public async Task<VolumeInformation> GetVolumeAsync()
        {
            var command = Cmd(AudioEnum.GetVolumeInformation);
            var response = await SendHttpCommandWithResponse(ApiServicesEnum.Audio, command, "1.0", new { });
            if (string.IsNullOrEmpty(response)) return null;
            try
            {
                var apiResponse = ApiResponse<VolumeInformation>.Parse(response);
                if (apiResponse?.Result != null && apiResponse.Result.Length > 0)
                {
                    return apiResponse.Result[0][0];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing volume information {0}", ex.Message);
            }

            return null;
        }

        public async Task SetMuteAsync(bool mute)
        {
            var command = Cmd(AudioEnum.SetAudioMute);
            var success =
                await SendHttpCommand(ApiServicesEnum.Audio, command, "1.0", new { mute, target = "speaker" });
            Console.WriteLine(success ? (mute ? "Muted" : "Unmuted"): "Failed to change mute status");

        }
    }
}