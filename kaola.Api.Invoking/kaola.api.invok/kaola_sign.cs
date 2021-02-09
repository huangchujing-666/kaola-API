using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace kaola.api.invok
{

    /// <summary>
    /// 考拉密钥加密
    /// </summary>
    public class kaola_sign
    {

        /// <summary>
        /// 生成考拉签名
        /// </summary>
        /// <param name="method">调用考拉api接口</param>
        /// <param name="access_token">令牌</param>
        /// <param name="app_key">密钥</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="v">版本</param>
        /// 
        /// host配置：123.58.181.62 openapi.kaola.com
        /// appKey：edb6c3b9ac4847e7584c38e2b630b14f
        /// appSecret：8200ee92ec22fcae76e2f00bc5c79247188e0593
        /// access_token：214f66a9-e3f9-48ec-9e70-196fc6aa63fb
        /// 
        /// <returns>返回签名</returns>
        public static string signValues(string s)
        {
            string sign = string.Empty;  //签名

            //把appSecret夹在字符串的两端
            //s = "8200ee92ec22fcae76e2f00bc5c79247188e0593" + s + "8200ee92ec22fcae76e2f00bc5c79247188e0593";
            string app_secret = System.Configuration.ConfigurationManager.AppSettings["app_secret"].ToString();
            s = app_secret + s + app_secret;

            //使用MD5进行加密，再转化成大写
            sign = To32Md5(s).ToUpper();

            return sign;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="access_token">令牌</param>
        /// <param name="app_key">密钥</param>
        /// <param name="method">调用接口</param>
        /// <param name="sign">签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <returns></returns>
        public static string kaola_Invok_URL(string access_token, string app_key, string method, string sign, string timestamp)
        {
            string URL = "http://openapi.kaola.com/router?access_token=" + access_token + "&app_key=" + app_key + "&method=" + method + "&sign=" + sign + "&timestamp=" + timestamp;
            return URL; 
        }

        


        /// <summary>
        /// MD5　32位加密（验证无误，和支付宝对接过）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string To32Md5(string str)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(str));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }



    }
}
