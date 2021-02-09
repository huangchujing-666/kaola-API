using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace kaola.api.invok.category
{

    /// <summary>
    /// 类目
    /// </summary>
    public class kaola_category : kaola_parameter
    {


        /// <summary>
        /// 获取标准商品类目属性
        /// </summary>
        /// <param name="method">接口名称</param>
        /// <param name="time">时间戳</param>
        /// <param name="category_id">类目id, 必须是末级类目</param>
        /// <returns></returns>
        public string kaola_itemprops_get(string method, string time, string category_id)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("access_token" + str_access_tokenli[2]);                                   //令牌
            sb.Append("app_key" + str_app_keyli[2]);                                        //密钥
            sb.Append("category_id" + category_id);                        //类目id, 必须是末级类目
            sb.Append(str_method + method);                                //调用接口
            sb.Append(str_timestamp + time);                               //时间戳

            string s = string.Empty;
            //把appSecret夹在字符串的两端
            //s = "8200ee92ec22fcae76e2f00bc5c79247188e0593" + s + "8200ee92ec22fcae76e2f00bc5c79247188e0593";
            string app_secret = app_secretli[2];
            s = app_secret + sb.ToString() + app_secret;

            //使用MD5进行加密，再转化成大写
            string sign = kaola_sign.To32Md5(s).ToUpper();

            //string sign = kaola_sign.signValues(sb.ToString());            //签名

            string url = kaola_sign.kaola_Invok_URL(str_access_tokenli[2], str_app_keyli[2], method, sign, time);
            url = url + "&category_id=" + category_id;//接口地址
            WebRequest request = (WebRequest)HttpWebRequest.Create(url);   //调用接口
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader read = new StreamReader(stream, Encoding.UTF8);
            string data = read.ReadToEnd();

            return data;
        }



        /// <summary>
        /// 获取标准商品类目属性的值
        /// </summary>
        /// <param name="method">接口名称</param>
        /// <param name="time">时间戳</param>
        /// <param name="property_value_id">属性值id</param>
        /// <returns></returns>
        public string kaola_itempropvalues_get(string method, string time, string property_value_id)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append(str_access_token);                                   //令牌
            sb.Append(str_app_key);                                        //密钥
            sb.Append(str_method + method);                                //调用接口
            sb.Append("property_value_id" + property_value_id);            //属性值id
            sb.Append(str_timestamp + time);                               //时间戳

            string sign = kaola_sign.signValues(sb.ToString());            //签名

            string url = kaola_sign.kaola_Invok_URL(access_token, app_key, method, sign, time);
            url = url + "&property_value_id=" + property_value_id;//接口地址
            WebRequest request = (WebRequest)HttpWebRequest.Create(url);   //调用接口
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader read = new StreamReader(stream, Encoding.UTF8);
            string data = read.ReadToEnd();

            return data;
        }





        /// <summary>
        /// 商品上架
        /// </summary>
        /// <param name="method">接口名称</param>
        /// <param name="time">时间戳</param>
        /// <param name="key_list">需要上架的商品的key集合</param>
        /// <returns></returns>
        public string kaola_item_update_listing(string method, string time, string key_list)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("access_token" + str_access_tokenli[2]);                                   //令牌
            sb.Append("app_key" + str_app_keyli[2]);                                        //密钥
            sb.Append("key_list" + key_list);                              //需要上架的商品的key集合
            //sb.Append("key_list" + "85370-3049");                              //需要上架的商品的key集合
            sb.Append(str_method + method);                                //调用接口
            sb.Append(str_timestamp + time);                               //时间戳

            string s = string.Empty;
            string app_secret = app_secretli[2];
            s = app_secret + sb.ToString() + app_secret;

            //使用MD5进行加密，再转化成大写
            string sign = kaola_sign.To32Md5(s).ToUpper();
            //string sign = kaola_sign.signValues(sb.ToString());            //签名

            string url = kaola_sign.kaola_Invok_URL(str_access_tokenli[2], str_app_keyli[2], method, sign, time);

            url = url + "&key_list=" + key_list;//接口地址
            WebRequest request = (WebRequest)HttpWebRequest.Create(url);   //调用接口
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader read = new StreamReader(stream, Encoding.UTF8);
            string data = read.ReadToEnd();

            return data;
        }


        //kaola.item.update.delisting(商品下架)
        /// <summary>
        /// 商品下架
        /// </summary>
        /// <param name="method">接口名称</param>
        /// <param name="time">时间戳</param>
        /// <param name="key_list">需要下架的商品的key集合</param>
        /// <returns></returns>
        public string kaola_item_update_delisting(string method, string time, string key_list)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("access_token" + str_access_tokenli[2]);                                   //令牌
            sb.Append("app_key" + str_app_keyli[2]);                                        //密钥
            sb.Append("key_list" + key_list);                              //需要下架的商品的key集合
            sb.Append(str_method + method);                                //调用接口
            sb.Append(str_timestamp + time);                               //时间戳

            string s = string.Empty;
            string app_secret = app_secretli[2];
            s = app_secret + sb.ToString() + app_secret;

            //使用MD5进行加密，再转化成大写
            string sign = kaola_sign.To32Md5(s).ToUpper();
           // string sign = kaola_sign.signValues(sb.ToString());            //签名

            string url = kaola_sign.kaola_Invok_URL(str_access_tokenli[2], str_app_keyli[2], method, sign, time);
            url = url + "&key_list=" + key_list;//接口地址
            WebRequest request = (WebRequest)HttpWebRequest.Create(url);   //调用接口
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader read = new StreamReader(stream, Encoding.UTF8);
            string data = read.ReadToEnd();

            return data;
        }


        //kaola_item_get
        /// <summary>
        /// 根据商品id获取单个商品的详细信息
        /// </summary>
        /// <param name="method">接口名称</param>
        /// <param name="time">时间戳</param>
        /// <param name="key">商品的key</param>
        /// <returns></returns>
        public string kaola_item_get(string method, string time, string key)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("access_token" + str_access_tokenli[2]);                                   //令牌
            sb.Append("app_key" + str_app_keyli[2]);                                        //密钥
            sb.Append("key" + key);                                        //商品的key
            sb.Append(str_method + method);                                //调用接口
            sb.Append(str_timestamp + time);                               //时间戳

            string s = string.Empty;
            string app_secret = app_secretli[2];
            s = app_secret + sb.ToString() + app_secret;

           string sign = kaola_sign.To32Md5(s).ToUpper();

            string url = kaola_sign.kaola_Invok_URL(str_access_tokenli[2], str_app_keyli[2], method, sign, time);
            url = url + "&key=" + key;//接口地址
            WebRequest request = (WebRequest)HttpWebRequest.Create(url);   //调用接口
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader read = new StreamReader(stream, Encoding.UTF8);
            string data = read.ReadToEnd();

            return data;
        }



        //kaola.item.batch.get(批量获取商品信息)
        /// <summary>
        /// 批量获取商品信息
        /// </summary>
        /// <param name="method">接口名称</param>
        /// <param name="time">时间戳</param>
        /// <param name="key_list">商品的key的集合</param>
        /// <returns></returns>
        public string kaola_item_batch_get(string method, string time, string[] key_list)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("access_token" + str_access_tokenli[2]);                                   //令牌
            sb.Append("app_key" + str_app_keyli[2]);                                        //密钥
            sb.Append("key_list" + key_list);                              //商品的key的集合
            sb.Append(str_method + method);                                //调用接口
            sb.Append(str_timestamp + time);                               //时间戳
            string s = string.Empty;
            string app_secret = app_secretli[2];
            s = app_secret + sb.ToString() + app_secret;
            string sign = kaola_sign.To32Md5(s).ToUpper();
            //string sign = kaola_sign.signValues(sb.ToString());            //签名

            string url = kaola_sign.kaola_Invok_URL(str_access_tokenli[2], str_app_keyli[2], method, sign, time);
            url = url + "&key_list=" + key_list;//接口地址
            WebRequest request = (WebRequest)HttpWebRequest.Create(url);   //调用接口
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader read = new StreamReader(stream, Encoding.UTF8);
            string data = read.ReadToEnd();

            return data;
        }


        /// <summary>
        /// 根据状态查询商品信息
        /// </summary>
        /// <param name="method">接口名称</param>
        /// <param name="time">时间戳</param>
        /// <param name="item_edit_status">商品的状态</param>
        /// <param name="page_no">默认为1</param>
        /// <param name="page_size">默认为20,最大为100</param>
        /// <returns></returns>
        public string kaola_item_batch_status_get(string method, string time, int item_edit_status, int page_no, int page_size)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("access_token" + str_access_tokenli[2]);                                   //令牌
            sb.Append("app_key" + str_app_keyli[2]);                                        //密钥
            sb.Append("item_edit_status" + item_edit_status);              //商品的key的集合
            sb.Append(str_method + method);                                //调用接口
            sb.Append("page_no" + page_no);                                //默认为1
            sb.Append("page_size" + page_size);                            //默认为20,最大为100
            sb.Append(str_timestamp + time);                               //时间戳
            string s = string.Empty;
            string app_secret = app_secretli[2];
            s = app_secret + sb.ToString() + app_secret;

            //使用MD5进行加密，再转化成大写
            string sign = kaola_sign.To32Md5(s).ToUpper();
            //string sign = kaola_sign.signValues(sb.ToString());            //签名

            string url = kaola_sign.kaola_Invok_URL(str_access_tokenli[2], str_app_keyli[2], method, sign, time);
            url = url + "&item_edit_status=" + item_edit_status + "&page_no=" + page_no + "&page_size=" + page_size;//接口地址
            WebRequest request = (WebRequest)HttpWebRequest.Create(url);   //调用接口
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader read = new StreamReader(stream, Encoding.UTF8);
            string data = read.ReadToEnd();

            return data;
        }


        /// <summary>
        /// 修改指定sku的库存
        /// </summary>
        /// <param name="method">接口名称</param>
        /// <param name="time">时间戳</param>
        /// <param name="key">Sku的key</param>
        /// <param name="stock">库存数</param>
        /// <returns></returns>
        public string kaola_item_sku_stock_update(string method, string time, string key, int stock)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("access_token" + str_access_tokenli[2]);                                   //令牌
            sb.Append("app_key" + str_app_keyli[2]);                                        //密钥
            sb.Append("key" + key);                                        //Sku的key
            sb.Append(str_method + method);                                //调用接口
            sb.Append("stock" + stock);                                    //库存数
            sb.Append(str_timestamp + time);                               //时间戳
            string s = string.Empty;
            string app_secret = app_secretli[2];
            s = app_secret + sb.ToString() + app_secret;

            //使用MD5进行加密，再转化成大写
            string sign = kaola_sign.To32Md5(s).ToUpper();
           // string sign = kaola_sign.signValues(sb.ToString());            //签名

            string url = kaola_sign.kaola_Invok_URL(str_access_tokenli[2], str_app_keyli[2], method, sign, time);
            url = url + "&key=" + key + "&stock=" + stock;//接口地址
            WebRequest request = (WebRequest)HttpWebRequest.Create(url);   //调用接口
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader read = new StreamReader(stream, Encoding.UTF8);
            string data = read.ReadToEnd();

            return data;
        }




        /// <summary>
        /// 修改指定sku的销售价
        /// </summary>
        /// <param name="method">接口名称</param>
        /// <param name="time">时间戳</param>
        /// <param name="key">Sku的key</param>
        /// <param name="sale_price">销售价，最多支持两位小数(即精确到分)，如果输入的数字为3位以上小数，系统会自动四舍五入成2位小数.</param>
        /// <returns></returns>
        public string kaola_item_sku_sale_price_update(string method, string time, string key, decimal sale_price)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("access_token" + str_access_tokenli[2]);                                   //令牌
            sb.Append("app_key" + str_app_keyli[2]);                                        //密钥
            sb.Append("key" + key);                                        //Sku的key
            sb.Append(str_method + method);                                //调用接口
            sb.Append("sale_price" + sale_price);                          //销售价，最多支持两位小数(即精确到分)，如果输入的数字为3位以上小数，系统会自动四舍五入成2位小数.
            sb.Append(str_timestamp + time);                               //时间戳
            string s = string.Empty;
            string app_secret = app_secretli[2];
            s = app_secret + sb.ToString() + app_secret;
            //使用MD5进行加密，再转化成大写
            string sign = kaola_sign.To32Md5(s).ToUpper();
            //string sign = kaola_sign.signValues(sb.ToString());            //签名

            string url = kaola_sign.kaola_Invok_URL(str_access_tokenli[2], str_app_keyli[2], method, sign, time);
            url = url + "&key=" + key + "&sale_price=" + sale_price;//接口地址
            WebRequest request = (WebRequest)HttpWebRequest.Create(url);   //调用接口
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader read = new StreamReader(stream, Encoding.UTF8);
            string data = read.ReadToEnd();

            return data;
        }






        /// <summary>
        /// 新增商品 kaola.item.add
        /// </summary>
        /// <param name="method">方法名称</param>
        /// <param name="time">时间戳</param>
        /// <param name="name">商品名称</param>
        /// <param name="sub_title">副标题</param>
        /// <param name="short_title">短标题</param>
        /// <param name="ten_words_desc">十字描述</param>
        /// <param name="item_NO">商品货号</param>
        /// <param name="brand_id">品牌id </param>
        /// <param name="original_country_code_id">原产国id</param>
        /// <param name="consign_area">发货地, 格式:省-市</param>
        /// <param name="consign_areaId">发货地id, 格式:省id-市id</param>
        /// <param name="description">详情描述</param>
        /// <param name="category_id">所属类目id </param>
        /// <param name="property_valueId_list">商品对应的预定义(下拉/单选/多选)属性值id列表，如果有多个属性值用|分隔</param>
        /// <param name="text_property_name_id">商品对应的自定义(单行/多行)文本框属性名id和属性值列表，属性名和属性值用^分隔，多个属性之间用|分隔</param>
        /// <param name="image_urls">商品图片url和类型列表，url和类型用^分隔,多个图片之间用|分隔, 商品类型1:商品主图片 2:APP标示</param>
        /// <param name="sku_market_prices">Sku市场价，多个sku的市场价用|分隔,支持2位小数，单位:元</param>
        /// <param name="sku_sale_prices">Sku销售价，多个sku的销售价用|分隔,支持2位小数，单位:元</param>
        /// <param name="sku_barcode">Sku条形码，多个sku的条形码用|分隔</param>
        /// <param name="sku_stock">Sku库存，整数，多个SKu的库存用|分隔</param>
        /// <param name="sku_property_value">Sku属性值Id和图片url,属性值id和图片url用^分隔，同一个sku不同的属性之间用,分隔，不同的sku属性之间用|分隔，只有颜色属性会有图片url</param>
        /// <returns></returns>
        public string kaola_item_add(string method, string time, string name, string sub_title, string short_title, string ten_words_desc, string item_NO, long brand_id, string original_country_code_id, string consign_area, string consign_areaId, string description, long category_id, string property_valueId_list, string text_property_name_id, string image_urls, string sku_market_prices = null, string sku_sale_prices = null, string sku_barcode = null, string sku_stock = null,string sku_property_value=null)//, string sku_property_value
        {


            //string sku_market_prices = null, string sku_sale_prices = null, string sku_barcode = null, string sku_stock = null

            StringBuilder sb = new StringBuilder(200);
            sb.Append("access_token" + str_access_tokenli[2]);                                   //令牌
            sb.Append("app_key" + str_app_keyli[2]);                                        //密钥
            sb.Append("brand_id" + brand_id);                              //品牌id
            sb.Append("category_id" + category_id);                        //所属类目id
            sb.Append("consign_area" + consign_area);                      //发货地
            sb.Append("consign_areaId" + consign_areaId);                  //发货地id
            sb.Append("description" + description);                        //详情描述
            sb.Append("image_urls" + image_urls);                          //商品图片url和类型列表，url和类型用^分隔,多个图片之间用|分隔, 商品类型1:商品主图片 2:APP标示      a.jpg^1|b.jpg^1|c.jpg^2
            sb.Append("item_NO" + item_NO);                                //商品货号
            sb.Append(str_method + method);                                //调用接口
            sb.Append("name" + name);                                        //商品名称
            sb.Append("original_country_code_id" + original_country_code_id);//原产国id

            #region 可选参数
            if (!string.IsNullOrWhiteSpace(property_valueId_list))
            {
                sb.Append("property_valueId_list" + property_valueId_list);    //商品对应的预定义(下拉/单选/多选)属性值id列表，如果有多个属性值用|分隔    例3|5|7
            }
            sb.Append("short_title" + short_title);                            //短标题
            if (!string.IsNullOrWhiteSpace(sku_barcode))
            {
                sb.Append("sku_barcode" + sku_barcode);                        //Sku条形码，多个sku的条形码用|分隔                            13123|234234|324234
            }
            if (!string.IsNullOrWhiteSpace(sku_market_prices))
            {
                sb.Append("sku_market_prices" + sku_market_prices);            //Sku市场价，多个sku的市场价用|分隔,支持2位小数，单位:元       100|200.12|300.22
            }
            if (!string.IsNullOrWhiteSpace(sku_property_value))
            {
                sb.Append("sku_property_value" + sku_property_value);           //Sku属性值Id和图片url,属性值id和图片url用^分隔， 同一个sku不同的属性之间用,分隔，不同的sku属性之间用|分隔，只有颜色属性会有图片url    101^a.jpg,102|103^b.jpg,104|105^c.jpg,106
            }
            if (!string.IsNullOrWhiteSpace(sku_sale_prices))
            {
                sb.Append("sku_sale_prices" + sku_sale_prices);                 //Sku销售价，多个sku的销售价用|分隔,支持2位小数，单位:元       100|200.12|300.22
            }
            if (!string.IsNullOrWhiteSpace(sku_stock))
            {
                sb.Append("sku_stock" + sku_stock);                             //Sku库存，整数，多个SKu的库存用|分隔                          100|200|300
            }
            #endregion

            description = UrlEncode(description);                               //URL转换，这里指详情描述（详情描述中带有图片，所以会使用到html标签，那么在此处就需要url编码）--------------------------------
            
            sb.Append("sub_title" + sub_title);                                 //副标题
            sb.Append("ten_words_desc" + ten_words_desc);                       //十字描述
            if (!string.IsNullOrWhiteSpace(text_property_name_id))
            {
                sb.Append("text_property_name_id" + text_property_name_id);     //商品对应的自定义(单行/多行)文本框属性名id和属性值列表，属性名和属性值用^分隔，多个属性之间用|分隔     13^text1|15^text2|17^text3
            }
            sb.Append(str_timestamp + time);                                    //时间戳

            string s = string.Empty;
            string app_secret = app_secretli[2];
            s = app_secret + sb.ToString() + app_secret;
            string sign = kaola_sign.To32Md5(s).ToUpper();
            //string sign = kaola_sign.signValues(sb.ToString());
            string url = kaola_sign.kaola_Invok_URL(str_access_tokenli[2], str_app_keyli[2], method, sign, time);

            url = url + "&brand_id=" + brand_id + "&category_id=" + category_id + "&consign_area=" + consign_area + "&consign_areaId="
                + consign_areaId + "&description=" + description + "&image_urls=" + image_urls + "&item_NO=" + item_NO + "&name=" + name
                + "&original_country_code_id=" + original_country_code_id + "&sub_title=" + sub_title + "&ten_words_desc=" + ten_words_desc + "&short_title=" + short_title;
            
            
            #region 可选参数
            if (!string.IsNullOrWhiteSpace(property_valueId_list))
            {
                url += "&property_valueId_list=" + property_valueId_list;
            }
            if (!string.IsNullOrWhiteSpace(sku_barcode))
            {
                url += "&sku_barcode=" + sku_barcode;
            }
            if (!string.IsNullOrWhiteSpace(sku_market_prices))
            {
                url += "&sku_market_prices=" + sku_market_prices;
            }
            if (!string.IsNullOrWhiteSpace(sku_property_value))
            {
                url += "&sku_property_value=" + sku_property_value;
            }
            if (!string.IsNullOrWhiteSpace(sku_sale_prices))
            {
                url += "&sku_sale_prices=" + sku_sale_prices;
            }
            if (!string.IsNullOrWhiteSpace(sku_stock))
            {
                url += "&sku_stock=" + sku_stock;
            }
            if (!string.IsNullOrWhiteSpace(text_property_name_id))
            {
                url += "&text_property_name_id=" + text_property_name_id;
            }
            #endregion



            WebRequest request = (WebRequest)HttpWebRequest.Create(url);        //调用接口
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader read = new StreamReader(stream, Encoding.UTF8);
            string data = read.ReadToEnd();
            return data;
        }


        /// <summary>
        /// URL转换
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }


        /// <summary>
        /// 添加商品的sku   kaola.item.sku.add
        /// </summary>
        /// <param name="method">接口名称</param>
        /// <param name="time">时间戳</param>
        /// <param name="item_key">所属商品的key</param>
        /// <param name="sku_market_prices">Sku市场价，多个sku的市场价用|分隔,支持2位小数，单位:元</param>
        /// <param name="sku_sale_prices">Sku销售价，多个sku的销售价用|分隔,支持2位小数，单位:元</param>
        /// <param name="sku_barcode">Sku条形码，多个sku的条形码用|分隔</param>
        /// <param name="sku_stock">Sku库存，整数，多个SKu的库存用|分隔</param>
        /// <param name="sku_property_value">Sku属性值Id和图片url,属性值id和图片url用^分隔， 同一个sku不同的属性之间用,分隔，不同的sku属性之间用|分隔</param>
        /// <returns></returns>
        public string kaola_item_sku_add(string method, string time, string item_key, string sku_market_prices, string sku_sale_prices, string sku_barcode, string sku_stock, string sku_property_value=null)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("access_token" + str_access_tokenli[2]);                                   //令牌
            sb.Append("app_key" + str_app_keyli[2]);                                        //密钥
            sb.Append("item_key" + item_key);                              //所属商品的key
            sb.Append(str_method + method);                                //调用接口
            sb.Append("sku_barcode" + sku_barcode);                        //Sku条形码，多个sku的条形码用|分隔
            sb.Append("sku_market_prices" + sku_market_prices);            //Sku市场价，多个sku的市场价用|分隔,支持2位小数，单位:元
            if (!string.IsNullOrWhiteSpace(sku_property_value))
            {
                sb.Append("sku_property_value" + sku_property_value);          //Sku属性值Id和图片url,属性值id和图片url用^分隔， 同一个sku不同的属性之间用,分隔，不同的sku属性之间用|分隔(101^a.jpg,102|103^b.jpg,104|105^c.jpg,106)
            }
            sb.Append("sku_sale_prices" + sku_sale_prices);                //Sku销售价，多个sku的销售价用|分隔,支持2位小数，单位:元
            sb.Append("sku_stock" + sku_stock);                            //Sku库存，整数，多个SKu的库存用|分隔
            sb.Append(str_timestamp + time);                               //时间戳

            string s = string.Empty;
            string app_secret = app_secretli[2];
            s = app_secret + sb.ToString() + app_secret;
            string sign = kaola_sign.To32Md5(s).ToUpper();

            //string sign = kaola_sign.signValues(sb.ToString());            //签名
            string url = kaola_sign.kaola_Invok_URL(str_access_tokenli[2], str_app_keyli[2], method, sign, time);
            url = url + "&item_key=" + item_key + "&sku_barcode=" + sku_barcode + "&sku_market_prices=" + sku_market_prices + "&sku_sale_prices=" + sku_sale_prices + "&sku_stock=" + sku_stock;//接口地址

            if (!string.IsNullOrWhiteSpace(sku_property_value))
            {
                url += "&sku_property_value=" + sku_property_value;
            }

            WebRequest request = (WebRequest)HttpWebRequest.Create(url);   //调用接口
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader read = new StreamReader(stream, Encoding.UTF8);
            string data = read.ReadToEnd();

            return data;
        }



        /// <summary>
        /// 上传产品图片  kaola.item.img.upload
        /// </summary>
        /// <param name="method">接口名称</param>
        /// <param name="time">时间戳</param>
        /// <param name="pic">图片字节数组</param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string kaola_item_img_upload(string method, string time, byte[] pic, string filePath)
        {



            NameValueCollection data = new NameValueCollection();
            data.Add("method", "kaola.item.img.upload");
            data.Add("access_token", "c9aadff0-8ee9-43ef-8cbc-59be572c0c2f");
            data.Add("app_key", "my-client");
            data.Add("timestamp", "2015-12-17 10:28:38");
            data.Add("sign", "E376A3B8121932F379EFFF85766D0DAE");

            StringBuilder sb = new StringBuilder();
            sb.Append(str_access_token);                                   //令牌
            sb.Append(str_app_key);                                        //密钥
            //sb.Append("filePath" + filePath);
            sb.Append(str_method + method);                                //调用接口
            //sb.Append("pic" + pic);                                        //文件内容--------------------
            sb.Append(str_timestamp + time);                               //时间戳

            string sign = kaola_sign.signValues(sb.ToString());            //签名

            string url = "http://openapi.kaola.com/router";

            return HttpPostData(url, 30000, "", "C:\\5_33521LIBAJF.jpg",sign);
            //string url = kaola_sign.kaola_Invok_URL(access_token, app_key, method, sign, time);
            //url = url + "&pic[]=" + pic + "&filePath=" + filePath;//接口地址
            //url = url + "&filePath=" + filePath;//接口地址  + "&pic[]=" + "123456"



            //WebRequest request = (WebRequest)HttpWebRequest.Create(url);   //调用接口
            //request.Method = "post";
            //WebClient client = new WebClient();
            //byte[] content = client.DownloadData("http://best-bms.pbxluxury.com/images/COH/L0100/33521/33521LIBAJF/5_33521LIBAJF.jpg");
            ////byte[] content = client.DownloadData(filePath);
            ////request.ContentLength = Encoding.UTF8.GetByteCount(content);
            ////request.ContentLength = content.Length;
            ////request.ContentType = "application/x-jpg";
            //request.ContentType = "multipart/form-data";

            ////?access_token=214f66a9-e3f9-48ec-9e70-196fc6aa63fb&app_key=edb6c3b9ac4847e7584c38e2b630b14f&method=kaola.item.img.upload&sign=30C36002898E74BEEFA0B04296569B45&timestamp=2015-12-17%2009:46:55
            //Stream myRequestStream = request.GetRequestStream();
            //StreamWriter myStreamWriter = new StreamWriter(myRequestStream);
            //myStreamWriter.Write("?access_token=214f66a9-e3f9-48ec-9e70-196fc6aa63fb&app_key=edb6c3b9ac4847e7584c38e2b630b14f&method=kaola.item.img.upload&sign=30C36002898E74BEEFA0B04296569B45&timestamp=2015-12-17%2009:46:55" + "&pic=" + content);
            //myStreamWriter.Close();

            //WebResponse response = request.GetResponse();
            //Stream stream = response.GetResponseStream();
            //StreamReader read = new StreamReader(stream, Encoding.UTF8);
            //string data = read.ReadToEnd();
            //return data;

            //return "";
        }





        //private static string HttpPostData(string url, int timeOut, string fileKeyName,string filePath, NameValueCollection stringDict)
        private static string HttpPostData(string url, int timeOut, string fileKeyName,string filePath,string sign)
        {

            string responseContent;
            var memStream = new MemoryStream();
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            // 边界符
            var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
            // 边界符
            var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            
            // 最后的结束符
            var endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");

            // 设置属性
            webRequest.Method = "POST";
            webRequest.Timeout = timeOut;
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;

            // 写入文件
            const string filePartHeader ="Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +"Content-Type: application/octet-stream\r\n\r\n";
            var header = string.Format(filePartHeader, fileKeyName, filePath);
            var headerbytes = Encoding.UTF8.GetBytes(header);

            memStream.Write(beginBoundary, 0, beginBoundary.Length);
            memStream.Write(headerbytes, 0, headerbytes.Length);

            var buffer = new byte[1024];
            int bytesRead; // =0

            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                memStream.Write(buffer, 0, bytesRead);
            }

            // 写入字符串的Key
            var stringKeyHeader = "\r\n--" + boundary +
                                   "\r\nContent-Disposition: form-data; name=\"{0}\"" +"\r\n\r\n{1}\r\n";
            var formitembytes = Encoding.UTF8.GetBytes(string.Format(stringKeyHeader, "app_key", "edb6c3b9ac4847e7584c38e2b630b14f"));
            memStream.Write(formitembytes, 0, formitembytes.Length);
            var formitembytes1 = Encoding.UTF8.GetBytes(string.Format(stringKeyHeader, "access_token", "214f66a9-e3f9-48ec-9e70-196fc6aa63fb"));
            memStream.Write(formitembytes1, 0, formitembytes1.Length);
            var formitembytes2 = Encoding.UTF8.GetBytes(string.Format(stringKeyHeader, "app_secret", "8200ee92ec22fcae76e2f00bc5c79247188e0593"));
            memStream.Write(formitembytes2, 0, formitembytes2.Length);
            var formitembytes3 = Encoding.UTF8.GetBytes(string.Format(stringKeyHeader, "timestamp", kaola_parameter.getDateTimeNow()));
            memStream.Write(formitembytes3, 0, formitembytes3.Length);
            var formitembytes4 = Encoding.UTF8.GetBytes(string.Format(stringKeyHeader, "sign", sign));
            memStream.Write(formitembytes4, 0, formitembytes4.Length);

            //var stringKeyHeader1 = "\r\n--" + boundary +
            //                       "\r\nContent-Disposition: form-data; name=\"{0}\"" + "\r\n\r\n{1}\r\n";
            //var formitembytes1 = Encoding.UTF8.GetBytes(string.Format(stringKeyHeader1, "access_token", "214f66a9-e3f9-48ec-9e70-196fc6aa63fb"));
            //memStream.Write(formitembytes1, 0, formitembytes1.Length);

            //foreach (byte[] formitembytes in from string key in stringDict.Keys
            //                                 select string.Format(stringKeyHeader, key, stringDict[key])
            //                                     into formitem
            //                                     select Encoding.UTF8.GetBytes(formitem))
            //{
            //    memStream.Write(formitembytes, 0, formitembytes.Length);
            //}

            // 写入最后的结束边界符
            memStream.Write(endBoundary, 0, endBoundary.Length);



            webRequest.ContentLength = memStream.Length;

            var requestStream = webRequest.GetRequestStream();

            memStream.Position = 0;
            var tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();

            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();

            var httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

            using (var httpStreamReader = new StreamReader(httpWebResponse.GetResponseStream(),Encoding.GetEncoding("utf-8")))
            {
                responseContent = httpStreamReader.ReadToEnd();
            }

            fileStream.Close();
            httpWebResponse.Close();
            webRequest.Abort();

            return responseContent;
        }





    }
}
