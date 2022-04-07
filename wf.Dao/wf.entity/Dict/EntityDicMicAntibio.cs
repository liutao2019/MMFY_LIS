using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 抗生素
    /// </summary>
    [Serializable]
    public class EntityDicMicAntibio : EntityBase
    {
        public EntityDicMicAntibio()
        {
            AntSortNo = 0;
            Checked = false;
        }
        /// <summary>
        /// 编码
        /// </summary>                       
        public String AntId { get; set; }

        /// <summary>
        /// 抗生素英文编码
        /// </summary>                       
        public String AntEname { get; set; }

        /// <summary>
        /// 抗生素名称
        /// </summary>                       
        public String AntCname { get; set; }

        /// <summary>
        /// 简码
        /// </summary>                       
        public String AntCode { get; set; }

        /// <summary>
        /// 敏感MIC
        /// </summary>                       
        public String AntStdUpperLimit { get; set; }

        /// <summary>
        /// 中介MIC
        /// </summary>                       
        public String AntStdMiddleLimit { get; set; }

        /// <summary>
        /// 耐药MIC
        /// </summary>                       
        public String AntStdLowerLimit { get; set; }

        /// <summary>
        /// Decimal:
        /// </summary>                       
        public Decimal AntZoneLowerLimit { get; set; }

        /// <summary>
        /// String:
        /// </summary>                       
        public Decimal AntZoneUpperLimit { get; set; }

        /// <summary>
        /// Int32:
        /// </summary>                       
        public Int32 AntFlag { get; set; }

        /// <summary>
        /// Z.R
        /// </summary>                       
        public String AntZoneDurgfast { get; set; }

        /// <summary>
        /// Zone中介
        /// </summary>                       
        public String AntZoneIntermed { get; set; }

        /// <summary>
        /// Z.S
        /// </summary>                       
        public String AntZoneSensitive { get; set; }

        /// <summary>
        /// 是否打印 0-不打印 1-打印
        /// </summary>                       
        public Int32 AntPirntFlag { get; set; }

        /// <summary>
        /// 使用方法
        /// </summary>                       
        public String AntMethod { get; set; }

        /// <summary>
        /// 血药浓度
        /// </summary>                       
        public String AntSerum { get; set; }

        /// <summary>
        /// 尿药浓度
        /// </summary>                       
        public String AntUrine { get; set; }

        /// <summary>
        /// 通道码
        /// </summary>                       
        public String AntMitNo { get; set; }

        /// <summary>
        /// WHO编码
        /// </summary>                       
        public String AntWhoNo { get; set; }

        /// <summary>
        /// 输入码
        /// </summary>                       
        public String AntCCode { get; set; }

        /// <summary>
        /// 拼音码
        /// </summary>                       
        public String AntPyCode { get; set; }

        /// <summary>
        /// 五笔码
        /// </summary>                       
        public String AntWbCode { get; set; }

        /// <summary>
        /// 序号
        /// </summary>                       
        public Int32 AntSortNo { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>                       
        public String AntDelFlag { get; set; }

        /// <summary>
        /// WHO编码(KB)
        /// </summary>                       
        public String AntKbWhoNo { get; set; }

        /// <summary>
        /// Clsi参考
        /// </summary>                       
        public String AntClsiComment { get; set; }
        /// <summary>
        /// String:
        /// </summary>
        public String AntNotes { get; set; }
        /// <summary>
        /// String
        /// </summary>
        public String AntWhoEtestNo { get; set; }

        /// <summary>
        /// 使用kb法时的whonet码
        /// </summary>
        public String AntKBWhoNo { get; set; }

        /// <summary>
        /// 抗生素类 关联dict_antibio_type.tp_id
        /// </summary>
        public String AntTypeId { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public String AntUnitName { get; set; }

        #region 附加字段 是否选中
        /// <summary>
        /// 是否选中
        /// </summary>
        public Boolean Checked { get; set; }
        #endregion

        #region 附加字段 统一ID
        /// <summary>
        /// 统一ID
        /// </summary>
        public String SpId
        {
            get
            {
                return AntId;
            }
        }
        #endregion
    }
}
