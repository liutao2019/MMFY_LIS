using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.wcf;
using System.IO;
using System.Xml;
using dcl.entity;
using System.Linq;

namespace dcl.client.oa
{
    public partial class ucHandOverLayout : UserControl
    {
        public ucHandOverLayout()
        {
            InitializeComponent();
            txthr_itr_fault_id_onBeforeFilter();
        }

        string CtypeID = string.Empty;

        /// <summary>
        /// 接收后未审核的标本信息
        /// </summary>
        public DataTable dtNullRes { get; set; }

        /// <summary>
        /// 是否保存接收后未审核的标本信息
        /// </summary>
        public bool SaveDtNullRes { get; set; }

        public void SetLayout(string typeid)
        {
            LayoutControlConfigForHand layconfig = new LayoutControlConfigForHand(layoutControl1, this.GetType().Name);
            layconfig.AllowCustomize = true;
            layconfig.TypeID = typeid;
            CtypeID = typeid;
            layconfig.ApplyConfig();
        }

        public void Init()
        {
            txthr_hand_code.valueMember = UserInfo.loginID;
            txthr_hand_code.displayMember = UserInfo.userName;
            txthr_receive_code.valueMember = "";
            txthr_receive_code.ClearSelect();
            txthr_qc_reason.Text = null;
            txthr_ext1.Text = null;
            txthr_ext2.Text = null;
            txthr_ext3.Text = null;
            txthr_ext4.Text = null;
            txthr_ext5.Text = null;
            txthr_ext6.Text = null;
            txthr_ext7.Text = null;
            txthr_ext8.Text = null;
            txthr_ext9.Text = null;
            txthr_hand_time.EditValue = DateTime.Now;
            txthr_handcomfirm_code.ClearSelect();
            txthr_recvconfirm_code.ClearSelect();
            txthr_report_count.Text = null;
            txthr_san_microbe.Text = null;
            txthr_san_room.Text = null;
            txthr_sp_complain.Text = null;
            txthr_sp_hydro.Text = null;
            txthr_sp_ifsam.Text = null;
            txthr_sp_machine.Text = null;
            txthr_totalRecv_count.Text = null;

            txthr_unreport_count.Text = null;
            txthr_urgent_count.Text = null;
            txthr_itr_fault_reason.Text = null;
            txthr_itr_fault_id.valueMember = null;
            txthr_itr_fault_id.ClearSelect();

            txthr_itr_fault_time.EditValue = null;
            txthr_itr_judge.Text = null;

            chkhr_itr_mflag.EditValue = null;
            txthr_noqutity_count.Text = null;
            chkhr_qc_flag.EditValue = null;


            if (layoutControlItem3.Visibility == DevExpress.XtraLayout.Utils.LayoutVisibility.Always)
            {
                EntityHoRecord info = new ProxyOaHoRecord().Service.GetHandoverStat(DateTime.Now.Date, DateTime.Now, CtypeID);

                if (info != null)
                {
                    txthr_noqutity_count.Text = info.HrNoqutityCount;
                    txthr_urgent_count.Text = info.HrUrgentCount;
                    txthr_totalRecv_count.Text = info.HrTotalRecvCount;
                    txthr_report_count.Text = info.HrReportCount;
                    txthr_unreport_count.Text = info.HrUnreportCount;

                    //接收后未审核的标本
                    if (!string.IsNullOrEmpty(info.HrExt9))
                    {
                        DataTable dtTemp = null;
                        if (ConvertXmlToDatatable(info.HrExt9, "dtNullRes", out dtTemp))
                        {
                            dtNullRes = dtTemp;
                        }
                    }
                }
            }
        }


