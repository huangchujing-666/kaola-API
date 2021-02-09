using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaola.api.invok.sellerMsgModel
{
    public class BrandModel
    {
        public BrandItem kaola_vender_brand_get_response { get; set; }
    }

    public class BrandItem
    {
        public List<BrandInfo> brand_list { get; set; }
    }

    /// <summary>
    /// 商家授权的品牌列表
    /// </summary>
    public class BrandInfo
    {
        /// <summary>
        /// 品牌id
        /// </summary>
        public long brand_id { get; set; }

        /// <summary>
        /// 品牌名称
        /// </summary>
        public string brand_name { get; set; }
    }
 }
