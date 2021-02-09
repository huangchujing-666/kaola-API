/**  版本信息模板在安装目录下，可自行修改。
* bms_kaola_rawpropertycategory.cs
*
* 功 能： N/A
* 类 名： bms_kaola_rawpropertycategory
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
	/// bms_kaola_rawpropertycategory:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bms_kaola_rawpropertycategory
	{
		public bms_kaola_rawpropertycategory()
		{}
		#region Model
		private string _property_name_id;
		private long _category_id=0;
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
		public long category_id
		{
			set{ _category_id=value;}
			get{return _category_id;}
		}
		#endregion Model

	}
}

