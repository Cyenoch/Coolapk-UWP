﻿using Newtonsoft.Json;
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
        /// <summary>
        /// 获取Unix时间戳
        /// </summary>
        /// <returns></returns>
        public static int DateToTimeStamp(DateTime date)
        {
            TimeSpan ts = date - new DateTime(1970, 1, 1, 8, 0, 0, 0);
            int seconds = Convert.ToInt32(ts.TotalSeconds);
            return seconds;
        }
        /// <summary>
        /// 获取double Unix时间戳
        /// </summary>
        /// <returns></returns>
        public static double DateTimeToUnixTimeStamp(DateTime date)
        {
            return Math.Round(date.ToUniversalTime()
                    .Subtract(unixDateBase)
                    .TotalSeconds);
        }
        /// <summary>
        /// 转化Unix时间戳
        /// </summary>
        /// <returns></returns>
        public static DateTime TimeStampToDate(int seconds)
        {
            var date = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(seconds);
            return date.ToLocalTime();
        }
        /// <summary>
        /// 转化Unix时间戳
        /// </summary>
        /// <returns></returns>
        public static DateTime TimeStampToDate(string seconds)
        {
            try
            {
                var date = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Convert.ToInt32(seconds));
                return date;
            }
            catch (Exception)
            {
                return DateTime.Now;
            }

        }
        /// <summary>
        /// 获取当前时间戳（秒）
        /// </summary>
        /// <returns></returns>
        public static int GetNowSeconds()
        {
            return DateToTimeStamp(DateTime.Now);
        }
        /// <summary>
        /// 获取当前时间戳（毫秒）
        /// </summary>
        /// <returns></returns>
        public static long GetNowMilliSeconds()
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 8, 0, 0, 0);
            long seconds = Convert.ToInt64(ts.TotalMilliseconds);
            return seconds;
        }
        /// <summary>
        /// 获取友好的时间表示
        /// </summary>
        /// <param name="seconds">秒</param>
        /// <returns></returns>
        public static string GetReadDateString(DateTime date)
        {
            var span = DateTime.Now - date;
            if (span.TotalSeconds < 60)
                return "刚刚";
            else if (span.TotalMinutes < 60)
                return span.Minutes + "分钟前";
            else if (span.TotalHours < 24)
                return span.Hours + "小时前";
            else if (span.TotalDays < 2)
                return "昨天";
            else if (span.TotalDays < 30)
                return span.Days + "天前";
            else if (span.TotalDays < 365)
                return date.ToString("MM-dd");
            else
                return date.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 获取友好的时间表示
        /// </summary>
        /// <param name="seconds">秒</param>
        /// <returns></returns>
        public static string GetReadDateString(int seconds)
        {
            var date = TimeStampToDate(seconds);
            var span = DateTime.Now - date;
            if (span.TotalSeconds < 60)
                return "刚刚";
            else if (span.TotalMinutes < 60)
                return span.Minutes + "分钟前";
            else if (span.TotalHours < 24)
                return span.Hours + "小时前";
            else if (span.TotalDays < 2)
                return "昨天";
            else if (span.TotalDays < 30)
                return span.Days + "天前";
            else if (span.TotalDays < 365)
                return date.ToString("MM-dd");
            else
                return date.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 将秒数转化为可读时间
        /// </summary>
        /// <param name="seconds">秒</param>
        /// <returns></returns>
        public static string GetReadDuration(int seconds)
        {
            var span = TimeSpan.FromSeconds(seconds);
            string time = span.Minutes.ToString("00") + ":" + Math.Abs(span.Seconds).ToString("00");
            if (span.Hours > 0)
                time = span.Hours.ToString() + ":" + time;
            return time;
        }

        /// <summary>
        /// 获取数字的缩写
        /// </summary>
        /// <param name="number">数字</param>
        /// <returns></returns>
        public static string GetNumberAbbreviation(double number)
        {
            string result;
            if (number < 10000)
                result = number.ToString();
            else
                result = Math.Round(number / 10000.0, 1).ToString() + "万";
            return result;
        }

        public static string GetMD5(string input)
        {
            using (var md5 = MD5.Create())
            {
                var r1 = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                var r2 = BitConverter.ToString(r1).ToLowerInvariant();
                return r2.Replace("-", "");
            }
        }
    }

    // 单向读
    public class StringToIntConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string) || objectType == typeof(int);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            try
            {
                return (uint)uint.Parse(reader.Value?.ToString() ?? "0");
            }
            catch (Exception _)
            {
                return (uint)0;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }
    }
}
