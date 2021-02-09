using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kaola.api.invok.sellerMsgModel
{
    public class CateItem
    {
        public List<Category> Item_cats { get; set; }
    }

    /// <summary>
    /// 商家类目
    /// </summary>
    public class Category
    {
        /// <summary>
        /// 类目id
        /// </summary>
        public long category_id { get; set; }

        /// <summary>
        /// 类目名称
        /// </summary>
        public string category_name { get; set; }

        /// <summary>
        /// 父目录id
        /// </summary>
        public long parent_id { get; set; }

        /// <summary>
        /// 类目所在层级  目前一级类目为2，二级类目为3，依此类推
        /// </summary>
        public int category_level { get; set; }

        /// <summary>
        /// 是否末级类目，1:末级类目 0:非末级类目
        /// </summary>
        public int is_leaf { get; set; }
    }
}
