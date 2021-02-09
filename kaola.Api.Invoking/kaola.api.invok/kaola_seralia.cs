using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace kaola.api.invok
{
    /// <summary>
    /// 对象序列化与反序列化
    /// </summary>
    public static class kaola_seralia
    {
        /// <summary>
        /// 将对象序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ScriptSerialize<T>(T t)
        {
            //ssssssssssssssssssssssss
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(t);   
        }

        /// <summary>
        /// 将对象反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static T ScriptDeserialize<T>(string strJson)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize<T>(strJson);
        }
    }
}
