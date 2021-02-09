using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaola.api.invok.basicMsgModel
{
    public class ProvinceModel
    {
        public List<Province> kaola_common_provinces_get_response { get; set; }
    }

    /// <summary>
    /// 省份信息
    /// </summary>
    public class Province
    {
        /// <summary>
        /// 省份code
        /// </summary>
        public long province_code { get; set; }

        /// <summary>
        /// 省名称
        /// </summary>
        public string province_name { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int order_value { get; set; }
    }
}