        public void BindToUI(EntityHoRecord info)
        {
            txthr_hand_code.valueMember = info.HrHandCode;
            txthr_hand_code.SelectByID(info.HrHandCode);
            txthr_receive_code.valueMember = info.HrReceiveCode;
            txthr_receive_code.SelectByID(info.HrReceiveCode);
            txthr_qc_reason.Text = info.HrQcReason;
            txthr_ext1.Text = info.HrExt1;
            txthr_ext2.Text = info.HrExt2;
            txthr_ext3.Text = info.HrExt3;
            txthr_ext4.Text = info.HrExt4;
            txthr_ext5.Text = info.HrExt5;
            txthr_ext6.Text = info.HrExt6;
            txthr_ext7.Text = info.HrExt7;
            txthr_ext8.Text = info.HrExt8;
            //txthr_ext9.Text = info.hr_ext9;
            txthr_hand_time.EditValue = info.HrHandTime;
            txthr_handcomfirm_code.SelectByID(info.HrHandcomfirmCode);
            txthr_recvconfirm_code.SelectByID(info.HrRecvconfirmCode);
            txthr_report_count.Text = info.HrReportCount;
            txthr_san_microbe.Text = info.HrSanMicrobe;
            txthr_san_room.Text = info.HrSanRoom;
            txthr_sp_complain.Text = info.HrSpComplain;
            txthr_sp_hydro.Text = info.HrSpHydro;
            txthr_sp_ifsam.Text = info.HrSpIfsam;
            txthr_sp_machine.Text = info.HrSpMachine;
            txthr_totalRecv_count.Text = info.HrTotalRecvCount;

            txthr_unreport_count.Text = info.HrUnreportCount;
            txthr_urgent_count.Text = info.HrUrgentCount;
            txthr_itr_fault_reason.Text = info.HrItrFaultReason;
            txthr_itr_fault_id.valueMember = info.HrItrFaultId;
            txthr_itr_fault_id.SelectByID(info.HrItrFaultId);
            if (info.HrItrFaultTime.HasValue)
                txthr_itr_fault_time.EditValue = info.HrItrFaultTime;
            txthr_itr_judge.Text = info.HrItrJudge;

            chkhr_itr_mflag.EditValue = info.HrItrMflag;
            txthr_noqutity_count.Text = info.HrNoqutityCount;
            chkhr_qc_flag.EditValue = info.HrQcFlag;
        }

        public EntityHoRecord GetDataFormUI()
        {
            EntityHoRecord info = new EntityHoRecord();

            info.HrHandCode = txthr_hand_code.valueMember;
            info.HrReceiveCode = txthr_receive_code.valueMember;
            info.HrQcReason = txthr_qc_reason.Text;
            info.HrExt1 = txthr_ext1.Text;
            info.HrExt2 = txthr_ext2.Text;
            info.HrExt3 = txthr_ext3.Text;
            info.HrExt4 = txthr_ext4.Text;
            info.HrExt5 = txthr_ext5.Text;
            info.HrExt6 = txthr_ext6.Text;
            info.HrExt7 = txthr_ext7.Text;
            info.HrExt8 = txthr_ext8.Text;
            //info.hr_ext9 = txthr_ext9.Text;

            if (dtNullRes != null && dtNullRes.Rows.Count > 0 && SaveDtNullRes)
            {
                info.HrExt9 = dtDataToXml(dtNullRes);
            }

            info.HrHandTime = (DateTime)txthr_hand_time.EditValue;
            info.HrHandcomfirmCode = txthr_handcomfirm_code.valueMember;
            info.HrRecvconfirmCode = txthr_recvconfirm_code.valueMember;
            info.HrReportCount = txthr_report_count.Text;
            info.HrSanMicrobe = txthr_san_microbe.Text;
            info.HrSanRoom = txthr_san_room.Text;
            info.HrSpComplain = txthr_sp_complain.Text;
            info.HrSpHydro = txthr_sp_hydro.Text;
            info.HrSpIfsam = txthr_sp_ifsam.Text;
            info.HrSpMachine = txthr_sp_machine.Text;
            info.HrTotalRecvCount = txthr_totalRecv_count.Text;

            info.HrUnreportCount = txthr_unreport_count.Text;
            info.HrUrgentCount = txthr_urgent_count.Text;
            info.HrItrFaultReason = txthr_itr_fault_reason.Text;
            info.HrItrFaultId = txthr_itr_fault_id.valueMember;
            if (txthr_itr_fault_time.EditValue != null)
                info.HrItrFaultTime = (DateTime)txthr_itr_fault_time.EditValue;
            info.HrItrJudge = txthr_itr_judge.Text;

            info.HrItrMflag = chkhr_itr_mflag.Checked ? "1" : "0";
            info.HrNoqutityCount = txthr_noqutity_count.Text;
            info.HrQcFlag = chkhr_qc_flag.Checked ? "1" : "0";
            return info;
        }


        public void SetHandConfirm(string code)
        {
            txthr_handcomfirm_code.SelectByID(code);
            //txthr_recvconfirm_code.SelectByID(info.hr_recvconfirm_code);
        }

        public void SetReciveConfirm(string code)
        {
            //txthr_handcomfirm_code.SelectByID(code);
            txthr_recvconfirm_code.SelectByID(code);
        }

