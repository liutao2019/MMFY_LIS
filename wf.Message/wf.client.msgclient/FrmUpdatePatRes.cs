using System;
using System.Windows.Forms;
using dcl.entity;

namespace dcl.client.msgclient
{
    public partial class FrmUpdatePatRes : Form
    {
        //public DataRow drUpdate = null;
        public EntityPidReportMain eyUpdate = null;

        private string pat_id { get; set; }

        //public FrmUpdatePatRes(DataRow dr)
        public FrmUpdatePatRes(EntityPidReportMain ey)
        {
            InitializeComponent();
            eyUpdate = ey;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void FrmUpdatePatRes_Load(object sender, EventArgs e)
        {
            if (eyUpdate != null)
            {
                //pat_id = drUpdate["pat_id"].ToString();
                //txtName.Text = drUpdate["pat_name"].ToString();
                pat_id = eyUpdate.RepId;
                txtName.Text = eyUpdate.PidName;

                //rtbText.Text = drUpdate["pat_res2"].ToString();
                rtbText.Text = eyUpdate.PidRes2;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(pat_id))
            {
                string strmsg = rtbText.Text;

                if (string.IsNullOrEmpty(strmsg) || strmsg.Trim().Length <= 0)
                {
                    MessageBox.Show("请填写处理意见");
                    rtbText.Focus();
                    return;
                }

                try
                {
                    //AuditInfo CheckerInfo = null;
                    EntityAuditInfo CheckerInfo = null;
                    //验证用户
                    FrmReadAffirm frmRA = null;

                    frmRA = new FrmReadAffirm();
                    frmRA.SetDocVisiable(false);
                    frmRA.IsCheckPass = true;
                    if (frmRA.ShowDialog() != DialogResult.Yes)
                    {
                        return;
                    }
                    CheckerInfo = frmRA.m_userInfo;

                    string[] strcolname = new string[2];
                    string[] colValue = new string[2];

                    strcolname[0] = "msg_content2";
                    colValue[0] = "'" + strmsg.Trim() + "'";

                    strcolname[1] = "msg_againlog_id";
                    colValue[1] = "'" + CheckerInfo.UserId + "'";

                    //保存病人扩展资料
                    // EntityOperationResult eoPatExt = PatientCRUDClient.NewInstance.AddOrUpdatePatientExt(strcolname, colValue, pat_id);
                    this.DialogResult = DialogResult.Yes;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

        }
    }
}
