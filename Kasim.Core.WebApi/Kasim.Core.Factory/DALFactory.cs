﻿/**
 *                             _ooOoo_
 *                            o8888888o
 *                            88" . "88
 *                            (| -_- |)
 *                            O\  =  /O
 *                         ____/`---'\____
 *                       .'  \\|     |//  `.
 *                      /  \\|||  :  |||//  \
 *                     /  _||||| -:- |||||-  \
 *                     |   | \\\  -  /// |   |
 *                     | \_|  ''\---/''  |   |
 *                     \  .-\__  `-`  ___/-. /
 *                   ___`. .'  /--.--\  `. . __
 *                ."" '<  `.___\_<|>_/___.'  >'"".
 *               | | :  `- \`.;`\ _ /`;.`/ - ` : | |
 *               \  \ `-.   \_ __\ /__ _/   .-` /  /
 *          ======`-.____`-.___\_____/___.-`____.-'======
 *                             `=---='
 *          ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
 *                     佛祖保佑        永无BUG
 *            佛曰:
 *                   写字楼里写字间，写字间里程序员；
 *                   程序人员写程序，又拿程序换酒钱。
 *                   酒醒只在网上坐，酒醉还来网下眠；
 *                   酒醉酒醒日复日，网上网下年复年。
 *                   但愿老死电脑间，不愿鞠躬老板前；
 *                   奔驰宝马贵者趣，公交自行程序员。
 *                   别人笑我忒疯癫，我笑自己命太贱；
 *                   不见满街漂亮妹，哪个归得程序员？
*/
/*----------------------------------------------------------------
** Copyright (C) 2017 
**
** file：DALFactory
** desc：
** 
** auth：KasimYe (KASIM)
** date：2017-12-05 14:38:38
**
** Ver.：V1.0.0
**----------------------------------------------------------------*/

using Kasim.Core.IDAL;
using Kasim.Core.IDAL.ConsoleApp;
using Kasim.Core.IDAL.WebApi;
using Kasim.Core.Model;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Kasim.Core.Factory
{
    public class DALFactory
    {
        private static readonly string _path = "Kasim.Core.SQLServerDAL";
        private DALFactory()
        {

        }

        public static ITaxCostPic28DAL CreateTaxCostPic28DAL()
        {
            string className = _path + ".WebApi.TaxCostPic28DAL";
            return (ITaxCostPic28DAL)Assembly.Load(_path).CreateInstance(className);
        }

        public static ISupplierReturnDAL CreateSupplierReturnDAL()
        {
            string className = _path + ".ConsoleApp.SupplierReturnDAL";
            return (ISupplierReturnDAL)Assembly.Load(_path).CreateInstance(className);
        }

        public static IProductOfferDAL CreateProductOfferDAL()
        {
            string className = _path + ".WebApi.ProductOfferDAL";
            return (IProductOfferDAL)Assembly.Load(_path).CreateInstance(className);
        }
    }

    public class DALFactory<T> where T: IBaseDAL<BaseEntity>
    {
        static public T CreateDAL(string _path, string _className)
        {
            string className = string.Format("{0}.{1}", _path, _className);
            return (T)Assembly.Load(_path).CreateInstance(className);
        }        
    }
}
