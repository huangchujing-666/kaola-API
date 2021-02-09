using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kaola.api.invok.orderModel
{

    public class _kaola_order_search_response
    {
        private _kaola_order_search_response kaola_order_search_response;

        public _kaola_order_search_response Kaola_order_search_response
        {
            get { return kaola_order_search_response; }
            set { kaola_order_search_response = value; }
        }
    }

    public class _orders
    {
        private _orders[] orders;

        public _orders[] Orders
        {
            get { return orders; }
            set { orders = value; }
        }
    }


    public class order_model
    {
        private string buyer_account;

        public string Buyer_account
        {
            get { return buyer_account; }
            set { buyer_account = value; }
        }
        private string buyer_phone;

        public string Buyer_phone
        {
            get { return buyer_phone; }
            set { buyer_phone = value; }
        }
        private string order_status;

        public string Order_status
        {
            get { return order_status; }
            set { order_status = value; }
        }
        private string order_id;

        public string Order_id
        {
            get { return order_id; }
            set { order_id = value; }
        }
        private string receiver_name;

        public string Receiver_name
        {
            get { return receiver_name; }
            set { receiver_name = value; }
        }
        private string receiver_phone;

        public string Receiver_phone
        {
            get { return receiver_phone; }
            set { receiver_phone = value; }
        }
        private string receiver_address_detail;

        public string Receiver_address_detail
        {
            get { return receiver_address_detail; }
            set { receiver_address_detail = value; }
        }
        private string pay_success_time;

        public string Pay_success_time
        {
            get { return pay_success_time; }
            set { pay_success_time = value; }
        }
        private string order_real_price;

        public string Order_real_price
        {
            get { return order_real_price; }
            set { order_real_price = value; }
        }
        private string order_origin_price;

        public string Order_origin_price
        {
            get { return order_origin_price; }
            set { order_origin_price = value; }
        }
        private string express_fee;

        public string Express_fee
        {
            get { return express_fee; }
            set { express_fee = value; }
        }
        private string pay_method_name;

        public string Pay_method_name
        {
            get { return pay_method_name; }
            set { pay_method_name = value; }
        }
        private string coupon_amount;

        public string Coupon_amount
        {
            get { return coupon_amount; }
            set { coupon_amount = value; }
        }
        private string finish_time;

        public string Finish_time
        {
            get { return finish_time; }
            set { finish_time = value; }
        }
        private string deliver_time;

        public string Deliver_time
        {
            get { return deliver_time; }
            set { deliver_time = value; }
        }
        private order_sku[] order_skus;

        public order_sku[] Order_skus
        {
            get { return order_skus; }
            set { order_skus = value; }
        }

        private string receiver_province_name;

        public string Receiver_province_name
        {
            get { return receiver_province_name; }
            set { receiver_province_name = value; }
        }

        private string receiver_post_code;

        public string Receiver_post_code
        {
            get { return receiver_post_code; }
            set { receiver_post_code = value; }
        }

        private string receiver_city_name;

        public string Receiver_city_name
        {
            get { return receiver_city_name; }
            set { receiver_city_name = value; }
        }

        private string receiver_district_name;

        public string Receiver_district_name
        {
            get { return receiver_district_name; }
            set { receiver_district_name = value; }
        }

        /// <summary>
        /// 实名认证姓名
        /// </summary>
        private string cert_name;

        public string Cert_name
        {
            get { return cert_name; }
            set { cert_name = value; }
        }
        /// <summary>
        /// 实名认证身份证号码
        /// </summary>
        private string cert_id_no;

        public string Cert_id_no
        {
            get { return cert_id_no; }
            set { cert_id_no = value; }
        }

        /// <summary>
        /// 订单税费
        /// </summary>
        private string tax_fee;

        public string Tax_fee
        {
            get { return tax_fee; }
            set { tax_fee = value; }
        }

        /// <summary>
        /// 订单交易流水号
        /// </summary>
        private string trade_no;

        public string Trade_no
        {
            get { return trade_no; }
            set { trade_no = value; }
        }

        /// <summary>
        /// 是否需要发票 1：是  0：否
        /// </summary>
        private string need_invoice;

        public string Need_invoice
        {
            get { return need_invoice; }
            set { need_invoice = value; }
        }

        /// <summary>
        /// 发票金额
        /// </summary>
        private string invoice_amount;

        public string Invoice_amount
        {
            get { return invoice_amount; }
            set { invoice_amount = value; }
        }

        /// <summary>
        /// 发票抬头
        /// </summary>
        private string invoice_title;

        public string Invoice_title
        {
            get { return invoice_title; }
            set { invoice_title = value; }
        }
    }


    public class order_sku
    {
        private string sku_key;

        public string Sku_key
        {
            get { return sku_key; }
            set { sku_key = value; }
        }
        private string product_name;

        public string Product_name
        {
            get { return product_name; }
            set { product_name = value; }
        }
        private string origin_price;

        public string Origin_price
        {
            get { return origin_price; }
            set { origin_price = value; }
        }
        private int count;

        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        private string real_totle_price;

        public string Real_totle_price
        {
            get { return real_totle_price; }
            set { real_totle_price = value; }
        }
        private string activity_totle_amount;

        public string Activity_totle_amount
        {
            get { return activity_totle_amount; }
            set { activity_totle_amount = value; }
        }
        private string goods_no;

        public string Goods_no
        {
            get { return goods_no; }
            set { goods_no = value; }
        }
        private string coupon_totle_amount;

        public string Coupon_totle_amount
        {
            get { return coupon_totle_amount; }
            set { coupon_totle_amount = value; }
        }
        private string barcode;

        public string Barcode
        {
            get { return barcode; }
            set { barcode = value; }
        }
    }
}
