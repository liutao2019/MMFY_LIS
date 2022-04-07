using System;
using System.Collections.Generic;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.wcf;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.msgsend
{
    public partial class FrmshowhispatView : FrmCommon
    {
        public FrmshowhispatView()
        {
            InitializeComponent();
        }

        public FrmAddDIYMsg frmadd = null;

        public List<EntityPidReportMain> dthispat = null;

        public FrmshowhispatView(FrmAddDIYMsg frmaddtemp, List<EntityPidReportMain> dttemp)
        {
            InitializeComponent();
            frmadd = frmaddtemp;
            dthispat = dttemp;
        }

        private void FrmshowhispatView_Load(object sender, EventArgs e)
        {
            try
            {
                List<EntityDicDoctor> temp_dtDoc = CacheClient.GetCache<EntityDicDoctor>();
                //加载科室信息
                List<EntityDicPubDept> dttemp_deptinfo = CacheClient.GetCache<EntityDicPubDept>();

                if (dthispat != null && dthispat.Count > 0)
                {

                    for (int i = 0; i < dthispat.Count; i++)
                    {
                        if (temp_dtDoc != null && temp_dtDoc.Count > 0)
                        {
                            List<EntityDicDoctor> list_tempSel = temp_dtDoc.FindAll(r => r.DoctorCode == dthispat[i].PidDoctorCode.ToString());
                            if (list_tempSel != null && list_tempSel.Count > 0)
                            {
                                dthispat[i].PidDocName = list_tempSel[0].DoctorName;
                            }
                        }

                        if (dttemp_deptinfo != null && dttemp_deptinfo.Count > 0)
                        {
                            List<EntityDicPubDept> list_tempSel = dttemp_deptinfo.FindAll(r => r.DeptCode == dthispat[i].PidDeptCode.ToString());
                            if (list_tempSel != null && list_tempSel.Count > 0)
                            {
                                dthispat[i].DeptName = list_tempSel[0].DeptName;
                            }
                        }

                        if (dthispat[i].PidSrcId.ToString().Length > 0)
                        {
                            if (dthispat[i].PidSrcId.ToString() == "107")
                            {
                                dthispat[i].SrcName = "门诊";
                            }
                            else if (dthispat[i].PidSrcId.ToString() == "108")
                            {
                                dthispat[i].SrcName = "住院";
                            }
                        }
                    }

                    gcLookData.DataSource = dthispat;
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                MessageBox.Show(ex.Message);
            }
        }

        private void gvLookData_DoubleClick(object sender, EventArgs e)
        {
            if (gcLookData.DataSource != null)
            {
                EntityPidReportMain dr = this.gvLookData.GetFocusedRow() as EntityPidReportMain;
                if (dr != null)
                {
                    if (frmadd != null)
                    {
                        frmadd.drhispatParam = dr;
                    }
                }
                this.Close();
            }
        }
    }
}
