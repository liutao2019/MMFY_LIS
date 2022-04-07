using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using dcl.client.common;
using dcl.client.frame;
using dcl.common;


using dcl.client.wcf;
using System.Reflection;
using dcl.entity;
using System.Linq;

namespace dcl.client.oa
{
    public partial class FrmDialog : FrmCommon
    {
        /// <summary>
        /// true 为上班，false为下班
        /// </summary>
        private bool IsSWork = false;
        /// <summary>
        /// ture 为员工自己选择的班次，并插入到排班计划表中
        /// </summary>
        private bool IsNewDutyPlan = false;
        /// <summary>
        /// 班次信息表
        /// </summary>
        List<EntityOaDicShift> listDuty = new List<EntityOaDicShift>();
        /// <summary>
        /// 用户信息表
        /// </summary>
        List<EntitySysUser> listUsers = new List<EntitySysUser>() ;
        /// <summary>
        /// 排班计划表
        /// </summary>
        List<EntityOaDicShiftDetail> listDutyPlan = new List<EntityOaDicShiftDetail>();
        /// <summary>
        /// 班次ID
        /// </summary>
        string strDutyID = null;
        /// <summary>
        /// 解密后的密码
        /// </summary>
        string userPassword = null;

        public EntityOaDicShift SelectList { get; set; }

        private string strUserInfoID = "";


        public string UserInfoID
        {
            get { return strUserInfoID; }
        }

        public FrmDialog()
        {
            InitializeComponent();
        }

        public FrmDialog(bool flag)
        {
            InitializeComponent();
            this.IsSWork = flag;
        }

        private void FrmDialog_Load(object sender, EventArgs e)
        {
            sysToolBar1.SetToolButtonStyle(new string[] { "BtnConfirm", "BtnClose" });
            ProxyOaShiftDict proxy = new ProxyOaShiftDict();
            listDuty = proxy.Service.GetDutyData();
            listUsers = proxy.Service.GetUser("All");
            listDutyPlan = proxy.Service.GetDutyPlan(Convert.ToDateTime(DateTime.Now.ToString("d")), Convert.ToDateTime(DateTime.Now.ToString("d")), "All");
            //this.lueLoginID.Properties.DataSource = dtUsers;
            //this.lueDuty.Properties.DataSource = dtduty;

        }

