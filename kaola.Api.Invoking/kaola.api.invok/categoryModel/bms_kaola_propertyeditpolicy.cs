/**  版本信息模板在安装目录下，可自行修改。
* bms_kaola_propertyeditpolicy.cs
*
* 功 能： N/A
* 类 名： bms_kaola_propertyeditpolicy
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
	/// bms_kaola_propertyeditpolicy:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bms_kaola_propertyeditpolicy
	{
		public bms_kaola_propertyeditpolicy()
		{}
		#region Model
		private string _property_name_id;
		private int? _input_type;
		private string _desc;
		private int? _max_len;
		private int? _is_multichoice;
		private int? _need_image;
		private int? _is_necessary;
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
		public int? input_type
		{
			set{ _input_type=value;}
			get{return _input_type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string desc
		{
			set{ _desc=value;}
			get{return _desc;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? max_len
		{
			set{ _max_len=value;}
			get{return _max_len;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? is_multichoice
		{
			set{ _is_multichoice=value;}
			get{return _is_multichoice;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? need_image
		{
			set{ _need_image=value;}
			get{return _need_image;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? is_necessary
		{
			set{ _is_necessary=value;}
			get{return _is_necessary;}
		}
		#endregion Model

	}
}

