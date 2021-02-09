using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaola.api.invok.basicMsgModel
{
    public class CountryModel
    {
        public List<Country> kaola_common_countries_get_response { get; set; }
    }

    /// <summary>
    /// 国家信息对象
    /// </summary>
    public class Country
    {
        /// <summary>
        /// 国家code
        /// </summary>
        public string country_code { get; set; }

        /// <summary>
        /// 国家名称
        /// </summary>
        public string country_name { get; set; }
    }
}