        public void SetCustomeText(string text)
        {
            //txthr_handcomfirm_code.SelectByID(code);
            groupControlItr.Text = text;
        }


        public int GetLayoutHeight()
        {
            return layoutControl1.Height + 20;
        }


        public void SetDictData(List<EntityDicPubEvaluate> listEva)
        {
            foreach (EntityDicPubEvaluate eva in listEva)
            {
                if (eva.EvaFlag == "14")
                {
                    txthr_san_microbe.Properties.Items.Add(eva.EvaContent.Trim());
                    txthr_san_room.Properties.Items.Add(eva.EvaContent.Trim());
                    txthr_sp_complain.Properties.Items.Add(eva.EvaContent.Trim());
                    txthr_sp_hydro.Properties.Items.Add(eva.EvaContent.Trim());
                    txthr_sp_ifsam.Properties.Items.Add(eva.EvaContent.Trim());
                    txthr_sp_machine.Properties.Items.Add(eva.EvaContent.Trim());
                }
            }
        }

        private void txthr_itr_fault_id_onBeforeFilter()
        {
            List<EntityDicInstrument> ItrList = this.txthr_itr_fault_id.getDataSource();
            if (this.ParentForm != null && this.ParentForm is dcl.client.oa.FrmHandOverMgr)
            {
                dcl.client.oa.FrmHandOverMgr frmTemp = this.ParentForm as dcl.client.oa.FrmHandOverMgr;
                if (frmTemp != null)
                {
                    string srValue = frmTemp.getTxtTypeValue();//获取当前物理组id
                    if (!string.IsNullOrEmpty(srValue))
                    {
                        ItrList = ItrList.Where(i => i.ItrLabId == srValue).ToList();
                        this.txthr_itr_fault_id.SetFilter(ItrList);
                    }
                }
            }
        }


        /// <summary>
        /// 将DataTable对象转换成XML字符串
        /// </summary>
        /// <param name="dt">DataTable对象</param>
        /// <returns>XML字符串</returns>
        private string dtDataToXml(DataTable dt)
        {
            string strRvXML = "";

            if (dt != null)
            {
                MemoryStream ms = null;
                XmlTextWriter XmlWt = null;
                try
                {
                    ms = new MemoryStream();
                    //根据ms实例化XmlWt
                    XmlWt = new XmlTextWriter(ms, Encoding.UTF8);
                    XmlWt.Formatting = Formatting.None;
                    //获取ds中的数据
                    dt.WriteXml(XmlWt);
                    int count = (int)ms.Length;
                    byte[] temp = new byte[count];
                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Read(temp, 0, count);
                    //返回Unicode编码的文本
                    //UnicodeEncoding ucode = new UnicodeEncoding();
                    //string returnValue = ucode.GetString(temp).Trim();
                    UTF8Encoding utfcode = new UTF8Encoding();
                    string returnValue = utfcode.GetString(temp).Trim();
                    return returnValue;
                }
                catch (System.Exception ex)
                {
                    Lib.LogManager.Logger.LogException("dtDataToXml", ex);
                }
                finally
                {
                    //释放资源
                    if (XmlWt != null)
                    {
                        XmlWt.Close();
                        ms.Close();
                        ms.Dispose();
                    }
                }
            }
            else
            {
                return "";
            }

            return strRvXML;
        }

        /// <summary>
        /// 把xml内容转成指定Datatable
        /// </summary>
        /// <param name="strXml"></param>
        /// <param name="tableName"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        private bool ConvertXmlToDatatable(string strXml, string tableName, out DataTable dt)
        {
            bool bln = false;
            dt = null;
            try
            {
                if (strXml != null && strXml.Length > 0)
                {
                    strXml = strXml.Trim();
                }

                XmlDocument doc = new XmlDocument();

                doc.LoadXml(strXml);
                DataSet ds = new DataSet();
                System.Xml.XmlNodeReader xmlReader = new System.Xml.XmlNodeReader(doc);
                ds.ReadXml(xmlReader);//把xml字符串生成DataSet
                xmlReader.Close();
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables.Contains(tableName) && ds.Tables[tableName] != null)
                    {
                        dt = ds.Tables[tableName].Copy();
                        bln = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(GetType().FullName, new Exception("把xml内容转成指定Datatable,遇到错误！\r\n" + ex.ToString()));
            }
            return bln;
        }

   


    }
}

