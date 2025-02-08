using System.Collections.Generic;

namespace BraviaControlLib
{
    public partial class BraviaDisplay
    {
        //Service System
        private const int GetCurrentTimeId = 1;
        private const int GetInterfaceInformationId = 2;
        private const int GetLedIndicatorStatusId = 3;
        private const int GetNetworkingSettingsId = 4;
        private const int GetPowerSavingModeId = 5;
        private const int GetPowerStatusId = 6;
        private const int GetRemoteControlInfoId = 7;
        private const int GetRemoteDeviceSettingsId = 8;
        private const int GetSystemInformationId = 9;
        private const int GetSystemSupportedFunctionId = 10;
        private const int GetWolModeId = 11;
        private const int RequestRebootId = 12;
        private const int SetLedIndicatorStatusId = 13;
        private const int SetLanguageId = 14;
        private const int SetPowerSavingModeId = 15;
        private const int SetPowerStatusId = 16;
        private const int SetWolModeId = 17;
        private const int GetInterfaceId = 18;
        //Service AVContent
        private const int GetContentCountId = 19;
        private const int GetContentListId = 20;
        private const int GetCurrentExternalInputStatusId = 21;
        private const int GetSchemeListId = 22;
        private const int GetSourceListId = 23;
        private const int GetPlayingContentInfoId = 24;
        private const int SetPlayContentId = 25;
        //Service Audio
        private const int GetSoundSettingsId = 26;
        private const int GetSpeakerSettingsId = 27;
        private const int GetVolumeInformationId = 28;
        private const int SetAudioMuteId = 29;
        private const int SetAudioVolumeId = 30;
        private const int SetSoundSettingsId = 31;
        private const int SetSpeakerSettingsId = 32;

        //Map Ids to Enums
        public enum SysEnums
        {
            GetCurrentTime = GetCurrentTimeId,
            GetInterfaceInformation = GetInterfaceId,
            GetLedIndicatorStatus = GetLedIndicatorStatusId,
            GetNetworkingSettings = GetNetworkingSettingsId,
            GetPowerSavingMode = GetPowerSavingModeId,
            GetPowerStatus = GetPowerStatusId,
            GetRemoteControlInfo = GetRemoteControlInfoId,
            GetRemoteDeviceSettings = GetRemoteDeviceSettingsId,
            GetSystemInformation = GetSystemInformationId,
            GetSystemSupportedFunction = GetSystemSupportedFunctionId,
            GetWolMode = GetWolModeId,
            RequestReboot = RequestRebootId,
            SetLedIndicatorStatus = SetLedIndicatorStatusId,
            SetLanguage = SetLanguageId,
            SetPowerSavingMode = SetPowerSavingModeId,
            SetPowerStatus = SetPowerStatusId,
            SetWolMode = SetWolModeId,
        }

        public enum AvEnums
        {
            GetContentCount = GetContentCountId,
            GetContentList = GetContentListId,
            GetCurrentExternalInputStatus = GetCurrentExternalInputStatusId,
            GetSchemeList = GetSchemeListId,
            GetSourceList = GetSourceListId,
            GetPlayingContentInfo = GetPlayingContentInfoId,
            SetPlayContent = SetPlayContentId,
            
        }

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

        //Include Enums in Dictionary
        public static readonly Dictionary<SysEnums, string> SysCmdDict =
            new Dictionary<SysEnums, string>
            {
                { SysEnums.GetCurrentTime, "getCurrentTime" },
                { SysEnums.GetInterfaceInformation, "getInterfaceInformation" },
                { SysEnums.GetLedIndicatorStatus, "getLEDIndicatorStatus" },
                { SysEnums.GetNetworkingSettings, "getNetworkSettings" },
                { SysEnums.GetPowerSavingMode, "getPowerSavingMode" },
                { SysEnums.GetPowerStatus, "getPowerStatus" },
                { SysEnums.GetRemoteControlInfo, "getRemoteControlInfo" },
                { SysEnums.GetRemoteDeviceSettings, "getRemoteDeviceSettings" },
                { SysEnums.GetSystemInformation, "getSystemInformation" },
                { SysEnums.GetSystemSupportedFunction, "getSystemSupportedFunction" },
                { SysEnums.GetWolMode, "getWolMode" },
                { SysEnums.RequestReboot, "requestReboot" },
                { SysEnums.SetLedIndicatorStatus, "setLEDIndicatorStatus" },
                { SysEnums.SetLanguage, "setLanguage" },
                { SysEnums.SetPowerSavingMode, "setPowerSavingMode" },
                { SysEnums.SetPowerStatus, "setPowerStatus" },
                { SysEnums.SetWolMode, "setWolMode" },
            };

        public static readonly Dictionary<AvEnums, string> AvCmdDict = new Dictionary<AvEnums, string>
        {
            { AvEnums.GetContentCount, "getContentCount" },
            { AvEnums.GetContentList, "getContentList" },
            { AvEnums.GetCurrentExternalInputStatus, "getCurrentExternalInputStatus" },
            { AvEnums.GetSchemeList, "getSchemeList" },
            { AvEnums.GetSourceList, "getSourceList" },
            { AvEnums.GetPlayingContentInfo, "getPlayingContentInfo" },
            { AvEnums.SetPlayContent, "setPlayContent" },
        };

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

        //Keyvalue pair association between Id and Command
        private static KeyValuePair<int, string> Cmd(SysEnums t) =>
            new KeyValuePair<int, string>((int)t, SysCmdDict[t]);

        private static KeyValuePair<int, string> Cmd(AvEnums t) => new KeyValuePair<int, string>((int)t, AvCmdDict[t]);

        private static KeyValuePair<int, string> Cmd(AudioEnum t) =>
            new KeyValuePair<int, string>((int)t, AudCmdDict[t]);








    }
}