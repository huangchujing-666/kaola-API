
using kaola.api.invok.basicMsgModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace kaola.api.invok.basicMsg
{
    public class kaola_basicMsg : kaola_parameter
    {

        /// <summary>
        /// kaola.common.countries.get 获得所有国家列表 
        /// </summary>
        /// <param name="method">kaola.common.countries.get</param>
        /// <param name="time">调用时间</param>
        /// <returns></returns>
        public string kaola_common_countries_get(string method, string time)//public List<Country> kaola_common_countries_get(string method, string time)
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
            List<Country> li = new List<Country>();
            CountryModel coumd = new CountryModel();
            coumd= kaola_seralia.ScriptDeserialize<CountryModel>(data);
            li = coumd.kaola_common_countries_get_response;
            return data;
        }

        /// <summary>
        /// 获取所有省份信息
        /// </summary>
        /// <param name="method">kaola.common.provinces.get</param>
        /// <param name="time"></param>
        /// <returns></returns>
        public string kaola_common_provinces_get(string method, string time)
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
            List<Province> li = new List<Province>();
            ProvinceModel pvmd = new ProvinceModel();
            pvmd=kaola_seralia.ScriptDeserialize<ProvinceModel>(data);
            li = pvmd.kaola_common_provinces_get_response;
            return data;
        }

        /// <summary>
        /// 根据省份编号获得城市编号
        /// </summary>
        /// <param name="method">kaola.common.city.get</param>
        /// <param name="province_code">省份编号</param>
        /// <param name="time">时间戳</param>
        /// <returns></returns>
        public string kaola_common_city_get(string method, string province_code, string time)
        {
            StringBuilder sb = new StringBuilder(200);
            sb.Append("access_token" + str_access_tokenli[2]);             //令牌
            sb.Append("app_key" + str_app_keyli[2]);                       //密钥
            sb.Append(str_method + method);                                //调用接口
            sb.Append("province_code" + province_code);                    //省编号
            sb.Append(str_timestamp + time);                               //时间戳
            string s = string.Empty;
            string app_secret = app_secretli[2];
            s = app_secret + sb.ToString() + app_secret;

            //使用MD5进行加密，再转化成大写
            string sign = kaola_sign.To32Md5(s).ToUpper();
           // string sign = kaola_sign.signValues(sb.ToString());            //签名

            string url = kaola_sign.kaola_Invok_URL(str_access_tokenli[2], str_app_keyli[2], method, sign, time);
            url = url + "&province_code=" + province_code;//接口地址
            WebRequest request = (WebRequest)HttpWebRequest.Create(url);//调用接口
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader read = new StreamReader(stream, Encoding.UTF8);
            string data = read.ReadToEnd();
            CityModel cmd = new CityModel();
            List<City> li = new List<City>();
            cmd =kaola_seralia.ScriptDeserialize<CityModel>(data);
            li = cmd.kaola_common_city_get_response;
            return data;
        }

        /// <summary>
        /// kaola.common.district.get (获取城市行政区信息)
        /// </summary>
        /// <param name="method"></param>
        /// <param name="city_code"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public string kaola_common_district_get(string method,string city_code,string time)
        {
            StringBuilder sb = new StringBuilder(200);
            sb.Append("access_token" + str_access_tokenli[2]);
            sb.Append("app_key" + str_app_keyli[2]);
            sb.Append("city_code"+city_code);
            sb.Append(str_method+method);
            sb.Append(str_timestamp+time);

            string s = string.Empty;
            string app_secret=app_secretli[2];
            s = app_secret + sb.ToString() + app_secret;

            string sign = kaola_sign.To32Md5(s).ToUpper();

            string url = kaola_sign.kaola_Invok_URL(str_access_tokenli[2], str_app_keyli[2], method, sign, time);
            url += "&city_code=" + city_code;
            WebRequest request = (WebRequest)HttpWebRequest.Create(url);//调用接口
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader read = new StreamReader(stream, Encoding.UTF8);
            string data = read.ReadToEnd();
            CityModel cmd = new CityModel();
            List<City> li = new List<City>();
            cmd = kaola_seralia.ScriptDeserialize<CityModel>(data);
            li = cmd.kaola_common_city_get_response;
            return data;
        }

        /// <summary>
        /// kaola.common.hscodes.get (根据关键字查询hs编码)
        /// </summary>
        /// <param name="p"></param>
        /// <param name="keyword"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public string kaola_common_hscodes_get(string method, string keyword, string time)
        {
            StringBuilder sb = new StringBuilder(200);
            sb.Append("access_token" + str_access_tokenli[2]);
            sb.Append("app_key" + str_app_keyli[2]);
            sb.Append("keyword" + keyword);
            sb.Append(str_method + method);
            sb.Append(str_timestamp + time);

            string s = string.Empty;
            string app_secret = app_secretli[2];
            s = app_secret + sb.ToString() + app_secret;
            string sign = kaola_sign.To32Md5(s).ToUpper();
            string url = kaola_sign.kaola_Invok_URL(str_access_tokenli[2], str_app_keyli[2], method, sign, time);

            url += "&keyword=" + keyword;
            WebRequest request = (WebRequest)HttpWebRequest.Create(url);//调用接口
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader read = new StreamReader(stream, Encoding.UTF8);
            string data = read.ReadToEnd();
            HsModel cmd = new HsModel();
            List<Hs> li = new List<Hs>();
            cmd = kaola_seralia.ScriptDeserialize<HsModel>(data);
            li = cmd.kaola_common_hscodes_get_response;
            return data;
        }
    }
}
