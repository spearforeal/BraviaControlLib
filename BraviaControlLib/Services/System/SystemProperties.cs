using System.Collections.Generic;

namespace BraviaControlLib
{
    public partial class Bravia
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

        //Map Ids to Enums
        public enum SysEnums
        {
            GetCurrentTime = GetCurrentTimeId,
            GetInterfaceInformation = GetInterfaceInformationId,
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

        //Include Enums in Dictionary
        private static readonly Dictionary<SysEnums, string> SysCmdDict =
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

        //Keyvalue pair association between Id and Command
        private static KeyValuePair<int, string> Cmd(SysEnums t) =>
            new KeyValuePair<int, string>((int)t, SysCmdDict[t]);
    }
}