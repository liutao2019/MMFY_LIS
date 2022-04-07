using System;
using System.Collections.Generic;
using dcl.client.frame;
using lis.client.control;
using dcl.root.logon;
using dcl.entity;
using dcl.client.wcf;
using System.Linq;

namespace dcl.client.sample
{
    public partial class FrmSampMonitor : FrmCommon
    {
        //用于判断病人ID是否进行了统一设置
        string PatientIDNameConfirm;

        //新增条码来源*******************************************//
        private int[] barOrigin = new int[3];

        //********************************************************//
        public FrmSampMonitor()
        {
            InitializeComponent();
            bool showConfirmBar = UserInfo.HaveFunction(304);
           // this.pnlSecondSend.Visible = showConfirmBar;

            //****************************************************************************************//
            //新增代码


            //this.tsZY.IsOn = true;
            //this.tsMZ.IsOn = true;
            //this.tsTJ.IsOn = true;

            barOrigin[0] = 1;
            barOrigin[1] = 2;
            barOrigin[2] = 3;

            //注册三个事件，包括住院、门诊、体检三个复选框的勾选变化事件。
            this.tsZY.Toggled += new EventHandler(ImpatientCBox_CheckedChanged);
            this.tsMZ.Toggled += new EventHandler(OutPatientsCBox_CheckedChanged);
            this.tsTJ.Toggled += new EventHandler(TJ_CheckedChanged);
            //****************************************************************************************//

            //系统配置:条码监控几分钟为超时
            //string valueOutTime = dcl.client.common.ConfigHelper.GetSysConfigValueWithoutLogin("BCMonitor_Timeout");

            //if (valueOutTime == "20分钟")
            //{
            //   // label1.Text = "条码在同一状态滞留时间超过20分钟为超时";
            //}
        }

        private void tmMonitor_Tick(object sender, EventArgs e)
        {
            getBCPatients();
        }

        private void FrmBCMonitor_Load(object sender, EventArgs e)
        {
            sysToolBar.SetToolButtonStyle(new[] { sysToolBar.BtnRefresh.Name
            , sysToolBar.BtnClose.Name});
        }

        void btnRefresh_Click(object sender, EventArgs e)
        {
            //获取最新数据
            getBCPatients();
        }

        //获取不同的住院、体检、门诊的各个状态下的条码
        private void getBCPatients()
        {
            //ProxyBarcodePrint proxySign = new ProxyBarcodePrint();
            //DataTable ds = proxySign.GetBcPatientsBarMonitor(barOrigin.ToArray());
            string str = string.Empty;
            foreach (int item in barOrigin)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    str += ",";
                }
                str += item.ToString();
            }
            ProxySampProcessMonitor proxyProMoitor = new ProxySampProcessMonitor();
            List<EntitySampProcessMonitor> listProMoitor = proxyProMoitor.Service.GetBCPatients();
            List<EntitySampProcessMonitor> listSampCount = proxyProMoitor.Service.GetSampCount(EnumBarcodeOperationCode.SampleCollect);
            if (listSampCount.Count > 0)
            {
                List<EntitySampProcessMonitor> cache2 = GetBcPatientsFiltrateType(this.GetBcPatientsBarMonitor(listSampCount, str));
                gcCountCommon.DataSource = cache2;
            }
            if (listProMoitor.Count > 0)
            {
                List<EntitySampProcessMonitor> listBcPatients = listProMoitor;
                //DataTable cache = GetBcPatientsFiltrateType(this.GetBcPatientsBarMonitor(dtBcPatients, str));
                List<EntitySampProcessMonitor> cache = GetBcPatientsFiltrateType(this.GetBcPatientsBarMonitor(listBcPatients, str));
                //将数据利用rowFilter过滤器进行根据字段属性过滤到急查采集未收取框中
                gcUgent.DataSource = cache.FindAll(w=>w.SampStatusId==EnumBarcodeOperationCode.SampleCollect.ToString() && w.UrgentFlag==1 );

            }
            else
            {
                MessageDialog.ShowAutoCloseDialog("无条码数据可监控", 2);
                return;
            }

            
        }

        public List<EntitySampProcessMonitor> GetBcPatientsBarMonitor(List<EntitySampProcessMonitor> Cache, string bcOrigin)
        {
            List<EntitySampProcessMonitor> result = new List<EntitySampProcessMonitor>();

            string[] BarOrigin = bcOrigin.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            try
            {
                int[] barOriginStr = new int[3];
                for (int i = 0; i < BarOrigin.Length; i++)
                {
                    switch (BarOrigin[i])
                    {
                        case "1":
                            barOriginStr[i] = 108;
                            break;
                        case "2":
                            barOriginStr[i] = 107;
                            break;
                        case "3":
                            barOriginStr[i] = 109;
                            break;
                        default:
                            barOriginStr[i] = 111;
                            break;
                    }
                }

                //将所查询到的结果进行过滤筛选
                //DataRow[] rows = Cache.Select(string.Format("bc_ori_id in ('{0}','{1}','{2}')", barOriginStr[0], barOriginStr[1], barOriginStr[2]));
                result = Cache.Where(w=>w.PidSrcId=="108").ToList();

            }
            catch (Exception ex)
            {
                Logger.WriteException("FrmBCMonitor", "ThreadRefresh.GetBcPatientsBarMonitor", ex.Message);
            }
            return result;
        }
        /// <summary>
        /// 按物理组过滤条码信息
        /// </summary>
        /// <param name="Cache"></param>
        /// <returns></returns>
        private List<EntitySampProcessMonitor> GetBcPatientsFiltrateType(List<EntitySampProcessMonitor> Cache)
        {
            List<EntitySampProcessMonitor> result = new List<EntitySampProcessMonitor>();
            try
            {
                if (!ctypeCBox.Checked)//不按物理组过滤条码信息
                {
                    result = Cache;
                }
                else//按物理组过滤条码信息
                {
                    if (string.IsNullOrEmpty(txtPatType.displayMember))//如果为空不过滤
                    {
                        result = Cache;
                    }
                    else
                    {
                        //将所查询到的结果通过物理组过滤
                        result = Cache.Where(w => w.SampType == txtPatType.valueMember).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException("FrmBCMonitor", "ThreadRefresh.GetBcPatientsFiltrateType", ex.Message);
            }
            return result;
        }

        //根据住院复选框的勾选变化进行筛选
        private void ImpatientCBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.tsZY.IsOn == false)
            {
                barOrigin[0] = 4;
            }
            else
            {
                barOrigin[0] = 1;
            }
            getBCPatients();

        }

        //根据门诊复选框的勾选变化进行筛选
        private void OutPatientsCBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.tsMZ.IsOn == false)
            {
                barOrigin[1] = 4;
            }
            else
            {
                barOrigin[1] = 2;
            }
            getBCPatients();
        }

        //根据体检复选框的勾选变化进行筛选
        private void TJ_CheckedChanged(object sender, EventArgs e)
        {
            if (this.tsTJ.IsOn == false)
            {
                barOrigin[2] = 4;
            }
            else
            {
                barOrigin[2] = 3;
            }
            getBCPatients();
        }
        /// <summary>
        /// 启动根据物理组筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ctypeCBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ctypeCBox.Checked)
            {
                this.txtPatType.Enabled = true;
            }
            else
            {
                this.txtPatType.Enabled = false;
            }
            getBCPatients();
        }
        private void sysToolBar_OnBtnRefreshClicked(object sender, EventArgs e)
        {
            btnRefresh_Click(null, null);
        }
        private void sysToolBar_OnCloseClicked(object sender, EventArgs e)
        {
            Close();
        }
    }
}
