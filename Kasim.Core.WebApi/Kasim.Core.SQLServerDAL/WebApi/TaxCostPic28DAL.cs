﻿/*********************************************************************
 **                             _ooOoo_                             **
 **                            o8888888o                            **
 **                            88" . "88                            **
 **                            (| -_- |)                            **
 **                            O\  =  /O                            **
 **                         ____/`---'\____                         **
 **                       .'  \\|     |//  `.                       **
 **                      /  \\|||  :  |||//  \                      **
 **                     /  _||||| -:- |||||-  \                     **
 **                     |   | \\\  -  /// |   |                     **
 **                     | \_|  ''\---/''  |   |                     **
 **                     \  .-\__  `-`  ___/-. /                     **
 **                   ___`. .'  /--.--\  `. . __                    **
 **                ."" '<  `.___\_<|>_/___.'  >'"".                 **
 **               | | :  `- \`.;`\ _ /`;.`/ - ` : | |               **
 **               \  \ `-.   \_ __\ /__ _/   .-` /  /               **
 **          ======`-.____`-.___\_____/___.-`____.-'======          **
 **                             `=---='                             **
 **          ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^          **
 **                     佛祖保佑        永无BUG                     **
 **            佛曰:                                                **
 **                   写字楼里写字间，写字间里程序员；              **
 **                   程序人员写程序，又拿程序换酒钱。              **
 **                   酒醒只在网上坐，酒醉还来网下眠；              **
 **                   酒醉酒醒日复日，网上网下年复年。              **
 **                   但愿老死电脑间，不愿鞠躬老板前；              **
 **                   奔驰宝马贵者趣，公交自行程序员。              **
 **                   别人笑我忒疯癫，我笑自己命太贱；              **
 **                   不见满街漂亮妹，哪个归得程序员？              **
 *********************************************************************/
/*----------------------------------------------------------------
** Copyright (C) 2017 
**
** file：TaxCostPic28DAL
** desc：
** 
** auth：KasimYe (KASIM)
** date：2017-12-05 13:25:02
**
** Ver.：V1.0.0
**----------------------------------------------------------------*/

using Kasim.Core.IDAL.WebApi;
using System;
using System.Collections.Generic;
using System.Text;
using Kasim.Core.Model.WebApi;
using System.Data;
using Kasim.Core.Factory;
using Dapper;
using System.Linq;

namespace Kasim.Core.SQLServerDAL.WebApi
{
    public class TaxCostPic28DAL : ITaxCostPic28DAL
    {
        private IDbConnection _conn;

        public IDbConnection Conn
        {
            get
            {
                var connString = ConnectionFactory.ConnectionStrings.DevConnection;
                return _conn = ConnectionFactory.CreateConnection(connString);
            }
        }

        public TaxCostPic28 GetEntity(int id)
        {
            using (Conn)
            {
                string query = "SELECT * FROM dbo.TaxCostPic28 WHERE CostID=@CostID";
                var result = Conn.Query<TaxCostPic28>(query, new { CostID = id }).SingleOrDefault();
                return result;
            }
        }

        public TaxCostPic28 GetEntityByTax(string invoiceCode, string invoiceID)
        {
            using (Conn)
            {
                string query = "SELECT TOP 1 PicUrl FROM dbo.TaxCostPic28 WHERE InvoiceCode=@InvoiceCode AND InvoiceID=@InvoiceID";
                var result = Conn.Query<TaxCostPic28>(query, new { InvoiceCode = invoiceCode, InvoiceID = invoiceID }).SingleOrDefault();
                return result;
            }
        }

        public int UpdateMd5(TaxCostPic28 taxCostPic)
        {
            using (Conn)
            {
                string query = "UPDATE dbo.TaxCostPic28 SET PicMD5=@PicMD5 WHERE PicId =@PicId";
                return Conn.Execute(query, taxCostPic);
            }
        }
    }
}
