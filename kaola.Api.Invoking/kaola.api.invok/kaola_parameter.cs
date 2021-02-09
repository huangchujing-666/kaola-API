using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaola.api.invok
{
    public class kaola_parameter
    {
        #region 正式密钥
        //店铺1
        //appKey：b8d6f0d18f891816f41aca3aff1536b8
        //appSecret：f48cb8e3845d609c8290ae319cfe9a2cc817eda4
        //access_token：ba6139b5-34f0-44a1-835d-4d9568a2ecfe

        //店铺2
        //appKey：eb2654b0c9a2ba09aaa322e4fb99d3e3
        //appSecret：651165f050b9fb8b7b0680c95fb89b0486fcde2c
        //access_token：8f12dce1-c049-45a8-83f0-8d5a3cefc886

        #endregion

        #region 测试密钥
        ///// <summary>
        ///// 令牌(214f66a9-e3f9-48ec-9e70-196fc6aa63fb)
        ///// </summary>
        //protected static string access_token = "214f66a9-e3f9-48ec-9e70-196fc6aa63fb"; 

        ///// <summary>
        ///// 密钥(edb6c3b9ac4847e7584c38e2b630b14f)
        ///// </summary>
        //protected static string app_key = "edb6c3b9ac4847e7584c38e2b630b14f";

        ///// <summary>
        ///// 密钥（8200ee92ec22fcae76e2f00bc5c79247188e0593）
        ///// </summary>
        //protected static string app_secret = "8200ee92ec22fcae76e2f00bc5c79247188e0593";

        ///// <summary>
        ///// 调用地址(http://openapi.kaola.com/router?)
        ///// </summary>
        //protected static string api_url = "http://openapi.kaola.com/router?";



        ///// <summary>
        ///// 加密令牌
        ///// </summary>
        //protected string str_access_token = "access_token214f66a9-e3f9-48ec-9e70-196fc6aa63fb";

        ///// <summary>
        ///// 加密密钥
        ///// </summary>
        //protected string str_app_key = "app_keyedb6c3b9ac4847e7584c38e2b630b14f";

        ///// <summary>
        ///// 加密时间戳
        ///// </summary>
        //protected string str_timestamp = "timestamp";

        ///// <summary>
        ///// 加密接口名称
        ///// </summary>
        //protected string str_method = "method";
        #endregion



        /// <summary>
        /// 令牌(214f66a9-e3f9-48ec-9e70-196fc6aa63fb)
        /// </summary>
        protected static string access_token = System.Configuration.ConfigurationManager.AppSettings["access_token"].ToString();

        /// <summary>
        /// 密钥(edb6c3b9ac4847e7584c38e2b630b14f)
        /// </summary>
        protected static string app_key = System.Configuration.ConfigurationManager.AppSettings["app_key"].ToString();

        /// <summary>
        /// 密钥（8200ee92ec22fcae76e2f00bc5c79247188e0593）
        /// </summary>
        protected static string app_secret = System.Configuration.ConfigurationManager.AppSettings["app_secret"].ToString();

        /// <summary>
        /// 调用地址(http://openapi.kaola.com/router?)
        /// </summary>
        protected static string api_url = System.Configuration.ConfigurationManager.AppSettings["api_url"].ToString();




        /// <summary>
        /// 加密令牌
        /// </summary>
        protected string str_access_token = "access_token"+access_token;

        /// <summary>
        /// 加密令牌数组
        /// </summary>
        protected string[] str_access_tokenli = new string[] { access_token, "8f12dce1-c049-45a8-83f0-8d5a3cefc886", "922033ef-f8a9-4e17-8260-c1b2eaf65aa1" };

        /// <summary>
        /// 加密密钥
        /// </summary>
        protected string str_app_key = "app_key"+app_key;

        /// <summary>
        /// 加密密钥数组
        /// </summary>
        protected string[] str_app_keyli = new string[] { app_key, "eb2654b0c9a2ba09aaa322e4fb99d3e3", "f037b9f5f52d7ae644f72abb91e5fde0" };

        /// <summary>
        /// 密钥数组
        /// </summary>
        protected string[] app_secretli = new string[] { "f48cb8e3845d609c8290ae319cfe9a2cc817eda4", "651165f050b9fb8b7b0680c95fb89b0486fcde2c", "02fe3f1d4b97801fd619bd36420ef9db28976633" };

        /// <summary>
        /// 加密时间戳
        /// </summary>
        protected string str_timestamp = "timestamp";

        /// <summary>
        /// 加密接口名称
        /// </summary>
        protected string str_method = "method";


        public static string getDateTimeNow()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

    }
}
