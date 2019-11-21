using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Akeem.Utiliy
{
    public static class CommonTool
    {
        #region Dictionary 扩展方法

        /// <summary>
        /// 往Dictionary 添加值，
        /// 键不存在-自动补充
        /// 键存在-值覆盖
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            if (dic.ContainsKey(key))
            {
                dic[key] = value;
            }
            else
            {
                dic.Add(key, value);
            }
        }

        public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
        {
            if (dic.ContainsKey(key))
            {
                return dic[key];
            }
            else
            {
                return default(TValue);
            }
        }

        /// <summary>
        /// 将任意类型添加值Dictionary中
        /// </summary>
        /// <typeparam name="TKey">Dictionary - 键</typeparam>
        /// <typeparam name="TValue">Dictionary - 任意类型值</typeparam>
        /// <param name="dic"></param>
        /// <param name="key">主键</param>
        /// <param name="value">值</param>
        public static void AddString<TKey, TValue>(this Dictionary<TKey, string> dic, TKey key, TValue value)
        {
            if (value.GetType().IsValueType)
            {
                dic.Set(key, Convert.ToString(value));
            }
            else
            {
                dic.Set(key, value.Object2Json());
            }
        }

        public static decimal GetDecimal<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
        {
            TValue value = dic.Get(key);
            decimal v = default;
            if (value.GetType().IsValueType)
            {
                decimal.TryParse(Convert.ToString(value), out v);
            }
            return v;
        }

        public static int GetInt<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
        {
            return Convert.ToInt32(dic.GetDecimal(key)); ;
        }

        public static string GetString<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
        {
            return Convert.ToString(dic.Get(key));
        }

        public static double GetDouble<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
        {
            return Convert.ToDouble(dic.GetDecimal(key));
        }

        public static float GetFloat<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
        {
            return (float)(dic.GetDouble(key));
        }

        public static bool GetBoolean<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
        {
            string value = dic.GetString(key);
            if (value == null)
            {
                return false;
            }
            return "true".Equals(value, StringComparison.CurrentCultureIgnoreCase) || "1".Equals(value, StringComparison.CurrentCultureIgnoreCase);
        }

        #endregion Dictionary 扩展方法

        #region Json 方法

        /// <summary>
        /// 对象转换成Json序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Object2Json<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 将Json反序列化成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T Json2Object<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        #endregion Json 方法

        #region 加密

        /// <summary>
        ///  md5加密字符串
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string ToMD5(this string message, bool lower = true)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = Encoding.UTF8.GetBytes(message);
            byte[] outStr = md5.ComputeHash(result);
            string md5string = BitConverter.ToString(outStr).Replace("-", "");
            if (lower)
            {
                md5string = md5string.ToLower();
            }
            return md5string;
        }

        #endregion 加密

        #region 按字节数截取字符串的方法

        /// <summary>
        /// 按字节数截取字符串的方法
        /// </summary>
        /// <param name="source">要截取的字符串（可空）</param>
        /// <param name="NumberOfBytes">要截取的字节数</param>
        /// <param name="encoding">System.Text.Encoding</param>
        /// <param name="suffix">结果字符串的后缀（超出部分显示为该后缀）</param>
        /// <returns></returns>
        public static string SubStringByBytes(this string source, int NumberOfBytes, System.Text.Encoding encoding, string suffix = "")
        {
            if (string.IsNullOrWhiteSpace(source) || source.Length == 0)
                return source;

            if (encoding.GetBytes(source).Length <= NumberOfBytes)
                return source;

            long tempLen = 0;
            StringBuilder sb = new StringBuilder();
            foreach (var c in source)
            {
                char[] _charArr = new char[] { c };
                byte[] _charBytes = encoding.GetBytes(_charArr);
                if ((tempLen + _charBytes.Length) > NumberOfBytes)
                {
                    if (!string.IsNullOrWhiteSpace(suffix))
                        sb.Append(suffix);
                    break;
                }
                else
                {
                    tempLen += _charBytes.Length;
                    sb.Append(encoding.GetString(_charBytes));
                }
            }
            return sb.ToString();
        }

        #endregion 按字节数截取字符串的方法

        #region 复制一个对象

        /// <summary>
        /// 复制一个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        public static T Copy<T>(T info) where T : new()
        {
            string json = JsonConvert.SerializeObject(info);
            return JsonConvert.DeserializeObject<T>(json);
        }

        #endregion 复制一个对象

        #region 在线翻译
        /// <summary>
        /// 在线翻译
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Task<string> Translate(TranslateLanguage from, TranslateLanguage to, string text)
        {
            return TranslateTool.Translate(from, to, text);
        }
        #endregion

    }
}