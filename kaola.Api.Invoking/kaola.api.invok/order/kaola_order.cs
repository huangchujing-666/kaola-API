using kaola.api.invok.orderModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace kaola.api.invok.order
{
    /// <summary>
    /// 订单
    /// </summary>
    public class kaola_order : kaola_parameter
    {

        /// <summary>
        /// 根据订单信息查询订单   kaola.order.search
        /// </summary>
        /// <param name="method">调用接口</param>
        /// <param name="time">时间戳</param>
        /// <param name="status">订单状态（1已付款，2已发货，3已签收，4缺货订单，5取消待确认，6已取消）</param>
        /// <param name="datetype">搜索日期类型（1支付时间，2发货时间，3签收时间）</param>
        /// <param name="stime">开始时间</param>
        /// <param name="etime">结束时间</param>
        /// <param name="orderid">订单号</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="pagesize">页面大小</param>
        /// <returns></returns>
        public string[] kaola_order_search(string method, string time, int status, int date_type, string stime, string etime, string orderid = null, int pageindex = 1, int pagesize = 20)
        {
            List<string> resultli = new List<string>();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str_access_tokenli.Length; i++)
            {

                sb.Append("access_token" + str_access_tokenli[i]);             //令牌
                sb.Append("app_key" + str_app_keyli[i]);                       //密钥
                sb.Append("date_type" + date_type);                            //搜索日期类型（1支付时间，2发货时间，3签收时间）
                sb.Append("end_time" + etime);                                 //结束时间
                sb.Append(str_method + method);                                //调用接口

                sb.Append("order_status" + status);                            //订单状态（1已付款，2已发货，3已签收，4缺货订单，5取消待确认，6已取消）
                if (string.IsNullOrWhiteSpace(orderid))                        //订单号
                {
                    sb.Append(orderid);
                }
                sb.Append("page_no" + pageindex);                              //默认为1,最大为50
                sb.Append("page_size" + pagesize);                             //默认为20,最大为100
                sb.Append("start_time" + stime);                               //开始时间
                sb.Append(str_timestamp + time);                               //时间戳

               // string sign = kaola_sign.signValues(sb.ToString());            //签名
                string s = string.Empty;
               // string sign = string.Empty;  //签名

                //把appSecret夹在字符串的两端
                //s = "8200ee92ec22fcae76e2f00bc5c79247188e0593" + s + "8200ee92ec22fcae76e2f00bc5c79247188e0593";
                string app_secret =app_secretli[i];
                s = app_secret + sb.ToString() + app_secret;

                //使用MD5进行加密，再转化成大写
                string sign = kaola_sign.To32Md5(s).ToUpper();

                //return sign;

                order_model ordermd = new order_model();
                string url = kaola_sign.kaola_Invok_URL(str_access_tokenli[i], str_app_keyli[i], method, sign, time);
                url = url + "&date_type=" + date_type + "&end_time=" + etime + "&order_status=" + status
                    + "&start_time=" + stime + "&page_no=" + pageindex + "&page_size=" + pagesize;//接口地址
                if (!string.IsNullOrWhiteSpace(orderid))
                {
                    url += "&order_id=" + orderid;
                }
                WebRequest request = (WebRequest)HttpWebRequest.Create(url);   //调用接口
                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader read = new StreamReader(stream, Encoding.UTF8);
                string data = read.ReadToEnd();// + "app_key='"+str_app_keyli[i]+"'"
                resultli.Add(data);
                sb.Clear();
            }
            return resultli.ToArray();
        }


        #region 原来的
        ///// <summary>
        ///// 根据订单信息查询订单   kaola.order.search
        ///// </summary>
        ///// <param name="method">调用接口</param>
        ///// <param name="time">时间戳</param>
        ///// <param name="status">订单状态（1已付款，2已发货，3已签收，4缺货订单，5取消待确认，6已取消）</param>
        ///// <param name="datetype">搜索日期类型（1支付时间，2发货时间，3签收时间）</param>
        ///// <param name="stime">开始时间</param>
        ///// <param name="etime">结束时间</param>
        ///// <param name="orderid">订单号</param>
        ///// <param name="pageindex">当前页</param>
        ///// <param name="pagesize">页面大小</param>
        ///// <returns></returns>
        //public string kaola_order_search(string method, string time, int status, int date_type, string stime, string etime, string orderid = null, int pageindex = 1, int pagesize = 20)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(str_access_token);                                   //令牌
        //    sb.Append(str_app_key);                                        //密钥
        //    sb.Append("date_type" + date_type);                            //搜索日期类型（1支付时间，2发货时间，3签收时间）
        //    sb.Append("end_time" + etime);                                 //结束时间
        //    sb.Append(str_method + method);                                //调用接口

        //    sb.Append("order_status" + status);                            //订单状态（1已付款，2已发货，3已签收，4缺货订单，5取消待确认，6已取消）
        //    if (string.IsNullOrWhiteSpace(orderid))                        //订单号
        //    {
        //        sb.Append(orderid);
        //    }
        //    sb.Append("page_no" + pageindex);                              //默认为1,最大为50
        //    sb.Append("page_size" + pagesize);                             //默认为20,最大为100
        //    sb.Append("start_time" + stime);                               //开始时间
        //    sb.Append(str_timestamp + time);                               //时间戳

        //    string sign = kaola_sign.signValues(sb.ToString());            //签名

        //    order_model ordermd = new order_model();
        //    string url = kaola_sign.kaola_Invok_URL(access_token, app_key, method, sign, time);
        //    url = url + "&date_type=" + date_type + "&end_time=" + etime + "&order_status=" + status
        //        + "&start_time=" + stime + "&page_no=" + pageindex + "&page_size=" + pagesize;//接口地址
        //    if (!string.IsNullOrWhiteSpace(orderid))
        //    {
        //        url += "&order_id=" + orderid;
        //    }
        //    WebRequest request = (WebRequest)HttpWebRequest.Create(url);   //调用接口
        //    WebResponse response = request.GetResponse();
        //    Stream stream = response.GetResponseStream();
        //    StreamReader read = new StreamReader(stream, Encoding.UTF8);
        //    string data = read.ReadToEnd();

        //    return data;
        //} 
        #endregion

        /// <summary>
        /// 获取指定订单的信息   kaola.order.get
        /// </summary>
        /// <param name="orderid">订单号</param>
        /// <returns></returns>
        public string kaola_order_get(string method,string time, string orderid)
        {
            StringBuilder sb = new StringBuilder();

                sb.Append("access_token" + str_access_tokenli[2]);                                   //令牌
                sb.Append("app_key" + str_app_keyli[2]);                                        //密钥
                sb.Append(str_method + method);                                //调用接口
                sb.Append("order_id" + orderid);                               //订单编号
                sb.Append(str_timestamp + time);                               //时间戳

                string s = string.Empty;
                //把appSecret夹在字符串的两端
                //s = "8200ee92ec22fcae76e2f00bc5c79247188e0593" + s + "8200ee92ec22fcae76e2f00bc5c79247188e0593";
                string app_secret = app_secretli[2];
                s = app_secret + sb.ToString() + app_secret;

                //使用MD5进行加密，再转化成大写
                string sign = kaola_sign.To32Md5(s).ToUpper();


                //签名
                // string sign = kaola_sign.signValues(sb.ToString());

                order_model ordermd = new order_model();
                string url = kaola_sign.kaola_Invok_URL(str_access_tokenli[2], str_app_keyli[2], method, sign, time);
                url = url + "&order_id=" + orderid;

                WebRequest request = (WebRequest)HttpWebRequest.Create(url);

                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader read = new StreamReader(stream, Encoding.UTF8);
                string data = read.ReadToEnd();
               
            return data ;


            #region MyRegion
            //StringBuilder sb = new StringBuilder();
            //sb.Append(str_access_token);                                   //令牌
            //sb.Append(str_app_key);                                        //密钥
            //sb.Append(str_method + method);                                //调用接口
            //sb.Append("order_id" + orderid);                               //订单编号
            //sb.Append(str_timestamp + time);                               //时间戳

            ////签名
            //string sign = kaola_sign.signValues(sb.ToString());

            //order_model ordermd = new order_model();
            //string url = kaola_sign.kaola_Invok_URL(access_token, app_key, method, sign, time);
            //url = url + "&order_id=" + orderid;

            //WebRequest request = (WebRequest)HttpWebRequest.Create(url);

            //WebResponse response = request.GetResponse();
            //Stream stream = response.GetResponseStream();
            //StreamReader read = new StreamReader(stream, Encoding.UTF8);
            //string data = read.ReadToEnd();
            //return data; 
            #endregion
        }

        /// <summary>
        /// 商家发货通知  kaola.logistics.deliver
        /// </summary>
        /// <param name="method">调用接口</param>
        /// <param name="time">时间戳</param>
        /// <param name="order_id">订单ID</param>
        /// <param name="express_company_code">快递公司代码，用分割（如：EMS,SF）</param>
        /// <param name="express_no">快递单号,用,分割</param>
        /// <param name="sku_info">skuid:num|skuid:num,skuid:num|skuid:num</param>
        /// <returns></returns>
        public string kaola_logistics_deliver(string method, string time, string order_id, string express_company_code, string express_no,DataTable dt,string sku_info = null)
        {
            string data = string.Empty;
            if (dt.Rows.Count>0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("access_token" + dt.Rows[0]["access_token"].ToString());                                   //令牌
                sb.Append("app_key" + dt.Rows[0]["app_key"].ToString());                                        //密钥
                sb.Append("express_company_code" + express_company_code);      //快递公司代码
                sb.Append("express_no" + express_no);                          //快递单号
                sb.Append(str_method + method);                                //调用接口
                sb.Append("order_id" + order_id);                              //订单编号
                if (!string.IsNullOrWhiteSpace(sku_info))
                {
                    sb.Append("sku_info" + sku_info);                          //属性
                }

                sb.Append(str_timestamp + time);                               //时间戳

                //签名
                string app_secret = dt.Rows[0]["app_secret"].ToString();
                string s = app_secret + sb.ToString() + app_secret;

                //使用MD5进行加密，再转化成大写
                string sign = kaola_sign.To32Md5(s).ToUpper();


                order_model ordermd = new order_model();
                string url = kaola_sign.kaola_Invok_URL(dt.Rows[0]["access_token"].ToString(), dt.Rows[0]["app_key"].ToString(), method, sign, time);
                url = url + "&express_company_code=" + express_company_code + "&express_no=" + express_no + "&order_id=" + order_id;

                if (!string.IsNullOrWhiteSpace(sku_info))
                {
                    url += "&sku_info=" + sku_info;
                }

                WebRequest request = (WebRequest)HttpWebRequest.Create(url);

                WebResponse response = request.GetResponse();
                Stream stream = response.GetResponseStream();
                StreamReader read = new StreamReader(stream, Encoding.UTF8);
                data = read.ReadToEnd();
            }
    

            return data;
        }

        #region 原来的
        /// <summary>
        /// 商家发货通知  kaola.logistics.deliver
        /// </summary>
        /// <param name="method">调用接口</param>
        /// <param name="time">时间戳</param>
        /// <param name="order_id">订单ID</param>
        /// <param name="express_company_code">快递公司代码，用分割（如：EMS,SF）</param>
        /// <param name="express_no">快递单号,用,分割</param>
        /// <param name="sku_info">skuid:num|skuid:num,skuid:num|skuid:num</param>
        /// <returns></returns>
        //public string kaola_logistics_deliver(string method, string time, string order_id, string express_company_code, string express_no, string sku_info = null)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append(str_access_token);                                   //令牌
        //    sb.Append(str_app_key);                                        //密钥
        //    sb.Append("express_company_code" + express_company_code);      //快递公司代码
        //    sb.Append("express_no" + express_no);                          //快递单号
        //    sb.Append(str_method + method);                                //调用接口
        //    sb.Append("order_id" + order_id);                              //订单编号
        //    if (!string.IsNullOrWhiteSpace(sku_info))
        //    {
        //        sb.Append("sku_info" + sku_info);                          //属性
        //    }

        //    sb.Append(str_timestamp + time);                               //时间戳

        //    //签名
        //    string sign = kaola_sign.signValues(sb.ToString());

        //    order_model ordermd = new order_model();
        //    string url = kaola_sign.kaola_Invok_URL(access_token, app_key, method, sign, time);
        //    url = url + "&express_company_code=" + express_company_code + "&express_no=" + express_no + "&order_id=" + order_id;

        //    if (!string.IsNullOrWhiteSpace(sku_info))
        //    {
        //        url += "&sku_info=" + sku_info;
        //    }

        //    WebRequest request = (WebRequest)HttpWebRequest.Create(url);

        //    WebResponse response = request.GetResponse();
        //    Stream stream = response.GetResponseStream();
        //    StreamReader read = new StreamReader(stream, Encoding.UTF8);
        //    string data = read.ReadToEnd();

        //    return data;
        //}
        
        #endregion

        /// <summary>
        /// 获取快递公司信息  kaola.logistics.companies.get
        /// </summary>
        /// <returns></returns>
        public string kaola_logistics_companies_get(string method,string time)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("access_token" + str_access_tokenli[2]);                                   //令牌
            sb.Append("app_key" + str_app_keyli[2]);                                        //密钥
            sb.Append(str_method + method);                                //调用接口
            sb.Append(str_timestamp + time);                               //时间戳


            string s = string.Empty;
            //把appSecret夹在字符串的两端
            //s = "8200ee92ec22fcae76e2f00bc5c79247188e0593" + s + "8200ee92ec22fcae76e2f00bc5c79247188e0593";
            string app_secret = app_secretli[2];
            s = app_secret + sb.ToString() + app_secret;

            //使用MD5进行加密，再转化成大写
            string sign = kaola_sign.To32Md5(s).ToUpper();


            ////签名
            //string sign = kaola_sign.signValues(sb.ToString());

            logistics_companys_model logisticsmd = new logistics_companys_model();

            string url = kaola_sign.kaola_Invok_URL(str_access_tokenli[2], str_app_keyli[2], method, sign, time);
            WebRequest request = (WebRequest)HttpWebRequest.Create(url);       //正式地址

            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader read = new StreamReader(stream, Encoding.UTF8);
            string data = read.ReadToEnd();

            return data;

        }

    }
}
