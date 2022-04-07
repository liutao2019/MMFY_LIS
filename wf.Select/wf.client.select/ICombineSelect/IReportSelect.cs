using System;
using System.Collections.Generic;
using System.Text;

namespace dcl.client.resultquery
{
    public abstract class IReportSelect
    {
        public virtual bool PlDateVisible
        {
            get { return true; }
        }

        public virtual bool PlSidVisible
        {
            get { return true; }
        }

        public virtual bool PlOrderVisible
        {
            get { return true; }
        }

        public virtual bool PlAgeVisible
        {
            get { return true; }
        }

        public virtual bool PlInstrmtVisible
        {
            get { return true; }
        }

        public virtual bool PlInstrmt2Visible
        {
            get { return true; }
        }

        public virtual bool PlDepartVisible
        {
            get { return true; }
        }

        public virtual bool PlIdTypeVisible
        {
            get { return true; }
        }

        public virtual bool PlIdVisible
        {
            get { return true; }
        }

        public virtual bool PlNameVisible
        {
            get { return true; }
        }

        public virtual bool PlSexVisible
        {
            get { return true; }
        }

        public virtual bool PlItemVisible
        {
            get { return true; }
        }

        public virtual bool PlBtypeVisible
        {
            get { return true; }
        }

        public virtual bool PlCtypeVisible
        {
            get { return true; }
        }

        public virtual bool PlSamPleVisible
        {
            get { return true; }
        }

        public virtual bool PlOriginVisible
        {
            get { return true; }
        }

        public virtual bool PlBarCodeVisible
        {
            get { return true; }
        }

        public virtual bool plTelVisible
        {
            get { return true; }
        }


        /// <summary>
        /// 是否显示检验科相关操作人
        /// </summary>
        public virtual bool plOpererVisible
        {
            get { return false; }
        }

        public virtual bool plBedVisible
        {
            get { return true; }
        }

        public virtual bool plPidVisible
        {
            get { return true; }
        }

        public virtual bool plDiagVisible
        {
            get { return true; }
        }

        public virtual bool PlInpatientMessageVisible
        {
            get { return true; }
        }

        public virtual bool PlYQVisible
        {
            get { return true; }
        }

        public virtual bool PlNo1AndiVisible
        {
            get { return true; }
        }

        public virtual bool PlNo2AndiVisible
        {
            get { return true; }
        }

        public virtual bool PlDoctorVisible
        {
            get { return true; }
        }

        public virtual bool PlChecktypeVisible
        {
            get { return true; }
        }

        public virtual bool SelectColumnsVisible
        {
            get
            {
                if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("SelectColumnsVisible") == "是")
                    return true;
                else
                    return false;
            }
        }

        public virtual System.Windows.Forms.DockStyle PanParDock
        {
            get { return System.Windows.Forms.DockStyle.Fill; }
        }

        public virtual int PanParHeight
        {
            get { return 170; }
        }

        public virtual System.Windows.Forms.DockStyle PlBottomDock
        {
            get { return System.Windows.Forms.DockStyle.Bottom; }
        }

        public virtual bool PlBottomVisible
        {
            get { return false; }
        }

        public virtual string[] ToolButton
        {
            get
            {
                return new string[] { "BtnSearch", "BtnSinglePrint", "btnReturn", "BtnClose" };
            }
        }

        public virtual bool DepartValidate
        {
            get
            {
                if (dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("checkDepartValidate") == "是")
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// 读取科室信息
        /// </summary>
        public virtual void readDepartInfo()
        {
            string departPath = dcl.client.common.PathManager.SettingLisPath + @"\reportSelectPower.xml";
        
            if (!System.IO.File.Exists(departPath))
                return;

        }


        private string depIdFilter = string.Empty;

        public virtual string DepIdFilter
        {
            get { return depIdFilter; }
            set { depIdFilter = value; }
        }
    }
}
