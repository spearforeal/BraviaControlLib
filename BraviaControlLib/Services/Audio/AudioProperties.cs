using System.Collections.Generic;

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

        public static readonly Dictionary<AudioEnum, string> AudCmdDict = new Dictionary<AudioEnum, string>
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
    }
}