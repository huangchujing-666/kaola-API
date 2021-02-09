/**  版本信息模板在安装目录下，可自行修改。
* bms_kaola_propertyvalue.cs
*
* 功 能： N/A
* 类 名： bms_kaola_propertyvalue
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/12/14 15:17:36   N/A    初版
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
	/// bms_kaola_propertyvalue:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bms_kaola_propertyvalue
	{
		public bms_kaola_propertyvalue()
		{}
		#region Model
		private string _property_value_id;
		private string _property_value;
		private string _property_name_id;
		private string _property_value_icon;
		private int? _is_sys_property;
		private int? _show_order;
		private int? _status;
		/// <summary>
		/// 
		/// </summary>
		public string property_value_id
		{
			set{ _property_value_id=value;}
			get{return _property_value_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string property_value
		{
			set{ _property_value=value;}
			get{return _property_value;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string property_name_id
		{
			set{ _property_name_id=value;}
			get{return _property_name_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string property_value_icon
		{
			set{ _property_value_icon=value;}
			get{return _property_value_icon;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? is_sys_property
		{
			set{ _is_sys_property=value;}
			get{return _is_sys_property;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? show_order
		{
			set{ _show_order=value;}
			get{return _show_order;}
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