        #region 单击确定按钮
        private void sysToolBar1_OnBtnConfirmClicked(object sender, EventArgs e)
        {
            try
            {
                #region 对输入的内容进行处理
                string strLoginID = txtLogin.Text.Trim();

                List<EntitySysUser> list = listUsers.Where(w => w.UserLoginid== strLoginID).ToList();
                if (list.Count < 1)
                {
                    lis.client.control.MessageDialog.Show("账号或者密码有误！请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    this.txtPassword.Text = "";
                    return;
                }
                strUserInfoID = list[0].UserId;
                userPassword = EncryptClass.Decrypt(list[0].UserPassword);
                string useType = list[0].UserSourceId;
                #endregion

                EntityOaWorkAttendance entity = new EntityOaWorkAttendance();

                ProxyAttendance proxy = new ProxyAttendance();


                #region 判断密码是否正确
                if (!txtPassword.Text.Equals(userPassword))
                {
                    lis.client.control.MessageDialog.Show("账号或者密码有误！请重新填写！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    this.txtPassword.Text = "";
                    //this.txtLogin.Text = "";
                    return;
                }
                #endregion

                string attdRecordID = proxy.Service.GetAttdRecordByUID(strUserInfoID + ";" + IsSWork.ToString().ToLower());

                //上班登记
                if (IsSWork)
                {
                    if (string.IsNullOrEmpty(attdRecordID))
                    {

                        if (SelectList != null)
                        {
                            strDutyID = SelectList.ShiftId;
                            #region 设置为新的插入到排班计划
                            EntityOaDicShiftDetail entityPlan = new EntityOaDicShiftDetail();
                            entityPlan.DetailDate = Convert.ToDateTime(DateTime.Now.ToString("d"));
                            entityPlan.DetailShiftId = strDutyID;
                            entityPlan.DetailUserId = strUserInfoID;

                            entity.AteStartDate = SelectList.ShiftStartDate;
                            entity.AteEndDate = SelectList.ShiftEndDate;
                            ProxyOaShiftDict proxyDict = new ProxyOaShiftDict();
                            int intRet = proxyDict.Service.InsertDutyPlan(entityPlan);


                            #endregion
                        }
                        else
                        {
                            #region 判断是否有排班计划，如果没有就按时间来判断班次并且插入到排班计划内

                            List<EntityOaDicShiftDetail> listplan = listDutyPlan.Where(w => w.DetailUserId==strUserInfoID && w.DetailDate.ToString()==DateTime.Now.ToString("d")).ToList();

                            if (listplan.Count < 1)//没有排班计划,从部门的班次中选中一个班次，并设置为新的插入到排班计划
                            {
                                List<EntityOaDicShift> listShift = new List<EntityOaDicShift>();
                                listShift = listDuty.Where(w => w.ShiftEndDate!=null && Convert.ToDateTime(w.ShiftEndDate) >= (DateTime.Now) && w.ShiftDeptId==useType).ToList();
                                if (listShift.Count >= 1)
                                {
                                    strDutyID = listDuty[0].ShiftId;

                                    #region 多班次，用户自己选择
                                    //DataTable t = dtduty.Clone();
                                    //foreach (DataRow drow in rows)
                                    //{
                                    //    t.Rows.Add(drow.ItemArray);
                                    //}
                                    //this.lueDuty.Properties.DataSource = t;
                                    //this.IsNewDutyPlan = true;
                                    #endregion

                                    #region 设置为新的插入到排班计划
                                    EntityOaDicShiftDetail entityPlan = new EntityOaDicShiftDetail();
                                    entityPlan.DetailDate = Convert.ToDateTime(DateTime.Now.ToString("d"));
                                    entityPlan.DetailShiftId = strDutyID;
                                    entityPlan.DetailUserId = strUserInfoID;

                                    entity.AteStartDate = listShift[0].ShiftStartDate;
                                    entity.AteEndDate = listShift[0].ShiftEndDate;
                                    ProxyOaShiftDict proxyDict = new ProxyOaShiftDict();
                                    int intRet = proxyDict.Service.InsertDutyPlan(entityPlan);
                                    #endregion

                                }
                                else
                                {
                                    //没有设置班次则系统默认8:00-12:00的上午班，14:30-17:30分的下午班，
                                    //然后中班跟夜班还是根据班次来显示
                                    //TODO 通用版需要在此处添加相关配置来确定时间段
                                    if (DateTime.Now.Hour < 12)
                                    {
                                        entity.AteShiftStartDate = "08:00:00";
                                        entity.AteShiftEndDate = "12:00:00";
                                    }
                                    else if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour < 14)
                                    {
                                        entity.AteShiftEndDate = "12:00:00";
                                        entity.AteShiftEndDate = "14:30:00";
                                    }
                                    else if (DateTime.Now.Hour >= 14 && DateTime.Now.Hour < 17)
                                    {
                                        entity.AteShiftEndDate = "14:30:00";
                                        entity.AteShiftEndDate = "17:30:00";
                                    }

                                    else if (DateTime.Now.Hour >= 17)
                                    {
                                        entity.AteShiftEndDate = "17:30:00";
                                        entity.AteShiftEndDate = "08:00:00";
                                    }
                                    //control.MessageDialog.Show(string.Format("对不起！你所在的物理组--{0}--目前没有设置班次内容！\n请到班次管理预设班次信息！", useType), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                                    //Close();
                                    //DialogResult = DialogResult.Abort;
                                    //return;
                                }
                            }
                            else//有排班计划，默认是第一个班次
                            {
                                string strDutys = "(";
                                for (int i = 0; i < listplan.Count - 1; i++)
                                {
                                    strDutys += "'" + listplan[i].DetailShiftId + "',";
                                }
                                strDutys += "'" + listplan[listplan.Count - 1].DetailShiftId+ "')";

                                List<EntityOaDicShift> listDic = listDuty.Where(w => w.ShiftId==strDutys).ToList();
                    
                                lueDuty.Properties.DataSource = listDic;

                                strDutyID = listplan[0].DetailShiftId;
                                entity.AteShiftStartDate = listDic[0].ShiftStartDate;
                                entity.AteShiftEndDate = listDic[0].ShiftEndDate;

                            }

                        }
                        lueDuty.EditValue = strDutyID;
                        #endregion

                        entity.AteId = proxy.Service.GetMaxAttdRecordID();

                        entity.AteDate = Convert.ToDateTime(DateTime.Now.ToString("d"));
                        entity.AteUserId = strUserInfoID;
                        entity.AteFlag = 1;
                        entity.AteShiftId = strDutyID;

                        entity.AteStartDate = DateTime.Now.ToString("HH:mm:ss");
                        //if (!string.IsNullOrEmpty(entity.attd_duty_sdate))
                        //{
                        //    entity.attd_exp = Convert.ToDateTime(entity.attd_sdate) > Convert.ToDateTime(entity.attd_duty_sdate) ? "迟到" : string.Empty;
                        //}
                        int intNum = proxy.Service.InsertAttdRecord(entity);

                        DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        lis.client.control.MessageDialog.Show(string.Format("对不起！您今天已上班登记并未下班登记，请先下班登记！"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        Close();
                        return;
                    }
                }
                //下班登记
                else
                {
                    if (string.IsNullOrEmpty(attdRecordID))
                    {
                        lis.client.control.MessageDialog.Show(string.Format("请先上班登记！"), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        Close();
                        return;
                    }


                    //entity.attd_edate = DateTime.Now.ToString("HH:mm:ss");
                    entity.AteId = attdRecordID;
                    //entity.attd_date = Convert.ToDateTime(DateTime.Now.ToString("d"));

                    int intRet = proxy.Service.ModifyAttdRecord(entity);
                    DialogResult = DialogResult.OK;

                }



                //proxy.Dispose();

                //this.Close();
                //this.sysToolBar1_OnCloseClicked(null, null);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                lis.client.control.MessageDialog.Show(string.Format("登记错误" + ex.Message), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }


        }

        #endregion

        #region 输入登陆ID
        private void txtLogin_EditValueChanged(object sender, EventArgs e)
        {
            #region 对输入的内容进行处理
            //string strLoginID = this.txtLogin.Text.Trim();

            //DataRow[] dr = dtUsers.Select(string.Format("loginId = '{0}'", strLoginID));
            //this.strUserInfoID = dr[0]["ID"].ToString();
            //userPassword = EncryptClass.Decrypt(dr[0]["password"].ToString());
            //string useType = dr[0]["typeSourceId"].ToString();
            #endregion

            //ProxyDutyDict proxy = new ProxyDutyDict();

            #region 判断是否有排班计划，如果没有就按时间来判断班次

            //DataRow[] rows = dtdutyplan.Select(string.Format("ddetail_user_id = '{0}'", strUserInfoID));

            //if (rows.Length < 1)//没有排班计划,从部门的班次中选中一个班次，并设置为新的插入到排班计划
            //{
            //    rows = dtduty.Select(string.Format("duty_edate >= '{0}' and duty_dept_id = '{1}'", DateTime.Now.ToString("HH:mm:ss"),useType), "duty_sdate ASC");
            //    if (rows.Length >= 1)
            //    {
            //        strDutyID = rows[0]["duty_id"].ToString();
            //        DataTable t = dtduty.Clone();
            //        foreach (DataRow drow in rows)
            //        {
            //            t.Rows.Add(drow.ItemArray);
            //        }
            //        this.lueDuty.Properties.DataSource = t;
            //        this.IsNewDutyPlan = true;
            //    }
            //    else
            //    {
            //        lis.client.control.MessageDialog.Show(string.Format("对不起！你所在的{0}目前没有设置班次内容！\n请到班次管理预设班次信息！",useType), "提示", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            //        this.Close();
            //    }
            //}
            //else//有排班计划，默认是第一个班次
            //{                
            //    string strDutys = "(";               
            //    for (int i = 0; i < rows.Length - 1; i++)
            //    {
            //        strDutys += "'" + rows[i]["ddetail_duty_id"].ToString() + "',";
            //    }
            //    strDutys += "'" + rows[rows.Length]["ddetail_duty_id"].ToString() + "')";

            //    DataRow[] drs = dtduty.Select(string.Format("duty_id in {0}", strDutys));
            //    DataTable dt = dtduty.Clone();
            //    foreach (DataRow r in drs)
            //    {
            //        dt.Rows.Add(r.ItemArray);
            //    }
            //    this.lueDuty.Properties.DataSource = dt;

            //    strDutyID = rows[0]["ddetail_duty_id"].ToString();
            //}

            //this.lueDuty.EditValue = strDutyID;
            #endregion

            // proxy.Dispose();
        }
        #endregion

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.sysToolBar1_OnBtnConfirmClicked(null, null);
                //this.Close();
            }

        }

        private void sysToolBar1_OnCloseClicked_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmDialog_Shown(object sender, EventArgs e)
        {
            txtLogin.Focus();
        }

    }
}
