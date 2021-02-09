/**  版本信息模板在安装目录下，可自行修改。
* bms_kaola_itemtextproperty.cs
*
* 功 能： N/A
* 类 名： bms_kaola_itemtextproperty
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
	/// bms_kaola_itemtextproperty:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bms_kaola_itemtextproperty
	{
		public bms_kaola_itemtextproperty()
		{}
		#region Model
		private long _id=0;
		private long? _item_id;
		private long? _business_id;
		private string _prop_name_id;
		private string _prop_name_cn;
		private string _text_value;
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
		public string text_value
		{
			set{ _text_value=value;}
			get{return _text_value;}
		}
		#endregion Model

	}
}

