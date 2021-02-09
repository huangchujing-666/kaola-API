using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaola.api.invok.basicMsgModel
{
    public class CityModel
    {
        public List<City> kaola_common_city_get_response { get; set; }
    }

    /// <summary>
    /// 城市信息
    /// </summary>
    public class City
    {
        /// <summary>
        /// 城市code
        /// </summary>
        public long city_code { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        public string city_name { get; set; }

        /// <summary>
        /// 所属的省code
        /// </summary>
        public long parent_code { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int order_value { get; set; }
    }
}
