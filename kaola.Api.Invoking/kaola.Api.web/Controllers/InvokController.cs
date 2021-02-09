using kaola.api.invok;
using kaola.api.invok.basicMsg;
using kaola.api.invok.basicMsgModel;
using kaola.api.invok.category;
using kaola.api.invok.order;
using kaola.api.invok.orderModel;
using kaola.api.invok.sellerMsg;
using kaola.api.invok.sellerMsgModel;
using Maticsoft.Model;
using MDBUtility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace kaola.Api.web.Controllers
{
    public class InvokController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 实例化订单对象
        /// </summary>
        kaola_order kl = new kaola_order();
        string time = kaola_order.getDateTimeNow();

        public string[] app_key_list = new string[] { "b8d6f0d18f891816f41aca3aff1536b8", "eb2654b0c9a2ba09aaa322e4fb99d3e3", "f037b9f5f52d7ae644f72abb91e5fde0" };

        // public string[] 
        #region 订单

        /// <summary>
        /// 根据订单信息查询订单   调试成功
        /// </summary>
        /// <param name="method">调用接口</param>
        /// <param name="time">时间戳</param>
        /// <param name="status">订单状态（1已付款,2已发货,3已签收,4缺货订单,5取消待确认,6已取消）</param>
        /// <param name="datetype">搜索日期类型（1支付时间,2发货时间,3签收时间）</param>
        /// <param name="stime">开始时间</param>
        /// <param name="etime">结束时间</param>
        /// <param name="orderid">订单号</param>
        /// <param name="pageindex">当前页</param>
        /// <param name="pagesize">页面大小</param>
        /// <returns></returns>
        public string kaola_order_search()
        {
            ApiErrorMsg abm = new ApiErrorMsg()
            {
                errorAction = "InvokController->kaola_order_search()",
                errorKey = "",
                errorMsg = "获取考拉订单开始",
                errorOrderId = "",
                errorTime = System.DateTime.Now.ToString("yyyyMMddHHmmss")
            };

            DbHelperMySQL.ExecuteSql("insert into bms_api_error(id,error_msg,error_time,error_action,error_order_id,error_key) values(null,'" + abm.errorMsg + "','" + abm.errorTime + "','" + abm.errorAction + "','" + abm.errorOrderId + "','" + abm.errorKey + "')");
            List<order_model> list = new List<order_model>();
            List<string> listSQL = new List<string>();
            List<string> oidli = new List<string>();                                                                         //订单id集合
            List<string> coli = new List<string>();                                                                          //货号集合
            List<string> serorderidli = new List<string>();                                                                  //服务器数据订单id集合
            List<string> skubarcodeli = new List<string>();                                                                  //订单sku集合
            int count = 0;                                                                                                   //受影响行数
            StringBuilder sb = new StringBuilder(200);                                                                       //拼接所有接口调用接口得到的数据
            StringBuilder sb2 = new StringBuilder(200);                                                                      //拼接调用接口失败的数据
            StringBuilder sb3 = new StringBuilder(200);                                                                      //拼接所有SQL语句
            StringBuilder nosucc = new StringBuilder(200);                                                                   //执行结果失败
            //string od = kl.kaola_order_search("kaola.order.search", time, 2, 1, "2015-10-10", "2015-12-12");
            string endtime = System.DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

            //for (int y = 1; y < 7; y++)                                                                                      //订单状态
            //{
            //    for (int z = 1; z < 4; z++)                                                                                  //搜索日期类型
            //    {
            for (int y = 1; y < 7; y++)                                                                                      //订单状态
            {
                for (int z = 1; z < 4; z++)                                                                                  //搜索日期类型
                {
                    //string od = string.Empty;
                    string[] od = kl.kaola_order_search("kaola.order.search", time, y, z, "2015-11-01", endtime);                     //调用接口获得数据
                    for (int g = 0; g < od.Length; g++)
                    {
                        if (od[g].Contains("kaola_order_search_response"))                                                          //判断调用是否成功
                        {
                            // int app_key_index=od[g].IndexOf("app_key=");
                            //  string app_key_str = od[g].Substring(app_key_index);
                            dynamic dc = JsonConvert.DeserializeObject(od[g].ToString());//od[g].Substring(0,app_key_index)
                            var ordercount = dc.kaola_order_search_response.totle_count;                                         //订单总数                       
                            int pagcount = 0;                                                                                    //页数        
                            if (int.Parse(ordercount.ToString()) > 0)                                                            //订单数是否大于0
                            {
                                pagcount = (int.Parse(ordercount.ToString()) % 20) == 0 ? int.Parse(ordercount.ToString()) / 20 : int.Parse(ordercount.ToString()) / 20 + 1;

                                for (int w = 1; w <= pagcount; w++)                                                              //对每一页数据进行获取
                                {
                                    //string orders = string.Empty;
                                    oidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select order_id from bms_kaola_order;"));
                                    string[] orders = kl.kaola_order_search("kaola.order.search", time, y, z, "2015-01-01", endtime, null, w, 20);
                                    for (int p = 0; p < orders.Length; p++)
                                    {
                                        dynamic dd = JsonConvert.DeserializeObject(orders[p]);
                                        var dc1 = dd.kaola_order_search_response.orders;                                             //订单集合
                                        sb.Append(dc1.ToString());                                                                   //调用接口得到的数据
                                        if (dc1 != null && dc1.Count > 0)
                                        {
                                            for (int i = 0; i < dc1.Count; i++)
                                            {
                                                string sql = string.Empty;
                                                string orderJson = dc1[i].ToString();
                                                dynamic o = JsonConvert.DeserializeObject(orderJson);

                                                order_model mdOrder = new order_model();
                                                mdOrder.Buyer_account = o.buyer_account.ToString();
                                                mdOrder.Buyer_phone = o.buyer_phone.ToString();
                                                mdOrder.Order_id = o.order_id.ToString();
                                                mdOrder.Receiver_name = o.receiver_name.ToString();
                                                mdOrder.Receiver_phone = o.receiver_phone.ToString();
                                                mdOrder.Receiver_address_detail = o.receiver_address_detail.ToString();
                                                mdOrder.Pay_success_time = o.pay_success_time.ToString();
                                                mdOrder.Order_real_price = o.order_real_price.ToString();
                                                mdOrder.Order_origin_price = o.order_origin_price.ToString();
                                                mdOrder.Express_fee = o.express_fee.ToString();
                                                mdOrder.Pay_method_name = o.pay_method_name.ToString();
                                                mdOrder.Coupon_amount = o.coupon_amount.ToString();
                                                mdOrder.Finish_time = o.finish_time.ToString();
                                                mdOrder.Deliver_time = o.deliver_time.ToString();
                                                mdOrder.Order_status = o.order_status.ToString();
                                                mdOrder.Receiver_province_name = o.receiver_province_name.ToString();
                                                mdOrder.Receiver_post_code = o.receiver_post_code.ToString();
                                                mdOrder.Receiver_city_name = o.receiver_city_name.ToString();
                                                mdOrder.Receiver_district_name = o.receiver_district_name.ToString();

                                                mdOrder.Cert_name = o.cert_name.ToString();
                                                mdOrder.Cert_id_no = o.cert_id_no.ToString();
                                                mdOrder.Tax_fee = o.tax_fee.ToString();
                                                mdOrder.Trade_no = o.trade_no.ToString();
                                                mdOrder.Need_invoice = o.need_invoice.ToString();
                                                mdOrder.Need_invoice = o.invoice_amount.ToString();
                                                mdOrder.Invoice_title = o.invoice_title.ToString();
                                                // coli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query(string.Format("select barcode from bms_kaola_order_sku where order_id='{0}';", mdOrder.Order_id)));
                                                int skuNum = o.order_skus.Count;
                                                order_sku[] listSku = new order_sku[skuNum];
                                                for (int j = 0; j < skuNum; j++)
                                                {
                                                    order_sku mdOrderSku = new order_sku();
                                                    mdOrderSku.Activity_totle_amount = o.order_skus[j].activity_totle_amount;
                                                    mdOrderSku.Barcode = o.order_skus[j].barcode;
                                                    mdOrderSku.Count = o.order_skus[j].count;
                                                    mdOrderSku.Coupon_totle_amount = o.order_skus[j].coupon_totle_amount;
                                                    mdOrderSku.Goods_no = o.order_skus[j].goods_no;
                                                    mdOrderSku.Origin_price = o.order_skus[j].origin_price;
                                                    mdOrderSku.Product_name = o.order_skus[j].product_name;
                                                    mdOrderSku.Real_totle_price = o.order_skus[j].real_totle_price;
                                                    mdOrderSku.Sku_key = o.order_skus[j].sku_key;

                                                    listSku[j] = mdOrderSku;
                                                    //当数据库中存在该订单下的该货品信息     同时该订单的  商品code集合中没有该商品code  进行更新操作
                                                    if (DbHelperMySQL.DataSetToList(DbHelperMySQL.Query(string.Format(@"select order_id from bms_kaola_order_sku where barcode='{0}' and order_id='{1}';", mdOrderSku.Barcode, mdOrder.Order_id))).Contains(mdOrder.Order_id.ToString()) && !skubarcodeli.Contains(mdOrderSku.Barcode.ToString()))
                                                    {
                                                        string ss = string.Format(@"update bms_kaola_order_sku set activity_totle_amount='{0}',count='{1}',coupon_totle_amount='{2}',goods_no='{3}',origin_price='{4}',product_name='{5}',real_totle_price='{6}',sku_key='{7}' where barcode='{8}' and order_id='{9}';",
                                              mdOrderSku.Activity_totle_amount, mdOrderSku.Count, mdOrderSku.Coupon_totle_amount, mdOrderSku.Goods_no, mdOrderSku.Origin_price, mdOrderSku.Product_name, mdOrderSku.Real_totle_price, mdOrderSku.Sku_key, mdOrderSku.Barcode, mdOrder.Order_id);
                                                        listSQL.Add(ss);
                                                        sb3.Append(ss + "0000000000000000000000000000000");
                                                    }
                                                    else if (!skubarcodeli.Contains(mdOrderSku.Barcode.ToString()))
                                                    {
                                                        string sss = string.Format(@"insert into bms_kaola_order_sku(activity_totle_amount,barcode,count,
                                            coupon_totle_amount,goods_no,origin_price,product_name,real_totle_price,sku_key,order_id) 
                                        values('{0}','{1}',{2},'{3}','{4}','{5}','{6}','{7}','{8}','{9}');",
                                                     mdOrderSku.Activity_totle_amount, mdOrderSku.Barcode, mdOrderSku.Count, mdOrderSku.Coupon_totle_amount,
                                                     mdOrderSku.Goods_no, mdOrderSku.Origin_price, mdOrderSku.Product_name, mdOrderSku.Real_totle_price, mdOrderSku.Sku_key, mdOrder.Order_id);
                                                        listSQL.Add(sss);
                                                        sb3.Append(sss + "0000000000000000000000000000000");
                                                    }
                                                    skubarcodeli.Add(mdOrderSku.Barcode.ToString());
                                                }
                                                skubarcodeli.Clear();//当该订单的sku[]遍历完之后，将barcode集合清空，进入下一个订单
                                                // coli.Clear();
                                                mdOrder.Order_skus = listSku;

                                                list.Add(mdOrder);

                                                if (oidli.Contains(mdOrder.Order_id.ToString()) && !serorderidli.Contains(mdOrder.Order_id.ToString()))//数据库订单表中存在该订单   进行更新操作
                                                {
                                                    string str = string.Format(@"update bms_kaola_order set buyer_account='{0}',buyer_phone='{1}',receiver_name='{2}',receiver_phone='{3}',receiver_address_detail='{4}',pay_success_time='{5}',order_real_price='{6}',order_origin_price='{7}',express_fee='{8}',pay_method_name='{9}',coupon_amount='{10}',finish_time='{11}',deliver_time='{12}',order_status='{13}',receiver_province_name='{14}',receiver_post_code='{15}',receiver_city_name='{16}',receiver_district_name='{17}',cert_name='{18}',cert_id_no='{19}',tax_fee='{20}',trade_no='{21}',need_invoice='{22}',invoice_amount='{23}',invoice_title='{24}' where order_id='{25}' and app_key='{26}'",
                                                                mdOrder.Buyer_account, mdOrder.Buyer_phone, mdOrder.Receiver_name, mdOrder.Receiver_phone, mdOrder.Receiver_address_detail, mdOrder.Pay_success_time, mdOrder.Order_real_price, mdOrder.Order_origin_price, mdOrder.Express_fee, mdOrder.Pay_method_name, mdOrder.Coupon_amount, mdOrder.Finish_time, mdOrder.Deliver_time, mdOrder.Order_status, mdOrder.Receiver_province_name, mdOrder.Receiver_post_code, mdOrder.Receiver_city_name, mdOrder.Receiver_district_name, mdOrder.Cert_name, mdOrder.Cert_id_no, mdOrder.Tax_fee, mdOrder.Trade_no, mdOrder.Need_invoice, mdOrder.Invoice_amount, mdOrder.Invoice_title, mdOrder.Order_id, app_key_list[g]);
                                                    listSQL.Add(str);
                                                    sb3.Append(str + "0000000000000000000000000000000");
                                                }
                                                else if (!serorderidli.Contains(mdOrder.Order_id.ToString()))
                                                {
                                                    string strs = string.Format(@"insert into bms_kaola_order(buyer_account,buyer_phone,order_id,receiver_name,
                    receiver_phone,receiver_address_detail,pay_success_time,order_real_price,order_origin_price,express_fee,
                    pay_method_name,coupon_amount,finish_time,deliver_time,order_status,
                    receiver_province_name,receiver_post_code,receiver_city_name,receiver_district_name,app_key,cert_name,cert_id_no,tax_fee,trade_no,need_invoice,invoice_amount,invoice_title) 
                values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}')",
                                    mdOrder.Buyer_account, mdOrder.Buyer_phone, mdOrder.Order_id, mdOrder.Receiver_name, mdOrder.Receiver_phone,
                                    mdOrder.Receiver_address_detail, mdOrder.Pay_success_time, mdOrder.Order_real_price, mdOrder.Order_origin_price, mdOrder.Express_fee
                                    , mdOrder.Pay_method_name, mdOrder.Coupon_amount, mdOrder.Finish_time, mdOrder.Deliver_time, mdOrder.Order_status
                                    , mdOrder.Receiver_province_name, mdOrder.Receiver_post_code, mdOrder.Receiver_city_name, mdOrder.Receiver_district_name, app_key_list[g], mdOrder.Cert_name, mdOrder.Cert_id_no, mdOrder.Tax_fee, mdOrder.Trade_no, mdOrder.Need_invoice, mdOrder.Invoice_amount, mdOrder.Invoice_title);
                                                    listSQL.Add(strs);
                                                    sb3.Append(strs + "0000000000000000000000000000000");
                                                }
                                                serorderidli.Add(mdOrder.Order_id.ToString());               //执行完之后将订单id添加到集合中
                                                //listSQL.Add(sql);
                                            }
                                        }
                                    }
                                    oidli.Clear();
                                }//每种情况遍历完之后要将订单id集合以及sku商品socde集合清空
                                serorderidli.Clear();   //服务器数据订单id集合
                                skubarcodeli.Clear();   //服务器数据订单sku 商品编号集合
                            }
                            if (listSQL != null && listSQL.Count > 0)        //每次遍历完第一种情况后事务执行SQL语句
                            {
                                //string sb33 = sb3.ToString();
                                count = DbHelperMySQL.ExecuteSqlTran(listSQL);

                                if (count > 0)//执行成功
                                {
                                    //if (count<listSQL.Count)
                                    //{
                                    //    int ddddddd = y;
                                    //    int ssssssss = z;
                                    //}
                                    count += count;
                                    listSQL.Clear();
                                }
                                else
                                {
                                    nosucc.Append("状态为：" + y + ";搜索日期类型为：" + z + "执行失败________________________________");
                                }
                            }
                        }
                        else   //调用失败
                        {
                            sb2.Append(od.ToString());//拼接调用失败返回的数据
                        }
                    }
                }
            }

            abm.errorMsg = "获取考拉订单结束，更新" + count.ToString() + "条数据";
            abm.errorTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
            DbHelperMySQL.ExecuteSql("insert into bms_api_error(id,error_msg,error_time,error_action,error_order_id,error_key) values(null,'" + abm.errorMsg + "','" + abm.errorTime + "','" + abm.errorAction + "','" + abm.errorOrderId + "','" + abm.errorKey + "')");
            return "执行结果:(1)执行完成的SQL数量：" + count.ToString() + "条数据；" + "执行失败：" + nosucc.ToString() + "；接口调用失败：" + sb2.ToString() + "；所有SQL语句：" + sb3.ToString() + "；接口调用得到的数据：" + sb.ToString();

        }

        #region 原来的
        //        /// <summary>
        //        /// 根据订单信息查询订单   调试成功
        //        /// </summary>
        //        /// <param name="method">调用接口</param>
        //        /// <param name="time">时间戳</param>
        //        /// <param name="status">订单状态（1已付款,2已发货,3已签收,4缺货订单,5取消待确认,6已取消）</param>
        //        /// <param name="datetype">搜索日期类型（1支付时间,2发货时间,3签收时间）</param>
        //        /// <param name="stime">开始时间</param>
        //        /// <param name="etime">结束时间</param>
        //        /// <param name="orderid">订单号</param>
        //        /// <param name="pageindex">当前页</param>
        //        /// <param name="pagesize">页面大小</param>
        //        /// <returns></returns>
        //        public string kaola_order_search()
        //        {
        //            List<order_model> list = new List<order_model>();
        //            List<string> listSQL = new List<string>();
        //            List<string> oidli = new List<string>();                                                                         //订单id集合
        //            List<string> coli = new List<string>();                                                                          //货号集合
        //            List<string> serorderidli = new List<string>();                                                                  //服务器数据订单id集合
        //            List<string> skubarcodeli = new List<string>();                                                                  //订单sku集合
        //            int count = 0;                                                                                                   //受影响行数
        //            StringBuilder sb = new StringBuilder(200);                                                                       //拼接所有接口调用接口得到的数据
        //            StringBuilder sb2 = new StringBuilder(200);                                                                      //拼接调用接口失败的数据
        //            StringBuilder sb3 = new StringBuilder(200);                                                                      //拼接所有SQL语句
        //            StringBuilder nosucc = new StringBuilder(200);                                                                   //执行结果失败
        //            //string od = kl.kaola_order_search("kaola.order.search", time, 2, 1, "2015-10-10", "2015-12-12");
        //            string endtime = System.DateTime.Now.ToString("yyyy-MM-dd");
        //            //for (int y = 1; y < 7; y++)                                                                                      //订单状态
        //            //{
        //            //    for (int z = 1; z < 4; z++)                                                                                  //搜索日期类型
        //            //    {
        //            for (int y = 1; y < 2; y++)                                                                                      //订单状态
        //            {
        //                for (int z = 1; z < 2; z++)                                                                                  //搜索日期类型
        //                {
        //                    string od = string.Empty;
        //                    od = kl.kaola_order_search("kaola.order.search", time, y, z, "2015-11-01", endtime);                     //调用接口获得数据
        //                    if (od.Contains("kaola_order_search_response"))                                                          //判断调用是否成功
        //                    {
        //                        dynamic dc = JsonConvert.DeserializeObject(od);
        //                        var ordercount = dc.kaola_order_search_response.totle_count;                                         //订单总数                       
        //                        int pagcount = 0;                                                                                    //页数        
        //                        if (int.Parse(ordercount.ToString()) > 0)                                                            //订单数是否大于0
        //                        {
        //                            pagcount = (int.Parse(ordercount.ToString()) % 20) == 0 ? int.Parse(ordercount.ToString()) / 20 : int.Parse(ordercount.ToString()) / 20 + 1;
        //                            for (int w = 1; w <= pagcount; w++)                                                              //对每一页数据进行获取
        //                            {
        //                                string orders = string.Empty;
        //                                oidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select order_id from bms_kaola_order;"));
        //                                orders = kl.kaola_order_search("kaola.order.search", time, y, z, "2015-01-01", endtime, null, w, 20);
        //                                dynamic dd = JsonConvert.DeserializeObject(orders);
        //                                var dc1 = dd.kaola_order_search_response.orders;                                             //订单集合
        //                                sb.Append(dc1.ToString());                                                                   //调用接口得到的数据
        //                                if (dc1 != null && dc1.Count > 0)
        //                                {
        //                                    for (int i = 0; i < dc1.Count; i++)
        //                                    {
        //                                        string sql = string.Empty;
        //                                        string orderJson = dc1[i].ToString();
        //                                        dynamic o = JsonConvert.DeserializeObject(orderJson);

        //                                        order_model mdOrder = new order_model();
        //                                        mdOrder.Buyer_account = o.buyer_account.ToString();
        //                                        mdOrder.Buyer_phone = o.buyer_phone.ToString();
        //                                        mdOrder.Order_id = o.order_id.ToString();
        //                                        mdOrder.Receiver_name = o.receiver_name.ToString();
        //                                        mdOrder.Receiver_phone = o.receiver_phone.ToString();
        //                                        mdOrder.Receiver_address_detail = o.receiver_address_detail.ToString();
        //                                        mdOrder.Pay_success_time = o.pay_success_time.ToString();
        //                                        mdOrder.Order_real_price = o.order_real_price.ToString();
        //                                        mdOrder.Order_origin_price = o.order_origin_price.ToString();
        //                                        mdOrder.Express_fee = o.express_fee.ToString();
        //                                        mdOrder.Pay_method_name = o.pay_method_name.ToString();
        //                                        mdOrder.Coupon_amount = o.coupon_amount.ToString();
        //                                        mdOrder.Finish_time = o.finish_time.ToString();
        //                                        mdOrder.Deliver_time = o.deliver_time.ToString();
        //                                        mdOrder.Order_status = o.order_status.ToString();
        //                                        mdOrder.Receiver_province_name = o.receiver_province_name.ToString();
        //                                        mdOrder.Receiver_post_code = o.receiver_post_code.ToString();
        //                                        mdOrder.Receiver_city_name = o.receiver_city_name.ToString();
        //                                        mdOrder.Receiver_district_name = o.receiver_district_name.ToString();

        //                                        // coli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query(string.Format("select barcode from bms_kaola_order_sku where order_id='{0}';", mdOrder.Order_id)));
        //                                        int skuNum = o.order_skus.Count;
        //                                        order_sku[] listSku = new order_sku[skuNum];
        //                                        for (int j = 0; j < skuNum; j++)
        //                                        {
        //                                            order_sku mdOrderSku = new order_sku();
        //                                            mdOrderSku.Activity_totle_amount = o.order_skus[j].activity_totle_amount;
        //                                            mdOrderSku.Barcode = o.order_skus[j].barcode;
        //                                            mdOrderSku.Count = o.order_skus[j].count;
        //                                            mdOrderSku.Coupon_totle_amount = o.order_skus[j].coupon_totle_amount;
        //                                            mdOrderSku.Goods_no = o.order_skus[j].goods_no;
        //                                            mdOrderSku.Origin_price = o.order_skus[j].origin_price;
        //                                            mdOrderSku.Product_name = o.order_skus[j].product_name;
        //                                            mdOrderSku.Real_totle_price = o.order_skus[j].real_totle_price;
        //                                            mdOrderSku.Sku_key = o.order_skus[j].sku_key;

        //                                            listSku[j] = mdOrderSku;
        //                                            //当数据库中存在该订单下的该货品信息     同时该订单的  商品code集合中没有该商品code  进行更新操作
        //                                            if (DbHelperMySQL.DataSetToList(DbHelperMySQL.Query(string.Format(@"select order_id from bms_kaola_order_sku where barcode='{0}' and order_id='{1}';", mdOrderSku.Barcode, mdOrder.Order_id))).Contains(mdOrder.Order_id.ToString()) && !skubarcodeli.Contains(mdOrderSku.Barcode.ToString()))
        //                                            {
        //                                                string ss = string.Format(@"update bms_kaola_order_sku set activity_totle_amount='{0}',count='{1}',coupon_totle_amount='{2}',goods_no='{3}',origin_price='{4}',product_name='{5}',real_totle_price='{6}',sku_key='{7}' where barcode='{8}' and order_id='{9}';",
        //                                      mdOrderSku.Activity_totle_amount, mdOrderSku.Count, mdOrderSku.Coupon_totle_amount, mdOrderSku.Goods_no, mdOrderSku.Origin_price, mdOrderSku.Product_name, mdOrderSku.Real_totle_price, mdOrderSku.Sku_key, mdOrderSku.Barcode, mdOrder.Order_id);
        //                                                listSQL.Add(ss);
        //                                                sb3.Append(ss + "0000000000000000000000000000000");
        //                                            }
        //                                            else if (!skubarcodeli.Contains(mdOrderSku.Barcode.ToString()))
        //                                            {
        //                                                string sss = string.Format(@"insert into bms_kaola_order_sku(activity_totle_amount,barcode,count,
        //                                            coupon_totle_amount,goods_no,origin_price,product_name,real_totle_price,sku_key,order_id) 
        //                                        values('{0}','{1}',{2},'{3}','{4}','{5}','{6}','{7}','{8}','{9}');",
        //                                             mdOrderSku.Activity_totle_amount, mdOrderSku.Barcode, mdOrderSku.Count, mdOrderSku.Coupon_totle_amount,
        //                                             mdOrderSku.Goods_no, mdOrderSku.Origin_price, mdOrderSku.Product_name, mdOrderSku.Real_totle_price, mdOrderSku.Sku_key, mdOrder.Order_id);
        //                                                listSQL.Add(sss);
        //                                                sb3.Append(sss + "0000000000000000000000000000000");
        //                                            }
        //                                            skubarcodeli.Add(mdOrderSku.Barcode.ToString());
        //                                        }
        //                                        skubarcodeli.Clear();//当该订单的sku[]遍历完之后，将barcode集合清空，进入下一个订单
        //                                        // coli.Clear();
        //                                        mdOrder.Order_skus = listSku;

        //                                        list.Add(mdOrder);

        //                                        if (oidli.Contains(mdOrder.Order_id.ToString()) && !serorderidli.Contains(mdOrder.Order_id.ToString()))//数据库订单表中存在该订单   进行更新操作
        //                                        {
        //                                            string str = string.Format(@"update bms_kaola_order set buyer_account='{0}',buyer_phone='{1}',receiver_name='{2}',receiver_phone='{3}',receiver_address_detail='{4}',pay_success_time='{5}',order_real_price='{6}',order_origin_price='{7}',express_fee='{8}',pay_method_name='{9}',coupon_amount='{10}',finish_time='{11}',deliver_time='{12}',order_status='{13}',receiver_province_name='{14}',receiver_post_code='{15}',receiver_city_name='{16}',receiver_district_name='{17}' where order_id='{18}'",
        //                                                        mdOrder.Buyer_account, mdOrder.Buyer_phone, mdOrder.Receiver_name, mdOrder.Receiver_phone, mdOrder.Receiver_address_detail, mdOrder.Pay_success_time, mdOrder.Order_real_price, mdOrder.Order_origin_price, mdOrder.Express_fee, mdOrder.Pay_method_name, mdOrder.Coupon_amount, mdOrder.Finish_time, mdOrder.Deliver_time, mdOrder.Order_status, mdOrder.Receiver_province_name, mdOrder.Receiver_post_code, mdOrder.Receiver_city_name, mdOrder.Receiver_district_name, mdOrder.Order_id);
        //                                            listSQL.Add(str);
        //                                            sb3.Append(str + "0000000000000000000000000000000");
        //                                        }
        //                                        else if (!serorderidli.Contains(mdOrder.Order_id.ToString()))
        //                                        {
        //                                            string strs = string.Format(@"insert into bms_kaola_order(buyer_account,buyer_phone,order_id,receiver_name,
        //                    receiver_phone,receiver_address_detail,pay_success_time,order_real_price,order_origin_price,express_fee,
        //                    pay_method_name,coupon_amount,finish_time,deliver_time,order_status,
        //                    receiver_province_name,receiver_post_code,receiver_city_name,receiver_district_name) 
        //                values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}');",
        //                            mdOrder.Buyer_account, mdOrder.Buyer_phone, mdOrder.Order_id, mdOrder.Receiver_name, mdOrder.Receiver_phone,
        //                            mdOrder.Receiver_address_detail, mdOrder.Pay_success_time, mdOrder.Order_real_price, mdOrder.Order_origin_price, mdOrder.Express_fee
        //                            , mdOrder.Pay_method_name, mdOrder.Coupon_amount, mdOrder.Finish_time, mdOrder.Deliver_time, mdOrder.Order_status
        //                            , mdOrder.Receiver_province_name, mdOrder.Receiver_post_code, mdOrder.Receiver_city_name, mdOrder.Receiver_district_name);
        //                                            listSQL.Add(strs);
        //                                            sb3.Append(strs + "0000000000000000000000000000000");
        //                                        }
        //                                        serorderidli.Add(mdOrder.Order_id.ToString());               //执行完之后将订单id添加到集合中
        //                                        //listSQL.Add(sql);
        //                                    }
        //                                }
        //                                oidli.Clear();
        //                            }//每种情况遍历完之后要将订单id集合以及sku商品socde集合清空
        //                            serorderidli.Clear();   //服务器数据订单id集合
        //                            skubarcodeli.Clear();   //服务器数据订单sku 商品编号集合
        //                        }
        //                        if (listSQL != null && listSQL.Count > 0)        //每次遍历完第一种情况后事务执行SQL语句
        //                        {
        //                            //string sb33 = sb3.ToString();
        //                            count = DbHelperMySQL.ExecuteSqlTran(listSQL);

        //                            if (count > 0)//执行成功
        //                            {
        //                                //if (count<listSQL.Count)
        //                                //{
        //                                //    int ddddddd = y;
        //                                //    int ssssssss = z;
        //                                //}
        //                                count += count;
        //                                listSQL.Clear();
        //                            }
        //                            else
        //                            {
        //                                nosucc.Append("状态为：" + y + ";搜索日期类型为：" + z + "执行失败________________________________");
        //                            }
        //                        }
        //                    }
        //                    else   //调用失败
        //                    {
        //                        sb2.Append(od.ToString());//拼接调用失败返回的数据
        //                    }
        //                }
        //            }
        //            return "执行结果:(1)执行完成的SQL数量：" + count.ToString() + "条数据；" + "执行失败：" + nosucc.ToString() + "；接口调用失败：" + sb2.ToString() + "；所有SQL语句：" + sb3.ToString() + "；接口调用得到的数据：" + sb.ToString();

        //        } 
        #endregion

        /// <summary>
        /// 根据订单编号获取指定订单的信息  调试成功 对接网易考拉C店 帐号：apenninesz@163.com
        /// </summary> 
        /// <returns></returns>
        public string kaola_order_get(string order_id)
        {
            List<order_model> list = new List<order_model>();
            List<string> listSQL = new List<string>();
            int count = 0;
            try
            {
                string odmes = kl.kaola_order_get("kaola.order.get", time, order_id);//"2016051113433111069942569"
                dynamic dc = JsonConvert.DeserializeObject(odmes);
                if (odmes.Contains("kaola_order_get_response"))
                {
                    var dc1 = dc.kaola_order_get_response.order;

                    List<string> oidli = new List<string>();
                    oidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select order_id from bms_kaola_order;"));
                    string sql = string.Empty;
                    string orderJson = dc1.ToString();
                    dynamic o = JsonConvert.DeserializeObject(orderJson);

                    order_model mdOrder = new order_model();
                    mdOrder.Buyer_account = o.buyer_account.ToString();
                    mdOrder.Buyer_phone = o.buyer_phone.ToString();
                    mdOrder.Order_id = o.order_id.ToString();
                    mdOrder.Receiver_name = o.receiver_name.ToString();
                    mdOrder.Receiver_phone = o.receiver_phone.ToString();
                    mdOrder.Receiver_address_detail = o.receiver_address_detail.ToString();
                    mdOrder.Pay_success_time = o.pay_success_time.ToString();
                    mdOrder.Order_real_price = o.order_real_price.ToString();
                    mdOrder.Order_origin_price = o.order_origin_price.ToString();
                    mdOrder.Express_fee = o.express_fee.ToString();
                    mdOrder.Pay_method_name = o.pay_method_name.ToString();
                    mdOrder.Coupon_amount = o.coupon_amount.ToString();
                    mdOrder.Finish_time = o.finish_time.ToString();
                    mdOrder.Deliver_time = o.deliver_time.ToString();
                    mdOrder.Order_status = o.order_status.ToString();
                    mdOrder.Receiver_province_name = o.receiver_province_name.ToString();
                    mdOrder.Receiver_post_code = o.receiver_post_code.ToString();
                    mdOrder.Receiver_city_name = o.receiver_city_name.ToString();
                    mdOrder.Receiver_district_name = o.receiver_district_name.ToString();
                    List<string> coli = new List<string>();
                    coli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query(string.Format(@"select barcode from bms_kaola_order_sku where order_id='{0}';", mdOrder.Order_id)));
                    int skuNum = o.order_skus.Count;
                    order_sku[] listSku = new order_sku[skuNum];
                    for (int j = 0; j < skuNum; j++)
                    {
                        order_sku mdOrderSku = new order_sku();
                        mdOrderSku.Activity_totle_amount = o.order_skus[j].activity_totle_amount;
                        mdOrderSku.Barcode = o.order_skus[j].barcode;
                        mdOrderSku.Count = o.order_skus[j].count;
                        mdOrderSku.Coupon_totle_amount = o.order_skus[j].coupon_totle_amount;
                        mdOrderSku.Goods_no = o.order_skus[j].goods_no;
                        mdOrderSku.Origin_price = o.order_skus[j].origin_price;
                        mdOrderSku.Product_name = o.order_skus[j].product_name;
                        mdOrderSku.Real_totle_price = o.order_skus[j].real_totle_price;
                        mdOrderSku.Sku_key = o.order_skus[j].sku_key;

                        listSku[j] = mdOrderSku;
                        if (coli.Contains(mdOrderSku.Barcode.ToString()))
                        {
                            sql += string.Format(@"update bms_kaola_order_sku set activity_totle_amount='{0}',count='{1}',coupon_totle_amount='{2}',goods_no='{3}',origin_price='{4}',product_name='{5}',real_totle_price='{6}',sku_key='{7}' where barcode='{8}' and order_id='{9}';",
                                 mdOrderSku.Activity_totle_amount, mdOrderSku.Count, mdOrderSku.Coupon_totle_amount, mdOrderSku.Goods_no, mdOrderSku.Origin_price, mdOrderSku.Product_name, mdOrderSku.Real_totle_price, mdOrderSku.Sku_key, mdOrderSku.Barcode, mdOrder.Order_id);
                        }
                        else
                        {
                            sql += string.Format(@"insert into bms_kaola_order_sku(activity_totle_amount,barcode,count,
                                            coupon_totle_amount,goods_no,origin_price,product_name,real_totle_price,sku_key,order_id) 
                                        values('{0}','{1}',{2},'{3}','{4}','{5}','{6}','{7}','{8}','{9}');",
                                          mdOrderSku.Activity_totle_amount, mdOrderSku.Barcode, mdOrderSku.Count, mdOrderSku.Coupon_totle_amount,
                                          mdOrderSku.Goods_no, mdOrderSku.Origin_price, mdOrderSku.Product_name, mdOrderSku.Real_totle_price, mdOrderSku.Sku_key, mdOrder.Order_id);
                        }
                    }
                    coli.Clear();
                    mdOrder.Order_skus = listSku;

                    list.Add(mdOrder);
                    if (oidli.Contains(mdOrder.Order_id.ToString()))
                    {
                        sql += string.Format(@"update bms_kaola_order set buyer_account='{0}',buyer_phone='{1}',receiver_name='{2}',receiver_phone='{3}',receiver_address_detail='{4}',pay_success_time='{5}',order_real_price='{6}',order_origin_price='{7}',express_fee='{8}',pay_method_name='{9}',coupon_amount='{10}',finish_time='{11}',deliver_time='{12}',order_status='{13}',receiver_province_name='{14}',receiver_post_code='{15}',receiver_city_name='{16}',receiver_district_name='{17}' where order_id='{18}'",
                                                    mdOrder.Buyer_account, mdOrder.Buyer_phone, mdOrder.Receiver_name, mdOrder.Receiver_phone, mdOrder.Receiver_address_detail, mdOrder.Pay_success_time, mdOrder.Order_real_price, mdOrder.Order_origin_price, mdOrder.Express_fee, mdOrder.Pay_method_name, mdOrder.Coupon_amount, mdOrder.Finish_time, mdOrder.Deliver_time, mdOrder.Order_status, mdOrder.Receiver_province_name, mdOrder.Receiver_post_code, mdOrder.Receiver_city_name, mdOrder.Receiver_district_name, mdOrder.Order_id);
                    }
                    else
                    {
                        sql += string.Format(@"insert into bms_kaola_order(buyer_account,buyer_phone,order_id,receiver_name,
                    receiver_phone,receiver_address_detail,pay_success_time,order_real_price,order_origin_price,express_fee,
                    pay_method_name,coupon_amount,finish_time,deliver_time,order_status,
                    receiver_province_name,receiver_post_code,receiver_city_name,receiver_district_name) 
                values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}');",
                           mdOrder.Buyer_account, mdOrder.Buyer_phone, mdOrder.Order_id, mdOrder.Receiver_name, mdOrder.Receiver_phone,
                           mdOrder.Receiver_address_detail, mdOrder.Pay_success_time, mdOrder.Order_real_price, mdOrder.Order_origin_price, mdOrder.Express_fee
                           , mdOrder.Pay_method_name, mdOrder.Coupon_amount, mdOrder.Finish_time, mdOrder.Deliver_time, mdOrder.Order_status
                           , mdOrder.Receiver_province_name, mdOrder.Receiver_post_code, mdOrder.Receiver_city_name, mdOrder.Receiver_district_name);
                    }
                    listSQL.Add(sql);
                    if (listSQL != null)
                    {
                        count = DbHelperMySQL.ExecuteSqlTran(listSQL);
                        if (count > 0)
                        {
                            return "订单号" + order_id + "数据成功，更新" + count + "条数据";//+odmes ;
                        }
                        else
                        {
                            return "获取订单信息成功，但更新失败";//odmes +
                        }
                    }
                }
                else if (odmes.Contains("kaola.order.get.not_found"))
                {
                    return "订单不存在";
                    //"{"error_response":
                    //{"code":"service_currently_exception","msg":"服务调用出错",
                    // "subErrors":[{"code":"kaola.order.get.not_found","msg":"订单不存在"}]}}"

                }
                else if (odmes.Contains("kaola.order.get.order_id_empty"))
                {
                    return "订单号为空";
                }
            }
            catch (Exception ex)
            {
                return "出错了，原因：" + ex.Message;
            }

            //if (count == dc1.Count)
            //{
            //    return "获取成功!";
            //}
            return "获取订单信息失败，更新订单信息失败！";
        }

        /// <summary>
        /// 商家发货通知 
        /// </summary>
        /// <returns></returns>
        public string kaola_logistics_deliver(string order_id, string expressName, string expressNO, DataTable dt)
        {
            string data = kl.kaola_logistics_deliver("kaola.logistics.deliver", time, order_id, expressName, expressNO, dt);
            //string data = kl.kaola_logistics_deliver("kaola.logistics.deliver", time, "2015121611171011020046283", "EMS", "1111");
            return data;
        }

        #region 原来的
        /// <summary>
        /// 商家发货通知
        /// </summary>
        /// <returns></returns>
        //public string kaola_logistics_deliver(string order_id, string expressName, string expressNO)
        //{

        //    string data = kl.kaola_logistics_deliver("kaola.logistics.deliver", time, order_id, expressName, expressNO);
        //    //string data = kl.kaola_logistics_deliver("kaola.logistics.deliver", time, "2015121611171011020046283", "EMS", "1111");

        //    return data;
        //}

        #endregion
        /// <summary>
        /// 获取快递公司的信息   调试成功
        /// </summary>
        /// <returns></returns>
        public string kaola_logistics_companies_get()
        {
            string data = kl.kaola_logistics_companies_get("kaola.logistics.companies.get", time);


            List<order_model> list = new List<order_model>();
            List<string> listSQL = new List<string>();

            List<string> ccli = new List<string>();
            int count;
            if (data.Contains("kaola_logistics_companies_get_response"))
            {
                ccli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select express_company_code from bms_kaola_logisticsCompany;"));
                dynamic dc = JsonConvert.DeserializeObject(data);
                var dc1 = dc.kaola_logistics_companies_get_response.logistics_companys;
                for (int i = 0; i < dc1.Count; i++)
                {
                    string sql = string.Empty;
                    string orderJson = dc1[i].ToString();
                    dynamic o = JsonConvert.DeserializeObject(orderJson);

                    if (!string.IsNullOrWhiteSpace(o.express_company_code.ToString()))
                    {
                        if (ccli.Contains(o.express_company_code.ToString()))
                        {
                            sql += string.Format(@"update bms_kaola_logisticsCompany set express_company_name='" + o.express_company_name.ToString() + "' where express_company_code='" + o.express_company_code.ToString() + "'");
                        }
                        else
                        {
                            sql += string.Format(@"insert into bms_kaola_logisticsCompany(express_company_code,express_company_name) 
                values('{0}','{1}');", o.express_company_code.ToString(), o.express_company_name.ToString());
                        }
                        listSQL.Add(sql);
                    }
                }

                count = DbHelperMySQL.ExecuteSqlTran(listSQL);
            }
            else
            {
                return "获取快递公司信息失败";
            }

            return data + "更新" + count + "条数据";
        }

        #endregion
        kaola_category kc = new kaola_category();

        #region 类目
        /// <summary>
        /// 获取标准商品类目属性 调用成功
        /// </summary>
        /// <returns></returns>
        public string kaola_itemprops_get()
        {
            string s = string.Empty;
            string data = string.Empty;
            string sqlInsert = string.Empty;
            List<string> listSQL = new List<string>();

            #region 获取商家类目属性
            string sqlSelect = string.Format("select category_id from bms_kaola_vendercategory where is_leaf=1");
            DataTable dt = DbHelperMySQL.Query(sqlSelect).Tables[0];
            if (dt != null)
            {
                int num = dt.Rows.Count;
                for (int i = 0; i < num; i++)
                {
                    string category_id = dt.Rows[i]["category_id"].ToString();                                                      //末级类目ID
                    data = kc.kaola_itemprops_get("kaola.itemprops.get", time, category_id);
                    dynamic o = JsonConvert.DeserializeObject(data);
                    if (o.kaola_itemprops_get_response != null)
                    {
                        if (o.kaola_itemprops_get_response.property_category_list != null)
                        {
                            var property_category_list = o.kaola_itemprops_get_response.property_category_list;
                            for (int j = 0; j < property_category_list.Count; j++)
                            {
                                var property_name = property_category_list[j].property_name;                                        //属性名称(包含属性编辑策略+属性名基本信息)

                                var property_edit_policy = property_name.property_edit_policy;                                      //属性编辑策略(属性规则)
                                #region 属性编辑策略(属性规则)
                                string desc = property_edit_policy.desc;                                                            //属性名ID
                                string input_type = property_edit_policy.input_type;                                                //输入控件类型: 1:单行文本 2:多行文本 3: 下拉列表 4: 单选框 5: 多选框 6:文件
                                string is_multichoice = property_edit_policy.is_multichoice;                                        //输入描述信息
                                string is_necessary = property_edit_policy.is_necessary;                                            //最大输入文字数
                                string max_len = property_edit_policy.max_len;                                                      //是否多选
                                string need_image = property_edit_policy.need_image;                                                //是否需要图片, 主要针对服装鞋帽类不同颜色
                                string property_name_id = property_edit_policy.property_name_id;                                    //是否必须
                                #region 判断自定义
                                //TEXT (1, "单行文本框"),
                                //TEXTAREA(2, "多行文本框"),
                                //SELECT(3, "下拉列表"),
                                //RADIO (4, "单选项"),
                                //CHECKBOX (5, "多选项"),
                                //FILE(6, "文件");


                                int type = 0;
                                if (input_type == "FILE") //1:单行文本 2:多行文本 3: 下拉列表 4: 单选框 5: 多选框 6:文件
                                {
                                    type = 6;
                                }
                                else if (input_type == "CHECKBOX")
                                {
                                    type = 5;
                                }
                                else if (input_type == "RADIO")
                                {
                                    type = 4;
                                }
                                else if (input_type == "SELECT")
                                {
                                    type = 3;
                                }
                                else if (input_type == "TEXTAREA")
                                {
                                    type = 2;
                                }
                                else if (input_type == "TEXT")
                                {
                                    type = 1;
                                }
                                #endregion
                                if (!string.IsNullOrWhiteSpace(desc))
                                {
                                    if (desc.Contains("'"))
                                    {
                                        desc = string.IsNullOrWhiteSpace(desc) ? desc : desc.Replace("'", "\\'");
                                    }
                                }
                                sqlInsert = string.Format("insert into  bms_kaola_propertyeditpolicy values(null,'{0}',{1},'{2}',{3},{4},{5},{6},'{7}')",
                                    property_name_id, type, desc, max_len, is_multichoice, need_image, is_necessary, category_id);
                                listSQL.Add(sqlInsert);
                                #endregion

                                var raw_property_name = property_name.raw_property_name;                                            //属性名基本信息
                                #region 属性名基本信息
                                string is_color = raw_property_name.is_color;                                                    //是否是颜色属性。对于服装类，颜色属性需要上传图片。1：是 0：否
                                string is_display = raw_property_name.is_display;                                                //是否显示 1：是 0：否
                                string is_filter = raw_property_name.is_filter;                                                  //是否筛选条件1：是 0：否
                                string is_logistics = raw_property_name.is_logistics;                                            //是否物流属性 1：是 0：否
                                string is_sku = raw_property_name.is_sku;                                                        //是否sku属性1：是 0：否
                                string prop_Name_en = raw_property_name.prop_Name_en;                                            //属性名英文
                                string prop_name_cn = raw_property_name.prop_name_cn;                                            //属性名中文
                                string prop_name_id = raw_property_name.prop_name_id;                                            //属性名id
                                string status = raw_property_name.status;                                                     //状态 1：正常 0：无效
                                sqlInsert = string.Format("insert into  bms_kaola_rawpropertyname values(null,'{0}','{1}','{2}',{3},{4},{5},{6},{7},{8},'{9}')",
                                    prop_name_id, prop_name_cn, prop_Name_en, is_sku, is_filter, is_display, is_color, is_logistics, status, category_id);
                                listSQL.Add(sqlInsert);
                                #endregion

                                var property_value_list = property_category_list[j].property_value_list;                            //属性值集合
                                for (int k = 0; k < property_value_list.Count; k++)
                                {
                                    #region 属性值集合
                                    string is_sys_property = property_value_list[k].is_sys_property;                                  //是否系统属性。1：系统属性 0：用户自定义
                                    string list_property_name_id = property_value_list[k].property_name_id;                           //属性名id
                                    string property_value = property_value_list[k].property_value;                                    //属性值
                                    string property_value_icon = property_value_list[k].property_value_icon;                          //属性值图标
                                    string property_value_id = property_value_list[k].property_value_id;                              //属性值id
                                    string show_order = property_value_list[k].show_order;                                            //显示顺序
                                    string list_status = property_value_list[k].status;                                               //状态 1：正常 0：无效
                                    sqlInsert = string.Format("insert into  bms_kaola_propertyvalue values(null,'{0}','{1}','{2}','{3}',{4},{5},{6},'{7}')",
                                        property_value_id, property_value, list_property_name_id, property_value_icon, is_sys_property, show_order, list_status, category_id);
                                    listSQL.Add(sqlInsert);
                                    #endregion
                                }
                                var raw_property_category = property_category_list[j].raw_property_category;                        //类目和属性对应信息
                                #region 类目和属性对应信息
                                string raw_category_id = raw_property_category.category_id;                                         //属性名id
                                string raw_property_name_id = raw_property_category.property_name_id;                               //末级节点类目
                                sqlInsert = string.Format("insert into  bms_kaola_rawpropertycategory values(null,{0},{1})", raw_property_name_id, raw_category_id);
                                listSQL.Add(sqlInsert);
                                #endregion
                            }
                        }
                    }
                }

                int count = DbHelperMySQL.ExecuteSqlTran(listSQL);
                if (count == listSQL.Count)
                {
                    s = "更新成功！";
                }
                else
                {
                    s = "更新失败！";
                }

            }
            #endregion

            return s;


            #region MyRegion
            //string ciddata = ks.kaola_vender_category_get("kaola.vender.category.get", time);//获取类目id
            //int count = 0;
            //List<string> listSQL = new List<string>();//sql集合
            //StringBuilder sb = new StringBuilder(200);
            //if (ciddata.Contains("kaola_vender_category_get_response"))
            //{
            //    dynamic cat = JsonConvert.DeserializeObject(ciddata);
            //    var cat1 = cat.kaola_vender_category_get_response.Item_cats;
            //    if (cat1 != null && cat1.Count > 0)//得到的类目信息是否为空
            //    {
            //        for (int y = 0; y < cat1.Count; y++)
            //        {
            //            string obj = cat1[y].ToString();
            //            dynamic obj1 = JsonConvert.DeserializeObject(obj);
            //            string data = kc.kaola_itemprops_get("kaola.itemprops.get", time, obj1.category_id.ToString());//根据类目id获取商品类目属性
            //            sb.Append(data);
            //            if (data.Contains("kaola_itemprops_get_response"))
            //            {
            //                List<order_model> list = new List<order_model>();

            //                dynamic dc = JsonConvert.DeserializeObject(data);
            //                var dc1 = dc.kaola_itemprops_get_response.property_category_list;

            //                if (dc1 != null && dc1.Count > 0)
            //                {
            //                    for (int i = 0; i < dc1.Count; i++)
            //                    {
            //                        string sql = string.Empty;
            //                        string orderJson = dc1[i].ToString();
            //                        dynamic o = JsonConvert.DeserializeObject(orderJson);
            //                        dynamic propertyPolicy = JsonConvert.DeserializeObject(o.property_name.property_edit_policy.ToString());  //property_edit_policy   1
            //                        dynamic rawPropertyName = JsonConvert.DeserializeObject(o.property_name.raw_property_name.ToString());    //raw_property_name      1
            //                        dynamic propertyValue = JsonConvert.DeserializeObject(o.property_value_list.ToString());                  //property_value_list
            //                        dynamic rawPropertyCategory = JsonConvert.DeserializeObject(o.raw_property_category.ToString());          //raw_property_category       1
            //                        int propertyValueCount = o.property_value_list.Count;    //存在数组

            //                        if (propertyPolicy != null)
            //                        {
            //                            #region 判断自定义
            //                            //TEXT (1, "单行文本框"),
            //                            //TEXTAREA(2, "多行文本框"),
            //                            //SELECT(3, "下拉列表"),
            //                            //RADIO (4, "单选项"),
            //                            //CHECKBOX (5, "多选项"),
            //                            //FILE(6, "文件");


            //                            int type = 0;
            //                            if (propertyPolicy.input_type == "FILE") //1:单行文本 2:多行文本 3: 下拉列表 4: 单选框 5: 多选框 6:文件
            //                            {
            //                                type = 6;
            //                            }
            //                            else if (propertyPolicy.input_type == "CHECKBOX")
            //                            {
            //                                type = 5;
            //                            }
            //                            else if (propertyPolicy.input_type == "RADIO")
            //                            {
            //                                type = 4;
            //                            }
            //                            else if (propertyPolicy.input_type == "SELECT")
            //                            {
            //                                type = 3;
            //                            }
            //                            else if (propertyPolicy.input_type == "TEXTAREA")
            //                            {
            //                                type = 2;
            //                            }
            //                            else if (propertyPolicy.input_type == "TEXT")
            //                            {
            //                                type = 1;
            //                            }
            //                            #endregion

            //                            //描述中有单引号
            //                            string descstr = propertyPolicy.desc.ToString();
            //                            if (descstr.Contains("'"))
            //                            {
            //                                descstr = descstr.Replace("'", "''");
            //                            }
            //                            if ((DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select category_id from bms_kaola_propertyeditpolicy where property_name_id='" + propertyPolicy.property_name_id + "';"))).Contains(obj1.category_id.ToString()))//pnili.Contains(propertyPolicy.property_name_id.ToString()) && 
            //                            {
            //                                sql += string.Format(@"update bms_kaola_propertyeditpolicy set input_type='{0}',`desc`='{1}',max_len='{2}',is_multichoice='{3}',need_image='{4}',is_necessary='{5}' where property_name_id='{6}' and category_id={7};", type, descstr, propertyPolicy.max_len, propertyPolicy.is_multichoice, propertyPolicy.need_image, propertyPolicy.is_necessary, propertyPolicy.property_name_id, obj1.category_id.ToString());
            //                            }
            //                            else
            //                            {
            //                                sql += string.Format("insert into bms_kaola_propertyeditpolicy values({0},'{1}',{2},'{3}',{4},{5},{6},{7},{8});",
            //                                "Null", propertyPolicy.property_name_id, type, descstr, propertyPolicy.max_len,
            //                                propertyPolicy.is_multichoice, propertyPolicy.need_image, propertyPolicy.is_necessary, obj1.category_id.ToString());
            //                            }
            //                        }

            //                        if (rawPropertyName != null)
            //                        {
            //                            if ((DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select category_id from bms_kaola_rawpropertyname where prop_name_id='" + rawPropertyName.prop_name_id + "';"))).Contains(obj1.category_id.ToString()))//prnid.Contains(rawPropertyName.prop_name_id.ToString()) && 
            //                            {
            //                                sql += string.Format(@"update bms_kaola_rawpropertyname set prop_name_cn='{0}',prop_name_en='{1}',is_sku='{2}',is_filter='{3}',is_display='{4}',is_color='{5}',is_logistics='{6}',`status`='{7}' where prop_name_id='{8}' and category_id={9};", rawPropertyName.prop_name_cn, rawPropertyName.prop_name_en, rawPropertyName.is_sku, rawPropertyName.is_filter, rawPropertyName.is_display, rawPropertyName.is_color, rawPropertyName.is_logistics, rawPropertyName.status, rawPropertyName.prop_name_id, obj1.category_id.ToString());
            //                            }
            //                            else
            //                            {
            //                                sql += string.Format("insert into bms_kaola_rawpropertyname values({0},'{1}','{2}','{3}',{4},{5},{6},{7},{8},{9},{10});",
            //                              "Null", rawPropertyName.prop_name_id, rawPropertyName.prop_name_cn, rawPropertyName.prop_name_en, rawPropertyName.is_sku,
            //                               rawPropertyName.is_filter, rawPropertyName.is_display, rawPropertyName.is_color, rawPropertyName.is_logistics, rawPropertyName.status, obj1.category_id.ToString());
            //                            }

            //                        }
            //                        if (rawPropertyCategory != null)
            //                        {
            //                            if (DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select property_name_id from bms_kaola_rawpropertycategory where property_name_id='" + rawPropertyCategory.property_name_id + "' and category_id='" + rawPropertyCategory.category_id + "'")).Count == 0)
            //                            {
            //                                sql += string.Format("insert into bms_kaola_rawpropertycategory values('{0}',{1});",
            //                                 rawPropertyCategory.property_name_id, rawPropertyCategory.category_id);
            //                            }
            //                        }
            //                        if (propertyValueCount > 0)
            //                        {
            //                            for (int m = 0; m < propertyValueCount; m++)//property_value_list
            //                            {
            //                                if ((DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select category_id from bms_kaola_propertyvalue where property_value_id='" + propertyValue[m].property_value_id.ToString() + "'"))).Contains(obj1.category_id.ToString()))//pvidli.Contains(propertyValue[m].property_value_id.ToString()) && 
            //                                {
            //                                    string sqlPropertyValue = string.Format(@"update bms_kaola_propertyvalue set property_value='{0}',property_name_id='{1}',property_value_icon='{2}',is_sys_property='{3}',show_order='{4}',`status`='{5}' where property_value_id='{6}' and category_id={7};", propertyValue[m].property_value, propertyValue[m].property_name_id, propertyValue[m].property_value_icon, propertyValue[m].is_sys_property, propertyValue[m].show_order, propertyValue[m].status, propertyValue[m].property_value_id, obj1.category_id.ToString());

            //                                    sql += sqlPropertyValue;
            //                                }
            //                                else
            //                                {
            //                                    string sqlPropertyValue = string.Format(@"insert into bms_kaola_propertyvalue  values({0},'{1}','{2}','{3}','{4}',{5},{6},{7},{8});", "Null", propertyValue[m].property_value_id, propertyValue[m].property_value, propertyValue[m].property_name_id, propertyValue[m].property_value_icon, propertyValue[m].is_sys_property, propertyValue[m].show_order, propertyValue[m].status, obj1.category_id.ToString());
            //                                    sql += sqlPropertyValue;
            //                                }

            //                            }
            //                        }
            //                        if (!string.IsNullOrWhiteSpace(sql))
            //                        {
            //                            listSQL.Add(sql);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        if (listSQL != null || listSQL.Count >= 0)
            //        {
            //            count = DbHelperMySQL.ExecuteSqlTran(listSQL);
            //            if (count > 0)
            //            {
            //                return sb.ToString() + "++++++++++++++++++++++++++++更新" + count + "条数据++++++++++++++++++++++++++++++++++";
            //            }
            //            else
            //            {
            //                return sb.ToString() + "++++++++++++++++++++++++++++更新出错+++++++++++++++++++++++++++++";
            //            }
            //        }
            //    }
            //}

            #endregion
            // return "获取失败";
        }

        /// <summary>
        /// 获取标准商品类目属性的值
        /// </summary>
        /// <returns></returns>
        public string kaola_itempropvalues_get()
        {
            #region 获取商家类目属性值
            string s = string.Empty;
            string sqlInsert = string.Empty;
            string sqlSelect = string.Format("select property_value_id from bms_kaola_propertyvalue");
            List<string> listSQL = new List<string>();

            DataTable dt = DbHelperMySQL.Query(sqlSelect).Tables[0];
            if (dt != null)
            {
                int num = dt.Rows.Count;
                for (int i = 0; i < num; i++)
                {
                    string dt_property_value_id = dt.Rows[i]["property_value_id"].ToString();                                                      //属性值ID
                    string data = kc.kaola_itempropvalues_get("kaola.itempropvalues.get", time, dt_property_value_id);                             //返回的josn格式数据
                    dynamic o = JsonConvert.DeserializeObject(data);
                    if (o.kaola_itempropvalues_get_response != null)
                    {
                        var js_property_value_list = o.kaola_itempropvalues_get_response;

                        #region 属性值集合
                        string is_sys_property = js_property_value_list.is_sys_property;                                  //是否系统属性。1：系统属性 0：用户自定义
                        string list_property_name_id = js_property_value_list.property_name_id;                           //属性名id
                        string property_value = js_property_value_list.property_value_list;                               //属性值
                        string property_value_icon = js_property_value_list.property_value_icon;                          //属性值图标
                        string property_value_id = js_property_value_list.property_value_id;                              //属性值id
                        string show_order = js_property_value_list.show_order;                                            //显示顺序
                        string list_status = js_property_value_list.status;                                               //状态 1：正常 0：无效
                        sqlInsert = string.Format("insert into  bms_kaola_propertyvalue values(null,'{0}','{1}','{2}','{3}',{4},{5},{6})",
                            property_value_id, property_value, list_property_name_id, property_value_icon, is_sys_property, show_order, list_status);
                        listSQL.Add(sqlInsert);
                        #endregion
                    }
                }

                int count = DbHelperMySQL.ExecuteSqlTran(listSQL);
                if (count == listSQL.Count)
                {
                    s = "更新成功！";
                }
                else
                {
                    s = "更新失败！";
                }
            }
            #endregion
            return s;


            //            string data = kc.kaola_itempropvalues_get("kaola.itempropvalues.get", time, "54474");
            //            int count = 0;
            //            if (data.Contains("kaola_itempropvalues_get_response"))
            //            {
            //                List<string> listSQL = new List<string>();
            //                dynamic dc = JsonConvert.DeserializeObject(data);
            //                var dc1 = dc.kaola_itempropvalues_get_response;
            //                List<string> pvli = new List<string>();
            //                pvli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select property_value_id from bms_kaola_propertyvalue;"));
            //                string sql = string.Empty;
            //                if (dc1 != null)
            //                {
            //                    string orderJson = dc1.ToString();
            //                    dynamic o = JsonConvert.DeserializeObject(orderJson);
            //                    if (pvli.Contains(o.property_value_id.ToString()))
            //                    {
            //                        sql = string.Format(@"update bms_kaola_propertyvalue set is_sys_property='{0}',property_name_id='{1}',property_value='{2}',property_value_icon='{3}',show_order='{4}',status='{5}' where property_value_id='{6}'", o.is_sys_property, o.property_name_id, o.property_value, o.property_value_icon, o.show_order, o.status, o.property_value_id);
            //                    }
            //                    else
            //                    {
            //                        sql = string.Format(@"insert into 
            //            bms_kaola_propertyvalue(is_sys_property,property_name_id,property_value,property_value_icon,property_value_id,show_order,status)
            //            values('{0}',{1},'{2}','{3}',{4},{5},{6},{7})",
            //                          o.is_sys_property, o.property_name_id, o.property_value,
            //                          o.property_value_icon, o.property_value_id, o.show_order, o.status, "Null");
            //                    }
            //                }


            //                listSQL.Add(sql);
            //                if (listSQL != null)
            //                {
            //                    count = DbHelperMySQL.ExecuteSqlTran(listSQL);
            //                    if (count > 0)
            //                    {
            //                        return data + "++++++++++++++++++++++++++++更新" + count + "条数据+++++++++++++++++++++++++++";
            //                    }
            //                    else
            //                    {
            //                        return data + "++++++++++++++++++++++++++++更新失败+++++++++++++++++++++++++++";
            //                    }
            //                }
            //                //int count = DbHelperMySQL.ExecuteSqlTran(listSQL);
            //            }
            //return data;

            return "";
        }

        /// <summary>
        /// 商品上架    调用成功（操作的商品不存在）
        /// </summary>
        /// <returns></returns>
        public string kaola_item_update_listing()
        {
            //85370-3049
            //2211546655
            //string data = kc.kaola_item_update_listing("kaola.item.update.listing", time, new string[] { "85370-3049" });
            string data = kc.kaola_item_update_listing("kaola.item.update.listing", time, "85370-3049");

            //string data = kc.kaola_item_update_listing("kaola.item.update.listing", time, new string[] { "2211546655" });
            return data;
        }

        /// <summary>   
        /// 商品下架     调用成功（操作的商品不存在）
        /// </summary>
        /// <returns></returns>
        public string kaola_item_update_delisting()
        {
            //string data = kc.kaola_item_update_delisting("kaola.item.update.delisting", time,  "83952-3049" );
            string data = kc.kaola_item_update_delisting("kaola.item.update.delisting", time, "85370-3049");

            return data;
        }

        /// <summary>
        /// 根据商品id获取单个商品的详细信息    调用成功（操作商品不存在）
        /// </summary>
        /// <returns></returns>
        public string kaola_item_get()
        {
            //有中括号的为一个集合
            //85359-3049
            //string data = kc.kaola_item_get("kaola.item.get", time, "83952-3049");
            string data = kc.kaola_item_get("kaola.item.get", time, "41346-12010");
            dynamic dc = JsonConvert.DeserializeObject(data);
            var dc1 = dc.kaola_item_get_response;
            if (dc1 != null)
            {
                List<string> listSQL = new List<string>();

                #region
                List<string> Sqlstr = new List<string>();

                List<string> icidli = new List<string>();
                icidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select id from bms_kaola_itemcategory;"));                //商品类目信息id

                List<string> iimidli = new List<string>();
                iimidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select id  from bms_kaola_itemimage;"));                 //商品图片id

                List<string> ipidli = new List<string>();
                ipidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select id from bms_kaola_itempostage;"));                 //运费信息id

                List<string> pepidli = new List<string>();
                pepidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select property_name_id from bms_kaola_propertyeditpolicy;"));  //属性编辑策略id

                List<string> rppnidli = new List<string>();
                rppnidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select prop_name_id from bms_kaola_rawpropertyname;"));//属性名基本信息id

                List<string> pvidli = new List<string>();
                pvidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select property_value_id from  bms_kaola_propertyvalue;")); //属性值id

                List<string> rippidli = new List<string>();
                rippidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select id from bms_kaola_rawitemproperty;"));             //商品属性对应信息id

                List<string> itpidli = new List<string>();
                itpidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select id from bms_kaola_itemtextproperty;"));            //商品自定义属性id

                List<string> rieidli = new List<string>();
                rieidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select goodid from bms_kaola_rawitemedit;"));               //商品基本信息id

                List<string> rskuidli = new List<string>();
                rskuidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select id from bms_kaola_rawsku;"));                        //SKU基本信息

                List<string> rskupid = new List<string>();
                rskupid = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select id from bms_kaola_rawskuproperty;"));              //SKU基本信息

                string sqlstrs = "";
                #region item_category_list       商品类目信息
                var dc2 = dc1.item_category_list;
                for (int j = 0; j < dc2.Count; j++)
                {
                    string cat = dc2[j].ToString();
                    dynamic c = JsonConvert.DeserializeObject(cat);
                    bms_kaola_itemcategory bkic = new bms_kaola_itemcategory();
                    if (c != null)
                    {
                        if (icidli.Contains(c.id.ToString()))
                        {
                            sqlstrs = string.Format(@"update bms_kaola_itemcategory set item_id={0},business_id={1},category_id={2},rank={3} where id={4}", (string.IsNullOrWhiteSpace(c.item_id.ToString())) ? "NULL" : c.item_id, (string.IsNullOrWhiteSpace(c.business_id.ToString())) ? "NULL" : c.business_id, (string.IsNullOrWhiteSpace(c.category_id.ToString())) ? "NULL" : c.category_id, (string.IsNullOrWhiteSpace(c.rank.ToString())) ? "NULL" : c.rank, (string.IsNullOrWhiteSpace(c.id.ToString())) ? "NULL" : c.id);
                        }
                        else
                        {
                            sqlstrs = string.Format(@"insert into bms_kaola_itemcategory values({0},{1},{2},{3},{4})", (string.IsNullOrWhiteSpace(c.id.ToString())) ? "NULL" : c.id, (string.IsNullOrWhiteSpace(c.item_id.ToString())) ? "NULL" : c.item_id, (string.IsNullOrWhiteSpace(c.business_id.ToString())) ? "NULL" : c.business_id, (string.IsNullOrWhiteSpace(c.category_id.ToString())) ? "NULL" : c.category_id, (string.IsNullOrWhiteSpace(c.rank.ToString())) ? "NULL" : c.rank);
                        }
                        Sqlstr.Add(sqlstrs);
                        sqlstrs = "";
                    }
                }

                #endregion

                #region item_image_list        商品图片
                var dc3 = dc1.item_image_list;
                for (int k = 0; k < dc3.Count; k++)
                {
                    string img = dc3[k].ToString();
                    dynamic im = JsonConvert.DeserializeObject(img);
                    if (im != null)
                    {
                        int img_type = 0;
                        if (im.image_type.ToString() == "MAIN")
                        {
                            img_type = 1;
                        }
                        else if (im.image_type.ToString() == "APP")
                        {
                            img_type = 2;
                        }
                        if (iimidli.Contains(im.id.ToString()))
                        {
                            sqlstrs = string.Format(@"update bms_kaola_itemimage set item_id={0},business_id={1},image_url='{2}',image_type='{3}',order_value={4} where id={5};", (string.IsNullOrWhiteSpace(im.item_id.ToString())) ? "NULL" : im.item_id, (string.IsNullOrWhiteSpace(im.business_id.ToString())) ? "NULL" : im.business_id, im.image_url, img_type, (string.IsNullOrWhiteSpace(im.order_value.ToString())) ? "NULL" : im.order_value, im.id);
                        }
                        else
                        {
                            sqlstrs = string.Format(@"insert into bms_kaola_itemimage values({0},{1},{2},'{3}','{4}',{5})", (string.IsNullOrWhiteSpace(im.id.ToString())) ? "NULL" : im.id, (string.IsNullOrWhiteSpace(im.item_id.ToString())) ? "NULL" : im.item_id, (string.IsNullOrWhiteSpace(im.business_id.ToString())) ? "NULL" : im.business_id, im.image_url, img_type, (string.IsNullOrWhiteSpace(im.order_value.ToString())) ? "NULL" : im.order_value);
                        }
                        Sqlstr.Add(sqlstrs);
                        sqlstrs = "";
                    }
                }
                #endregion

                #region item_postage         运费信息
                var dc4 = dc1.item_postage;
                dynamic itp = JsonConvert.DeserializeObject(dc4.ToString());
                if (itp != null)
                {
                    if (ipidli.Contains(itp.id.ToString()))
                    {
                        sqlstrs = string.Format(@"update bms_kaola_itempostage set item_id={0},business_id={1},is_postage_free={2},post_fee={3},express_fee={4},ems_fee={5},postage_template_id='{6}' where id={7};",
                               (string.IsNullOrWhiteSpace(itp.item_id.ToString())) ? "NULL" : itp.item_id, (string.IsNullOrWhiteSpace(itp.business_id.ToString())) ? "NULL" : itp.business_id, (string.IsNullOrWhiteSpace(itp.is_postage_free.ToString())) ? "NULL" : itp.is_postage_free, (string.IsNullOrWhiteSpace(itp.post_fee.ToString())) ? "NULL" : itp.post_fee, (string.IsNullOrWhiteSpace(itp.express_fee.ToString())) ? "NULL" : itp.express_fee, (string.IsNullOrWhiteSpace(itp.ems_fee.ToString())) ? "NULL" : itp.ems_fee, itp.postage_template_id, (string.IsNullOrWhiteSpace(itp.id.ToString())) ? "NULL" : itp.id);
                    }
                    else
                    {
                        sqlstrs = string.Format("insert into bms_kaola_itempostage values({0},{1},{2},{3},{4},{5},{6},'{7}')", (string.IsNullOrWhiteSpace(itp.id.ToString())) ? "NULL" : itp.id, (string.IsNullOrWhiteSpace(itp.item_id.ToString())) ? "NULL" : itp.item_id, (string.IsNullOrWhiteSpace(itp.business_id.ToString())) ? "NULL" : itp.business_id, (string.IsNullOrWhiteSpace(itp.is_postage_free.ToString())) ? "NULL" : itp.is_postage_free, (string.IsNullOrWhiteSpace(itp.post_fee.ToString())) ? "NULL" : itp.post_fee, (string.IsNullOrWhiteSpace(itp.express_fee.ToString())) ? "NULL" : itp.express_fee, (string.IsNullOrWhiteSpace(itp.ems_fee.ToString())) ? "NULL" : itp.ems_fee, itp.postage_template_id);
                    }
                    Sqlstr.Add(sqlstrs);
                    sqlstrs = "";
                }
                #endregion

                #region item_property_list    商品预定义属性
                var dc5 = dc1.item_property_list;
                for (int l = 0; l < dc5.Count; l++)
                {
                    string pro = dc5[l].ToString();
                    dynamic pr = JsonConvert.DeserializeObject(pro);
                    //属性编辑策略
                    var pep = pr.property_name.property_edit_policy;

                    dynamic pepy = JsonConvert.DeserializeObject(pep.ToString());
                    bms_kaola_propertyeditpolicy bkp = new bms_kaola_propertyeditpolicy();
                    if (pepy != null)
                    {
                        #region 判断自定义
                        //TEXT (1, "单行文本框"),
                        //TEXTAREA(2, "多行文本框"),
                        //SELECT(3, "下拉列表"),
                        //RADIO (4, "单选项"),
                        //CHECKBOX (5, "多选项"),
                        //FILE(6, "文件");


                        int type = 0;
                        if (pepy.input_type == "FILE") //1:单行文本 2:多行文本 3: 下拉列表 4: 单选框 5: 多选框 6:文件
                        {
                            type = 6;
                        }
                        else if (pepy.input_type == "CHECKBOX")
                        {
                            type = 5;
                        }
                        else if (pepy.input_type == "RADIO")
                        {
                            type = 4;
                        }
                        else if (pepy.input_type == "SELECT")
                        {
                            type = 3;
                        }
                        else if (pepy.input_type == "TEXTAREA")
                        {
                            type = 2;
                        }
                        else if (pepy.input_type == "TEXT")
                        {
                            type = 1;
                        }

                        #endregion
                        if (pepidli.Contains(pepy.property_name_id.ToString()))
                        {
                            sqlstrs = string.Format(@"update bms_kaola_propertyeditpolicy set input_type={0},`desc`='{1}',max_len={2},is_multichoice={3},need_image={4},is_necessary={5} where property_name_id='{6}'", type, pepy.desc, (string.IsNullOrWhiteSpace(pepy.max_len.ToString())) ? "Null" : pepy.max_len, (string.IsNullOrWhiteSpace(pepy.is_multichoice.ToString())) ? "Null" : pepy.is_multichoice, (string.IsNullOrWhiteSpace(pepy.need_image.ToString())) ? "Null" : pepy.need_image, (string.IsNullOrWhiteSpace(pepy.is_necessary.ToString())) ? "Null" : pepy.is_necessary, pepy.property_name_id);
                        }
                        else
                        {
                            sqlstrs = string.Format(@"insert into bms_kaola_propertyeditpolicy  values(NULL,'{0}',{1},'{2}',{3},{4},{5},{6});", pepy.property_name_id, type, pepy.desc, (string.IsNullOrWhiteSpace(pepy.max_len.ToString())) ? "Null" : pepy.max_len, (string.IsNullOrWhiteSpace(pepy.is_multichoice.ToString())) ? "Null" : pepy.is_multichoice, (string.IsNullOrWhiteSpace(pepy.need_image.ToString())) ? "Null" : pepy.need_image, (string.IsNullOrWhiteSpace(pepy.is_necessary.ToString())) ? "Null" : pepy.is_necessary);
                        }
                        Sqlstr.Add(sqlstrs);
                        sqlstrs = "";
                    }
                    //属性名   属性名基本信息
                    var rp = pr.property_name.raw_property_name;
                    dynamic rpn = JsonConvert.DeserializeObject(rp.ToString());
                    if (rpn != null)
                    {
                        if (rppnidli.Contains(rpn.prop_name_id.ToString()))
                        {
                            sqlstrs = string.Format(@"update bms_kaola_rawpropertyname set prop_name_cn='{0}',prop_name_en='{1}',is_sku={2},is_filter={3},is_display={4},is_color={5},is_logistics={6},`status`={7} where prop_name_id='{8}';", rpn.prop_name_cn, rpn.prop_Name_en, (string.IsNullOrWhiteSpace(rpn.is_sku.ToString())) ? "Null" : rpn.is_sku, (string.IsNullOrWhiteSpace(rpn.is_filter.ToString())) ? "Null" : rpn.is_filter, (string.IsNullOrWhiteSpace(rpn.is_display.ToString())) ? "Null" : rpn.is_display, (string.IsNullOrWhiteSpace(rpn.is_color.ToString())) ? "Null" : rpn.is_color, (string.IsNullOrWhiteSpace(rpn.is_logistics.ToString())) ? "Null" : rpn.is_logistics, (string.IsNullOrWhiteSpace(rpn.status.ToString())) ? "Null" : rpn.status, rpn.prop_name_id);
                        }
                        else
                        {
                            sqlstrs = string.Format(@"insert into bms_kaola_rawpropertyname values(NULL,'{0}','{1}','{2}',{3},{4},{5},{6},{7},{8});", rpn.prop_name_id, rpn.prop_name_cn, rpn.prop_Name_en, (string.IsNullOrWhiteSpace(rpn.is_sku.ToString())) ? "Null" : rpn.is_sku, (string.IsNullOrWhiteSpace(rpn.is_filter.ToString())) ? "Null" : rpn.is_filter, (string.IsNullOrWhiteSpace(rpn.is_display.ToString())) ? "Null" : rpn.is_display, (string.IsNullOrWhiteSpace(rpn.is_color.ToString())) ? "Null" : rpn.is_color, (string.IsNullOrWhiteSpace(rpn.is_logistics.ToString())) ? "Null" : rpn.is_logistics, (string.IsNullOrWhiteSpace(rpn.status.ToString())) ? "Null" : rpn.status);
                        }
                        Sqlstr.Add(sqlstrs);
                        sqlstrs = "";
                    }
                    //属性值
                    var pv = pr.property_value;
                    dynamic pvl = JsonConvert.DeserializeObject(pv.ToString());
                    if (pvl != null)
                    {
                        if (pvidli.Contains(pvl.property_value_id.ToString()))
                        {
                            sqlstrs = string.Format(@"update bms_kaola_propertyvalue set property_value='{0}',property_name_id='{1}',property_value_icon='{2}',is_sys_property={3},show_order={4},`status`={5} where property_value_id='{6}';", pvl.property_value, pvl.property_name_id, pvl.property_value_icon, (string.IsNullOrWhiteSpace(pvl.is_sys_property.ToString())) ? "Null" : pvl.is_sys_property, (string.IsNullOrWhiteSpace(pvl.show_order.ToString())) ? "Null" : pvl.show_order, (string.IsNullOrWhiteSpace(pvl.status.ToString())) ? "Null" : pvl.status, pvl.property_value_id);
                        }
                        //重复
                        else
                        {
                            sqlstrs = string.Format("insert into bms_kaola_propertyvalue values(NULL,'{0}','{1}','{2}','{3}',{4},{5},{6})", pvl.property_value_id, pvl.property_value, pvl.property_name_id, pvl.property_value_icon, (string.IsNullOrWhiteSpace(pvl.is_sys_property.ToString())) ? "Null" : pvl.is_sys_property, (string.IsNullOrWhiteSpace(pvl.show_order.ToString())) ? "Null" : pvl.show_order, (string.IsNullOrWhiteSpace(pvl.status.ToString())) ? "Null" : pvl.status);
                        }
                        Sqlstr.Add(sqlstrs);
                        sqlstrs = "";

                    }
                    //商品属性对应信息
                    var rip = pr.raw_item_property;
                    dynamic ripy = JsonConvert.DeserializeObject(rip.ToString());
                    if (ripy != null)
                    {
                        if (rippidli.Contains(ripy.id.ToString()))
                        {
                            sqlstrs = string.Format(@"update bms_kaola_rawitemproperty set item_id={0},business_id={1},property_value_id={2} where id={3};", (string.IsNullOrWhiteSpace(ripy.item_id.ToString())) ? "Null" : ripy.item_id, (string.IsNullOrWhiteSpace(ripy.business_id.ToString())) ? "Null" : ripy.business_id, ripy.property_value_id, ripy.id);
                        }
                        else
                        {
                            sqlstrs = string.Format("insert into bms_kaola_rawitemproperty values({0},{1},{2},'{3}')", (string.IsNullOrWhiteSpace(ripy.id.ToString())) ? "Null" : ripy.id, (string.IsNullOrWhiteSpace(ripy.item_id.ToString())) ? "Null" : ripy.item_id, (string.IsNullOrWhiteSpace(ripy.business_id.ToString())) ? "Null" : ripy.business_id, ripy.property_value_id);
                        }
                        Sqlstr.Add(sqlstrs);
                        sqlstrs = "";

                    }
                }
                #endregion

                #region item_text_property_list    商品自定义属性
                var dc6 = dc1.item_text_property_list;
                for (int m = 0; m < dc6.Count; m++)
                {
                    string tex = dc6[m].ToString();
                    dynamic itxt = JsonConvert.DeserializeObject(tex);
                    if (itxt != null)
                    {
                        if (itpidli.Contains(itxt.id.ToString()))
                        {
                            sqlstrs = string.Format(@"update bms_kaola_itemtextproperty set item_id={0},business_id={1},prop_name_id='{2}',prop_name_cn='{3}',text_value='{4}' where id={5};", (string.IsNullOrWhiteSpace(itxt.item_id.ToString())) ? "Null" : itxt.item_id, (string.IsNullOrWhiteSpace(itxt.business_id.ToString())) ? "Null" : itxt.business_id, itxt.prop_name_id, itxt.propn_name_cn, itxt.text_value, itxt.id);
                        }
                        else
                        {
                            sqlstrs = string.Format("insert into bms_kaola_itemtextproperty values({0},{1},{2},'{3}','{4}','{5}')", (string.IsNullOrWhiteSpace(itxt.id.ToString())) ? "Null" : itxt.id, (string.IsNullOrWhiteSpace(itxt.item_id.ToString())) ? "Null" : itxt.item_id, (string.IsNullOrWhiteSpace(itxt.business_id.ToString())) ? "Null" : itxt.business_id, itxt.prop_name_id, itxt.propn_name_cn, itxt.text_value);
                        }
                        Sqlstr.Add(sqlstrs);
                        sqlstrs = "";
                    }
                }
                #endregion

                //key  商品的key
                var dc7 = dc1.key;
                //string kaola_key = dc7.ToString();   
                #region raw_item_edit    商品的基本信息
                string dc8 = dc1.raw_item_edit.ToString();
                dynamic ri = JsonConvert.DeserializeObject(dc8);
                if (ri != null)
                {

                    int orderStatus = 0;
                    #region 订单状态
                    if (ri.item_status == "TO_BE_AUDITED")
                    {
                        orderStatus = 1;
                    }
                    else if (ri.item_status == "AUDITING")
                    {
                        orderStatus = 2;
                    }
                    else if (ri.item_status == "AUDIT_FAIL")
                    {
                        orderStatus = 3;
                    }
                    else if (ri.item_status == "AUDIT_SUCCESS")
                    {
                        orderStatus = 4;
                    }
                    else if (ri.item_status == "ON_SALE")
                    {
                        orderStatus = 5;
                    }
                    else if (ri.item_status == "UN_SALE")
                    {
                        orderStatus = 6;
                    }
                    else if (ri.item_status == "DELETED")
                    {
                        orderStatus = 7;
                    }
                    else if (ri.item_status == "FORCE_UN_SALE")
                    {
                        orderStatus = 8;
                    }
                    #endregion
                    if (rieidli.Contains(ri.id.ToString()))
                    {
                        sqlstrs = string.Format("update bms_kaola_rawitemedit set business_id={0},name='{1}',sub_title='{2}',short_title='{3}',ten_words_desc='{4}',item_no='{5}',brand_id='{6}',original_country_code_id='{7}',consign_area='{8}',consign_area_id='{9}',description='{10}',item_edit_status={11} where goodid={12};", (string.IsNullOrWhiteSpace(ri.business_id.ToString())) ? "Null" : ri.business_id, ri.name, ri.sub_title, ri.short_title, ri.ten_words_desc, ri.item_NO, ri.brand_id, ri.original_country_code_id, ri.consign_area, ri.consign_area_id, ri.description, orderStatus, ri.id);
                    }
                    else
                    {
                        sqlstrs = string.Format("insert into bms_kaola_rawitemedit values({0},{1},{2},'{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}',{13});", "NULL", (string.IsNullOrWhiteSpace(ri.id.ToString())) ? "Null" : ri.id, (string.IsNullOrWhiteSpace(ri.business_id.ToString())) ? "Null" : ri.business_id, ri.name, ri.sub_title, ri.short_title, ri.ten_words_desc, ri.item_NO, ri.brand_id, ri.original_country_code_id, ri.consign_area, ri.consign_area_id, ri.description, orderStatus);
                    }
                    Sqlstr.Add(sqlstrs);
                    sqlstrs = "";

                }
                #endregion

                #region sku_list        商品包含的SKU
                var dc9 = dc1.sku_list;
                for (int n = 0; n < dc9.Count; n++)
                {
                    string key = dc9[n].key.ToString();

                    //raw_sku    SKU基本信息
                    string rsku = dc9[n].raw_sku.ToString();
                    dynamic rsk = JsonConvert.DeserializeObject(rsku);
                    //#region sku_key(非spu_key)
                    //sql += string.Format("update bms_kaola_addproduct set kaola_sku_key ='{0}' where item_sku='{1}';", sku_key, "");
                    //#endregion
                    if (rsk != null)
                    {
                        //string updpro = string.Empty;
                        //if (DbHelperMySQL.DataSetToList(DbHelperMySQL.Query(string.Format(@"select item_sku from bms_kaola_addproduct where kaola_key='{0}';", kaola_key))).Contains(rsk.bar_code.ToString()))
                        //{
                        //    //kaola_sku_key    sku的key    插入bms_kaola_addproduct 中  kaola_sku_key
                        //    //kaola_key        dc7   kaola_key
                        //    //item_sku         rsk.bar_code                                                                                          key
                        //    updpro = string.Format(@"update bms_kaola_addproduct set kaola_sku_key='{0}' where kaola_key='{1}' and item_sku='{2}';", key, kaola_key, rsk.bar_code);
                        //}
                        if (rskuidli.Contains(rsk.id.ToString()))
                        {
                            sqlstrs = string.Format(@"update bms_kaola_rawsku set item_id={0},business_id={1},market_price={2},sale_price={3},bar_code='{4}',stock_can_sale={5},stock_freeze={6} where id={7};", (string.IsNullOrWhiteSpace(rsk.item_id.ToString())) ? "Null" : rsk.item_id, (string.IsNullOrWhiteSpace(rsk.business_id.ToString())) ? "Null" : rsk.business_id, (string.IsNullOrWhiteSpace(rsk.market_price.ToString())) ? "Null" : rsk.market_price, (string.IsNullOrWhiteSpace(rsk.sale_price.ToString())) ? "Null" : rsk.sale_price, "", (string.IsNullOrWhiteSpace(rsk.stock_can_sale.ToString())) ? "Null" : rsk.stock_can_sale, (string.IsNullOrWhiteSpace(rsk.stock_freeze.ToString())) ? "Null" : rsk.stock_freeze, rsk.id);
                        }
                        else
                        {
                            sqlstrs = string.Format(@"insert into bms_kaola_rawsku values({0},{1},{2},{3},{4},'{5}',{6},{7});", (string.IsNullOrWhiteSpace(rsk.id.ToString())) ? "Null" : rsk.id, (string.IsNullOrWhiteSpace(rsk.item_id.ToString())) ? "Null" : rsk.item_id, (string.IsNullOrWhiteSpace(rsk.business_id.ToString())) ? "Null" : rsk.business_id, (string.IsNullOrWhiteSpace(rsk.market_price.ToString())) ? "Null" : rsk.market_price, (string.IsNullOrWhiteSpace(rsk.sale_price.ToString())) ? "Null" : rsk.sale_price, "", (string.IsNullOrWhiteSpace(rsk.stock_can_sale.ToString())) ? "Null" : rsk.stock_can_sale, (string.IsNullOrWhiteSpace(rsk.stock_freeze.ToString())) ? "Null" : rsk.stock_freeze);
                        }
                        Sqlstr.Add(sqlstrs);
                        //if (!string.IsNullOrWhiteSpace(updpro))
                        //{
                        //    Sqlstr.Add(updpro);
                        //}
                        //updpro = "";
                        sqlstrs = "";
                    }
                    //sku_property_list   SKU属性列表
                    var spl = dc9[n].sku_property_list;
                    for (int o = 0; o < spl.Count; o++)
                    {
                        //property_name     SKU属性列表
                        string spli = spl[o].property_name.ToString();
                        dynamic pn = JsonConvert.DeserializeObject(spli);
                        //property_edit_policy     属性编辑策略
                        string prpcy = pn.property_edit_policy.ToString();
                        dynamic pyepy = JsonConvert.DeserializeObject(prpcy);
                        if (pyepy != null)
                        {
                            #region 判断自定义
                            //TEXT (1, "单行文本框"),
                            //TEXTAREA(2, "多行文本框"),
                            //SELECT(3, "下拉列表"),
                            //RADIO (4, "单选项"),
                            //CHECKBOX (5, "多选项"),
                            //FILE(6, "文件");


                            int type = 0;
                            if (pyepy.input_type == "FILE") //1:单行文本 2:多行文本 3: 下拉列表 4: 单选框 5: 多选框 6:文件
                            {
                                type = 6;
                            }
                            else if (pyepy.input_type == "CHECKBOX")
                            {
                                type = 5;
                            }
                            else if (pyepy.input_type == "RADIO")
                            {
                                type = 4;
                            }
                            else if (pyepy.input_type == "SELECT")
                            {
                                type = 3;
                            }
                            else if (pyepy.input_type == "TEXTAREA")
                            {
                                type = 2;
                            }
                            else if (pyepy.input_type == "TEXT")
                            {
                                type = 1;
                            }

                            #endregion
                            if (pepidli.Contains(pyepy.property_name_id.ToString()))
                            {
                                sqlstrs = string.Format(@"update bms_kaola_propertyeditpolicy set input_type={0},`desc`='{1}',max_len={2},is_multichoice={3},need_image={4},is_necessary={5} where property_name_id='{6}'", type, pyepy.desc, (string.IsNullOrWhiteSpace(pyepy.max_len.ToString())) ? "Null" : pyepy.max_len, (string.IsNullOrWhiteSpace(pyepy.is_multichoice.ToString())) ? "Null" : pyepy.is_multichoice, (string.IsNullOrWhiteSpace(pyepy.need_image.ToString())) ? "Null" : pyepy.need_image, (string.IsNullOrWhiteSpace(pyepy.is_necessary.ToString())) ? "Null" : pyepy.is_necessary, pyepy.property_name_id);
                            }
                            else
                            {
                                sqlstrs = string.Format(@"insert into bms_kaola_propertyeditpolicy  values(NULL,'{0}',{1},'{2}',{3},{4},{5},{6});", pyepy.property_name_id, type, pyepy.desc, (string.IsNullOrWhiteSpace(pyepy.max_len.ToString())) ? "Null" : pyepy.max_len, (string.IsNullOrWhiteSpace(pyepy.is_multichoice.ToString())) ? "Null" : pyepy.is_multichoice, (string.IsNullOrWhiteSpace(pyepy.need_image.ToString())) ? "Null" : pyepy.need_image, (string.IsNullOrWhiteSpace(pyepy.is_necessary.ToString())) ? "Null" : pyepy.is_necessary);
                            }
                            Sqlstr.Add(sqlstrs);
                            sqlstrs = "";
                        }
                        //raw_property_name    属性名基本信息
                        string rpn = pn.raw_property_name.ToString();
                        dynamic rpyn = JsonConvert.DeserializeObject(rpn);
                        if (rpyn != null)
                        {
                            if (rppnidli.Contains(rpyn.prop_name_id.ToString()))
                            {
                                sqlstrs = string.Format(@"update bms_kaola_rawpropertyname set prop_name_cn='{0}',prop_name_en='{1}',is_sku={2},is_filter={3},is_display={4},is_color={5},is_logistics={6},`status`={7} where prop_name_id='{8}';", rpyn.prop_name_cn, rpyn.prop_Name_en, (string.IsNullOrWhiteSpace(rpyn.is_sku.ToString())) ? "Null" : rpyn.is_sku, (string.IsNullOrWhiteSpace(rpyn.is_filter.ToString())) ? "Null" : rpyn.is_filter, (string.IsNullOrWhiteSpace(rpyn.is_display.ToString())) ? "Null" : rpyn.is_display, (string.IsNullOrWhiteSpace(rpyn.is_color.ToString())) ? "Null" : rpyn.is_color, (string.IsNullOrWhiteSpace(rpyn.is_logistics.ToString())) ? "Null" : rpyn.is_logistics, (string.IsNullOrWhiteSpace(rpyn.status.ToString())) ? "Null" : rpyn.status, rpyn.prop_name_id);
                            }
                            else
                            {
                                sqlstrs = string.Format(@"insert into bms_kaola_rawpropertyname values(NULL,'{0}','{1}','{2}',{3},{4},{5},{6},{7},{8});", rpyn.prop_name_id, rpyn.prop_name_cn, rpyn.prop_Name_en, (string.IsNullOrWhiteSpace(rpyn.is_sku.ToString())) ? "Null" : rpyn.is_sku, (string.IsNullOrWhiteSpace(rpyn.is_filter.ToString())) ? "Null" : rpyn.is_filter, (string.IsNullOrWhiteSpace(rpyn.is_display.ToString())) ? "Null" : rpyn.is_display, (string.IsNullOrWhiteSpace(rpyn.is_color.ToString())) ? "Null" : rpyn.is_color, (string.IsNullOrWhiteSpace(rpyn.is_logistics.ToString())) ? "Null" : rpyn.is_logistics, (string.IsNullOrWhiteSpace(rpyn.status.ToString())) ? "Null" : rpyn.status);
                            }
                            Sqlstr.Add(sqlstrs);
                            sqlstrs = "";
                        }
                        //property_value    属性值
                        string pptv = spl[o].property_value.ToString();
                        dynamic pptve = JsonConvert.DeserializeObject(pptv);
                        if (pptve != null)
                        {
                            if (pvidli.Contains(pptve.property_value_id.ToString()))
                            {
                                sqlstrs = string.Format(@"update bms_kaola_propertyvalue set property_value='{0}',property_name_id='{1}',property_value_icon='{2}',is_sys_property={3},show_order={4},`status`={5} where property_value_id='{6}';", pptve.property_value, pptve.property_name_id, pptve.property_value_icon, (string.IsNullOrWhiteSpace(pptve.is_sys_property.ToString())) ? "Null" : pptve.is_sys_property, (string.IsNullOrWhiteSpace(pptve.show_order.ToString())) ? "Null" : pptve.show_order, (string.IsNullOrWhiteSpace(pptve.status.ToString())) ? "Null" : pptve.status, pptve.property_value_id);
                            }
                            else
                            {
                                sqlstrs = string.Format("insert into bms_kaola_propertyvalue values(NULL,'{0}','{1}','{2}','{3}',{4},{5},{6})", pptve.property_value_id, pptve.property_value, pptve.property_name_id, pptve.property_value_icon, (string.IsNullOrWhiteSpace(pptve.is_sys_property.ToString())) ? "Null" : pptve.is_sys_property, (string.IsNullOrWhiteSpace(pptve.show_order.ToString())) ? "Null" : pptve.show_order, (string.IsNullOrWhiteSpace(pptve.status.ToString())) ? "Null" : pptve.status);
                            }
                            Sqlstr.Add(sqlstrs);
                            sqlstrs = "";
                        }
                        //raw_sku_property    SKU基本信息
                        string rsp = spl[o].raw_sku_property.ToString();
                        dynamic rsppy = JsonConvert.DeserializeObject(rsp);
                        if (rsppy != null)
                        {
                            if (rskupid.Contains(rsppy.id.ToString()))
                            {
                                sqlstrs = string.Format(@"update bms_kaola_rawskuproperty set sku_id={0},business_id={1},property_value_id='{2}',image_url='{3}' where id={4};", (string.IsNullOrWhiteSpace(rsppy.sku_id.ToString())) ? "Null" : rsppy.sku_id, (string.IsNullOrWhiteSpace(rsppy.business_id.ToString())) ? "Null" : rsppy.business_id, rsppy.property_value_id, rsppy.image_url, rsppy.id);
                            }
                            else
                            {
                                sqlstrs = string.Format("insert into bms_kaola_rawskuproperty values({0},{1},{2},'{3}','{4}')", (string.IsNullOrWhiteSpace(rsppy.id.ToString())) ? "Null" : rsppy.id, (string.IsNullOrWhiteSpace(rsppy.sku_id.ToString())) ? "Null" : rsppy.sku_id, (string.IsNullOrWhiteSpace(rsppy.business_id.ToString())) ? "Null" : rsppy.business_id, rsppy.property_value_id, rsppy.image_url);
                            }
                            Sqlstr.Add(sqlstrs);
                            sqlstrs = "";
                        }
                    }
                }
                #endregion


                int count = DbHelperMySQL.ExecuteSqlTran(Sqlstr);
                if (count > 0)
                {
                    return data + "__________________插入" + count + "条数据____________";
                }
            }
            return data;
            //return "";
                #endregion



            #region 原来的
            //var item_category_list = dc.kaola_item_get_response.item_category_list;                             //item_category_list
            //var item_image_list = dc.kaola_item_get_response.item_image_list;                                   //item_image_list
            //var item_postage = dc.kaola_item_get_response.item_postage;                                         //item_postage
            //var item_text_property_list = dc.kaola_item_get_response.item_text_property_list;                   //item_postage
            //var key = dc.kaola_item_get_response.key;                                                           //key
            //var raw_item_edit = dc.kaola_item_get_response.raw_item_edit;                                       //raw_item_edit
            //var sku_list = dc.kaola_item_get_response.sku_list;                                                 //sku_list

            //#region item_category_list
            //if (item_category_list != null)
            //                {
            //    for (int i = 0; i < item_category_list.Count; i++)
            //                {
            //        string sql = string.Empty;
            //        string orderJson = item_category_list[i].ToString();
            //        dynamic o = JsonConvert.DeserializeObject(orderJson);
            //        //bms_kaola_itemcategory
            //        sql = string.Format("insert into bms_kaola_itemcategory values({0},{1},{2},{3},{4});", o.id, o.item_id, o.business_id, o.category_id, o.rank);
            //        listSQL.Add(sql);
            //                }
            //                }
            //#endregion

            //#region item_image_list
            //if (item_image_list != null)
            //                {
            //    for (int i = 0; i < item_image_list.Count; i++)
            //                {
            //        string sql = string.Empty;
            //        string orderJson = item_image_list[i].ToString();
            //        dynamic o = JsonConvert.DeserializeObject(orderJson);
            //        sql = string.Format("insert into bms_kaola_itemimage values({0},{1},{2},'{3}','{4}',{5});", o.id, o.item_id, o.business_id, o.image_url, o.image_type, o.order_value);
            //        listSQL.Add(sql);
            //                }
            //                }
            //                #endregion

            //#region item_postage
            //if (item_postage != null)
            //                {
            //    for (int i = 0; i < item_postage.Count; i++)
            //                {
            //        string sql = string.Empty;
            //        string orderJson = item_postage[i].ToString();
            //        dynamic o = JsonConvert.DeserializeObject(orderJson);
            //        sql = string.Format("insert into bms_kaola_itempostage values({0},{1},{2},{3},{4},{5},{6},'{7}');", o.id, o.item_id, o.business_id, o.is_postage_free, o.post_fee, o.express_fee, o.ems_fee, o.postage_template_id);
            //        listSQL.Add(sql);
            //                }
            //            }
            //#endregion

            //#region item_text_property_list
            //if (item_text_property_list != null)
            //            {
            //    for (int i = 0; i < item_text_property_list.Count; i++)
            //                {
            //        string sql = string.Empty;
            //        string orderJson = item_text_property_list[i].ToString();
            //        dynamic o = JsonConvert.DeserializeObject(orderJson);
            //        sql = string.Format("insert into bms_kaola_itemtextproperty values({0},{1},{2},'{3}','{4}','{5}');", o.id, o.item_id, o.business_id, o.prop_name_id, o.propn_name_cn, o.text_value);
            //        listSQL.Add(sql);
            //                }
            //                }
            //    #endregion

            //#region key
            //if (key != null && !string.IsNullOrWhiteSpace(key.ToString()))
            //    {
            //    string orderJson = key.ToString();
            //                }
            //    #endregion

            //#region raw_item_edit
            //if (raw_item_edit != null)
            //    {
            //    //TO_BE_AUDITED   1     待提交审核
            //    //AUDITING        2     审核中/待审核
            //    //AUDIT_FAIL      3     审核未通过
            //    //AUDIT_SUCCESS   4     待上架（审核已通过）
            //    //ON_SALE         5     在售
            //    //UN_SALE         6     已下架
            //    //DELETED         7     已删除
            //    //FORCE_UN_SALE   8     强制下架

            //    string sql = string.Empty;
            //    string orderJson = raw_item_edit.ToString();
            //    dynamic o = JsonConvert.DeserializeObject(orderJson);

            //        int orderStatus = 0;
            //        #region 订单状态
            //    if (o.item_status == "TO_BE_AUDITED")
            //        {
            //            orderStatus = 1;
            //        }
            //    else if (o.item_status == "AUDITING")
            //        {
            //            orderStatus = 2;
            //        }
            //    else if (o.item_status == "AUDIT_FAIL")
            //        {
            //            orderStatus = 3;
            //        }
            //    else if (o.item_status == "AUDIT_SUCCESS")
            //        {
            //            orderStatus = 4;
            //        }
            //    else if (o.item_status == "ON_SALE")
            //        {
            //            orderStatus = 5;
            //        }
            //    else if (o.item_status == "UN_SALE")
            //        {
            //            orderStatus = 6;
            //        }
            //    else if (o.item_status == "DELETED")
            //        {
            //            orderStatus = 7;
            //        }
            //    else if (o.item_status == "FORCE_UN_SALE")
            //        {
            //            orderStatus = 8;
            //        }
            //        #endregion

            //        string descstr = ri.description.ToString();
            //        if (descstr.Contains("'"))
            //        {
            //            descstr = descstr.Replace("'", "''");
            //        }
            //        if (rieidli.Contains(ri.id.ToString()))
            //        {
            //            sqlstrs = string.Format("update bms_kaola_rawitemedit set business_id={0},name='{1}',sub_title='{2}',short_title='{3}',ten_words_desc='{4}',item_no='{5}',brand_id='{6}',original_country_code_id='{7}',consign_area='{8}',consign_area_id='{9}',description='{10}',item_edit_status={11} where goodid={12};", (string.IsNullOrWhiteSpace(ri.business_id.ToString())) ? "Null" : ri.business_id, ri.name, ri.sub_title, ri.short_title, ri.ten_words_desc, ri.item_NO, ri.brand_id, ri.original_country_code_id, ri.consign_area, ri.consign_area_id, descstr, orderStatus, ri.id);
            //        }
            //        else
            //        {
            //            sqlstrs = string.Format("insert into bms_kaola_rawitemedit values({0},{1},{2},'{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}',{13});", "NULL", (string.IsNullOrWhiteSpace(ri.id.ToString())) ? "Null" : ri.id, (string.IsNullOrWhiteSpace(ri.business_id.ToString())) ? "Null" : ri.business_id, ri.name, ri.sub_title, ri.short_title, ri.ten_words_desc, ri.item_NO, ri.brand_id, ri.original_country_code_id, ri.consign_area, ri.consign_area_id, descstr, orderStatus);
            //        }
            //        Sqlstr.Add(sqlstrs);
            //        sqlstrs = "";

            //    sql = string.Format("insert into bms_kaola_rawitemedit values(null,{0},{1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',{12});",
            //         o.id, o.business_id, o.name, o.sub_title, o.short_title, o.ten_words_desc, o.item_no, o.brand_id, o.original_country_code_id, o.consign_area, o.consign_area_id, o.description, orderStatus);
            //    listSQL.Add(sql);
            //    }
            //    #endregion

            //#region sku_list
            //if (sku_list != null && !string.IsNullOrWhiteSpace(sku_list.ToString()))
            //    {
            //    string sql = string.Empty;
            //    string orderJson = sku_list.ToString();
            //    dynamic o = JsonConvert.DeserializeObject(orderJson);
            //    for (int i = 0; i < o.Count; i++)
            //        {
            //        var sku_key = string.Empty;
            //        var sku_list_rawSku = o[i].raw_sku;
            //        var sku_list_PropertyList = o[i].sku_property_list;

            //            }
            //            //sku_property_list   SKU属性列表
            //            var spl = dc9[n].sku_property_list;
            //            if (spl != null)
            //            {
            //            var sku_list_Property_name = sku_list_PropertyList[j].property_name;//bms_kaola_propertyeditpolicy-----------------bms_kaola_rawpropertyname---------
            //            var sku_list_Property_value = sku_list_PropertyList[j].property_value; //bms_kaola_propertyvalue
            //            var sku_list_Raw_Sku_Property = sku_list_PropertyList[j].raw_sku_property; //bms_kaola_rawskuproperty

            //            #region sku_list_Property_name
            //            var sku_list_Property_name_policy = sku_list_Property_name.property_edit_policy;
            //            var sku_list_Property_name_raw = sku_list_Property_name.raw_property_name;

            //                        #region 判断自定义
            //                        //TEXT (1, "单行文本框"),
            //                        //TEXTAREA(2, "多行文本框"),
            //                        //SELECT(3, "下拉列表"),
            //                        //RADIO (4, "单选项"),
            //                        //CHECKBOX (5, "多选项"),
            //                        //FILE(6, "文件");


            //                        int type = 0;
            //            if (sku_list_Property_name_policy.input_type == "CHECKBOX") //1:单行文本 2:多行文本 3: 下拉列表 4: 单选框 5: 多选框 6:文件
            //                        {
            //                            type = 5;
            //                        }
            //            else if (true)
            //                        {

            //    }
            //    #endregion
            //    if (Sqlstr != null)
            //    {
            //        count = DbHelperMySQL.ExecuteSqlTran(Sqlstr);
            //        if (count > 0)
            //        {
            //            return data + "__________________更新" + count + "条数据____________";
            //        }
            //        else
            //        {
            //            return data + "__________________更新数据失败____________";
            //        }
            //    }
            //}
            //return data;
            //#region 原来的
            ////var item_category_list = dc.kaola_item_get_response.item_category_list;                             //item_category_list
            ////var item_image_list = dc.kaola_item_get_response.item_image_list;                                   //item_image_list
            ////var item_postage = dc.kaola_item_get_response.item_postage;                                         //item_postage
            ////var item_text_property_list = dc.kaola_item_get_response.item_text_property_list;                   //item_postage
            ////var key = dc.kaola_item_get_response.key;                                                           //key
            ////var raw_item_edit = dc.kaola_item_get_response.raw_item_edit;                                       //raw_item_edit
            ////var sku_list = dc.kaola_item_get_response.sku_list;                                                 //sku_list

            ////#region item_category_list
            ////if (item_category_list != null)
            ////{
            ////    for (int i = 0; i < item_category_list.Count; i++)
            ////    {
            ////        string sql = string.Empty;
            ////        string orderJson = item_category_list[i].ToString();
            ////        dynamic o = JsonConvert.DeserializeObject(orderJson);
            ////        //bms_kaola_itemcategory
            ////        sql = string.Format("insert into bms_kaola_itemcategory values({0},{1},{2},{3},{4});", o.id, o.item_id, o.business_id, o.category_id, o.rank);
            ////        listSQL.Add(sql);
            ////    }
            ////}
            ////#endregion

            ////#region item_image_list
            ////if (item_image_list != null)
            ////{
            ////    for (int i = 0; i < item_image_list.Count; i++)
            ////    {
            ////        string sql = string.Empty;
            ////        string orderJson = item_image_list[i].ToString();
            ////        dynamic o = JsonConvert.DeserializeObject(orderJson);
            ////        sql = string.Format("insert into bms_kaola_itemimage values({0},{1},{2},'{3}','{4}',{5});",o.id, o.item_id, o.business_id, o.image_url, o.image_type, o.order_value);
            ////        listSQL.Add(sql);
            ////    }
            ////}
            ////#endregion

            ////#region item_postage
            ////if (item_postage != null)
            ////{
            ////    for (int i = 0; i < item_postage.Count; i++)
            ////    {
            ////        string sql = string.Empty;
            ////        string orderJson = item_postage[i].ToString();
            ////        dynamic o = JsonConvert.DeserializeObject(orderJson);
            ////        sql = string.Format("insert into bms_kaola_itempostage values({0},{1},{2},{3},{4},{5},{6},'{7}');",o.id, o.item_id, o.business_id, o.is_postage_free, o.post_fee, o.express_fee, o.ems_fee, o.postage_template_id);
            ////        listSQL.Add(sql);
            ////    }
            ////}
            ////#endregion

            ////#region item_text_property_list
            ////if (item_text_property_list != null)
            ////{
            ////    for (int i = 0; i < item_text_property_list.Count; i++)
            ////    {
            ////        string sql = string.Empty;
            ////        string orderJson = item_text_property_list[i].ToString();
            ////        dynamic o = JsonConvert.DeserializeObject(orderJson);
            ////        sql = string.Format("insert into bms_kaola_itemtextproperty values({0},{1},{2},'{3}','{4}','{5}');",o.id, o.item_id, o.business_id, o.prop_name_id, o.propn_name_cn, o.text_value);
            ////        listSQL.Add(sql);
            ////    }
            ////}
            ////#endregion

            ////#region key
            ////if (key != null && !string.IsNullOrWhiteSpace(key.ToString()))
            ////{
            ////    string orderJson = key.ToString();
            ////}
            ////#endregion

            ////#region raw_item_edit
            ////if (raw_item_edit != null)
            ////{
            ////    //TO_BE_AUDITED   1     待提交审核
            ////    //AUDITING        2     审核中/待审核
            ////    //AUDIT_FAIL      3     审核未通过
            ////    //AUDIT_SUCCESS   4     待上架（审核已通过）
            ////    //ON_SALE         5     在售
            ////    //UN_SALE         6     已下架
            ////    //DELETED         7     已删除
            ////    //FORCE_UN_SALE   8     强制下架

            ////    string sql = string.Empty;
            ////    string orderJson = raw_item_edit.ToString();
            ////    dynamic o = JsonConvert.DeserializeObject(orderJson);

            ////    int orderStatus = 0;
            ////    #region 订单状态
            ////    if (o.item_status == "TO_BE_AUDITED")
            ////    {
            ////        orderStatus = 1;
            ////    }
            ////    else if (o.item_status == "AUDITING")
            ////    {
            ////        orderStatus = 2;
            ////    }
            ////    else if (o.item_status == "AUDIT_FAIL")
            ////    {
            ////        orderStatus = 3;
            ////    }
            ////    else if (o.item_status == "AUDIT_SUCCESS")
            ////    {
            ////        orderStatus = 4;
            ////    }
            ////    else if (o.item_status == "ON_SALE")
            ////    {
            ////        orderStatus = 5;
            ////    }
            ////    else if (o.item_status == "UN_SALE")
            ////    {
            ////        orderStatus = 6;
            ////    }
            ////    else if (o.item_status == "DELETED")
            ////    {
            ////        orderStatus = 7;
            ////    }
            ////    else if (o.item_status == "FORCE_UN_SALE")
            ////    {
            ////        orderStatus = 8;
            ////    }
            ////    #endregion

            ////    sql = string.Format("insert into bms_kaola_rawitemedit values(null,{0},{1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}',{12});",
            ////         o.id, o.business_id, o.name, o.sub_title, o.short_title, o.ten_words_desc, o.item_no, o.brand_id, o.original_country_code_id, o.consign_area, o.consign_area_id, o.description, orderStatus);
            ////    listSQL.Add(sql);
            ////}
            ////#endregion

            ////#region sku_list
            ////if (sku_list != null && !string.IsNullOrWhiteSpace(sku_list.ToString()))
            ////{
            ////    string sql = string.Empty;
            ////    string orderJson = sku_list.ToString();
            ////    dynamic o = JsonConvert.DeserializeObject(orderJson);
            ////    for (int i = 0; i < o.Count; i++)
            ////    {
            ////        var sku_key = string.Empty;
            ////        var sku_list_rawSku = o[i].raw_sku;
            ////        var sku_list_PropertyList = o[i].sku_property_list;

            ////        #region sku_key(非spu_key)
            ////        sql += string.Format("update bms_kaola_addproduct set kaola_sku_key ='{0}' where item_sku='{1}';", sku_key, "");
            ////        #endregion

            ////        #region raw_sku
            ////        sql += string.Format("insert into bms_kaola_rawsku values({0},{1},{2},{3},{4},'{5}',{6},{7});",
            ////            sku_list_rawSku.id, sku_list_rawSku.item_id, sku_list_rawSku.business_id, sku_list_rawSku.market_price, sku_list_rawSku.sale_price, sku_list_rawSku.bar_code, sku_list_rawSku.stock_can_sale, sku_list_rawSku.stock_freeze);
            ////        #endregion

            ////        #region sku_property_list
            ////        for (int j = 0; j < sku_list_PropertyList.Count; j++)
            ////        {
            ////            var sku_list_Property_name = sku_list_PropertyList[j].property_name;//bms_kaola_propertyeditpolicy-----------------bms_kaola_rawpropertyname---------
            ////            var sku_list_Property_value = sku_list_PropertyList[j].property_value; //bms_kaola_propertyvalue
            ////            var sku_list_Raw_Sku_Property = sku_list_PropertyList[j].raw_sku_property; //bms_kaola_rawskuproperty

            ////            #region sku_list_Property_name
            ////            var sku_list_Property_name_policy = sku_list_Property_name.property_edit_policy;
            ////            var sku_list_Property_name_raw = sku_list_Property_name.raw_property_name;

            ////            #region 判断自定义
            ////            //TEXT (1, "单行文本框"),
            ////            //TEXTAREA(2, "多行文本框"),
            ////            //SELECT(3, "下拉列表"),
            ////            //RADIO (4, "单选项"),
            ////            //CHECKBOX (5, "多选项"),
            ////            //FILE(6, "文件");


            ////            int type = 0;
            ////            if (sku_list_Property_name_policy.input_type == "CHECKBOX") //1:单行文本 2:多行文本 3: 下拉列表 4: 单选框 5: 多选框 6:文件
            ////            {
            ////                type = 5;
            ////            }
            ////            else if (true)
            ////            {

            ////            }
            ////            #endregion

            //            sql += string.Format("insert into bms_kaola_propertyeditpolicy values(null,'{0}',{1},'{2}',{3},{4},{5},{6});",
            //                sku_list_Property_name_policy.property_name_id, type, sku_list_Property_name_policy.desc, sku_list_Property_name_policy.max_len,
            //                sku_list_Property_name_policy.is_multichoice, sku_list_Property_name_policy.need_image, sku_list_Property_name_policy.is_necessary); ;


            //            sql += string.Format("insert into bms_kaola_rawpropertyname values(null,'{0}','{1}','{2}',{3},{4},{5},{6},{7},{8});",
            //                sku_list_Property_name_raw.prop_name_id, sku_list_Property_name_raw.prop_name_cn, sku_list_Property_name_raw.prop_name_en, sku_list_Property_name_raw.is_sku,
            //                sku_list_Property_name_raw.is_filter, sku_list_Property_name_raw.is_display, sku_list_Property_name_raw.is_color, sku_list_Property_name_raw.is_logistics, sku_list_Property_name_raw.status);
            //            #endregion

            //            #region sku_list_Property_value
            //            sql += string.Format(@"insert into bms_kaola_propertyvalue values(null,'{0}',{1},'{2}','{3}',{4},{5},{6});",
            //        sku_list_Property_value.is_sys_property, sku_list_Property_value.property_name_id, sku_list_Property_value.property_value,
            //        sku_list_Property_value.property_value_icon, sku_list_Property_value.property_value_id, sku_list_Property_value.show_order, sku_list_Property_value.status);
            //            #endregion

            //            #region sku_list_Raw_Sku_Property
            //            //bms_kaola_rawskuproperty
            //            sql += string.Format(@"insert into bms_kaola_rawskuproperty values({0},{1},{2},'{3}','{4}');",
            //        sku_list_Raw_Sku_Property.id, sku_list_Raw_Sku_Property.sku_id, sku_list_Raw_Sku_Property.business_id,
            //        sku_list_Raw_Sku_Property.property_value_id, sku_list_Raw_Sku_Property.image_url);
            //            #endregion

            //            listSQL.Add(sql);
            //        }
            //        #endregion
            //    }
            //    listSQL.Add(sql);
            //}
            //#endregion

            ////int count = DbHelperMySQL.ExecuteSqlTran(listSQL);

            //return data; 
            #endregion
        }

        /// <summary>
        /// 批量获取商品信息    调用成功（操作商品不存在）
        /// </summary>
        /// <returns></returns>
        public string kaola_item_batch_get()
        {
            string data = kc.kaola_item_batch_get("kaola.item.batch.get", time, new string[] { "41856-12010", "41622-12010" });
            return data;
        }

        /// <summary>
        /// 根据状态查询商品信息    调用成功
        /// </summary>
        /// <returns></returns>
        public string kaola_item_batch_status_get()
        {
            //string data = kc.kaola_item_batch_status_get("kaola.item.batch.status.get", time, 2, 1, 20);
            string data = kc.kaola_item_batch_status_get("kaola.item.batch.status.get", time, 5, 1, 20);
            int count = 0;
            if (data.Contains("kaola_item_batch_status_get_response"))
            {
                dynamic dc = JsonConvert.DeserializeObject(data);
                var dc1 = dc.kaola_item_batch_status_get_response.item_edit_list;
                StringBuilder sb = new StringBuilder(200);
                List<string> Sqlstr = new List<string>();

                List<string> icidli = new List<string>();
                icidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select id from bms_kaola_itemcategory;"));                //商品类目信息id

                List<string> iimidli = new List<string>();
                iimidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select id  from bms_kaola_itemimage;"));                 //商品图片id

                List<string> ipidli = new List<string>();
                ipidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select id from bms_kaola_itempostage;"));                 //运费信息id

                List<string> pepidli = new List<string>();
                pepidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select property_name_id from bms_kaola_propertyeditpolicy;"));  //属性编辑策略id

                List<string> rppnidli = new List<string>();
                rppnidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select prop_name_id from bms_kaola_rawpropertyname;"));//属性名基本信息id

                List<string> pvidli = new List<string>();
                pvidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select property_value_id from  bms_kaola_propertyvalue;")); //属性值id

                List<string> rippidli = new List<string>();
                rippidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select id from bms_kaola_rawitemproperty;"));             //商品属性对应信息id

                List<string> itpidli = new List<string>();
                itpidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select id from bms_kaola_itemtextproperty;"));            //商品自定义属性id

                List<string> rieidli = new List<string>();
                rieidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select goodid from bms_kaola_rawitemedit;"));               //商品基本信息id

                List<string> rskuidli = new List<string>();
                rskuidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select id from bms_kaola_rawsku;"));                        //SKU基本信息

                List<string> rskupid = new List<string>();
                rskupid = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select id from bms_kaola_rawskuproperty;"));              //SKU基本信息
                //List<string> 
                if (dc1 != null)
                {
                    for (int i = 0; i < dc1.Count; i++)
                    {
                        string sqlstrs = "";
                        #region item_category_list       商品类目信息
                        var dc2 = dc1[i].item_category_list;
                        if (dc2 != null)
                        {
                            for (int j = 0; j < dc2.Count; j++)
                            {
                                string cat = dc2[j].ToString();
                                dynamic c = JsonConvert.DeserializeObject(cat);
                                bms_kaola_itemcategory bkic = new bms_kaola_itemcategory();
                                if (c != null)
                                {
                                    if (icidli.Contains(c.id.ToString()))
                                    {
                                        sqlstrs = string.Format(@"update bms_kaola_itemcategory set item_id={0},business_id={1},category_id={2},rank={3} where id={4}", (string.IsNullOrWhiteSpace(c.item_id.ToString())) ? "NULL" : c.item_id, (string.IsNullOrWhiteSpace(c.business_id.ToString())) ? "NULL" : c.business_id, (string.IsNullOrWhiteSpace(c.category_id.ToString())) ? "NULL" : c.category_id, (string.IsNullOrWhiteSpace(c.rank.ToString())) ? "NULL" : c.rank, (string.IsNullOrWhiteSpace(c.id.ToString())) ? "NULL" : c.id);
                                    }
                                    else
                                    {
                                        sqlstrs = string.Format(@"insert into bms_kaola_itemcategory values({0},{1},{2},{3},{4})", (string.IsNullOrWhiteSpace(c.id.ToString())) ? "NULL" : c.id, (string.IsNullOrWhiteSpace(c.item_id.ToString())) ? "NULL" : c.item_id, (string.IsNullOrWhiteSpace(c.business_id.ToString())) ? "NULL" : c.business_id, (string.IsNullOrWhiteSpace(c.category_id.ToString())) ? "NULL" : c.category_id, (string.IsNullOrWhiteSpace(c.rank.ToString())) ? "NULL" : c.rank);
                                    }
                                    Sqlstr.Add(sqlstrs);
                                    sqlstrs = "";
                                }
                            }
                        }

                        #endregion

                        #region item_image_list        商品图片
                        var dc3 = dc1[i].item_image_list;
                        if (dc3 != null)
                        {
                            for (int k = 0; k < dc3.Count; k++)
                            {
                                string img = dc3[k].ToString();
                                dynamic im = JsonConvert.DeserializeObject(img);
                                if (im != null)
                                {
                                    int img_type = 0;
                                    if (im.image_type.ToString() == "MAIN")
                                    {
                                        img_type = 1;
                                    }
                                    else if (im.image_type.ToString() == "APP")
                                    {
                                        img_type = 2;
                                    }
                                    if (iimidli.Contains(im.id.ToString()))
                                    {
                                        sqlstrs = string.Format(@"update bms_kaola_itemimage set item_id={0},business_id={1},image_url='{2}',image_type='{3}',order_value={4} where id={5};", (string.IsNullOrWhiteSpace(im.item_id.ToString())) ? "NULL" : im.item_id, (string.IsNullOrWhiteSpace(im.business_id.ToString())) ? "NULL" : im.business_id, im.image_url, img_type, (string.IsNullOrWhiteSpace(im.order_value.ToString())) ? "NULL" : im.order_value, im.id);
                                    }
                                    else
                                    {
                                        sqlstrs = string.Format(@"insert into bms_kaola_itemimage values({0},{1},{2},'{3}','{4}',{5})", (string.IsNullOrWhiteSpace(im.id.ToString())) ? "NULL" : im.id, (string.IsNullOrWhiteSpace(im.item_id.ToString())) ? "NULL" : im.item_id, (string.IsNullOrWhiteSpace(im.business_id.ToString())) ? "NULL" : im.business_id, im.image_url, img_type, (string.IsNullOrWhiteSpace(im.order_value.ToString())) ? "NULL" : im.order_value);
                                    }
                                    Sqlstr.Add(sqlstrs);
                                    sqlstrs = "";
                                }
                            }
                        }
                        #endregion

                        #region item_postage         运费信息
                        var dc4 = dc1[i].item_postage;
                        dynamic itp = JsonConvert.DeserializeObject(dc4.ToString());
                        if (itp != null)
                        {
                            if (ipidli.Contains(itp.id.ToString()))
                            {
                                sqlstrs = string.Format(@"update bms_kaola_itempostage set item_id={0},business_id={1},is_postage_free={2},post_fee={3},express_fee={4},ems_fee={5},postage_template_id='{6}' where id={7};",
                                       (string.IsNullOrWhiteSpace(itp.item_id.ToString())) ? "NULL" : itp.item_id, (string.IsNullOrWhiteSpace(itp.business_id.ToString())) ? "NULL" : itp.business_id, (string.IsNullOrWhiteSpace(itp.is_postage_free.ToString())) ? "NULL" : itp.is_postage_free, (string.IsNullOrWhiteSpace(itp.post_fee.ToString())) ? "NULL" : itp.post_fee, (string.IsNullOrWhiteSpace(itp.express_fee.ToString())) ? "NULL" : itp.express_fee, (string.IsNullOrWhiteSpace(itp.ems_fee.ToString())) ? "NULL" : itp.ems_fee, itp.postage_template_id, (string.IsNullOrWhiteSpace(itp.id.ToString())) ? "NULL" : itp.id);
                            }
                            else
                            {
                                sqlstrs = string.Format("insert into bms_kaola_itempostage values({0},{1},{2},{3},{4},{5},{6},'{7}')", (string.IsNullOrWhiteSpace(itp.id.ToString())) ? "NULL" : itp.id, (string.IsNullOrWhiteSpace(itp.item_id.ToString())) ? "NULL" : itp.item_id, (string.IsNullOrWhiteSpace(itp.business_id.ToString())) ? "NULL" : itp.business_id, (string.IsNullOrWhiteSpace(itp.is_postage_free.ToString())) ? "NULL" : itp.is_postage_free, (string.IsNullOrWhiteSpace(itp.post_fee.ToString())) ? "NULL" : itp.post_fee, (string.IsNullOrWhiteSpace(itp.express_fee.ToString())) ? "NULL" : itp.express_fee, (string.IsNullOrWhiteSpace(itp.ems_fee.ToString())) ? "NULL" : itp.ems_fee, itp.postage_template_id);
                            }
                            Sqlstr.Add(sqlstrs);
                            sqlstrs = "";
                        }
                        #endregion

                        #region item_property_list    商品预定义属性
                        var dc5 = dc1[i].item_property_list;
                        if (dc5 != null)
                        {
                            for (int l = 0; l < dc5.Count; l++)
                            {
                                string pro = dc5[l].ToString();
                                dynamic pr = JsonConvert.DeserializeObject(pro);
                                //属性编辑策略
                                var pep = pr.property_name.property_edit_policy;

                                dynamic pepy = JsonConvert.DeserializeObject(pep.ToString());
                                bms_kaola_propertyeditpolicy bkp = new bms_kaola_propertyeditpolicy();
                                if (pepy != null)
                                {
                                    #region 判断自定义
                                    //TEXT (1, "单行文本框"),
                                    //TEXTAREA(2, "多行文本框"),
                                    //SELECT(3, "下拉列表"),
                                    //RADIO (4, "单选项"),
                                    //CHECKBOX (5, "多选项"),
                                    //FILE(6, "文件");


                                    int type = 0;
                                    if (pepy.input_type == "FILE") //1:单行文本 2:多行文本 3: 下拉列表 4: 单选框 5: 多选框 6:文件
                                    {
                                        type = 6;
                                    }
                                    else if (pepy.input_type == "CHECKBOX")
                                    {
                                        type = 5;
                                    }
                                    else if (pepy.input_type == "RADIO")
                                    {
                                        type = 4;
                                    }
                                    else if (pepy.input_type == "SELECT")
                                    {
                                        type = 3;
                                    }
                                    else if (pepy.input_type == "TEXTAREA")
                                    {
                                        type = 2;
                                    }
                                    else if (pepy.input_type == "TEXT")
                                    {
                                        type = 1;
                                    }

                                    #endregion
                                    if (pepidli.Contains(pepy.property_name_id.ToString()))
                                    {
                                        sqlstrs = string.Format(@"update bms_kaola_propertyeditpolicy set input_type={0},`desc`='{1}',max_len={2},is_multichoice={3},need_image={4},is_necessary={5} where property_name_id='{6}'", type, pepy.desc, (string.IsNullOrWhiteSpace(pepy.max_len.ToString())) ? "Null" : pepy.max_len, (string.IsNullOrWhiteSpace(pepy.is_multichoice.ToString())) ? "Null" : pepy.is_multichoice, (string.IsNullOrWhiteSpace(pepy.need_image.ToString())) ? "Null" : pepy.need_image, (string.IsNullOrWhiteSpace(pepy.is_necessary.ToString())) ? "Null" : pepy.is_necessary, pepy.property_name_id);
                                    }
                                    else
                                    {
                                        sqlstrs = string.Format(@"insert into bms_kaola_propertyeditpolicy  values(NULL,'{0}',{1},'{2}',{3},{4},{5},{6});", pepy.property_name_id, type, pepy.desc, (string.IsNullOrWhiteSpace(pepy.max_len.ToString())) ? "Null" : pepy.max_len, (string.IsNullOrWhiteSpace(pepy.is_multichoice.ToString())) ? "Null" : pepy.is_multichoice, (string.IsNullOrWhiteSpace(pepy.need_image.ToString())) ? "Null" : pepy.need_image, (string.IsNullOrWhiteSpace(pepy.is_necessary.ToString())) ? "Null" : pepy.is_necessary);
                                    }
                                    Sqlstr.Add(sqlstrs);
                                    sqlstrs = "";
                                }
                                //属性名   属性名基本信息
                                var rp = pr.property_name.raw_property_name;
                                dynamic rpn = JsonConvert.DeserializeObject(rp.ToString());
                                if (rpn != null)
                                {
                                    if (rppnidli.Contains(rpn.prop_name_id.ToString()))
                                    {
                                        sqlstrs = string.Format(@"update bms_kaola_rawpropertyname set prop_name_cn='{0}',prop_name_en='{1}',is_sku={2},is_filter={3},is_display={4},is_color={5},is_logistics={6},`status`={7} where prop_name_id='{8}';", rpn.prop_name_cn, rpn.prop_Name_en, (string.IsNullOrWhiteSpace(rpn.is_sku.ToString())) ? "Null" : rpn.is_sku, (string.IsNullOrWhiteSpace(rpn.is_filter.ToString())) ? "Null" : rpn.is_filter, (string.IsNullOrWhiteSpace(rpn.is_display.ToString())) ? "Null" : rpn.is_display, (string.IsNullOrWhiteSpace(rpn.is_color.ToString())) ? "Null" : rpn.is_color, (string.IsNullOrWhiteSpace(rpn.is_logistics.ToString())) ? "Null" : rpn.is_logistics, (string.IsNullOrWhiteSpace(rpn.status.ToString())) ? "Null" : rpn.status, rpn.prop_name_id);
                                    }
                                    else
                                    {
                                        sqlstrs = string.Format(@"insert into bms_kaola_rawpropertyname values(NULL,'{0}','{1}','{2}',{3},{4},{5},{6},{7},{8});", rpn.prop_name_id, rpn.prop_name_cn, rpn.prop_Name_en, (string.IsNullOrWhiteSpace(rpn.is_sku.ToString())) ? "Null" : rpn.is_sku, (string.IsNullOrWhiteSpace(rpn.is_filter.ToString())) ? "Null" : rpn.is_filter, (string.IsNullOrWhiteSpace(rpn.is_display.ToString())) ? "Null" : rpn.is_display, (string.IsNullOrWhiteSpace(rpn.is_color.ToString())) ? "Null" : rpn.is_color, (string.IsNullOrWhiteSpace(rpn.is_logistics.ToString())) ? "Null" : rpn.is_logistics, (string.IsNullOrWhiteSpace(rpn.status.ToString())) ? "Null" : rpn.status);
                                    }
                                    Sqlstr.Add(sqlstrs);
                                    sqlstrs = "";
                                }
                                //属性值
                                var pv = pr.property_value;
                                dynamic pvl = JsonConvert.DeserializeObject(pv.ToString());
                                if (pvl != null)
                                {
                                    if (pvidli.Contains(pvl.property_value_id.ToString()))
                                    {
                                        sqlstrs = string.Format(@"update bms_kaola_propertyvalue set property_value='{0}',property_name_id='{1}',property_value_icon='{2}',is_sys_property={3},show_order={4},`status`={5} where property_value_id='{6}';", pvl.property_value, pvl.property_name_id, pvl.property_value_icon, (string.IsNullOrWhiteSpace(pvl.is_sys_property.ToString())) ? "Null" : pvl.is_sys_property, (string.IsNullOrWhiteSpace(pvl.show_order.ToString())) ? "Null" : pvl.show_order, (string.IsNullOrWhiteSpace(pvl.status.ToString())) ? "Null" : pvl.status, pvl.property_value_id);
                                    }
                                    //重复
                                    else
                                    {
                                        sqlstrs = string.Format("insert into bms_kaola_propertyvalue values(NULL,'{0}','{1}','{2}','{3}',{4},{5},{6})", pvl.property_value_id, pvl.property_value, pvl.property_name_id, pvl.property_value_icon, (string.IsNullOrWhiteSpace(pvl.is_sys_property.ToString())) ? "Null" : pvl.is_sys_property, (string.IsNullOrWhiteSpace(pvl.show_order.ToString())) ? "Null" : pvl.show_order, (string.IsNullOrWhiteSpace(pvl.status.ToString())) ? "Null" : pvl.status);
                                    }
                                    Sqlstr.Add(sqlstrs);
                                    sqlstrs = "";

                                }
                                //商品属性对应信息
                                var rip = pr.raw_item_property;
                                dynamic ripy = JsonConvert.DeserializeObject(rip.ToString());
                                if (ripy != null)
                                {
                                    if (rippidli.Contains(ripy.id.ToString()))
                                    {
                                        sqlstrs = string.Format(@"update bms_kaola_rawitemproperty set item_id={0},business_id={1},property_value_id={2} where id={3};", (string.IsNullOrWhiteSpace(ripy.item_id.ToString())) ? "Null" : ripy.item_id, (string.IsNullOrWhiteSpace(ripy.business_id.ToString())) ? "Null" : ripy.business_id, ripy.property_value_id, ripy.id);
                                    }
                                    else
                                    {
                                        sqlstrs = string.Format("insert into bms_kaola_rawitemproperty values({0},{1},{2},'{3}')", (string.IsNullOrWhiteSpace(ripy.id.ToString())) ? "Null" : ripy.id, (string.IsNullOrWhiteSpace(ripy.item_id.ToString())) ? "Null" : ripy.item_id, (string.IsNullOrWhiteSpace(ripy.business_id.ToString())) ? "Null" : ripy.business_id, ripy.property_value_id);
                                    }
                                    Sqlstr.Add(sqlstrs);
                                    sqlstrs = "";

                                }
                            }
                        }
                        #endregion

                        #region item_text_property_list    商品自定义属性
                        var dc6 = dc1[i].item_text_property_list;
                        if (dc6 != null)
                        {
                            for (int m = 0; m < dc6.Count; m++)
                            {
                                string tex = dc6[m].ToString();
                                dynamic itxt = JsonConvert.DeserializeObject(tex);
                                if (itxt != null)
                                {
                                    if (itpidli.Contains(itxt.id.ToString()))
                                    {
                                        sqlstrs = string.Format(@"update bms_kaola_itemtextproperty set item_id={0},business_id={1},prop_name_id='{2}',prop_name_cn='{3}',text_value='{4}' where id={5};", (string.IsNullOrWhiteSpace(itxt.item_id.ToString())) ? "Null" : itxt.item_id, (string.IsNullOrWhiteSpace(itxt.business_id.ToString())) ? "Null" : itxt.business_id, itxt.prop_name_id, itxt.propn_name_cn, itxt.text_value, itxt.id);
                                    }
                                    else
                                    {
                                        sqlstrs = string.Format("insert into bms_kaola_itemtextproperty values({0},{1},{2},'{3}','{4}','{5}')", (string.IsNullOrWhiteSpace(itxt.id.ToString())) ? "Null" : itxt.id, (string.IsNullOrWhiteSpace(itxt.item_id.ToString())) ? "Null" : itxt.item_id, (string.IsNullOrWhiteSpace(itxt.business_id.ToString())) ? "Null" : itxt.business_id, itxt.prop_name_id, itxt.propn_name_cn, itxt.text_value);
                                    }
                                    Sqlstr.Add(sqlstrs);
                                    sqlstrs = "";
                                }
                            }
                        }
                        #endregion

                        //key  商品的key
                        var dc7 = dc1[i].key;

                        #region raw_item_edit    商品的基本信息
                        string dc8 = dc1[i].raw_item_edit.ToString();
                        dynamic ri = JsonConvert.DeserializeObject(dc8);
                        if (ri != null)
                        {

                            int orderStatus = 0;
                            #region 订单状态
                            if (ri.item_status == "TO_BE_AUDITED")
                            {
                                orderStatus = 1;
                            }
                            else if (ri.item_status == "AUDITING")
                            {
                                orderStatus = 2;
                            }
                            else if (ri.item_status == "AUDIT_FAIL")
                            {
                                orderStatus = 3;
                            }
                            else if (ri.item_status == "AUDIT_SUCCESS")
                            {
                                orderStatus = 4;
                            }
                            else if (ri.item_status == "ON_SALE")
                            {
                                orderStatus = 5;
                            }
                            else if (ri.item_status == "UN_SALE")
                            {
                                orderStatus = 6;
                            }
                            else if (ri.item_status == "DELETED")
                            {
                                orderStatus = 7;
                            }
                            else if (ri.item_status == "FORCE_UN_SALE")
                            {
                                orderStatus = 8;
                            }
                            #endregion

                            string descstr = ri.description.ToString();
                            if (descstr.Contains("'"))
                            {
                                descstr = descstr.Replace("'", "''");
                            }
                            if (rieidli.Contains(ri.id.ToString()))
                            {
                                sqlstrs = string.Format("update bms_kaola_rawitemedit set business_id={0},name='{1}',sub_title='{2}',short_title='{3}',ten_words_desc='{4}',item_no='{5}',brand_id='{6}',original_country_code_id='{7}',consign_area='{8}',consign_area_id='{9}',description='{10}',item_edit_status={11} where goodid={12};", (string.IsNullOrWhiteSpace(ri.business_id.ToString())) ? "Null" : ri.business_id, ri.name, ri.sub_title, ri.short_title, ri.ten_words_desc, ri.item_NO, ri.brand_id, ri.original_country_code_id, ri.consign_area, ri.consign_area_id, descstr, orderStatus, ri.id);
                            }
                            else
                            {
                                sqlstrs = string.Format("insert into bms_kaola_rawitemedit values({0},{1},{2},'{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}',{13});", "NULL", (string.IsNullOrWhiteSpace(ri.id.ToString())) ? "Null" : ri.id, (string.IsNullOrWhiteSpace(ri.business_id.ToString())) ? "Null" : ri.business_id, ri.name, ri.sub_title, ri.short_title, ri.ten_words_desc, ri.item_NO, ri.brand_id, ri.original_country_code_id, ri.consign_area, ri.consign_area_id, descstr, orderStatus);
                            }
                            Sqlstr.Add(sqlstrs);
                            sqlstrs = "";

                        }
                        #endregion

                        #region sku_list        商品包含的SKU
                        var dc9 = dc1[i].sku_list;
                        if (dc9 != null)
                        {
                            for (int n = 0; n < dc9.Count; n++)
                            {
                                string key = dc9[n].key.ToString(); ;
                                //raw_sku    SKU基本信息
                                string rsku = dc9[n].raw_sku.ToString();
                                dynamic rsk = JsonConvert.DeserializeObject(rsku);
                                if (rsk != null)
                                {
                                    if (rskuidli.Contains(rsk.id.ToString()))
                                    {
                                        sqlstrs = string.Format(@"update bms_kaola_rawsku set item_id={0},business_id={1},market_price={2},sale_price={3},bar_code='{4}',stock_can_sale={5},stock_freeze={6} where id={7};", (string.IsNullOrWhiteSpace(rsk.item_id.ToString())) ? "Null" : rsk.item_id, (string.IsNullOrWhiteSpace(rsk.business_id.ToString())) ? "Null" : rsk.business_id, (string.IsNullOrWhiteSpace(rsk.market_price.ToString())) ? "Null" : rsk.market_price, (string.IsNullOrWhiteSpace(rsk.sale_price.ToString())) ? "Null" : rsk.sale_price, "", (string.IsNullOrWhiteSpace(rsk.stock_can_sale.ToString())) ? "Null" : rsk.stock_can_sale, (string.IsNullOrWhiteSpace(rsk.stock_freeze.ToString())) ? "Null" : rsk.stock_freeze, rsk.id);
                                    }
                                    else
                                    {
                                        sqlstrs = string.Format(@"insert into bms_kaola_rawsku values({0},{1},{2},{3},{4},'{5}',{6},{7});", (string.IsNullOrWhiteSpace(rsk.id.ToString())) ? "Null" : rsk.id, (string.IsNullOrWhiteSpace(rsk.item_id.ToString())) ? "Null" : rsk.item_id, (string.IsNullOrWhiteSpace(rsk.business_id.ToString())) ? "Null" : rsk.business_id, (string.IsNullOrWhiteSpace(rsk.market_price.ToString())) ? "Null" : rsk.market_price, (string.IsNullOrWhiteSpace(rsk.sale_price.ToString())) ? "Null" : rsk.sale_price, "", (string.IsNullOrWhiteSpace(rsk.stock_can_sale.ToString())) ? "Null" : rsk.stock_can_sale, (string.IsNullOrWhiteSpace(rsk.stock_freeze.ToString())) ? "Null" : rsk.stock_freeze);
                                    }
                                    Sqlstr.Add(sqlstrs);
                                    sqlstrs = "";

                                }
                                //sku_property_list   SKU属性列表
                                var spl = dc9[n].sku_property_list;
                                if (spl != null)
                                {
                                    for (int o = 0; o < spl.Count; o++)
                                    {
                                        //property_name     SKU属性列表
                                        string spli = spl[o].property_name.ToString();
                                        dynamic pn = JsonConvert.DeserializeObject(spli);
                                        //property_edit_policy     属性编辑策略
                                        string prpcy = pn.property_edit_policy.ToString();
                                        dynamic pyepy = JsonConvert.DeserializeObject(prpcy);
                                        if (pyepy != null)
                                        {
                                            #region 判断自定义
                                            //TEXT (1, "单行文本框"),
                                            //TEXTAREA(2, "多行文本框"),
                                            //SELECT(3, "下拉列表"),
                                            //RADIO (4, "单选项"),
                                            //CHECKBOX (5, "多选项"),
                                            //FILE(6, "文件");


                                            int type = 0;
                                            if (pyepy.input_type == "FILE") //1:单行文本 2:多行文本 3: 下拉列表 4: 单选框 5: 多选框 6:文件
                                            {
                                                type = 6;
                                            }
                                            else if (pyepy.input_type == "CHECKBOX")
                                            {
                                                type = 5;
                                            }
                                            else if (pyepy.input_type == "RADIO")
                                            {
                                                type = 4;
                                            }
                                            else if (pyepy.input_type == "SELECT")
                                            {
                                                type = 3;
                                            }
                                            else if (pyepy.input_type == "TEXTAREA")
                                            {
                                                type = 2;
                                            }
                                            else if (pyepy.input_type == "TEXT")
                                            {
                                                type = 1;
                                            }

                                            #endregion
                                            if (pepidli.Contains(pyepy.property_name_id.ToString()))
                                            {
                                                sqlstrs = string.Format(@"update bms_kaola_propertyeditpolicy set input_type={0},`desc`='{1}',max_len={2},is_multichoice={3},need_image={4},is_necessary={5} where property_name_id='{6}'", type, pyepy.desc, (string.IsNullOrWhiteSpace(pyepy.max_len.ToString())) ? "Null" : pyepy.max_len, (string.IsNullOrWhiteSpace(pyepy.is_multichoice.ToString())) ? "Null" : pyepy.is_multichoice, (string.IsNullOrWhiteSpace(pyepy.need_image.ToString())) ? "Null" : pyepy.need_image, (string.IsNullOrWhiteSpace(pyepy.is_necessary.ToString())) ? "Null" : pyepy.is_necessary, pyepy.property_name_id);
                                            }
                                            else
                                            {
                                                sqlstrs = string.Format(@"insert into bms_kaola_propertyeditpolicy  values(NULL,'{0}',{1},'{2}',{3},{4},{5},{6});", pyepy.property_name_id, type, pyepy.desc, (string.IsNullOrWhiteSpace(pyepy.max_len.ToString())) ? "Null" : pyepy.max_len, (string.IsNullOrWhiteSpace(pyepy.is_multichoice.ToString())) ? "Null" : pyepy.is_multichoice, (string.IsNullOrWhiteSpace(pyepy.need_image.ToString())) ? "Null" : pyepy.need_image, (string.IsNullOrWhiteSpace(pyepy.is_necessary.ToString())) ? "Null" : pyepy.is_necessary);
                                            }
                                            Sqlstr.Add(sqlstrs);
                                            sqlstrs = "";
                                        }
                                        //raw_property_name    属性名基本信息
                                        string rpn = pn.raw_property_name.ToString();
                                        dynamic rpyn = JsonConvert.DeserializeObject(rpn);
                                        if (rpyn != null)
                                        {
                                            if (rppnidli.Contains(rpyn.prop_name_id.ToString()))
                                            {
                                                sqlstrs = string.Format(@"update bms_kaola_rawpropertyname set prop_name_cn='{0}',prop_name_en='{1}',is_sku={2},is_filter={3},is_display={4},is_color={5},is_logistics={6},`status`={7} where prop_name_id='{8}';", rpyn.prop_name_cn, rpyn.prop_Name_en, (string.IsNullOrWhiteSpace(rpyn.is_sku.ToString())) ? "Null" : rpyn.is_sku, (string.IsNullOrWhiteSpace(rpyn.is_filter.ToString())) ? "Null" : rpyn.is_filter, (string.IsNullOrWhiteSpace(rpyn.is_display.ToString())) ? "Null" : rpyn.is_display, (string.IsNullOrWhiteSpace(rpyn.is_color.ToString())) ? "Null" : rpyn.is_color, (string.IsNullOrWhiteSpace(rpyn.is_logistics.ToString())) ? "Null" : rpyn.is_logistics, (string.IsNullOrWhiteSpace(rpyn.status.ToString())) ? "Null" : rpyn.status, rpyn.prop_name_id);
                                            }
                                            else
                                            {
                                                sqlstrs = string.Format(@"insert into bms_kaola_rawpropertyname values(NULL,'{0}','{1}','{2}',{3},{4},{5},{6},{7},{8});", rpyn.prop_name_id, rpyn.prop_name_cn, rpyn.prop_Name_en, (string.IsNullOrWhiteSpace(rpyn.is_sku.ToString())) ? "Null" : rpyn.is_sku, (string.IsNullOrWhiteSpace(rpyn.is_filter.ToString())) ? "Null" : rpyn.is_filter, (string.IsNullOrWhiteSpace(rpyn.is_display.ToString())) ? "Null" : rpyn.is_display, (string.IsNullOrWhiteSpace(rpyn.is_color.ToString())) ? "Null" : rpyn.is_color, (string.IsNullOrWhiteSpace(rpyn.is_logistics.ToString())) ? "Null" : rpyn.is_logistics, (string.IsNullOrWhiteSpace(rpyn.status.ToString())) ? "Null" : rpyn.status);
                                            }
                                            Sqlstr.Add(sqlstrs);
                                            sqlstrs = "";
                                        }
                                        //property_value    属性值
                                        string pptv = spl[o].property_value.ToString();
                                        dynamic pptve = JsonConvert.DeserializeObject(pptv);
                                        if (pptve != null)
                                        {
                                            if (pvidli.Contains(pptve.property_value_id.ToString()))
                                            {
                                                sqlstrs = string.Format(@"update bms_kaola_propertyvalue set property_value='{0}',property_name_id='{1}',property_value_icon='{2}',is_sys_property={3},show_order={4},`status`={5} where property_value_id='{6}';", pptve.property_value, pptve.property_name_id, pptve.property_value_icon, (string.IsNullOrWhiteSpace(pptve.is_sys_property.ToString())) ? "Null" : pptve.is_sys_property, (string.IsNullOrWhiteSpace(pptve.show_order.ToString())) ? "Null" : pptve.show_order, (string.IsNullOrWhiteSpace(pptve.status.ToString())) ? "Null" : pptve.status, pptve.property_value_id);
                                            }
                                            else
                                            {
                                                sqlstrs = string.Format("insert into bms_kaola_propertyvalue values(NULL,'{0}','{1}','{2}','{3}',{4},{5},{6})", pptve.property_value_id, pptve.property_value, pptve.property_name_id, pptve.property_value_icon, (string.IsNullOrWhiteSpace(pptve.is_sys_property.ToString())) ? "Null" : pptve.is_sys_property, (string.IsNullOrWhiteSpace(pptve.show_order.ToString())) ? "Null" : pptve.show_order, (string.IsNullOrWhiteSpace(pptve.status.ToString())) ? "Null" : pptve.status);
                                            }
                                            Sqlstr.Add(sqlstrs);
                                            sqlstrs = "";
                                        }
                                        //raw_sku_property    SKU基本信息
                                        string rsp = spl[o].raw_sku_property.ToString();
                                        dynamic rsppy = JsonConvert.DeserializeObject(rsp);
                                        if (rsppy != null)
                                        {
                                            if (rskupid.Contains(rsppy.id.ToString()))
                                            {
                                                sqlstrs = string.Format(@"update bms_kaola_rawskuproperty set sku_id={0},business_id={1},property_value_id='{2}',image_url='{3}' where id={4};", (string.IsNullOrWhiteSpace(rsppy.sku_id.ToString())) ? "Null" : rsppy.sku_id, (string.IsNullOrWhiteSpace(rsppy.business_id.ToString())) ? "Null" : rsppy.business_id, rsppy.property_value_id, rsppy.image_url, rsppy.id);
                                            }
                                            else
                                            {
                                                sqlstrs = string.Format("insert into bms_kaola_rawskuproperty values({0},{1},{2},'{3}','{4}')", (string.IsNullOrWhiteSpace(rsppy.id.ToString())) ? "Null" : rsppy.id, (string.IsNullOrWhiteSpace(rsppy.sku_id.ToString())) ? "Null" : rsppy.sku_id, (string.IsNullOrWhiteSpace(rsppy.business_id.ToString())) ? "Null" : rsppy.business_id, rsppy.property_value_id, rsppy.image_url);
                                            }
                                            Sqlstr.Add(sqlstrs);
                                            sqlstrs = "";
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    if (Sqlstr != null)
                    {
                        count = DbHelperMySQL.ExecuteSqlTran(Sqlstr);
                        if (count > 0)
                        {
                            return data + "__________________更新" + count + "条数据________________";
                        }
                        else
                        {
                            return data + "_________________更新失败________________";
                        }
                    }

                }


            }
            return data;
        }

        /// <summary>
        /// 更新考拉库存接口    1秒 10次
        /// </summary>
        /// <returns></returns>
        public string kaola_item_stock_update()
        {
            ApiErrorMsg abm = new ApiErrorMsg()
            {
                errorAction = "InvokController->kaola_item_stock_update()",
                errorKey = "",
                errorMsg = "更新考拉库存开始",
                errorOrderId = "",
                errorTime = System.DateTime.Now.ToString("yyyyMMddHHmmss")
            };
            DbHelperMySQL.ExecuteSql("insert into bms_api_error(id,error_msg,error_time,error_action,error_order_id,error_key) values(null,'" + abm.errorMsg + "','" + abm.errorTime + "','" + abm.errorAction + "','" + abm.errorOrderId + "','" + abm.errorKey + "')");
            //更新开始 日志记录
            string data = kc.kaola_item_batch_status_get("kaola.item.batch.status.get", time, 5, 1, 20);
            List<string> kaolakeyli = new List<string>();
            int totalcount;
            //字典集合 商品的KEY   货号
            Dictionary<string, string> d = new Dictionary<string, string>();

            //int count = 0;
            if (data.Contains("kaola_item_batch_status_get_response"))
            {
                dynamic dc = JsonConvert.DeserializeObject(data);
                //1.0  首先获取在售状态的商品数目
                var count = dc.kaola_item_batch_status_get_response.total_count;//商品总数
                if (int.Parse(count.ToString()) <= 0)
                {
                    abm.errorKey = data;
                    abm.errorMsg = "接口调用失败，更新考拉库存失败！";
                    DbHelperMySQL.ExecuteSql("insert into bms_api_error(id,error_msg,error_time,error_action,error_order_id,error_key) values(null,'" + abm.errorMsg + "','" + abm.errorTime + "','" + abm.errorAction + "','" + abm.errorOrderId + "','" + abm.errorKey + "')");
                    //更新结束  日志记录
                    return "";
                }
                totalcount = int.Parse(count.ToString());
                //2.0  根据总数计算调用接口次数，依次调用接口得到商品的key，barcode存放在Dictionary中
                int InvokeCount = totalcount % 100 == 0 ? (totalcount / 20) : (totalcount / 20 + 1);//调用次数
                for (int i = 0; i < InvokeCount; i++)
                {
                    data = string.Empty;
                    data = kc.kaola_item_batch_status_get("kaola.item.batch.status.get", time, 5, i + 1, 20);

                    if (!data.Contains("kaola_item_batch_status_get_response"))//返回结果错误
                    {
                        int j = 0;
                        while (!data.Contains("kaola_item_batch_status_get_response") && j <= 5)//
                        {
                            data = kc.kaola_item_batch_status_get("kaola.item.batch.status.get", time, 5, i + 1, 20);
                        }
                        if (!data.Contains("kaola_item_batch_status_get_response"))
                        {
                            //日志记录第几次更新失败
                            abm.errorMsg = "第" + i + 1 + "次调用接口失败！";
                            DbHelperMySQL.ExecuteSql("insert into bms_api_error(id,error_msg,error_time,error_action,error_order_id,error_key) values(null,'" + abm.errorMsg + "','" + abm.errorTime + "','" + abm.errorAction + "','" + abm.errorOrderId + "','" + abm.errorKey + "')");
                            continue;//尝试四次之后调用失败
                        }
                    }

                    dynamic item = JsonConvert.DeserializeObject(data);
                    var dc1 = item.kaola_item_batch_status_get_response.item_edit_list;//商品集合
                    if (dc1 != null)
                    {
                        for (int o = 0; o < dc1.Count; o++)
                        {
                            //string kaolakey = dc1[o].key.ToString();//商品的key
                            var sku_list = dc1[o].sku_list;
                            if (sku_list != null)
                            {
                                for (int n = 0; n < sku_list.Count; n++)
                                {
                                    // string kaolakey = string.Empty;
                                    string bar_code = string.Empty;
                                    string key = string.Empty;
                                    key = sku_list[n].key.ToString();
                                    //raw_sku    SKU基本信息
                                    string rsku = sku_list[n].raw_sku.ToString();
                                    dynamic rsk = JsonConvert.DeserializeObject(rsku);
                                    if (rsk != null)
                                    {
                                        bar_code = rsk.bar_code.ToString();
                                    }
                                    else
                                    {
                                        continue;
                                    }
                                    if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(bar_code))
                                    {
                                        if (kaolakeyli.Contains(key))
                                        {
                                            continue;
                                        }
                                        d.Add(key, bar_code);
                                        kaolakeyli.Add(key);
                                    }
                                }
                            }
                            //kaolakey = string.Empty;         
                        }
                        dc1 = null;
                        data = string.Empty;
                    }
                }
                //3.0  遍历字典调用库存更新接口更新库存  注意：一分钟考拉接口只能调用100次
                if (kaolakeyli.Count > 0 || kaolakeyli != null)
                {
                    for (int i = 0; i <1; i++)
                    {
                        string stock = DbHelperMySQL.Query("select IFNULL(sum(sale_qty),0) from bms_warehouse_stock where warehouse_code in ('SZC','TMC','SHSC','JMC','DSC','MGJSC','SPC','YJC','MYSC','1','MLSC') and item_sku ='" + d[kaolakeyli[i]] + "'").Tables[0].Rows[0][0].ToString();
                        int s = string.IsNullOrWhiteSpace(stock) ? 0 : int.Parse(stock);
                        string result = kc.kaola_item_sku_stock_update("kaola.item.sku.stock.update", time, kaolakeyli[i], s);
                        if (result.Contains("kaola_item_sku_stock_update_response"))
                        {
                            dynamic resultobj = JsonConvert.DeserializeObject(result);

                            if (resultobj.kaola_item_sku_stock_update_response.result.ToString() == "0")
                            {
                                abm.errorKey = kaolakeyli[i];
                                abm.errorMsg = "考拉库存更新失败，货号考拉:" + kaolakeyli[i] + "SKU_ID" + d[kaolakeyli[i]];
                                DbHelperMySQL.ExecuteSql("insert into bms_api_error(id,error_msg,error_time,error_action,error_order_id,error_key) values(null,'" + abm.errorMsg + "','" + abm.errorTime + "','" + abm.errorAction + "','" + abm.errorOrderId + "','" + abm.errorKey + "')");
                            }
                        }

                    }
                }
                abm.errorMsg = "考拉库存更新结束！";
                DbHelperMySQL.ExecuteSql("insert into bms_api_error(id,error_msg,error_time,error_action,error_order_id,error_key) values(null,'" + abm.errorMsg + "','" + abm.errorTime + "','" + abm.errorAction + "','" + abm.errorOrderId + "','" + abm.errorKey + "')");

            }
            else
            {
                abm.errorMsg = "考拉更新结束，调用接口出错";
                DbHelperMySQL.ExecuteSql("insert into bms_api_error(id,error_msg,error_time,error_action,error_order_id,error_key) values(null,'" + abm.errorMsg + "','" + abm.errorTime + "','" + abm.errorAction + "','" + abm.errorOrderId + "','" + abm.errorKey + "')");
            
            }
           //调用出错  日志记录
            return "";
        }



        /// <summary>
        /// 修改指定sku的库存     调用成功（操作商品不存在）
        /// </summary>
        /// <returns></returns>
        public string kaola_item_sku_stock_update()
        {
            string data = kc.kaola_item_sku_stock_update("kaola.item.sku.stock.update", time, "83952-3049", 2);
            return data;
        }

        /// <summary>
        /// 修改指定sku的销售价   调用成功（操作商品不存在）
        /// </summary>
        /// <returns></returns>
        public string kaola_item_sku_sale_price_update()
        {
            string data = kc.kaola_item_sku_sale_price_update("kaola.item.sku.sale.price.update", time, "83952-3049", 2000);

            return data;
        }

        /// <summary>
        /// 新增商品
        /// </summary>
        /// <returns></returns>
        public string kaola_item_add()
        {
            //string data1 = kc.kaola_item_add("kaola.item.add", time, "商品名称", "副标题", "短标题", "描述", "2211546655", 1003, "303",
            //    "广东省-广州市", "440000-440100", "详情描述", 3607, "52821", "1287^单行文本框",
            //    "http://pop.nosdn.127.net/0a2bed72-6a9b-4624-8848-b2e68352ee62^1|http://pop.nosdn.127.net/0a2bed72-6a9b-4624-8848-b2e68352ee62^2",
            //    "400", "400", "13123", "12", "54474^http://pop.nosdn.127.net/0a2bed72-6a9b-4624-8848-b2e68352ee62");


            string data1 = kc.kaola_item_add("kaola.item.add", time, "商品名称11", "副标题11", "短标题11", "描述11", "29060", 1003, "303", "广东省-广州市", "440000-440100",
                "详情描述11", 3607, "52821", "1287^123456789", "http://pop.nosdn.127.net/0a2bed72-6a9b-4624-8848-b2e68352ee62^1|http://pop.nosdn.127.net/0a2bed72-6a9b-4624-8848-b2e68352ee62^2",
                "800", "800", "2233445566", "10", "54474^http://pop.nosdn.127.net/0a2bed72-6a9b-4624-8848-b2e68352ee62");



            return data1;
        }

        /// <summary>
        /// 添加商品的sku
        /// </summary>
        /// <returns></returns>
        public string kaola_item_sku_add()
        {
            //{"kaola_item_add_response":{"create_time":"2015-12-22 14:29:53","key":"100047-3049"}}
            //string data = kc.kaola_item_sku_add("kaola.item.sku.add", time, "85359-3049", "100", "100", "789789", "100","54476^http://pic13.secooimg.com/push/15/12/66f1262c20634f9ca681fbbd2a7fadad.jpg");
            string data = kc.kaola_item_sku_add("kaola.item.sku.add", time, "100047-3049", "200", "200", "123456789", "5", "54476^http://pic13.secooimg.com/push/15/12/66f1262c20634f9ca681fbbd2a7fadad.jpg");
            return data;
        }

        /// <summary>
        /// 上传产品图片
        /// </summary>
        /// <param name="string_img">图片地址（如：new string[] { "http://best-bms.pbxluxury.com/images/COH/L0100/33521/33521LIBAJF/5_33521LIBAJF.jpg" }）</param>
        /// <returns></returns>
        public string kaola_item_img_upload(string[] string_img)
        {
            StringBuilder sb = new StringBuilder();
            #region 测试KEY
            //sb.Append("access_token214f66a9-e3f9-48ec-9e70-196fc6aa63fb");                                                                //令牌
            //sb.Append("app_keyedb6c3b9ac4847e7584c38e2b630b14f");                                                                         //密钥
            #endregion
            sb.Append("access_token" + System.Configuration.ConfigurationManager.AppSettings["access_token"].ToString());                   //令牌
            sb.Append("app_key" + System.Configuration.ConfigurationManager.AppSettings["app_key"].ToString());                             //密钥
            sb.Append("methodkaola.item.img.upload");                                                                                       //调用接口
            sb.Append("timestamp" + time);                                                                                                  //时间戳
            string sign = kaola_sign.signValues(sb.ToString());                                                                             //签名

            NameValueCollection data = new NameValueCollection();
            data.Add("method", "kaola.item.img.upload");
            #region 测试KEY
            //data.Add("access_token", "214f66a9-e3f9-48ec-9e70-196fc6aa63fb");
            //data.Add("app_key", "edb6c3b9ac4847e7584c38e2b630b14f");
            #endregion
            data.Add("access_token", System.Configuration.ConfigurationManager.AppSettings["access_token"].ToString());
            data.Add("app_key", System.Configuration.ConfigurationManager.AppSettings["app_key"].ToString());
            data.Add("timestamp", time);
            data.Add("sign", sign);

            //bms.pbxluxury.com/images/COH/L0100/33521/33521LIBAJF/5_33521LIBAJF.jpg
            //string data1 = UploadHelper.HttpUploadFile("http://openapi.kaola.com/router", new string[] { @"C:\\5_33521LIBAJF.jpg" }, data);
            //{"kaola_item_img_upload_response":{"created":"2015-12-18 10:13:18","result":true,"url":"http://pop.nosdn.127.net/43363eb2-38ee-4dce-8a4a-1d8200a7cb80"}}
            //string data1 = UploadHelper.HttpUploadFile("http://openapi.kaola.com/router", new string[] { "http://best-bms.pbxluxury.com/images/COH/L0100/33521/33521LIBAJF/5_33521LIBAJF.jpg" }, data);
            string data1 = UploadHelper.HttpUploadFile("http://openapi.kaola.com/router", string_img, data);
            return data1;
        }
        #endregion

        kaola_basicMsg kb = new kaola_basicMsg();

        #region 基本信息
        /// <summary>
        /// kaola.common.countries.get (获取所有国家信息)
        /// </summary>
        /// <returns></returns>
        public string kaola_common_countries_get()
        {
            string data = kb.kaola_common_countries_get("kaola.common.countries.get", time);
            int count = 0;
            if (data.Contains("kaola_common_countries_get_response"))
            {
                List<CountryModel> c = new List<CountryModel>();
                CountryModel kaolacountry = kaola_seralia.ScriptDeserialize<CountryModel>(data);
                List<Country> Lcoun = new List<Country>();
                Lcoun = kaolacountry.kaola_common_countries_get_response;
                List<string> sqlli = new List<string>();
                List<string> cli = new List<string>();
                cli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select country_code from bms_kaola_commoncountries;"));
                if (Lcoun.Count > 0 || Lcoun != null)
                {
                    foreach (Country item in Lcoun)
                    {
                        if (cli.Contains(item.country_code))
                        {
                            sqlli.Add("update bms_kaola_commoncountries set country_name='" + item.country_name + "' where country_code='" + item.country_code + "';");
                        }
                        else
                        {
                            sqlli.Add("insert into bms_kaola_commoncountries(country_code,country_name) values('" + item.country_code + "','" + item.country_name + "');");
                        }
                    }
                    if (sqlli != null)
                    {
                        count = DbHelperMySQL.ExecuteSqlTran(sqlli);
                        if (count > 0)
                        {
                            return data + "______________________________更新" + count + "条数据__________________________________";
                        }
                        else
                        {
                            return data + "______________________________更新失败__________________________________";
                        }
                    }
                }

            }
            return data;
        }

        /// <summary>
        /// kaola.common.provinces.get (获取所有省份信息)
        /// </summary>
        /// <returns></returns>
        public string kaola_common_provinces_get()
        {
            string data = kb.kaola_common_provinces_get("kaola.common.provinces.get", time);
            int count = 0;
            if (data.Contains("kaola_common_provinces_get_response"))
            {
                List<ProvinceModel> c = new List<ProvinceModel>();
                ProvinceModel kaolaProvince = kaola_seralia.ScriptDeserialize<ProvinceModel>(data);
                List<Province> Lcoun = new List<Province>();
                Lcoun = kaolaProvince.kaola_common_provinces_get_response;
                List<string> sqlstr = new List<string>();
                List<string> pcli = new List<string>();
                pcli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select region_code from bms_kaola_commonprovcity where parent_code=0;"));
                if (Lcoun.Count > 0 || Lcoun != null)
                {
                    foreach (Province item in Lcoun)
                    {
                        if (pcli.Contains(item.province_code.ToString()))
                        {
                            sqlstr.Add("update bms_kaola_commonprovcity set region_name='" + item.province_name + "' where region_code='" + item.province_code + "'");
                        }
                        else
                        {
                            sqlstr.Add("insert into bms_kaola_commonprovcity(region_name,region_code,region_order,parent_code) values('" + item.province_name + "','" + item.province_code + "','" + item.order_value + "','0')");
                        }

                    }
                    if (sqlstr != null)
                    {
                        count = DbHelperMySQL.ExecuteSqlTran(sqlstr);
                        if (count > 0)
                        {
                            return data + "______________________________更新" + count + "条数据__________________________________";
                        }
                        else
                        {
                            return data + "______________________________更新失败__________________________________";
                        }
                    }
                }


            }
            return data;
        }

        /// <summary>
        /// kaola.common.city.get (获取城市信息)
        /// </summary>
        /// <returns></returns>
        public string kaola_common_city_get()
        {
            //1.获得所有的省份编号
            string data = kb.kaola_common_provinces_get("kaola.common.provinces.get", time);
            int count = 0;
            if (data.Contains("kaola_common_provinces_get_response"))
            {
                List<ProvinceModel> c = new List<ProvinceModel>();
                ProvinceModel kaolaProvince = kaola_seralia.ScriptDeserialize<ProvinceModel>(data);
                List<Province> Lcoun = new List<Province>();
                Lcoun = kaolaProvince.kaola_common_provinces_get_response;
                List<string> sqlstr = new List<string>();
                StringBuilder sb = new StringBuilder(200);
                List<string> ccli = new List<string>();

                if (Lcoun.Count > 0 || Lcoun != null)
                {
                    foreach (Province item in Lcoun)
                    {
                        ccli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select region_code from bms_kaola_commonprovcity where parent_code='" + item.province_code + "';"));
                        string data1 = kb.kaola_common_city_get("kaola.common.city.get", item.province_code.ToString(), time);
                        if (data1.Contains("kaola_common_city_get_response"))
                        {
                            CityModel cit = new CityModel();
                            List<City> li = new List<City>();
                            cit = kaola_seralia.ScriptDeserialize<CityModel>(data1);
                            li = cit.kaola_common_city_get_response;
                            foreach (City city in li)
                            {
                                if (ccli.Contains(city.city_code.ToString()))
                                {
                                    sqlstr.Add("update bms_kaola_commonprovcity set region_name='" + city.city_name + "',region_order='" + city.order_value + "' where region_code='" + city.city_code + "'");
                                }
                                else
                                {
                                    sqlstr.Add("insert into bms_kaola_commonprovcity(region_name,region_code,region_order,parent_code) values('" + city.city_name + "','" + city.city_code + "','" + city.order_value + "','" + city.parent_code + "');");
                                }
                            }
                        }
                        ccli.Clear();
                        sb.Append(item.province_name + "**********************");

                        //sqlstr.Clear();
                    }
                    if (sqlstr != null)
                    {
                        count = DbHelperMySQL.ExecuteSqlTran(sqlstr);
                        if (count > 0)
                        {
                            return data + "______________________________更新" + count + "条数据__________________________________";
                        }
                        else
                        {
                            return data + "______________________________更新失败__________________________________";
                        }
                    }
                }
                // string data1 = kb.kaola_common_city_get("kaola.common.city.get", "440000", time);

            }

            return data;
        }


        /// <summary>
        /// kaola.common.district.get  获取城市行政区信息
        /// </summary>
        /// <returns></returns>
        public string kaola_common_district_get()
        {
            StringBuilder sb = new StringBuilder(200);
            List<string> ccli = new List<string>();//城市code集合
            ccli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select region_code from bms_kaola_commonprovcity where parent_code!=0"));
            List<string> sqlstr = new List<string>();
            int count;
            foreach (string item in ccli)
            {
                string data = kb.kaola_common_district_get("kaola.common.district.get", item, time);
                if (data.Contains("kaola_common_district_get_response"))
                {
                    DistrictModel dct = new DistrictModel();
                    List<District> li = new List<District>();
                    dct = kaola_seralia.ScriptDeserialize<DistrictModel>(data);
                    li = dct.kaola_common_district_get_response;
                    foreach (District d in li)
                    {
                        if (ccli.Contains(d.district_code))
                        {
                            sqlstr.Add("UPDATE bms_kaola_commonprovcity set region_name='" + d.district_name + "',region_order='" + d.order_value + "'  WHERE region_code='" + d.district_code + "' and parent_code='" + d.city_code + "'");
                        }
                        else
                        {
                            sqlstr.Add("insert into bms_kaola_commonprovcity VALUES(null,'" + d.district_name + "','" + d.district_code + "','" + d.order_value + "','" + d.city_code + "')");
                        }
                    }
                    data = string.Empty;
                    sb.Append(item + "--");
                }



            }
            if (sqlstr != null)
            {
                count = DbHelperMySQL.ExecuteSqlTran(sqlstr);
                if (count > 0)
                {
                    return "更新成功！更新" + count.ToString() + "条数据";

                }
                else
                {
                    return "更新失败！";
                }

            }
            return "";
        }

        /// <summary>
        /// kaola.common.hscodes.get 根据关键字查询hs编码
        /// </summary>
        /// <returns></returns>
        public string kaola_common_hscodes_get(string keyword)
        {
            string result = "查询失败！";
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return result + "关键字不能为空！";
            }
            string data = kb.kaola_common_hscodes_get("kaola.common.hscodes.get", keyword, time);
            if (data.Contains("kaola_common_hscodes_get_response"))
            {
                return data;
            }
            return result;
        }
        #endregion

        kaola_seller ks = new kaola_seller();
        #region 商家
        /// <summary>
        /// kaola.vender.category.get(获取商家类目)
        /// </summary>
        /// <returns></returns>
        public string kaola_vender_category_get()
        {
            string data = ks.kaola_vender_category_get("kaola.vender.category.get", time);
            int count = 0;
            if (data.Contains("kaola_vender_category_get_response"))
            {
                List<Category> li = new List<Category>();
                CateObjModel com = new CateObjModel();
                com = kaola_seralia.ScriptDeserialize<CateObjModel>(data);
                CateItem ci = new CateItem();
                ci = com.kaola_vender_category_get_response;
                li = ci.Item_cats;
                List<string> sqlli = new List<string>();
                List<string> cidli = new List<string>();
                //DataSet ds= DbHelperMySQL.Query("select category_id from bms_kaola_vendercategory;");
                cidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select category_id from bms_kaola_vendercategory;"));
                if (li.Count > 0 || li != null)
                {
                    foreach (Category item in li)
                    {
                        if (cidli.Contains(item.category_id.ToString()))
                        {
                            sqlli.Add("update bms_kaola_vendercategory set category_name='" + item.category_name + "',parent_id='" + item.parent_id + "',category_level='" + item.category_level + "',is_leaf='" + item.is_leaf + "' where category_id='" + item.category_id + "'");

                        }
                        else
                        {
                            sqlli.Add("insert into bms_kaola_vendercategory(category_id,category_name,parent_id,category_level,is_leaf) values('" + item.category_id + "','" + item.category_name + "','" + item.parent_id + "','" + item.category_level + "','" + item.is_leaf + "')");
                        }
                    }
                    if (sqlli != null)
                    {
                        count = DbHelperMySQL.ExecuteSqlTran(sqlli);
                        if (count > 0)
                        {
                            return data + "______________________________更新" + count + "条数据__________________________________";
                        }
                        else
                        {
                            return data + "_________________________________更新失败__________________________________";
                        }
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// kaola.vender.brand.get(获取商家品牌)
        /// </summary>
        /// <returns></returns>
        public string kaola_vender_brand_get()
        {
            string data = ks.kaola_vender_brand_get("kaola.vender.brand.get", time);
            int count = 0;
            if (data.Contains("kaola_vender_brand_get_response"))
            {
                BrandModel brand = new BrandModel();
                BrandItem item = new BrandItem();
                List<BrandInfo> li = new List<BrandInfo>();
                brand = kaola_seralia.ScriptDeserialize<BrandModel>(data);
                item = brand.kaola_vender_brand_get_response;
                li = item.brand_list;
                List<string> sqlstr = new List<string>();
                List<string> bidli = new List<string>();
                bidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select brand_id from bms_kaola_venderbrand;"));
                if (li.Count > 0 || li != null)
                {
                    foreach (BrandInfo bi in li)
                    {
                        if (bidli.Contains(bi.brand_id.ToString()))
                        {
                            sqlstr.Add("update bms_kaola_venderbrand set brand_name='" + bi.brand_name + "' where brand_id='" + bi.brand_id + "'");

                        }
                        else
                        {
                            sqlstr.Add("insert into bms_kaola_venderbrand(brand_id,brand_name) values('" + bi.brand_id + "','" + bi.brand_name + "');");
                        }
                    }
                    if (sqlstr != null)
                    {
                        count = DbHelperMySQL.ExecuteSqlTran(sqlstr);
                        if (count > 0)
                        {
                            return data + "______________________________更新" + count + "条数据__________________________________";
                        }
                        else
                        {
                            return data + "______________________________更新失败__________________________________";
                        }
                    }
                }
            }

            return data;
        }

        //http://www.kaola.com/#access_token=8f12dce1-c049-45a8-83f0-8d5a3cefc886&token_type=bearer&state=mycode&expires_in=31535979&scope=read
        /// <summary>
        /// kaola.vender.info.get (获取商家基本信息)
        /// </summary>
        /// <returns></returns>
        public string kaola_vender_info_get()
        {
            string[] data = ks.kaola_vender_info_get("kaola.vender.info.get", time);
            StringBuilder sb = new StringBuilder(200);
            int count = 0;
            for (int i = 0; i < data.Length; i++)
            {
                sb.Append(data[i]);
                if (data[i].Contains("kaola_vender_info_get_response"))
                {
                    VenderModel vm = new VenderModel();
                    vm = kaola_seralia.ScriptDeserialize<VenderModel>(data[i]);
                    vender_info1 vi1 = new vender_info1();
                    vi1 = vm.kaola_vender_info_get_response;
                    vender_info vi = new vender_info();
                    vi = vi1.vender_info;
                    List<string> vidli = new List<string>();
                    vidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select vender_id from bms_kaola_venderinfo;"));
                    List<string> sqlstrli = new List<string>();
                    if (vi != null)
                    {
                        if (vidli.Contains(vi.vender_id.ToString()))
                        {
                            sqlstrli.Add("update bms_kaola_venderinfo set vender_name='" + vi.vender_name + "',vender_alias='" + vi.vender_alias + "',business_scope='" + vi.business_scope + "',refund_name='" + vi.refund_name + "',refund_tel='" + vi.refund_tel + "',refund_mobile='" + vi.refund_mobile + "',refund_province='" + vi.refund_province + "',refund_city='" + vi.refund_city + "',refund_district='" + vi.refund_district + "',refund_address='" + vi.refund_address + "' where vender_id='" + vi.vender_id + "' and  app_key='" + app_key_list[i] + "';");

                        }
                        else
                        {
                            sqlstrli.Add("insert into bms_kaola_venderinfo(vender_id,vender_name,vender_alias,business_scope,refund_name,refund_tel,refund_mobile,refund_province,refund_city,refund_district,refund_address,app_key) values('" + vi.vender_id + "','" + vi.vender_name + "','" + vi.vender_alias + "','" + vi.business_scope + "','" + vi.refund_name + "','" + vi.refund_tel + "','" + vi.refund_mobile + "','" + vi.refund_province + "','" + vi.refund_city + "','" + vi.refund_district + "','" + vi.refund_address + "','" + app_key_list[i] + "');");
                        }
                        if (sqlstrli != null)
                        {
                            count = DbHelperMySQL.ExecuteSqlTran(sqlstrli);
                            if (count > 0)
                            {
                                sb.Append(data + "______________________________更新" + count + "条数据__________________________________");
                            }
                            else
                            {
                                sb.Append(data + "______________________________更新失败__________________________________");
                            }
                        }
                    }
                    //int 
                    //if (count > 0)
                    //{
                    //    
                    //} 
                }
            }

            return sb.ToString();

            #region 原来的代码
            //string data = ks.kaola_vender_info_get("kaola.vender.info.get", time);
            //int count = 0;
            //if (data.Contains("kaola_vender_info_get_response"))
            //{
            //    VenderModel vm = new VenderModel();
            //    vm = kaola_seralia.ScriptDeserialize<VenderModel>(data);
            //    vender_info1 vi1 = new vender_info1();
            //    vi1 = vm.kaola_vender_info_get_response;
            //    vender_info vi = new vender_info();
            //    vi = vi1.vender_info;
            //    List<string> vidli = new List<string>();
            //    vidli = DbHelperMySQL.DataSetToList(DbHelperMySQL.Query("select vender_id from bms_kaola_venderinfo;"));
            //    List<string> sqlstrli = new List<string>();
            //    if (vi != null)
            //    {
            //        if (vidli.Contains(vi.vender_id.ToString()))
            //        {
            //            sqlstrli.Add("update bms_kaola_venderinfo set vender_name='" + vi.vender_name + "',vender_alias='" + vi.vender_alias + "',business_scope='" + vi.business_scope + "',refund_name='" + vi.refund_name + "',refund_tel='" + vi.refund_tel + "',refund_mobile='" + vi.refund_mobile + "',refund_province='" + vi.refund_province + "',refund_city='" + vi.refund_city + "',refund_district='" + vi.refund_district + "',refund_address='" + vi.refund_address + "' where vender_id='" + vi.vender_id + "';");

            //        }
            //        else
            //        {
            //            sqlstrli.Add("insert into bms_kaola_venderinfo(vender_id,vender_name,vender_alias,business_scope,refund_name,refund_tel,refund_mobile,refund_province,refund_city,refund_district,refund_address) values('" + vi.vender_id + "','" + vi.vender_name + "','" + vi.vender_alias + "','" + vi.business_scope + "','" + vi.refund_name + "','" + vi.refund_tel + "','" + vi.refund_mobile + "','" + vi.refund_province + "','" + vi.refund_city + "','" + vi.refund_district + "','" + vi.refund_address + "');");
            //        }
            //        if (sqlstrli != null)
            //        {
            //            count = DbHelperMySQL.ExecuteSqlTran(sqlstrli);
            //            if (count > 0)
            //            {
            //                return data + "______________________________更新" + count + "条数据__________________________________";
            //            }
            //            else
            //            {
            //                return data + "______________________________更新失败__________________________________";
            //            }
            //        }
            //    }
            //    //int 
            //    //if (count > 0)
            //    //{
            //    //    
            //    //} 
            //}
            //return data; 
            #endregion
        }
        #endregion



        #region BMS后台使用
        ///// <summary>
        ///// 商品录入（BMS后台调用）
        ///// </summary>
        ///// <param name="style">款号</param>
        ///// <returns></returns>
        //public string productAdd(string style,string[] scodes)
        //{
        //    //style = "2BL710950";
        //    style = "F2426342";
        //    string s = string.Empty;
        //    string kaola_img_url = string.Empty;                                                                                    //上传图片到考拉服务器的图片路径
        //    //string sqlSelect = string.Format("select * from bms_item where tag_style='{0}' and tag_style<>''  limit 0,1", style);   //SQL语句
        //    string sqlSelect = string.Format("select * from bms_item where item_sku='{0}'", scodes[0].ToString());   //SQL语句
        //    DataTable tb = DbHelperMySQL.Query(sqlSelect).Tables[0];
        //    string title = tb.Rows[0]["title"].ToString();                                                                          //标题 --商品名称", "副标题", "短标题", "描述
        //    string qty = tb.Rows[0]["qty"].ToString();                                                                              //库存
        //    string retail_price = tb.Rows[0]["retail_price"].ToString();                                                            //市场价
        //    string vip_price = tb.Rows[0]["vip_price"].ToString();                                                                  //销售价
        //    string scode = tb.Rows[0]["item_sku"].ToString();                                                                       //货号
        //    string thumb_img_url = tb.Rows[0]["thumb_image_url"].ToString();                                                        //缩略图

        //    //string result = "http://pop.nosdn.127.net/0a2bed72-6a9b-4624-8848-b2e68352ee62";
        //    string result = productAddThumbImg(thumb_img_url);                                                                   //考拉缩略图
        //    string detailImg = "<img src='" + result + "' />";                                                                   //详情描述图

        //    string kaola_thumb_img_url = "54474^" + result;
        //    #region 读取图片
        //    kaola_img_url = productAddImg(scode);
        //    //kaola_img_url = "http://pop.nosdn.127.net/0a2bed72-6a9b-4624-8848-b2e68352ee62^1|http://pop.nosdn.127.net/0a2bed72-6a9b-4624-8848-b2e68352ee62^2";
        //    #endregion

        //    //s = kc.kaola_item_add("kaola.item.add", time, "商品名称", "副标题", "短标题", "描述", "F2426342110138", 1003, "303", "广东省-广州市", "440000-440100", "详情描述", 3607, "52821", "1287^单行文本框", "http://pop.nosdn.127.net/0a2bed72-6a9b-4624-8848-b2e68352ee62^1|http://pop.nosdn.127.net/0a2bed72-6a9b-4624-8848-b2e68352ee62^2", "400", "400", "13123", "12", "54474^http://pop.nosdn.127.net/0a2bed72-6a9b-4624-8848-b2e68352ee62");//, "53539|54458|54459|54460|54461"

        //    s = kc.kaola_item_add("kaola.item.add", time, title, title, title, title, style, 1003, "303", "香港特别行政区-香港", "810000-810100",
        //        detailImg, 3607, "52821", "1287^单行文本框", kaola_img_url, retail_price, vip_price, scode, qty, kaola_thumb_img_url);//, "53539|54458|54459|54460|54461"

        //    dynamic o = JsonConvert.DeserializeObject(s);
        //    if (o.kaola_item_add_response != null)
        //    {
        //        string kaola_key = o.kaola_item_add_response.key;                                                                       //商品录入成功返回的SKU_KEY
        //        string sql = string.Format("insert into bms_kaola_addproduct(item_style,item_sku,kaola_key,business_platform) values('{0}','{1}','{2}','{3}')", style, scode, kaola_key, "考拉");
        //        int count = DbHelperMySQL.ExecuteSql(sql);
        //        if (count > 0)
        //        {
        //            s += "商品录入成功！";
        //        }
        //        else
        //        {
        //            s += "商品录入失败！";
        //        }
        //    }            

        //    return s;
        //}


        public string add123()
        {
            //67290|------331^中国|
            //TEDALMD
            //'item_sku':'TEDALMD261F','color':'251784','price':''
            scode_colors sc = new scode_colors();
            sc.item_sku = "TUSTONSM412F";
            sc.color = "213736";

            //scode_colors sc1 = new scode_colors();
            //sc1.item_sku = "BREITHORN09F";
            //sc1.color = "266556";

            List<scode_colors> lists = new List<scode_colors>();
            //string style = "BREITHORN";
            string style = "TUSTONSM ";
            lists.Add(sc);
            //lists.Add(sc1);
            //string s = productAdd<scode_colors>(style, lists, "67299|67300|67308-----331^112");
            string product_property = "213736-----";
            string s = productAdd<scode_colors>(style, lists, product_property);
            return s;


            //属性批量：sku 采购地 材质 颜色 ；
            //价格批量：sku 价格；
            //上架批量：sku 上架／下架；
            //选品批量：sku 选品；


        }


        /// <summary>
        /// 商品录入（BMS后台调用）
        /// </summary>
        /// <param name="style">款号</param>
        /// <typeparam name="T"></typeparam>
        /// <param name="mylist">货号，颜色集合</param>
        /// <param name="product_property">商品属性</param>
        /// <returns></returns>
        public string productAdd<T>(string style, List<T> mylist, string product_property)
        {
            #region 新增商品相关参数、属性
            //基本信息
            //销售属性：颜色，价格，条形码（货号），库存
            //物流运输属性：所在地（默认香港，待开通意大利货品同路，需修改）
            //商品图片：商品主图，app主图
            //详情描述：文字，图片描述
            //商品属性：采购地，材质，其它（需在考拉后台去查看）
            #endregion


            //scode_colors sc = new scode_colors();
            //sc.item_sku = "33996LICHKF";
            //sc.color = "54474";
            List<scode_colors> lists = mylist as List<scode_colors>;
            //style = "2BL710950";
            //style = "F2426342";
            //style = "3846557";
            //style = "33996";
            //scodes = new string[] { "F2416342110138", "F2416342110140", "F2416342110142", "F2416342110144" };
            //scodes = new string[] { "38465573560836", "38465573560838", "38465573560840" };
            //scodes = new string[] { "33996LICHKF", "33996LIBLKF" };
            //lists.Add(sc);
            string s = string.Empty;
            string kaola_img_url = string.Empty;                                                                                    //上传图片到考拉服务器的图片路径(主图)

            //string sqlSelect = string.Format("select * from bms_item where tag_style='{0}' and tag_style<>''  limit 0,1", style);   //SQL语句
            string sqlSelect1 = string.Format("select title,thumb_image_url,brand_id,category_id from bms_item where item_sku='{0}'", lists[0].item_sku.ToString());   //SQL语句
            DataTable tb1 = DbHelperMySQL.Query(sqlSelect1).Tables[0];
            string title = tb1.Rows[0]["title"].ToString();                                                                          //标题 --商品名称", "副标题", "短标题", "描述
            string brand = tb1.Rows[0]["brand_id"].ToString();                                                                       //品牌
            string categroy = tb1.Rows[0]["category_id"].ToString();                                                                 //类别
            string kaola_thumb_img_url = string.Empty;                                                                               //缩略图
            List<string> list_thumb_img_url = new List<string>();                                                                    //缩略图集合
            List<string> list_details_img_url = new List<string>();                                                                  //详情图集合
            List<string> list_sql_insert = new List<string>();                                                                       //插入数据库SQL语句

            string kala_key = string.Empty;
            string sku_market_prices = string.Empty;                                                                                    //价格（Sku市场价，多个sku的市场价用|分隔,支持2位小数，单位:元）
            string sku_sale_prices = string.Empty;                                                                                      //价格（Sku销售价，多个sku的销售价用|分隔,支持2位小数，单位:元）
            string sku_barcode = string.Empty;                                                                                          //条形码（Sku条形码，多个sku的条形码用|分隔）
            string sku_stock = string.Empty;                                                                                            //库存（Sku库存，整数，多个SKu的库存用|分隔）
            string sku_property_value = string.Empty;                                                                                   //属性图片（Sku属性值Id和图片url,属性值id和图片url用^分隔，同一个sku不同的属性之间用,分隔，不同的sku属性之间用|分隔）
            string sql = string.Empty;

            #region 获取品牌、类目
            brand = DbHelperMySQL.Query("select * from bms_kaola_brand where bms_id='" + brand + "'").Tables[0].Rows[0]["kaola_id"].ToString();
            categroy = DbHelperMySQL.Query("select * from bms_kaola_category where bms_id='" + categroy + "'").Tables[0].Rows[0]["kaola_id"].ToString();
            #endregion

            #region 获取类目属性
            string customProperty = string.Empty; //自定义属性
            string predefineProperty = string.Empty; //预定义属性值

            #region 传递进来的商品属性值
            string[] propertys = product_property.Split(new string[] { "-----" }, StringSplitOptions.RemoveEmptyEntries);
            customProperty = propertys.Length > 1 ? propertys[1] : string.Empty;
            predefineProperty = propertys[0].ToString();

            #endregion

            #region 这个为自行添加商品属性
            //string sql_select_category = string.Format("select * from bms_kaola_rawpropertyname where category_id = '{0}' and is_filter = 0",categroy);
            //string sql_select_category_value = string.Format("select * from bms_kaola_propertyvalue where category_id = '{0}' ",categroy);
            //DataTable dt_category = DbHelperMySQL.Query(sql_select_category).Tables[0];
            //DataTable dt_category_value = DbHelperMySQL.Query(sql_select_category_value).Tables[0];
            //if (dt_category != null)
            //{
            //    for (int i = 0; i < dt_category.Rows.Count; i++) //自定义属性
            //    {
            //        string category_name = dt_category.Rows[i]["prop_name_id"].ToString();
            //        customProperty += category_name + "^长宽高|";
            //    }
            //}
            //customProperty = customProperty.Trim('|');

            //if (dt_category_value != null)
            //{
            //    for (int i = 0; i < dt_category_value.Rows.Count; i++) //预定义属性值
            //    {
            //        string category_name = dt_category_value.Rows[i]["property_value_id"].ToString();
            //        //property_name_id
            //        //property_value_id
            //        predefineProperty += category_name + "|";
            //    }
            //}
            //predefineProperty = predefineProperty.Trim('|');
            #endregion

            #endregion



            int num = lists.Count;
            if (num < 1)
            {
                return "请选择SKU";
            }

            for (int i = 0; i < num; i++)
            {
                string scode = lists[i].item_sku.ToString();

                string sqlSelect = string.Format("select * from bms_item where item_sku='{0}'", scode);                                 //SQL语句
                DataTable tb = DbHelperMySQL.Query(sqlSelect).Tables[0];
                string qty = tb.Rows[0]["qty"].ToString();                                                                              //库存
                string retail_price = tb.Rows[0]["tag_price"].ToString();                                                            //市场价
                string vip_price = tb.Rows[0]["retail_price"].ToString();                                                                  //销售价
                string thumb_img_url = tb.Rows[0]["thumb_image_url"].ToString();                                                       //缩略图
                if (i == 0)
                {
                    string sqlSelectStyleKey = string.Format("select kaola_key from bms_kaola_addproduct where item_style='{0}'", style);    //获取style_key
                    DataTable tb_style_key = DbHelperMySQL.Query(sqlSelectStyleKey).Tables[0];
                    if (tb_style_key != null)
                    {
                        for (int j = 0; j < tb_style_key.Rows.Count; j++)
                        {
                            string key = tb_style_key.Rows[j]["kaola_key"].ToString();
                            if (!string.IsNullOrWhiteSpace(key))
                            {
                                kala_key = key;
                            }
                        }
                    }
                }

                sql += string.Format("insert into bms_kaola_addproduct(item_style,item_sku,kaola_key,business_platform) values('{0}','{1}','{2}','{3}');", style, scode, kala_key, "考拉");

                #region 读取图片
                //kaola_img_url += productAddImg(scode) + "|";
                //kaola_img_url = productAddImg(scode,"主图") + "|";                                                                                 //主图
                //string kaola_details_img_url = productAddImg(scode, "考拉详情图") + "|";                                                                  //详情描述图
                kaola_img_url = productAddImg(scode, "考拉主图") + "|";                                                                                 //主图
                string kaola_details_img_url = productAddImg(scode, "考拉详情") + "|";                                                                  //详情描述图
                string thumb_img_url_result = productAddThumbImg(thumb_img_url);                                                                   //考拉缩略图(返回考拉图片URL)

                string details_img_url_result = string.Empty;
                if (!string.IsNullOrWhiteSpace(kaola_details_img_url) && kaola_details_img_url != "|")
                {
                    string[] s_details_img_url = kaola_details_img_url.Split('|');
                    for (int m = 0; m < s_details_img_url.Length; m++)
                    {
                        if (!string.IsNullOrWhiteSpace(s_details_img_url[m]))
                        {
                            //details_img_url_result = productAddThumbImg(s_details_img_url[m].ToString());                                                         //考拉详情图(返回考拉图片URL)
                            if (!string.IsNullOrWhiteSpace(s_details_img_url[m].ToString()))
                            {
                                list_details_img_url.Add(s_details_img_url[m].ToString());
                            }
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(thumb_img_url_result))
                {

                    list_thumb_img_url.Add(thumb_img_url_result);
                }

                #endregion
                sku_market_prices += retail_price + "|";
                sku_sale_prices += vip_price + "|";
                sku_barcode += scode + "|";
                sku_stock += qty + "|";
                //sku_property_value += kaola_img_url + "|";
            }
            kaola_img_url = kaola_img_url.Trim('|');
            sku_market_prices = sku_market_prices.Trim('|');
            sku_sale_prices = sku_sale_prices.Trim('|');
            sku_barcode = sku_barcode.Trim('|');
            sku_stock = sku_stock.Trim('|');
            sku_property_value = sku_property_value.Trim('|');

            string result = string.Empty;
            string detailImg = string.Empty;                                                                                                        //详情描述图
            //for (int i = 0; i < list_thumb_img_url.Count; i++)
            //{
            //    detailImg = "<img src='" + list_thumb_img_url[i].ToString() + "' />";
            //    kaola_thumb_img_url += "|" + (54474 + i).ToString() + "^" + list_thumb_img_url[i].ToString();
            //}

            for (int i = 0; i < list_thumb_img_url.Count; i++)
            {
                kaola_thumb_img_url += "|" + lists[i].color.Trim().ToString() + "^" + list_thumb_img_url[i].ToString();                             //考拉缩略图==一个SKU图片+SKU颜色
            }

            if (list_details_img_url.Count < 1)
            {
                detailImg = title;
            }

            for (int i = 0; i < list_details_img_url.Count; i++)
            {
                detailImg += "<img src='" + list_details_img_url[i].ToString() + "' />";                                                               //详情描述图
            }

            kaola_thumb_img_url = kaola_thumb_img_url.Trim('|');

            #region 测试（非自动匹配，需修改）
            //kaola_thumb_img_url = "54474^" + result;                                                                                                //考拉缩略图
            //for (int i = 54474; i < 54474 + num; i++)                                                                                               //这里需要在调用的时候传递颜色ID过来进行匹配
            //{
            //    kaola_thumb_img_url += "|" + i.ToString() + "^" + result;
            //}
            #endregion

            //s = kc.kaola_item_add("kaola.item.add", time, title, title, title, title, style, 1003, "303", "香港特别行政区-香港", "810000-810100",
            //    detailImg, 3607, "52821", "1287^单行文本框", kaola_img_url, sku_market_prices, sku_sale_prices, sku_barcode, sku_stock, kaola_thumb_img_url);//, "53539|54458|54459|54460|54461"

            //s = kc.kaola_item_add("kaola.item.add", time, title, title, title, title, style, int.Parse(brand), "303", "香港特别行政区-香港", "810000-810100",
            //    detailImg, int.Parse(categroy), "", "1022^手提包", kaola_img_url, sku_market_prices, sku_sale_prices, sku_barcode, sku_stock, kaola_thumb_img_url);

            //s = kc.kaola_item_add("kaola.item.add", time, title, title, title, title, style, int.Parse(brand), "303", "香港特别行政区-香港", "810000-810100",
            //    detailImg, int.Parse(categroy), "67290", "331^手提包", kaola_img_url, sku_market_prices, sku_sale_prices, sku_barcode, sku_stock, "");//, "53539|54458|54459|54460|54461"


            /*注意商品属性和销售属性和物流属性的区别*/
            /*20160129 这里在调用多sku出错，单个应用是没问题的，原因为缺少kaola_thumb_img_url这个缩略图参数*/
            //s = kc.kaola_item_add("kaola.item.add", time, title, title, title, title, style, int.Parse(brand), "303", "香港特别行政区-香港", "810000-810100",
            //    detailImg, int.Parse(categroy), predefineProperty, customProperty, kaola_img_url, sku_market_prices, sku_sale_prices, sku_barcode, sku_stock, "");//, "53539|54458|54459|54460|54461"

            s = kc.kaola_item_add("kaola.item.add", time, title, title, title, title, style, int.Parse(brand), "303", "香港特别行政区-香港", "810000-810100",
                detailImg, int.Parse(categroy), "", customProperty, kaola_img_url, sku_market_prices, sku_sale_prices, sku_barcode, sku_stock, kaola_thumb_img_url);//, "53539|54458|54459|54460|54461"
            System.IO.File.WriteAllText("c:\\kaola_log.txt", "kaola.item.add" + "===" + time + "===" + title + "===" + title + "===" + title + "===" + title + "===" + style + "===" + int.Parse(brand) + "===" + "303" + "===" + "香港特别行政区-香港" + "===" + "810000-810100" + "===" + detailImg + "===" + int.Parse(categroy) + "===" + predefineProperty + "===" + customProperty + "===" + kaola_img_url + "===" + sku_market_prices + "===" + sku_sale_prices + "===" + sku_barcode + "===" + sku_stock + "===" + kaola_thumb_img_url, Encoding.UTF8);
            dynamic o = JsonConvert.DeserializeObject(s);
            if (o.kaola_item_add_response != null)
            {
                string kaola_key = o.kaola_item_add_response.key;                                                                                   //商品录入成功返回的SKU_KEY

                for (int j = 0; j < num; j++)
                {
                    string sql2 = string.Format("insert into bms_kaola_addproduct(item_style,item_sku,kaola_key,business_platform) values('{0}','{1}','{2}','{3}');", style, lists[j].item_sku.ToString(), kaola_key, "考拉");
                    list_sql_insert.Add(sql2);
                }

                int count = DbHelperMySQL.ExecuteSqlTran(list_sql_insert);
                if (count == num)
                {
                    s += "商品录入成功！";
                }
                else
                {
                    s += "商品录入失败！";
                }
            }

            return s;
        }


        /// <summary>
        /// 获取上架中的条形码集合
        /// </summary>
        /// <param name="statu">商品状态</param>
        /// <returns></returns>
        public List<Dictionary<string, string>> GetUpProduct(string statu)
        {
            //string statu = "在售";
            List<Dictionary<string, string>> barcodeli = new List<Dictionary<string, string>>();
            int itemstatu;
            #region 商品状态
            if (string.IsNullOrWhiteSpace(statu))
            {
                return null;
            }
            else if (statu == "待提交审核")
            {
                itemstatu = 1;
            }
            else if (statu == "审核中")
            {
                itemstatu = 2;
            }
            else if (statu == "审核未通过")
            {
                itemstatu = 3;
            }
            else if (statu == "待上架")
            {
                itemstatu = 4;
            }
            else if (statu == "在售")
            {
                itemstatu = 5;
            }
            else if (statu == "下架")
            {
                itemstatu = 6;
            }
            else if (statu == "已删除")
            {
                itemstatu = 7;
            }
            else if (statu == "强制下架")
            {
                itemstatu = 8;
            }
            else
            {
                return null;
            }
            #endregion


            #region 获取数据库中已存在的sku_key
            string sql_skukey_select = string.Format("select item_sku from bms_kaola_addproduct");
            DataTable dt_sku = DbHelperMySQL.Query(sql_skukey_select).Tables[0];
            string[] str_sku = new string[dt_sku.Rows.Count];
            if (dt_sku != null && dt_sku.Rows.Count > 0)
            {
                for (int i = 0; i < dt_sku.Rows.Count; i++)
                {
                    str_sku[i] = dt_sku.Rows[i]["item_sku"].ToString().Trim();
                }
            }
            #endregion

            int sale_count = 0;//在售总条数
            int page_count = 0;//页面总条数
            string data_count = kc.kaola_item_batch_status_get("kaola.item.batch.status.get", time, itemstatu, 1, 20);//在售总数
            dynamic dc_count = JsonConvert.DeserializeObject(data_count);
            if (dc_count.kaola_item_batch_status_get_response.total_count != null)  //在售总数
            {
                sale_count = dc_count.kaola_item_batch_status_get_response.total_count;
            }

            page_count = sale_count / 100;
            if (sale_count % 100 != 0)
            {
                page_count = page_count + 1;
            }
            //sale_count = 
            for (int z = 1; z <= page_count; z++)
            {
                string data = kc.kaola_item_batch_status_get("kaola.item.batch.status.get", time, itemstatu, z, 100);
                dynamic dc = JsonConvert.DeserializeObject(data);
                var dc1 = dc.kaola_item_batch_status_get_response.item_edit_list;
                if (dc1 != null)
                {
                    for (int i = 0; i < dc1.Count; i++)
                    {
                        if (dc1[i] != null)
                        {
                            dynamic dc2 = JsonConvert.DeserializeObject(dc1[i].ToString());
                            if (dc2.sku_list != null)
                            {
                                var dc3 = dc2.sku_list;
                                for (int j = 0; j < dc3.Count; j++)
                                {
                                    Dictionary<string, string> key_value = new Dictionary<string, string>();
                                    string sku_key = string.Empty;//（kaola_key）
                                    string bar_code = string.Empty; //条形码（货号）
                                    dynamic dc4 = JsonConvert.DeserializeObject(dc3[j].ToString());
                                    if (dc4.key != null)
                                    {
                                        sku_key = dc4.key;  //值
                                    }
                                    var dc5 = dc4.raw_sku;
                                    if (dc5 != null)
                                    {
                                        if (dc5.bar_code != null)
                                        {
                                            bar_code = dc5.bar_code; //键
                                        }
                                    }
                                    key_value.Add(bar_code, sku_key);
                                    barcodeli.Add(key_value);

                                    string sql_skukey_insert = string.Empty;
                                    if (str_sku.Contains(bar_code.Trim()))
                                    {
                                        sql_skukey_insert = string.Format("update bms_kaola_addproduct set kaola_sku_key='{0}',business_platform='{1}',sale_status={2} where item_sku='{3}'", sku_key, "考拉", 1, bar_code);
                                    }
                                    else
                                    {
                                        sql_skukey_insert = string.Format("insert into bms_kaola_addproduct(item_sku,kaola_sku_key,business_platform,sale_status) values('{0}','{1}','{2}',{3})", bar_code, sku_key, "考拉", 1);
                                    }
                                    DbHelperMySQL.ExecuteSql(sql_skukey_insert);
                                }
                            }
                        }
                    }
                }
            }


            return barcodeli;
        }























        /// <summary>
        /// 图片录入（BMS后台调用）
        /// </summary>
        /// <param name="scode">货号</param>
        private string productAddImg(string scode, string img_type)
        {
            string s = string.Empty;
            string kaola_img_url = string.Empty;
            string sqlImg = string.Format("select image_url from bms_item_image where item_sku = '{0}' and image_type='{1}' order by sort", scode, img_type);
            DataTable dtImg = DbHelperMySQL.Query(sqlImg).Tables[0];
            string[] str_img = new string[dtImg.Rows.Count];                                                                        //图片数组
            if (dtImg != null)
            {
                for (int i = 0; i < dtImg.Rows.Count; i++)
                {
                    str_img[i] = dtImg.Rows[i]["image_url"].ToString();
                }
            }

            #region 测试
            //if (str_img.Length<1)
            //{
            //    str_img = new string[1] { "http://best-bms.pbxluxury.com/images/BV/M3100/274472/274472640990/7_274472640990.jpg@1e_400w_400h_1c_0i_1o_90Q_1x.jpg" };
            //}
            #endregion

            if (str_img.Length < 1)
            {
                s += scode + "暂无图片;";
            }
            else
            {
                List<string> list_sql_insert_img = new List<string>();                                                               //插入图片sql语句集合
                for (int i = 0; i < str_img.Length; i++)
                {
                    string[] str_img_up = new string[] { str_img[i].ToString() };
                    //{"kaola_item_img_upload_response":{"created":"2015-12-18 10:13:18","result":true,"url":"http://pop.nosdn.127.net/43363eb2-38ee-4dce-8a4a-1d8200a7cb80"}}
                    string img_up_result = productImgAdd(str_img_up);
                    dynamic img_up_result_split = JsonConvert.DeserializeObject(img_up_result);
                    if (img_up_result_split.kaola_item_img_upload_response != null)
                    {
                        if (img_up_result_split.kaola_item_img_upload_response.result == "true")
                        {
                            string img_url_result = img_up_result_split.kaola_item_img_upload_response.url;                               //主图
                            string sql_insert_img = string.Format("insert into bms_kaola_item_image values('{0}','{1}')", scode, img_url_result);
                            //if (img_type=="主图")
                            if (img_type == "考拉主图")
                            {
                                kaola_img_url += img_url_result + "^1|" + img_url_result + "^2" + "|";
                            }
                            //else if (img_type=="考拉详情图")
                            else if (img_type == "考拉详情")
                            {
                                kaola_img_url += img_url_result + "|";
                            }
                            list_sql_insert_img.Add(sql_insert_img);
                        }
                    }
                }
                kaola_img_url = kaola_img_url.Trim('|');

                //int img_count = DbHelperMySQL.ExecuteSqlTran(list_sql_insert_img);
                //if (img_count != str_img.Length)                                                                                    //判断插入数据的图片集是否和原数据库中图片的数量一致
                //{
                //    s += "共有图片" + str_img.Length + "张,上传成功" + img_count + "张";
                //}
            }
            return kaola_img_url;
        }

        /// <summary>
        /// 上传图片（返回商品缩略图）
        /// </summary>
        /// <param name="thumb_img_url"></param>
        /// <returns></returns>
        private string productAddThumbImg(string thumb_img_url)
        {
            string kaola_thumb_img_url = string.Empty;
            string[] img = new string[] { thumb_img_url };
            string result = productImgAdd(img);
            dynamic o = JsonConvert.DeserializeObject(result);
            //{"kaola_item_img_upload_response":{"created":"2015-12-22 17:58:09","result":true,"url":"http://pop.nosdn.127.net/ce360b49-a763-453e-8010-8fddbb1ee751"}}
            if (o.kaola_item_img_upload_response != null)
            {
                kaola_thumb_img_url = o.kaola_item_img_upload_response.url;
            }

            return kaola_thumb_img_url;
        }


        /// <summary>
        /// 商品SKU录入（BMS后台调用）
        /// </summary>
        /// <param name="scodes">货号数组</param>
        /// <returns></returns>
        //public string productAddSku(string[] scodes)
        //{
        //    //scodes = new string[] { "F2416342110138", "F2416342110140", "F2416342110142", "F2416342110144" };

        //    string s = string.Empty;
        //    string style = string.Empty;
        //    string kala_key = string.Empty;
        //    string sku_market_prices = string.Empty;                                                                                    //价格（Sku市场价，多个sku的市场价用|分隔,支持2位小数，单位:元）
        //    string sku_sale_prices = string.Empty;                                                                                      //价格（Sku销售价，多个sku的销售价用|分隔,支持2位小数，单位:元）
        //    string sku_barcode = string.Empty;                                                                                          //条形码（Sku条形码，多个sku的条形码用|分隔）
        //    string sku_stock = string.Empty;                                                                                            //库存（Sku库存，整数，多个SKu的库存用|分隔）
        //    string sku_property_value = string.Empty;                                                                                   //属性图片（Sku属性值Id和图片url,属性值id和图片url用^分隔，同一个sku不同的属性之间用,分隔，不同的sku属性之间用|分隔）
        //    string sql = string.Empty;

        //    int num = scodes.Length;
        //    if (num < 1)
        //    {
        //        return "请选择SKU";
        //    }

        //    for (int i = 0; i < num; i++)
        //    {
        //        string scode = scodes[i].ToString();
        //        string kaola_img_url = string.Empty;                                                                                    //上传图片到考拉服务器的图片路径
        //        string sqlSelect = string.Format("select * from bms_item where item_sku='{0}'", scode);                                 //SQL语句
        //        DataTable tb = DbHelperMySQL.Query(sqlSelect).Tables[0];
        //        string qty = tb.Rows[0]["qty"].ToString();                                                                              //库存
        //        string retail_price = tb.Rows[0]["retail_price"].ToString();                                                            //市场价
        //        string vip_price = tb.Rows[0]["vip_price"].ToString();                                                                  //销售价
        //        style = tb.Rows[0]["style"].ToString();                                                                                 //款号
        //        if (i == 0)
        //        {
        //            string sqlSelectStyleKey = string.Format("select kaola_key from bms_kaola_addproduct where item_style='{0}'", style);    //获取style_key
        //            DataTable tb_style_key = DbHelperMySQL.Query(sqlSelectStyleKey).Tables[0];
        //            if (tb_style_key != null)
        //            {
        //                for (int j = 0; j < tb_style_key.Rows.Count; j++)
        //                {
        //                    string key = tb_style_key.Rows[j]["kaola_key"].ToString();
        //                    if (!string.IsNullOrWhiteSpace(key))
        //                    {
        //                        kala_key = key;
        //                    }
        //                }
        //            }
        //        }

        //        sql += string.Format("insert into bms_kaola_addproduct(item_style,item_sku,kaola_key,business_platform) values('{0}','{1}','{2}','{3}');", style, scode, kala_key, "考拉");

        //        #region 读取图片
        //        kaola_img_url = productAddImg(scode);
        //        #endregion
        //        sku_market_prices += retail_price + "|";
        //        sku_sale_prices += vip_price + "|";
        //        sku_barcode += scode + "|";
        //        sku_stock += qty + "|";
        //        sku_property_value += kaola_img_url + "|";
        //    }

        //    sku_market_prices = sku_market_prices.Trim('|');
        //    sku_sale_prices = sku_sale_prices.Trim('|');
        //    sku_barcode = sku_barcode.Trim('|');
        //    sku_stock = sku_stock.Trim('|');
        //    sku_property_value = sku_property_value.Trim('|');

        //    //string data = kc.kaola_item_sku_add("kaola.item.sku.add", time, "100047-3049", "200", "200", "123456789", "5", "54476^http://pic13.secooimg.com/push/15/12/66f1262c20634f9ca681fbbd2a7fadad.jpg");

        //    //s = kc.kaola_item_sku_add("kaola.item.sku.add", time, kala_key, sku_market_prices, sku_sale_prices, sku_barcode, sku_stock,
        //    //    "54477^http://pic13.secooimg.com/push/15/12/66f1262c20634f9ca681fbbd2a7fadad.jpg|54479^http: //pic13.secooimg.com/push/15/12/66f1262c20634f9ca681fbbd2a7fadad.jpg|54480^http://pic13.secooimg.com/push/15/12/66f1262c20634f9ca681fbbd2a7fadad.jpg|54481^http: //pic13.secooimg.com/push/15/12/66f1262c20634f9ca681fbbd2a7fadad.jpg");

        //    s = kc.kaola_item_sku_add("kaola.item.sku.add", time, kala_key, sku_market_prices, sku_sale_prices, sku_barcode, sku_stock, sku_property_value);


        //    dynamic o = JsonConvert.DeserializeObject(s);
        //    string kaola_key = o.kaola_item_sku_add_response.result;                                                                          //商品录入成功返回的SKU_KEY
        //    int count = DbHelperMySQL.ExecuteSql(sql);
        //    if (count > 0)
        //    {
        //        s += "SKU录入成功！";
        //    }
        //    else
        //    {
        //        s += "SKU录入失败！";
        //    }

        //    return s;
        //}





        public string add456()
        {
            scode_colors sc = new scode_colors();
            sc.item_sku = "33996LIBLKF";
            sc.color = "54475";
            List<scode_colors> lists = new List<scode_colors>();
            //style = "2BL710950";
            //style = "F2426342";
            //style = "3846557";
            string style = "33996";
            //scodes = new string[] { "F2416342110138", "F2416342110140", "F2416342110142", "F2416342110144" };
            //scodes = new string[] { "38465573560836", "38465573560838", "38465573560840" };
            //scodes = new string[] { "33996LICHKF", "33996LIBLKF" };
            lists.Add(sc);
            string s = productAddSku<scode_colors>(style, lists);
            return s;
        }



        public string productAddSku<T>(string style, List<T> mylist)
        {
            List<scode_colors> lists = mylist as List<scode_colors>;
            //scodes = new string[] { "F2416342110138", "F2416342110140", "F2416342110142", "F2416342110144" };

            string s = string.Empty;
            string kala_key = string.Empty;
            string sku_market_prices = string.Empty;                                                                                    //价格（Sku市场价，多个sku的市场价用|分隔,支持2位小数，单位:元）
            string sku_sale_prices = string.Empty;                                                                                      //价格（Sku销售价，多个sku的销售价用|分隔,支持2位小数，单位:元）
            string sku_barcode = string.Empty;                                                                                          //条形码（Sku条形码，多个sku的条形码用|分隔）
            string sku_stock = string.Empty;                                                                                            //库存（Sku库存，整数，多个SKu的库存用|分隔）
            string sku_property_value = string.Empty;                                                                                   //属性图片（Sku属性值Id和图片url,属性值id和图片url用^分隔，同一个sku不同的属性之间用,分隔，不同的sku属性之间用|分隔）
            string sql = string.Empty;

            int num = lists.Count;
            if (num < 1)
            {
                return "请选择SKU";
            }

            for (int i = 0; i < num; i++)
            {
                string scode = lists[i].item_sku.ToString();
                string kaola_img_url = string.Empty;                                                                                    //上传图片到考拉服务器的图片路径
                string sqlSelect = string.Format("select * from bms_item where item_sku='{0}'", scode);                                 //SQL语句
                DataTable tb = DbHelperMySQL.Query(sqlSelect).Tables[0];
                string qty = tb.Rows[0]["qty"].ToString();                                                                              //库存
                string retail_price = string.Empty;
                if (string.IsNullOrWhiteSpace(lists[i].price))//如果没有传入价格，就选择数据库的值
                {
                    retail_price = tb.Rows[0]["retail_price"].ToString();                                                               //市场价
                }
                else
                {
                    retail_price = lists[i].price;                                                                                      //市场价
                }

                string vip_price = tb.Rows[0]["vip_price"].ToString();                                                                  //销售价
                string thumb_img_url = tb.Rows[0]["thumb_image_url"].ToString();                                                        //缩略图
                //style = tb.Rows[0]["style"].ToString();                                                                               //款号
                if (i == 0)
                {
                    string sqlSelectStyleKey = string.Format("select kaola_key from bms_kaola_addproduct where item_style='{0}'", style);    //获取style_key
                    DataTable tb_style_key = DbHelperMySQL.Query(sqlSelectStyleKey).Tables[0];
                    if (tb_style_key != null)
                    {
                        for (int j = 0; j < tb_style_key.Rows.Count; j++)
                        {
                            string key = tb_style_key.Rows[j]["kaola_key"].ToString();
                            if (!string.IsNullOrWhiteSpace(key))
                            {
                                kala_key = key;
                            }
                        }
                    }
                }

                sql += string.Format("insert into bms_kaola_addproduct(item_style,item_sku,kaola_key,business_platform) values('{0}','{1}','{2}','{3}');", style, scode, kala_key, "考拉");

                #region 读取图片
                //kaola_img_url = productAddImg(scode);
                kaola_img_url = lists[i].color + "^" + productAddThumbImg(thumb_img_url);
                #endregion
                sku_market_prices += retail_price + "|";
                sku_sale_prices += vip_price + "|";
                sku_barcode += scode + "|";
                sku_stock += qty + "|";
                sku_property_value += kaola_img_url + "|";
            }

            sku_market_prices = sku_market_prices.Trim('|');
            sku_sale_prices = sku_sale_prices.Trim('|');
            sku_barcode = sku_barcode.Trim('|');
            sku_stock = sku_stock.Trim('|');
            sku_property_value = sku_property_value.Trim('|');

            //string data = kc.kaola_item_sku_add("kaola.item.sku.add", time, "100047-3049", "200", "200", "123456789", "5", "54476^http://pic13.secooimg.com/push/15/12/66f1262c20634f9ca681fbbd2a7fadad.jpg");

            //s = kc.kaola_item_sku_add("kaola.item.sku.add", time, kala_key, sku_market_prices, sku_sale_prices, sku_barcode, sku_stock,
            //    "54477^http://pic13.secooimg.com/push/15/12/66f1262c20634f9ca681fbbd2a7fadad.jpg|54479^http: //pic13.secooimg.com/push/15/12/66f1262c20634f9ca681fbbd2a7fadad.jpg|54480^http://pic13.secooimg.com/push/15/12/66f1262c20634f9ca681fbbd2a7fadad.jpg|54481^http: //pic13.secooimg.com/push/15/12/66f1262c20634f9ca681fbbd2a7fadad.jpg");

            s = kc.kaola_item_sku_add("kaola.item.sku.add", time, kala_key, sku_market_prices, sku_sale_prices, sku_barcode, sku_stock, sku_property_value);


            dynamic o = JsonConvert.DeserializeObject(s);
            string kaola_key = o.kaola_item_sku_add_response.result;                                                                          //商品录入成功返回的SKU_KEY
            int count = DbHelperMySQL.ExecuteSql(sql);
            if (count > 0)
            {
                s += "SKU录入成功！";
            }
            else
            {
                s += "SKU录入失败！";
            }

            return s;
        }


        /// <summary>
        /// 商品图片上传
        /// </summary>
        /// <param name="string_img">图片地址（如：new string[] { "http://best-bms.pbxluxury.com/images/COH/L0100/33521/33521LIBAJF/5_33521LIBAJF.jpg" }）</param>
        /// <returns></returns>
        public string productImgAdd(string[] string_img)
        {
            if (string_img.Length > 1)
            {
                return "只允许一次上传一张图片";
            }
            return kaola_item_img_upload(string_img);
        }

        /// <summary>
        /// 商品上架
        /// </summary>
        /// <param name="kaola_key">考拉SKU-key</param>
        /// <returns></returns>
        public string productUpper(string kaola_key)
        {
            string s = string.Empty;

            //s = kc.kaola_item_update_listing("kaola.item.update.listing", time, "85370-3049");
            s = kc.kaola_item_update_listing("kaola.item.update.listing", time, kaola_key);
            dynamic o = JsonConvert.DeserializeObject(s);
            string upperResult = o.kaola_item_update_listing_response.result;
            //string kaola_key = o.kaola_item_add_response.key;
            //bms_item
            //string sql = string.Format("update bms_item set kaola_key='{0}' where item_sku = '{1}'", kaola_key, scode);
            //int i = DbHelperMySQL.ExecuteSql(sql);
            if (int.Parse(upperResult) > 0)
            {
                s = "商品上架成功！";
            }
            else
            {
                s = "商品上架失败！";
            }
            return s;
        }


        /// <summary>
        /// 商品下架
        /// </summary>
        /// <param name="kaola_key">考拉SKU-key</param>
        /// <returns></returns>
        public string productLower(string kaola_key)
        {
            //kc.kaola_item_get("", time, kaola_key);

            string s = string.Empty;
            if (string.IsNullOrWhiteSpace(kaola_key))
            {
                kaola_key = "85359-3049";
            }
            s = kc.kaola_item_update_delisting("kaola.item.update.delisting", time, kaola_key);
            //s = kc.kaola_item_update_delisting("kaola.item.update.delisting", time, "85370-3049");
            dynamic o = JsonConvert.DeserializeObject(s);
            string lowerResult = o.kaola_item_update_delisting_response.result;
            //string kaola_key = o.kaola_item_add_response.key;
            //bms_item
            //string sql = string.Format("update bms_item set kaola_key='{0}' where item_sku = '{1}'", kaola_key, scode);
            //int i = DbHelperMySQL.ExecuteSql(sql);
            if (int.Parse(lowerResult) > 0)
            {
                s = "商品下架成功！";
            }
            else
            {
                s = "商品下架失败！";
            }
            return s;
        }


        /// <summary>
        /// 修改商品价格
        /// </summary>
        /// <param name="sku_key">货号</param>
        /// <param name="sale_price">修改的价格</param>
        /// <returns></returns>
        public string productUpdatePrice(string sku_key, decimal sale_price)
        //public string productUpdatePrice()
        {
            //string sku_key = string.Empty;






            //decimal sale_price = 0;
            //if (string.IsNullOrWhiteSpace(sku_key))
            //{
            //    sku_key = "85363-3049";
            //}
            //sale_price = 2;

            string data = kc.kaola_item_sku_sale_price_update("kaola.item.sku.sale.price.update", time, sku_key, sale_price);
            dynamic dc = JsonConvert.DeserializeObject(data);
            string result_price = string.Empty;
            if (dc.kaola_item_sku_sale_price_update_response != null)
            {
                if (dc.kaola_item_sku_sale_price_update_response.result != null)
                {
                    result_price = dc.kaola_item_sku_sale_price_update_response.result;
                }
                else
                {
                    result_price = data;
                }
            }
            else
            {
                result_price = data;
            }

            return result_price;
        }


        /// <summary>
        /// 修改商品库存
        /// </summary>
        /// <param name="sku_key">货号</param>
        /// <param name="stock">修改的库存量</param>
        /// <returns></returns>
        public string productUpdateStock(string sku_key, int stock)
        //public string productUpdateStock()
        {
            //string sku_key = string.Empty;
            //int stock = 0;
            //if (string.IsNullOrWhiteSpace(sku_key))
            //{
            //    sku_key = "85363-3049";
            //}
            //stock = 38;

            string data = kc.kaola_item_sku_stock_update("kaola.item.sku.stock.update", time, sku_key, stock);
            dynamic dc = JsonConvert.DeserializeObject(data);
            string result_stock = string.Empty;
            if (dc.kaola_item_get_response != null)
            {
                if (dc.kaola_item_sku_sale_price_update_response.result != null)
                {
                    result_stock = dc.kaola_item_sku_sale_price_update_response.result;
                }
                else
                {
                    result_stock = data;
                }
            }
            else
            {
                result_stock = data;
            }
            return result_stock;
        }

        /// <summary>
        ///  获取订单状态
        /// </summary>
        /// <param name="orderId">订单ID</param>
        /// <returns></returns>
        public string selectOrderStatus(string orderId)
        {
            //string s = kl.kaola_order_get("kaola.order.get", time, "2015121214401011040043737");
            string s = kl.kaola_order_get("kaola.order.get", time, orderId);
            dynamic dc = JsonConvert.DeserializeObject(s);
            string order_status = dc.kaola_order_get_response.order.order_status;
            return order_status;
        }

        /// <summary>
        /// 订单发货通知
        /// </summary>
        /// <param name="order_id">订单ID</param>
        /// <param name="expressName">快递公司</param>
        /// <param name="expressNo">快递单号</param>
        /// <param name="expressNo">店铺名称  </param>
        /// <returns></returns>
        public string orderSend(string order_id, string expressName, string expressNo, string platform)
        {
            DataSet ds = new DataSet();
            //根据店铺名称获取相关键值
            ds = DbHelperMySQL.Query(string.Format(@"select * from bms_appkey_platform where platform='{0}'", platform));
            //2015121611171011020046283
            string s = kaola_logistics_deliver(order_id, expressName, expressNo, ds.Tables[0]);
            //string s = kaola_logistics_deliver("2015121611171011020046283", "联邦快递", "12345678912");
            return s;
        }

        #region 原来的
        /// <summary>
        /// 订单发货通知
        /// </summary>
        /// <param name="order_id">订单ID</param>
        /// <param name="expressName">快递公司</param>
        /// <param name="expressNo">快递单号</param>
        /// <returns></returns>
        //public string orderSend(string order_id, string expressName, string expressNo)
        //{
        //    //2015121611171011020046283
        //    string s = kaola_logistics_deliver(order_id, expressName, expressNo);
        //    //string s = kaola_logistics_deliver("2015121611171011020046283", "联邦快递", "12345678912");
        //    return s;
        //} 
        #endregion

        /// <summary>
        /// 获取SKU_KEY
        /// </summary>
        /// <returns></returns>
        public string getSkuKey(out string out_sku_key, string item_sku_scode = null)
        {
            out_sku_key = string.Empty;
            string s = string.Empty;
            string sqlUpdate = string.Empty;                                                                                                                    //根据spu_key更新sku_key可以为空的数据
            string sqlSelect = string.Empty;
            if (string.IsNullOrWhiteSpace(item_sku_scode))
            {
                sqlSelect = string.Format("select kaola_key,kaola_sku_key from bms_kaola_addproduct where (kaola_sku_key ='' or kaola_sku_key is null)");     //查询有多少sku_key为空的spu_key
            }
            else
            {
                sqlSelect = string.Format("select kaola_key,kaola_sku_key from bms_kaola_addproduct where item_sku='{0}'", item_sku_scode);                    //查询有多少sku_key为空的spu_key
            }
            List<string> listSQL = new List<string>();

            DataTable dt = DbHelperMySQL.Query(sqlSelect).Tables[0];
            if (dt != null)
            {
                int num = dt.Rows.Count;
                for (int i = 0; i < num; i++)
                {
                    string spu_key = dt.Rows[i]["kaola_key"].ToString();
                    if (!string.IsNullOrWhiteSpace(spu_key))                                                        //商品KEY不为空
                    {
                        string data = kc.kaola_item_get("kaola.item.get", time, spu_key);                           //获取sku_key，返回此商品的所有sku及其相关属性
                        dynamic o = JsonConvert.DeserializeObject(data);
                        if (o.kaola_item_get_response != null)
                        {
                            if (o.kaola_item_get_response.sku_list != null)
                            {
                                dynamic sku_list = JsonConvert.DeserializeObject(o.kaola_item_get_response.sku_list.ToString());
                                for (int j = 0; j < sku_list.Count; j++)
                                {
                                    string sku_key = sku_list[j].key;                                               //获取SKU_KEY
                                    string item_sku = sku_list[j].raw_sku.bar_code;                                 //获取SKU条形码（货号）

                                    out_sku_key = item_sku;
                                    sqlUpdate = string.Format("update bms_kaola_addproduct set kaola_sku_key ='{0}' where item_sku='{1}' and kaola_key='{2}';", sku_key, item_sku, spu_key);
                                    listSQL.Add(sqlUpdate);
                                }
                            }
                            else
                                return data;
                        }
                        else
                            return data;
                    }
                }

                int count = DbHelperMySQL.ExecuteSqlTran(listSQL);
                if (count == num)
                    s = "更新SKU_KEY成功";
                else
                    s = "更新SKU_KEY失败";
            }
            return s;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string get_sale_sku(string statu)
        {
            string s = string.Empty;
            int itemstatu = 0;
            #region 商品状态
            if (string.IsNullOrWhiteSpace(statu))
            {
                return null;
            }
            else if (statu == "待提交审核")
            {
                itemstatu = 1;
            }
            else if (statu == "审核中")
            {
                itemstatu = 2;
            }
            else if (statu == "审核未通过")
            {
                itemstatu = 3;
            }
            else if (statu == "待上架")
            {
                itemstatu = 4;
            }
            else if (statu == "在售")
            {
                itemstatu = 5;
            }
            else if (statu == "下架")
            {
                itemstatu = 6;
            }
            else if (statu == "已删除")
            {
                itemstatu = 7;
            }
            else if (statu == "强制下架")
            {
                itemstatu = 8;
            }
            else
            {
                return null;
            }
            #endregion
            string data = kc.kaola_item_batch_status_get("kaola.item.batch.status.get", time, itemstatu, 1, 100);

            return s;
        }

        #endregion


    }


    public class ApiErrorMsg
    {
        /// <summary>
        /// 错误方法
        /// </summary>
        public string errorAction { get; set; }
        /// <summary>
        /// 错误详情
        /// </summary>
        public string errorMsg { get; set; }
        /// <summary>
        /// 错误时间
        /// </summary>
        public string errorTime { get; set; }
        /// <summary>
        /// 出错单号
        /// </summary>
        public string errorOrderId { get; set; }
        /// <summary>
        /// 更新次数唯一标识
        /// </summary>
        public string errorKey { get; set; }
    }
}





