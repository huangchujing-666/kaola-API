/**  版本信息模板在安装目录下，可自行修改。
* bms_kaola_item.cs
*
* 功 能： N/A
* 类 名： bms_kaola_item
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
	/// bms_kaola_item:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bms_kaola_item
	{
		public bms_kaola_item()
		{}
		#region Model
		private string _name;
		private string _sub_title;
		private string _short_title;
		private string _ten_words_desc;
		private string _item_no;
		private long _brand_id;
		private string _original_country_code_id;
		private string _consign_area;
		private string _consign_areaid;
		private string _description;
		private long _category_id;
		private string _property_values_list;
		private string _text_property_name_id;
		private string _image_urls;
		private string _sku_market_prices;
		private string _sku_sale_prices;
		private string _sku_barcode;
		private string _sku_stock;
		private string _sku_property_value;
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
		public long brand_id
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
		public string consign_areaId
		{
			set{ _consign_areaid=value;}
			get{return _consign_areaid;}
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
		public long category_id
		{
			set{ _category_id=value;}
			get{return _category_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string property_values_list
		{
			set{ _property_values_list=value;}
			get{return _property_values_list;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string text_property_name_id
		{
			set{ _text_property_name_id=value;}
			get{return _text_property_name_id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string image_urls
		{
			set{ _image_urls=value;}
			get{return _image_urls;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sku_market_prices
		{
			set{ _sku_market_prices=value;}
			get{return _sku_market_prices;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sku_sale_prices
		{
			set{ _sku_sale_prices=value;}
			get{return _sku_sale_prices;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sku_barcode
		{
			set{ _sku_barcode=value;}
			get{return _sku_barcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sku_stock
		{
			set{ _sku_stock=value;}
			get{return _sku_stock;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sku_property_value
		{
			set{ _sku_property_value=value;}
			get{return _sku_property_value;}
		}
		#endregion Model

	}
}

