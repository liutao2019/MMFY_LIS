using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.IO;

namespace dcl.entity
{
    public class EntityLocalSetting
    {
        /// <summary>
        /// 默认医区ID
        /// </summary>
        public string HospitalID { get; set; }

        /// <summary>
        /// 默认医区名称
        /// </summary>
        public string HospitalName { get; set; }

        /// <summary>
        /// 默认检验物理组ID
        /// </summary>
        public string CType_id { get; set; }

        /// <summary>
        /// 默认检验物理组名称
        /// </summary>
        public string CType_name { get; set; }

        /// <summary>
        /// 病区ID
        /// </summary>
        public string DeptID { get; set; }

        /// <summary>
        /// 病区名称
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 默认检验仪器ID
        /// </summary>
        public string Itr_id { get; set; }

        /// <summary>
        /// 条码执行科室Id
        /// </summary>
        public string Barcode_Dep_id { get; set; }

        /// <summary>
        /// 条码执行科室名称
        /// </summary>
        public string Barcode_Dep_name { get; set; }

        /// <summary>
        ///  当前计算机备注
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否启动危急值内部提醒 0为false 1为true
        /// </summary>
        public string IsUrgentNotity { get; set; }

        /// <summary>
        /// 是否启动仪器危急值提醒 0为false 1为true
        /// </summary>
        public string IsItrUrgentNotity { get; set; }

        /// <summary>
        /// 是否启动质控提醒 0为false 1为true
        /// </summary>
        public string IsQCNotify { get; set; }

        /// <summary>
        /// 是否启动TAT提醒 0为false 1为true
        /// </summary>
        public string IsTATNotify { get; set; }

        /// <summary>
        /// 是否启动条码TAT提醒 0为false 1为true
        /// </summary>
        public string IsBcTATNotify { get; set; }

        /// <summary>
        /// 仪器ID集合
        /// </summary>
        public string ItrIDList { get; set; }

        /// <summary>
        /// 物理组ID集合
        /// </summary>
        public string TypeIDList { get; set; }

        /// <summary>
        /// 物理组ID集合(签收地点)
        /// </summary>
        public string QsTypeIDList { get; set; }

        /// <summary>
        /// 物理组ID集合(送达地点)
        /// </summary>
        public string SdTypeIDList { get; set; }

        /// <summary>
        /// 仪器ID
        /// </summary>
        public string LocalItrID { get; set; }

        /// <summary>
        /// 报告名称
        /// </summary>
        public string ReportWord { get; set; }

        /// <summary>
        /// 采血窗口
        /// </summary>
        public string BloodWindow { get; set; }

        /// <summary>
        /// 采血区域
        /// </summary>
        public string BloodArea { get; set; }

        /// <summary>
        /// 审核名称
        /// </summary>
        public string AuditWord { get; set; }


        public string IDTypeFlag { get; set; }

        /// <summary>
        /// 常规检验双列显示方式 0 代码 1 名称
        /// </summary>
        public string LabResultShowType { get; set; }

        /// <summary>
        /// 完整描述 物理组+备注
        /// </summary>
        public string FullDescription
        {
            get
            {
                string val = this.CType_name + " " + this.Description;

                if (string.IsNullOrEmpty(CType_name)
                    || string.IsNullOrEmpty(Description)
                    )
                {
                    val = val.Trim();
                }

                return val;
            }
        }

        public EntityLocalSetting()
        {

        }

        /// <summary>
        /// 门诊默认标本过滤信息
        /// </summary>
        public string MzDefaultSam { get; set; }


        /// <summary>
        /// 审核密码缓存时间
        /// </summary>
        public string CachePwTime { get; set; }

    }
}
