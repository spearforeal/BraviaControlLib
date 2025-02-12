using System.Collections.Generic;

namespace BraviaControlLib
{
    public partial class Bravia
    {
        private const int GetContentCountId = 19;
        private const int GetContentListId = 20;
        private const int GetCurrentExternalInputStatusId = 21;
        private const int GetSchemeListId = 22;
        private const int GetSourceListId = 23;
        private const int GetPlayingContentInfoId = 24;
        private const int SetPlayContentId = 25;
        
        private const string Hdmi1Uri = "extInput:hdmi?port=1";
        private const string Hdmi2Uri = "extInput:hdmi?port=2";
        private const string Hdmi3Uri = "extInput:hdmi?port=3";
        private const string Hdmi4Uri = "extInput:hdmi?port=4";
        private const string Comp1Uri = "extInput:component?port1;";
        private const string Comp2Uri = "extInput:component?port2;";
        private const string Comp3Uri = "extInput:component?port3;";
        private const string Comp4Uri = "extInput:component?port4;";
        private const string WifiUri = "extInput:widi?port=1";
        private const string Cec1Uri = "extInput:cec?type=player&port=1";
        private const string Cec2Uri = "extInput:cec?type=player&port=2";
        private const string Cec3Uri = "extInput:cec?type=player&port=3";

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
public enum InputSrcEnums
    {
        Hdmi1,
        Hdmi2,
        Hdmi3,
        Hdmi4,
        Component1,
        Component2,
        Component3,
        Component4,
        Wifi,
        Cec1,
        Cec2,
        Cec3
    }
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

        private static KeyValuePair<int, string> Cmd(AvEnums t) => new KeyValuePair<int, string>((int)t, AvCmdDict[t]);

        public static readonly Dictionary<InputSrcEnums, string> InputSrcDict = new Dictionary<InputSrcEnums, string>
        {
            { InputSrcEnums.Hdmi1, Hdmi1Uri },
            { InputSrcEnums.Hdmi2, Hdmi2Uri },
            { InputSrcEnums.Hdmi3, Hdmi3Uri },
            { InputSrcEnums.Hdmi4, Hdmi4Uri },
            { InputSrcEnums.Component1, Comp1Uri },
            { InputSrcEnums.Component2, Comp2Uri },
            { InputSrcEnums.Component3, Comp3Uri },
            { InputSrcEnums.Component4, Comp4Uri },
            { InputSrcEnums.Wifi, WifiUri },
            { InputSrcEnums.Cec1, Cec1Uri },
            { InputSrcEnums.Cec2, Cec2Uri },
            { InputSrcEnums.Cec3, Cec3Uri },
        };


        private static KeyValuePair<string, string> InputCmd(InputSrcEnums t) =>
            new KeyValuePair<string, string>(t.ToString(), InputSrcDict[t]);
    }
}