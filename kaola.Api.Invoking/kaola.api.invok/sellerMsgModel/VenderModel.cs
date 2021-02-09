using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaola.api.invok.sellerMsgModel
{
    public class VenderModel
    {
        public vender_info1 kaola_vender_info_get_response { get; set; }
          
    }
    public class vender_info1
    {
        public vender_info vender_info { get; set; }
    }
    /// <summary>
    /// 商家基本信息
    /// </summary>    
    public class vender_info
        {
            /// <summary>
            /// 主营范围
            /// </summary>
            public string business_scope { get; set; }

            /// <summary>
            /// 退货详细地址
            /// </summary>
            public string refund_address { get; set; }

            /// <summary>
            /// 退货地址城市
            /// </summary>
            public string refund_city { get; set; }


            /// <summary>
            /// 退货地址县区
            /// </summary>
            public string refund_district { get; set; }

            /// <summary>
            /// 退货手机号码
            /// </summary>
            public string refund_mobile { get; set; }

            /// <summary>
            /// 退货收件人姓名
            /// </summary>
            public string refund_name { get; set; }


            /// <summary>
            /// 退货地址省份
            /// </summary>
            public string refund_province { get; set; }


            /// <summary>
            /// 退货固定电话
            /// </summary>
            public string refund_tel { get; set; }


            /// <summary>
            /// 商家别名
            /// </summary>
            public string vender_alias { get; set; }


            /// <summary>
            /// 商家id
            /// </summary>
            public long vender_id { get; set; }

            /// <summary>
            /// 商家名称
            /// </summary>
            public string vender_name { get; set; }


        }

}

