using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace dcl.entity
{
    /// <summary>
    /// 系统操作状态枚举类，条码部分需要跟bc_status表同步
    /// </summary>
    public class EnumBarcodeOperationCode
    {
        /// <summary>
        /// 删除条码明细
        /// </summary>
        [FieldNamingAttribute(Desc = "删除条码明细")]
        public const int DeleteDetail = 500;

        /// <summary>
        /// 删除条码
        /// </summary>
        [FieldNamingAttribute(Desc = "删除条码")]
        public const int DeleteBarcode = 510;

        /// <summary>
        /// 移除组合
        /// </summary>
        [FieldNamingAttribute(Desc = "移除组合")]
        public const int RemoveCombine = 520;

        /// <summary>
        /// 查看危急值报告
        /// </summary>
        [FieldNamingAttribute(Desc = "查看危急值报告")]
        public const int ViewCirticalReport = 540;

        /// <summary>
        /// 查看急查报告
        /// </summary>
        [FieldNamingAttribute(Desc = "查看急查报告")]
        public const int ViewUrgentReport = 550;

        /// <summary>
        /// 重置预置条码
        /// </summary>
        [FieldNamingAttribute(Desc = "重置预置条码")]
        public const int ResetPrePlaceBarcode = 570;

        /// <summary>
        /// 追加条码
        /// </summary>
        [FieldNamingAttribute(Desc = "追加条码")]
        public const int AppendBarcode = 560;

        /// <summary>
        /// 删除病人资料
        /// </summary>
        [FieldNamingAttribute(Desc = "删除病人资料")]
        public const int DeletePatient = 530;

        /// <summary>
        /// 条码生成
        /// </summary>
        [FieldNamingAttribute(Desc = "条码生成")]
        public const int BarcodeGenerate = 0;

        /// <summary>
        /// 条码打印
        /// </summary>
        [FieldNamingAttribute(Desc = "条码打印")]
        public const int BarcodePrint = 1;

        /// <summary>
        /// 标本采集
        /// </summary>
        [FieldNamingAttribute(Desc = "标本采集")]
        public const int SampleCollect = 2;

        /// <summary>
        /// 标本收取/标本送检
        /// </summary>
        [FieldNamingAttribute(Desc = "标本收取")]
        public const int SampleSend = 3;

        /// <summary>
        /// 标本送达
        /// </summary>
        [FieldNamingAttribute(Desc = "标本送达")]
        public const int SampleReach = 4;

        /// <summary>
        /// 标本签收
        /// </summary>
        [FieldNamingAttribute(Desc = "标本签收")]
        public const int SampleReceive = 5;

        /// <summary>
        /// 离心
        /// </summary>
        [FieldNamingAttribute(Desc = "离心")]
        public const int Centrifugate = 6;

        /// <summary>
        /// 耗材领取
        /// </summary>
        [FieldNamingAttribute(Desc = "耗材领取")]
        public const int Ren = 121;

        /// <summary>
        /// 标本上机
        /// </summary>
        [FieldNamingAttribute(Desc = "标本上机")]
        public const int InLab = 7;

        /// <summary>
        /// 标本二次送检
        /// </summary>
        [FieldNamingAttribute(Desc = "标本二次送检")]
        public const int SampleSecondSend = 8;

        /// <summary>
        /// 标本回退
        /// </summary>
        [FieldNamingAttribute(Desc = "标本回退")]
        public const int SampleReturn = 9;

        /// <summary>
        /// 标本登记
        /// </summary>
        [FieldNamingAttribute(Desc = "标本登记")]
        public const int SampleRegister = 20;


        /// <summary>
        /// 标本上机
        /// </summary>
        [FieldNamingAttribute(Desc = "标本上机")]
        public const int OperateOnMachine = 30;

        //**新增操作属性************************************************/

        ///<summary>
        ///修改病人结果
        ///</summary>
        [FieldNamingAttribute(Desc = "修改病人结果")]
        public const int ModifyPatResult = 35;


        ///<summary>
        ///预审
        ///</summary>
        [FieldNamingAttribute(Desc = "预审")]
        public const int PreAudit = 39;

        ///<summary>
        ///取消预审
        ///</summary>
        [FieldNamingAttribute(Desc = "取消预审")]
        public const int UndoPreAudit = 38;

        //**************************************************************/

        /// <summary>
        /// 资料审核
        /// </summary>
        [FieldNamingAttribute(Desc = "资料审核")]
        public const int Audit = 40;

        /// <summary>
        /// 资料取消审核
        /// </summary>
        [FieldNamingAttribute(Desc = "资料取消审核")]
        public const int UndoAudit = 50;

        /// <summary>
        /// 资料报告
        /// </summary>
        [FieldNamingAttribute(Desc = "资料报告")]
        public const int Report = 60;

        /// <summary>
        /// 资料取消报告
        /// </summary>
        [FieldNamingAttribute(Desc = "资料取消报告")]
        public const int UndoReport = 70;

        /// <summary>
        /// 报告打印
        /// </summary>
        [FieldNamingAttribute(Desc = "报告打印")]
        public const int ReportPrint = 100;

        public static string GetNameByCode(string code)
        {
            string ret = null;

            FieldInfo[] fields = typeof(EnumBarcodeOperationCode).GetFields();
            FieldInfo foundfield = null;

            foreach (FieldInfo item in fields)
            {
                object val = item.GetValue(null);

                if (val != null && val.ToString() == code)
                {
                    foundfield = item;
                    break;
                }
            }

            if (foundfield != null)
            {
                FieldNamingAttribute[] attrs = foundfield.GetCustomAttributes(typeof(FieldNamingAttribute), true) as FieldNamingAttribute[];

                if (attrs != null && attrs.Length > 0)
                {
                    ret = attrs[0].Desc;
                }
            }

            return ret;
        }

        class FieldNamingAttribute : Attribute
        {
            public string Desc { get; set; }
        }
    }
}
