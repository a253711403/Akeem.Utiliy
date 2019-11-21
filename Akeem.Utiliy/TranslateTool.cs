using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Akeem.Utiliy
{
    public class TranslateTool
    {
        public static Task<string> Translate(TranslateLanguage from, TranslateLanguage to, string text)
        {
            return BaiduTranslate(from, to, text);
        }
        public static Task<string> BaiduTranslate(TranslateLanguage from, TranslateLanguage to, string text)
        {
            // 源语言
            string fromStr = from.ToString();
            // 目标语言
            string toStr = to.ToString();
            // 改成您的APP ID
            string appId = "20191121000359178";
            Random rd = new Random();
            string salt = rd.Next(100000).ToString();
            // 改成您的密钥
            string secretKey = "BU8TFzwhN8C1iMHKNMD6";
            string sign = CommonTool.ToMD5(appId + text + salt + secretKey);
            string url = "https://fanyi-api.baidu.com/api/trans/vip/translate?";
            url += "q=" + HttpUtility.UrlEncode(text);
            url += "&from=" + fromStr;
            url += "&to=" + to;
            url += "&appid=" + appId;
            url += "&salt=" + salt;
            url += "&sign=" + sign;
            return Task.Run(async () =>
            {
                HttpClient http = new HttpClient();
                string retString = await http.GetStringAsync(url);

                try
                {
                    var obj = retString.Json2Object<Dictionary<string, object>>();
                    if (obj.ContainsKey("trans_result"))
                    {
                        var trans_result = obj.GetString("trans_result").Json2Object<List<Dictionary<string, string>>>();
                        return trans_result[0].GetString("dst");
                    }
                    return text;
                }
                catch (Exception)
                {
                    return text;
                }
            });
        }
    }
}
