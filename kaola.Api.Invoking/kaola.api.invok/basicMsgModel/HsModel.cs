using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaola.api.invok.basicMsgModel
{
    public class HsModel
    {
        public List<Hs> kaola_common_hscodes_get_response { get; set; }
    }

    public class Hs
    {
        /// <summary>
        /// Hs编码code
        /// </summary>
        public string hs_code { get; set; }

        /// <summary>
        /// 国家分类名称和HS编码组合唯一
        /// </summary>
        public string hs_key { get; set; }

        /// <summary>
        /// 国家分类名
        /// </summary>
        public string national_category { get; set;}

        /// <summary>
        /// 增值税号
        /// </summary>
        public string vat_code { get; set; }

        /// <summary>
        /// 消费税号
        /// </summary>
        public string consumer_code { get; set; }
    }
}
