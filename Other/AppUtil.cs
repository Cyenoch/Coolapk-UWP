using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Coolapk_UWP.Other
{
    // https://github.com/Richasy/BiliBili-UWP/blob/master/BiliBili-Lib/Tools/AppTool.cs
    public class AppSettingsKey
    {
        public static string IsThemeWithSystem = "isThemeWithSystem";
        public static string Theme = "theme";
    }

    public class AppUtil
    {
        public string StringReplaceEmojiNode(string sourceString)
        {
            return sourceString;
        }

        private static readonly DateTime unixDateBase = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        /// <summary>
        /// 写入本地设置
        /// </summary>
        /// <param name="key">设置名</param>
        /// <param name="value">设置值</param>
        public static void WriteLocalSetting(string key, string value)
        {
            var localSetting = ApplicationData.Current.LocalSettings;
            var localcontainer = localSetting.CreateContainer("Coolapk", ApplicationDataCreateDisposition.Always);
            localcontainer.Values[key] = value;
        }
        /// <summary>
        /// 读取本地设置
        /// </summary>
        /// <param name="key">设置名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetLocalSetting(string key, string defaultValue)
        {
            var localSetting = ApplicationData.Current.LocalSettings;
            var localcontainer = localSetting.CreateContainer("Coolapk", ApplicationDataCreateDisposition.Always);
            bool isKeyExist = localcontainer.Values.ContainsKey(key.ToString());
            if (isKeyExist)
            {
                return localcontainer.Values[key.ToString()].ToString();
            }
            else
            {
                WriteLocalSetting(key, defaultValue);
                return defaultValue;
            }
        }
        /// <summary>
        /// 获取布尔值的设置
        /// </summary>
        /// <param name="key">设置名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static bool GetBoolSetting(string key, bool defaultValue = true)
        {
            return Convert.ToBoolean(GetLocalSetting(key, defaultValue.ToString()));
        }
        
        
    }
}
