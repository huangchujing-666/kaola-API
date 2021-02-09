/**  版本信息模板在安装目录下，可自行修改。
* bms_kaola_rawitemedit.cs
*
* 功 能： N/A
* 类 名： bms_kaola_rawitemedit
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
	/// bms_kaola_rawitemedit:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bms_kaola_rawitemedit
	{
		public bms_kaola_rawitemedit()
		{}
		#region Model
		private long? _id;
		private long? _business_id;
		private string _name;
		private string _sub_title;
		private string _short_title;
		private string _ten_words_desc;
		private string _item_no;
		private string _brand_id;
		private string _original_country_code_id;
		private string _consign_area;
		private string _consign_area_id;
		private string _description;
		private int? _item_edit_status;
		/// <summary>
		/// 
		/// </summary>
		public long? id
		{
			set{ _id=value;}
			get{return _id;}
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
		public string name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sub_title
		{
			set{ _sub_title=value;}
			get{return _sub_title;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string short_title
		{
			set{ _short_title=value;}
			get{return _short_title;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ten_words_desc
		{
			set{ _ten_words_desc=value;}
			get{return _ten_words_desc;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string item_no
		{
			set{ _item_no=value;}
			get{return _item_no;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string brand_id
		{
			set{ _brand_id=value;}
			get{return _brand_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string original_country_code_id
		{
			set{ _original_country_code_id=value;}
			get{return _original_country_code_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string consign_area
		{
			set{ _consign_area=value;}
			get{return _consign_area;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string consign_area_id
		{
			set{ _consign_area_id=value;}
			get{return _consign_area_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string description
		{
			set{ _description=value;}
			get{return _description;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? item_edit_status
		{
			set{ _item_edit_status=value;}
			get{return _item_edit_status;}
		}
		#endregion Model

	}
}

