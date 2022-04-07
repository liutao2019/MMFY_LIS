using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public class EntityWhonetResult : EntityBase
    {
        /// <summary>
        /// 抗生素whonet码，3位
        /// </summary>
        public string AntiWhoCode { get; set; }

        /// <summary>
        /// 使用KB法时使用的whonet码
        /// </summary>
        public string KBAntiWhoCode { get; set; }

        /// <summary>
        /// kb法剂量/效价强度
        /// </summary>
        public string Potency { get; set; }

        /// <summary>
        /// 实验方法：KB/MIC/ETEST
        /// </summary>
        public string TestMethod { get; set; }

        private string _value;
        /// <summary>
        /// 数值结果
        /// </summary>
        public string Value
        {
            //get;
            //set;
            get
            {/*BETA_LACT 、BETA_LACT_NM、ESBL 、ESBL_NM、INDUC_CLI_NM显示为+、-*/
                if ((this.fullAntiWhoCode == "BETA_LACT"
                    || this.fullAntiWhoCode == "BETA_LACT_NM"
                    || this.fullAntiWhoCode == "ESBL_NM"
                    || this.fullAntiWhoCode == "INDUC_CLI_NM"
                    || this.fullAntiWhoCode == "ESBL")
                    && this._value == "Pos")
                { return "+"; }
                else if ((this.fullAntiWhoCode == "BETA_LACT"
                    || this.fullAntiWhoCode == "BETA_LACT_NM"
                    || this.fullAntiWhoCode == "ESBL_NM"
                    || this.fullAntiWhoCode == "INDUC_CLI_NM"
                    || this.fullAntiWhoCode == "ESBL")
                    && this._value == "Neg")
                { return "-"; }
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        /// <summary>
        /// 药敏结果SIR
        /// </summary>
        public string Result { get; set; }

        private string fullAntiWhoCode;
        /// <summary>
        /// 完整的whonet码
        /// </summary>
        public string FullAntiWhoCode
        {
            get
            {
                if (this.TestMethod.ToLower() == "mic")
                {
                    fullAntiWhoCode = this.AntiWhoCode + "_NM";
                    return fullAntiWhoCode;
                }
                else if (this.TestMethod.ToLower() == "kb")
                {
                    if (!string.IsNullOrEmpty(this.Potency)
                        && this.Potency.Trim() != string.Empty)
                    {
                        fullAntiWhoCode = this.AntiWhoCode + "_ND" + this.Potency;
                        return fullAntiWhoCode;
                    }
                    else if (!string.IsNullOrEmpty(this.KBAntiWhoCode))
                    {
                        fullAntiWhoCode = this.KBAntiWhoCode;
                        return fullAntiWhoCode;
                    }
                }
                else if (this.TestMethod.ToLower() == "etest")
                {
                    fullAntiWhoCode = this.AntiWhoCode + "_NE";
                    return fullAntiWhoCode;
                }
                return string.Empty;
            }
        }

        public EntityWhonetResult Clone()
        {
            EntityWhonetResult obj = this.MemberwiseClone() as EntityWhonetResult;
            return obj;
        }

        public override string ToString()
        {
            string str = string.Format("{0}|{2}", this.TestMethod, this.AntiWhoCode);
            return str;
        }

        /// <summary>
        /// 使用Etest法时使用的whonet码
        /// </summary>
        public string EtestAntiWhoCode { get; set; }
    }
}
