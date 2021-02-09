/**  版本信息模板在安装目录下，可自行修改。
* bms_kaola_rawpropertyname.cs
*
* 功 能： N/A
* 类 名： bms_kaola_rawpropertyname
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
	/// bms_kaola_rawpropertyname:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bms_kaola_rawpropertyname
	{
		public bms_kaola_rawpropertyname()
		{}
		#region Model
		private string _prop_name_id;
		private string _prop_name_cn;
		private string _prop_name_en;
		private int? _is_sku;
		private int? _is_filter;
		private int? _is_display;
		private int? _is_color;
		private int? _is_logistics;
		private int? _status;
		/// <summary>
		/// 
		/// </summary>
		public string prop_name_id
		{
			set{ _prop_name_id=value;}
			get{return _prop_name_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string prop_name_cn
		{
			set{ _prop_name_cn=value;}
			get{return _prop_name_cn;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string prop_name_en
		{
			set{ _prop_name_en=value;}
			get{return _prop_name_en;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? is_sku
		{
			set{ _is_sku=value;}
			get{return _is_sku;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? is_filter
		{
			set{ _is_filter=value;}
			get{return _is_filter;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? is_display
		{
			set{ _is_display=value;}
			get{return _is_display;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? is_color
		{
			set{ _is_color=value;}
			get{return _is_color;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? is_logistics
		{
			set{ _is_logistics=value;}
			get{return _is_logistics;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? status
		{
			set{ _status=value;}
			get{return _status;}
		}
		#endregion Model

	}
}

