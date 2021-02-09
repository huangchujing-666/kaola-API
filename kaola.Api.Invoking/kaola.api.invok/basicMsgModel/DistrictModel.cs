using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaola.api.invok.basicMsgModel
{
    public class DistrictModel {
        public List<District> kaola_common_district_get_response { get; set; }
    }
   public  class District
    {  

       /// <summary>
       /// 区域编号
       /// </summary>
       public string district_code { get; set; }

       /// <summary>
       /// 区域名称
       /// </summary>
       public string district_name { get; set; }


       /// <summary>
       /// 城市编号
       /// </summary>
       public string city_code { get; set; }

       /// <summary>
       /// 排序
       /// </summary>
       public int order_value { get; set; }
    }
}
