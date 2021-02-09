
using kaola.api.invok.sellerMsgModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace kaola.api.invok.sellerMsg
{
    public class kaola_seller:kaola_parameter
    {
        /// <summary>
        /// 获得商家类目
        /// </summary>
        /// <param name="method"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public string kaola_vender_category_get(string method, string time)//public List<Category> GetAllCategory(string method,string time)
        {
            StringBuilder sb = new StringBuilder(200);
            sb.Append("access_token" + str_access_tokenli[2]);                                   //令牌
            sb.Append("app_key" + str_app_keyli[2]);                                        //密钥
            sb.Append(str_method + method);                                //调用接口
            sb.Append(str_timestamp + time);                               //时间戳
            string s = string.Empty;
            string app_secret = app_secretli[2];
            s = app_secret + sb.ToString() + app_secret;

            //使用MD5进行加密，再转化成大写
            string sign = kaola_sign.To32Md5(s).ToUpper();
            //string sign = kaola_sign.signValues(sb.ToString());            //签名


            string url = kaola_sign.kaola_Invok_URL(str_access_tokenli[2], str_app_keyli[2], method, sign, time);
            WebRequest request = (WebRequest)HttpWebRequest.Create(url);  //调用接口
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader read = new StreamReader(stream, Encoding.UTF8);
            string data = read.ReadToEnd();
            List<Category> li = new List<Category>();
            CateObjModel com = new CateObjModel();
            com = kaola_seralia.ScriptDeserialize<CateObjModel>(data);
            CateItem ci = new CateItem();
            ci = com.kaola_vender_category_get_response;
            li = ci.Item_cats;
            return data;

        }

        public string kaola_vender_brand_get(string method, string time)//public List<BrandInfo> GetAllBrand(string method, string time)
        {
            StringBuilder sb = new StringBuilder(200);
            sb.Append("access_token" + str_access_tokenli[2]);                                   //令牌
            sb.Append("app_key" + str_app_keyli[2]);                                        //密钥
            sb.Append(str_method + method);                                //调用接口
            sb.Append(str_timestamp + time);                               //时间戳
            string s = string.Empty;
            string app_secret = app_secretli[2];
            s = app_secret + sb.ToString() + app_secret;

            //使用MD5进行加密，再转化成大写
            string sign = kaola_sign.To32Md5(s).ToUpper();
            //string sign = kaola_sign.signValues(sb.ToString());            //签名


            string url = kaola_sign.kaola_Invok_URL(str_access_tokenli[2], str_app_keyli[2], method, sign, time);
            WebRequest request = (WebRequest)HttpWebRequest.Create(url);  //调用接口
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader read = new StreamReader(stream, Encoding.UTF8);
            string data = read.ReadToEnd();
            BrandModel bi = new BrandModel();
            bi=kaola_seralia.ScriptDeserialize<BrandModel>(data);
            List<BrandInfo> li = new List<BrandInfo>();
            BrandItem bim = new BrandItem();
            bim = bi.kaola_vender_brand_get_response;
            li = bim.brand_list;
            return data;
        }


        /// <summary>
        /// 获取商家基本信息   kaola.vender.info.get
        /// </summary>
        /// <param name="method">方法名</param>
        /// <param name="time">时间戳</param>
        /// <returns></returns>
        public string[] kaola_vender_info_get(string method, string time)//public List<BrandInfo> GetAllBrand(string method, string time)
        {
            List<string> resultli = new List<string>();
            StringBuilder sb = new StringBuilder(200);
            for (int i = 0; i < str_access_tokenli.Length; i++)
            {


                sb.Append("access_token" + str_access_tokenli[i]);                              //令牌
                sb.Append("app_key"+str_app_keyli[i]);                                        //密钥
                sb.Append(str_method + method);                                //调用接口
                sb.Append(str_timestamp + time);                               //时间戳

               // string sign = kaola_sign.signValues(sb.ToString());            //签名

                string sign = string.Empty;  //签名

                //把appSecret夹在字符串的两端
                //s = "8200ee92ec22fcae76e2f00bc5c79247188e0593" + s + "8200ee92ec22fcae76e2f00bc5c79247188e0593";
                string app_secret = app_secretli[i];
                string s = app_secret + sb.ToString() + app_secret;

                //使用MD5进行加密，再转化成大写
                sign = kaola_sign.To32Md5(s).ToUpper();

                //return sign;

                string url = kaola_sign.kaola_Invok_URL(str_access_tokenli[i], str_app_keyli[i], method, sign, time);
                WebRequest request = (WebRequest)HttpWebRequest.Create(url);  //调用接口
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader read = new StreamReader(stream, Encoding.UTF8);
                string data = read.ReadToEnd();
                resultli.Add(data);
                sb.Clear();
            }
            
            return resultli.ToArray();
        }

        #region 原来的
        /// <summary>
        /// 获取商家基本信息   kaola.vender.info.get
        /// </summary>
        /// <param name="method">方法名</param>
        /// <param name="time">时间戳</param>
        /// <returns></returns>
        //public string kaola_vender_info_get(string method, string time)//public List<BrandInfo> GetAllBrand(string method, string time)
        //{
        //    StringBuilder sb = new StringBuilder(200);
        //    sb.Append(str_access_token);                                   //令牌
        //    sb.Append(str_app_key);                                        //密钥
        //    sb.Append(str_method + method);                                //调用接口
        //    sb.Append(str_timestamp + time);                               //时间戳

        //    string sign = kaola_sign.signValues(sb.ToString());            //签名


        //    string url = kaola_sign.kaola_Invok_URL(access_token, app_key, method, sign, time);
        //    WebRequest request = (WebRequest)HttpWebRequest.Create(url);  //调用接口
        //    WebResponse response = request.GetResponse();
        //    Stream stream = response.GetResponseStream();
        //    StreamReader read = new StreamReader(stream, Encoding.UTF8);
        //    string data = read.ReadToEnd();
        //    return data;
        //} 
        #endregion
    }
}
