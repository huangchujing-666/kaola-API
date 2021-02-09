/**  版本信息模板在安装目录下，可自行修改。
* bms_kaola_itemimage.cs
*
* 功 能： N/A
* 类 名： bms_kaola_itemimage
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/12/14 15:17:34   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace Maticsoft.Model
{
	/// <summary>
	/// bms_kaola_itemimage:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bms_kaola_itemimage
	{
		public bms_kaola_itemimage()
		{}
		#region Model
		private long _id;
		private long? _item_id;
		private long? _business_id;
		private string _image_url;
        private string _image_type;
		private int? _order_value;
		/// <summary>
		/// auto_increment
		/// </summary>
		public long id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long? item_id
		{
			set{ _item_id=value;}
			get{return _item_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public long? business_id
		{
			set{ _business_id=value;}
			get{return _business_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string image_url
		{
			set{ _image_url=value;}
			get{return _image_url;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string image_type
		{
			set{ _image_type=value;}
			get{return _image_type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? order_value
		{
			set{ _order_value=value;}
			get{return _order_value;}
		}
		#endregion Model

	}
}

