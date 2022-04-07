using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace dcl.entity
{
    [Serializable]
    public class EntityPatientsMi_4Barcode
    {
        /// <summary>
        /// 是否被选中(界面调用)
        /// </summary>
        public bool selected { get; set; }

        /// <summary>
        /// 病人ID
        /// </summary>
        public string pat_id { get; set; }

        /// <summary>
        /// 检验组合ID
        /// </summary>
        public string pat_com_id { get; set; }

        /// <summary>
        /// 检验组合名称
        /// </summary>
        public string pat_com_name { get; set; }

        /// <summary>
        /// 组合HIS编码
        /// </summary>
        public string pat_his_code { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public decimal? pat_com_price { get; set; }

        /// <summary>
        /// 医嘱ID
        /// </summary>
        public string pat_yz_id { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int pat_seq { get; set; }

        /// <summary>
        /// 申请日期
        /// </summary>
        public DateTime? com_occ_date { get; set; }

        /// <summary>
        /// 送检科室
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 送检科室编码
        /// </summary>
        public string DeptCode { get; set; }

        /// <summary>
        /// 送检医生
        /// </summary>
        public string DoctName { get; set; }

        /// <summary>
        /// 送检医生编码
        /// </summary>
        public string DoctID { get; set; }

        /// <summary>
        /// 临床诊断
        /// </summary>
        public string PatDiag { get; set; }

        /// <summary>
        /// 允许选中
        /// </summary>
        public Boolean AllowSelect { get; set; }

        /// <summary>
        /// 条码登记标志
        /// </summary>
        public int bc_flag { get; set; }


        public String pat_bar_code { get; set; }

        public bool CanSelect { get; set; }

        public string Description { get; set; }

        public EntityPatientsMi_4Barcode()
        {
            this.pat_seq = 0;
            this.pat_com_price = null;
            this.pat_id = string.Empty;
            this.pat_com_id = string.Empty;
            this.pat_com_name = string.Empty;
            this.pat_his_code = string.Empty;
            this.pat_yz_id = string.Empty;
            this.bc_flag = 0;
            this.selected = false;
            this.AllowSelect = true;
            this.pat_bar_code = string.Empty;
        }
    }
}
