using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 打印参数实体
    /// </summary>
    [Serializable]
    public class EntityDCLPrintParameter : EntityBase
    {
        public EntityDCLPrintParameter()
        {
            ListBarId = new List<String>();
            listSampSn = new List<String>();
            ListRepId = new List<String>();
            CustomParameter = new Dictionary<String, Object>();
            ListMicRepId = new List<string>();
            ListRackBarCode = new List<string>();
        }

        /// <summary>
        /// 报表代码
        /// </summary>
        public String ReportCode { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public Int32 Sequence { get; set; }

        /// <summary>
        /// 病人标识ID
        /// </summary>
        public String RepId { get; set; }

        /// <summary>
        /// 病人姓名 A4连续打印用于分组使用
        /// </summary>
        public String PatName { get; set; }

        /// <summary>
        /// 病人科室名称 A4连续打印用于分组使用
        /// </summary>
        public String PatDepName { get; set; }

        /// <summary>
        /// 病人标识ID（多用于打印清单）
        /// </summary>
        public List<String> ListRepId { get; set; }

        /// <summary>
        /// 条码号集合
        /// </summary>
        public List<String> ListBarId { get; set; }

        /// <summary>
        /// 条码号集合
        /// </summary>
        public List<String> ListReagentBarId { get; set; }

        /// <summary>
        /// 架子条码号集合
        /// </summary>
        public List<string> ListRackBarCode { get; set; }

        /// <summary>
        /// 条码主键
        /// </summary>
        public List<String> listSampSn { get; set; }

        /// <summary>
        /// 自定义参数
        /// </summary>
        public Dictionary<String, Object> CustomParameter { get; set; }





        /// <summary>
        /// 报告主id(微生物专用,检验系统不能使用)
        /// </summary>
        public String MicRepId { get; set; }


        /// <summary>
        /// 病人主表主键(微生物专用,检验系统不能使用)
        /// </summary>
        public String MicRepPatKey { get; set; }

        /// <summary>
        /// 样本号(微生物专用,检验系统不能使用)
        /// </summary>
        public String MicRepSid { get; set; }

        /// <summary>
        /// 微生物所属科室,对应主表(可能是deptcode也可能是deptid)
        /// </summary>
        public String MicDept { get; set; }


        /// <summary>
        /// 病人标识ID(微生物专用,检验系统不能使用)
        /// </summary>
        public List<string> ListMicRepId { get; set; }

    }
}
