/**  版本信息模板在安装目录下，可自行修改。
* bms_kaola_rawsku.cs
*
* 功 能： N/A
* 类 名： bms_kaola_rawsku
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/12/14 15:17:37   N/A    初版
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
	/// bms_kaola_rawsku:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bms_kaola_rawsku
	{
		public bms_kaola_rawsku()
		{}
		#region Model
		private long _id=0;
		private long? _item_id;
		private long? _business_id;
		private decimal? _market_price;
		private decimal? _sale_price;
		private string _bar_code;
		private int? _stock_can_sale;
		private int? _stock_freeze;
		/// <summary>
		/// 
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
		public decimal? market_price
		{
			set{ _market_price=value;}
			get{return _market_price;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? sale_price
		{
			set{ _sale_price=value;}
			get{return _sale_price;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string bar_code
		{
			set{ _bar_code=value;}
			get{return _bar_code;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? stock_can_sale
		{
			set{ _stock_can_sale=value;}
			get{return _stock_can_sale;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? stock_freeze
		{
			set{ _stock_freeze=value;}
			get{return _stock_freeze;}
		}
		#endregion Model

	}
}

