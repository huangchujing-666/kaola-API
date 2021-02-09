/**  版本信息模板在安装目录下，可自行修改。
* bms_kaola_itempostage.cs
*
* 功 能： N/A
* 类 名： bms_kaola_itempostage
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/12/14 15:17:35   N/A    初版
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
	/// bms_kaola_itempostage:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bms_kaola_itempostage
	{
		public bms_kaola_itempostage()
		{}
		#region Model
		private long _id;
		private long? _item_id;
		private long? _business_id;
		private int? _is_postage_free;
		private decimal? _post_fee;
		private decimal? _express_fee;
		private decimal? _ems_fee;
		private string _postage_template_id;
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
		public int? is_postage_free
		{
			set{ _is_postage_free=value;}
			get{return _is_postage_free;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? post_fee
		{
			set{ _post_fee=value;}
			get{return _post_fee;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? express_fee
		{
			set{ _express_fee=value;}
			get{return _express_fee;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? ems_fee
		{
			set{ _ems_fee=value;}
			get{return _ems_fee;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string postage_template_id
		{
			set{ _postage_template_id=value;}
			get{return _postage_template_id;}
		}
		#endregion Model

	}
}

