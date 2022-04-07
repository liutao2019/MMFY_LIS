using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using dcl.client.wcf;
using System.IO;
using System.Xml;
using dcl.entity;
using System.Linq;

namespace dcl.client.msgclient
{
    public partial class FrmSetting : Form
    {

        FrmMessages frmMessages = null;

        FrmMsgGather frmMsgGather = null;

        /// <summary>
        /// 是否打开frmMsgGather
        /// </summary>
        private bool IsOpenMsgGather { get; set; }

        /// <summary>
        /// 科室信息
        /// </summary>
        //DataTable m_dtbDeptInfo = new DataTable();
        List<EntityDicPubDept> m_dtbDeptInfo = new List<EntityDicPubDept>();

        /// <summary>
        /// 科室代码集合
        /// </summary>
        List<string> strListDepcodes = new List<string>();

        public FrmSetting()
        {
            InitializeComponent();
            string soundMode = PlaySoundMgr.Instance.PlaySoundMode;
            if (string.IsNullOrEmpty(soundMode) || soundMode == "1")
            {
                this.rbMac.Checked = true;
            }
            else if (soundMode == "2")
            {
                this.rbS.Checked = true;
            }
            else
            {
                this.rbNone.Checked = true;
            }
            this.cbRunWhenStart.CheckedChanged += new EventHandler(cbRunWhenStart_CheckedChanged);

        }

        void cbRunWhenStart_CheckedChanged(object sender, EventArgs e)
        {
            CommonTool.runWhenStart(this.cbRunWhenStart.Checked);
        }

        public FrmSetting(FrmMessages p_frmMessages)
            : this()
        {

            IsOpenMsgGather = false;
            frmMessages = p_frmMessages;
        }

        public FrmSetting(FrmMsgGather p_frmMsgGather)
            : this()
        {

            IsOpenMsgGather = true;
            frmMsgGather = p_frmMsgGather;
        }


        private void FrmSetting_Load(object sender, EventArgs e)
        {

            this.m_mthLoadDeptInfo();

            this.m_mthLoadInt();

            readXml_Load();//加载xml配置信息

        }


        /// <summary>
        /// 加载xml配置信息
        /// </summary>
        private void readXml_Load()
        {
            string str_neglectDep = readXml("NEGLECTDEP");//忽略科室

            if (!string.IsNullOrEmpty(str_neglectDep))
            {
                if (str_neglectDep == "1") cbNeglectDep.Checked = true;
            }

            string str_ori_id = readXml("ORIID");//病人来源

            if (!string.IsNullOrEmpty(str_ori_id))//不为空时,加载下拉框
            {
                //str_ori_id值为1111 分别代表 住院,门诊,体检,其他
                char[] chr = str_ori_id.ToCharArray();

                for (int i = 0; i < chr.Length; i++)
                {
                    if (i == 0)
                    {
                        if (chr[i] == '0') cbOriZY.Checked = false; else cbOriZY.Checked = true;
                    }
                    if (i == 1)
                    {
                        if (chr[i] == '0') cbOriMZ.Checked = false; else cbOriMZ.Checked = true;
                    }
                    if (i == 2)
                    {
                        if (chr[i] == '0') cbOriTJ.Checked = false; else cbOriTJ.Checked = true;
                    }
                    if (i == 3)
                    {
                        if (chr[i] == '0') cbOriQT.Checked = false; else cbOriQT.Checked = true;
                    }
                }
            }


        }



        /// <summary>
        /// 加载科室信息
        /// </summary>
        private void m_mthLoadDeptInfo()
        {
            //ProxyMessage proxy = new ProxyMessage();
            ProxyObrMessage proxyObrMsg = new ProxyObrMessage();

            //加载科室信息
            m_dtbDeptInfo = proxyObrMsg.Service.GetDeptInfo();

            //selected,添加复选字段  实体中已包含是否选中字段
            //if (m_dtbDeptInfo != null && m_dtbDeptInfo.Columns.Count > 0 && !m_dtbDeptInfo.Columns.Contains("selected"))
            //{
            //    m_dtbDeptInfo.Columns.Add("selected", typeof(bool));

            //    for (int i = 0; i < m_dtbDeptInfo.Rows.Count; i++)
            //    {
            //        m_dtbDeptInfo.Rows[i]["selected"] = false;
            //    }
            //    m_dtbDeptInfo.AcceptChanges();
            //}

            bsDeptInfo.DataSource = m_dtbDeptInfo;
        }


        /// <summary>
        /// 加载配置文件数据
        /// </summary>
        private void m_mthLoadInt()
        {
            string strDeptId = ConfigurationManager.AppSettings["dep_code"];
            //&amp;
            if (strDeptId.Contains("&"))
            {
                string[] strspl = strDeptId.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
                //if (strspl.Length > 0) strDeptId = strspl[0];
                for (int i = 0; i < strspl.Length; i++)
                {
                    if (!string.IsNullOrEmpty(strspl[i]) && !strListDepcodes.Contains(strspl[i]))
                    {
                        strListDepcodes.Add(strspl[i]);
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(strDeptId))
                {
                    strListDepcodes.Add(strDeptId);
                }
            }

            string strDepNames = "";//科室名称
            //if (strListDepcodes.Count > 0 && m_dtbDeptInfo != null && m_dtbDeptInfo.Rows.Count>0)
            if (strListDepcodes.Count > 0 && m_dtbDeptInfo != null && m_dtbDeptInfo.Count > 0)
            {
                foreach (var drtempDep in m_dtbDeptInfo)
                {
                    if (!string.IsNullOrEmpty(drtempDep.DeptCode) && strListDepcodes.Contains(drtempDep.DeptCode))
                    {
                        drtempDep.Checked = true;
                        strDepNames += drtempDep.DeptName + ";";
                    }
                }

                if (!string.IsNullOrEmpty(strDepNames))
                {
                    strDepNames = strDepNames.TrimEnd(new char[] { ';' });
                }
            }

            this.txtDept.Text = strDepNames;

            this.txtUrl.Text = ConfigurationManager.AppSettings["WebSelectUrl"];

            this.cbRunWhenStart.Checked = ConfigurationManager.AppSettings["RunWhenStart"] == "1";
        }

        private void txtDept_TextChanged(object sender, EventArgs e)
        {
            //m_dtbDeptInfo.DefaultView.RowFilter = "dep_code like '" + txtDept.Text.Trim() + "%' or dep_name like '" + txtDept.Text.Trim() + "%'";
            //bsDeptInfo.DataSource = m_dtbDeptInfo.DefaultView.ToTable();
        }

        private void txtDept_Click(object sender, EventArgs e)
        {
            this.dataGridView1.Visible = txtselDept.Visible = true;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //m_mthChooseDeptInfo();
        }

        /// <summary>
        /// 选择所选的科室
        /// </summary>
        private void m_mthChooseDeptInfo()
        {
            #region 先前就注释掉了的老代码
            //this.txtDept.Tag = this.dataGridView1.CurrentRow.Cells["dep_code"].Value.ToString();
            //this.txtDept.Text = this.dataGridView1.CurrentRow.Cells["dep_name"].Value.ToString();
            //this.dataGridView1.Visible =txtselDept.Visible= false;
            //this.dataGridView1.CurrentRow.Cells["selected=1"].Value = true;
            #endregion 
            dataGridView1.EndEdit();

            //m_dtbDeptInfo.AcceptChanges();

            strListDepcodes.Clear();

            //DataRow[] drseltemp = m_dtbDeptInfo.Select("selected=1");
            List<EntityDicPubDept> drseltemp = m_dtbDeptInfo.Where(w => w.Checked == true).ToList();

            string strDepNames = "";
            foreach (var info in drseltemp) //for (int i = 0; i < drseltemp.Length; i++)
            {
                if (!string.IsNullOrEmpty(info.DeptCode) && !strListDepcodes.Contains(info.DeptCode))
                {
                    strListDepcodes.Add(info.DeptCode);
                }
                strDepNames += info.DeptName + ";";
            }
            if (!string.IsNullOrEmpty(strDepNames)) { strDepNames = strDepNames.TrimEnd(new char[] { ';' }); }

            this.txtDept.Text = strDepNames;
        }

        private void txtDept_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                //m_mthChooseDeptInfo();
            }
            else
            {
                if (!this.dataGridView1.Visible)
                {
                    this.dataGridView1.Visible = txtselDept.Visible = true;
                }
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                m_mthChooseDeptInfo();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrEmpty(this.txtDept.Text))
            if (strListDepcodes.Count <= 0)
            {
                MessageBox.Show("请设置科室！");
                return;
            }

            string strDepcodeSave = "";

            for (int i = 0; i < strListDepcodes.Count; i++)
            {
                strDepcodeSave += strListDepcodes[i] + "&";
            }
            if (!string.IsNullOrEmpty(strDepcodeSave))
            {
                strDepcodeSave = strDepcodeSave.TrimEnd(new char[] { '&' });
            }

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings["dep_code"].Value = strDepcodeSave;// this.txtDept.Tag.ToString();
            config.AppSettings.Settings["WebSelectUrl"].Value = this.txtUrl.Text.Trim();
            config.AppSettings.Settings.Remove("RunWhenStart");
            config.AppSettings.Settings.Add("RunWhenStart", this.cbRunWhenStart.Checked ? "1" : "0");



            config.Save();
            string soundMode = string.Empty;
            if (this.rbNone.Checked)
            {
                soundMode = "0";
            }
            else if (this.rbMac.Checked)
            {
                soundMode = "1";
            }
            else
            {
                soundMode = "2";
            }
            PlaySoundMgr.Instance.SaveConfig(soundMode);
            PlaySoundMgr.Instance.Refresh();
            //刷新配置文件参数
            ConfigurationManager.RefreshSection("appSettings");

            //保存xml配置
            saveXml();

            if (this.frmMessages != null)
            {
                if (IsOpenMsgGather)
                {
                    //重新加载配置数据
                    this.frmMsgGather.deptID = ConfigurationManager.AppSettings["dep_code"];

                    //&amp;
                    if (this.frmMsgGather.deptID.Contains("&"))
                    {
                        string[] strspl = this.frmMsgGather.deptID.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
                        if (strspl.Length > 0)
                        {
                            //this.frmMsgGather.deptID = strspl[0];
                            this.frmMsgGather.deptID = "";

                            for (int i = 0; i < strspl.Length; i++)
                            {
                                this.frmMsgGather.deptID += "'" + strspl[i] + "',";
                            }
                            if (!string.IsNullOrEmpty(this.frmMsgGather.deptID))
                            {
                                this.frmMsgGather.deptID = this.frmMsgGather.deptID.TrimEnd(new char[] { ',' });
                            }
                        }
                    }

                    //重新加载xml配置数据
                    string str_neglect_dep = readXml("NEGLECTDEP");//忽略科室
                    this.frmMsgGather.patNeglectDep = str_neglect_dep;


                    string str_ori_id = readXml("ORIID");//病人来源
                    this.frmMsgGather.patOriID = str_ori_id;

                }
                else
                {
                    //重新加载配置数据
                    this.frmMessages.deptID = ConfigurationManager.AppSettings["dep_code"];
                    //&amp;
                    if (this.frmMsgGather.deptID.Contains("&"))
                    {
                        string[] strspl = this.frmMsgGather.deptID.Split(new string[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
                        if (strspl.Length > 0) this.frmMsgGather.deptID = strspl[0];
                    }
                }
            }




            MessageBox.Show("保存成功！");
            this.DialogResult = DialogResult.OK;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 保存xmlMSG配置文件
        /// </summary>
        private void saveXml()
        {
            string filepath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "MSGSETTING.XML";
            try
            {
                string strXml = null;

                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }

                strXml = this.createStrXml();
                if (!string.IsNullOrEmpty(strXml))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(strXml);

                    doc.Save(filepath);
                    #region 先前就已经注释掉了的老代码
                    //DataSet ds = new DataSet();
                    //System.Xml.XmlNodeReader xmlReader = new System.Xml.XmlNodeReader(doc);
                    //ds.ReadXml(xmlReader);//把xml字符串生成DataSet
                    //xmlReader.Close();
                    //if (ds.Tables.Count > 0)
                    //{
                    //    if (ds.Tables[0].Rows.Count > 0)
                    //    {
                    //        ds.WriteXml(filepath, XmlWriteMode.IgnoreSchema);
                    //        ds.Clear();
                    //    }
                    //}
                    #endregion
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 生成xml字符串
        /// </summary>
        /// <returns></returns>
        private string createStrXml()
        {

            string str_ori_id = "1111";//病人来源 分别代表 住院,门诊,体检,其他

            str_ori_id = string.Format("{0}{1}{2}{3}", cbOriZY.Checked ? "1" : "0"
                , cbOriMZ.Checked ? "1" : "0"
                , cbOriTJ.Checked ? "1" : "0"
                , cbOriQT.Checked ? "1" : "0");

            string strXml = null;
            using (StringWriter sw = new StringWriter())
            {
                XmlTextWriter xtw = new XmlTextWriter(sw);
                xtw.WriteStartDocument();
                //根节点
                xtw.WriteStartElement("MSGSETTING");

                //子节点
                xtw.WriteStartElement("MSGINFO");

                xtw.WriteComment("'ORIID'为病人来源");

                //ORIID
                xtw.WriteStartElement("ORIID");
                xtw.WriteString(str_ori_id);
                xtw.WriteEndElement();


                xtw.WriteComment("'NEGLECTDEP'为忽略科室(或病区)");
                //NEGLECTDEP
                xtw.WriteStartElement("NEGLECTDEP");
                xtw.WriteString(cbNeglectDep.Checked ? "1" : "0");
                xtw.WriteEndElement();


                xtw.WriteEndElement();//MSGINFO
                xtw.WriteEndElement();//MSGSETTING
                xtw.WriteEndDocument();

                strXml = sw.ToString();
            }
            return strXml;
        }

        /// <summary>
        /// 读取xml关于某键值
        /// </summary>
        /// <param name="tKey"></param>
        private string readXml(string tKey)
        {
            string tValue = "";

            string filepath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "MSGSETTING.XML";

            try
            {
                if (!File.Exists(filepath))
                {
                    saveXml();
                    return tValue;
                }
                //下面的不需要改造
                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);
                DataSet ds = new DataSet(); 
                System.Xml.XmlNodeReader xmlReader = new System.Xml.XmlNodeReader(doc);
                ds.ReadXml(xmlReader);//把xml字符串生成DataSet
                xmlReader.Close();
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains(tKey))
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        tValue = row[tKey].ToString();
                        ds.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                if (File.Exists(filepath))//有误,重新生成
                {
                    File.Delete(filepath);
                    saveXml();
                }
            }
            return tValue;
        }

        /// <summary>
        /// 筛选科室信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtselDept_TextChanged(object sender, EventArgs e)
        {
            //if (bsDeptInfo.DataSource != null && bsDeptInfo.DataSource is DataTable)
            if (bsDeptInfo.DataSource != null && bsDeptInfo.DataSource is List<EntityDicPubDept>)
            {
                if (string.IsNullOrEmpty(txtselDept.Text.Trim()))
                {
                    //bsDeptInfo.Filter = "1=1";
                    bsDeptInfo.DataSource = m_dtbDeptInfo;
                }
                else
                {
                    //bsDeptInfo.Filter = "dep_code like '" + txtselDept.Text.Trim() + "%' or dep_name like '" + txtselDept.Text.Trim() + "%' or selected=1";
                    List<EntityDicPubDept> listDept = new List<EntityDicPubDept>();
                    listDept = m_dtbDeptInfo.Where(w => w.DeptCode.Contains(txtselDept.Text.Trim()) ||
                                                      w.DeptName.Contains(txtselDept.Text.Trim()) ||
                                                      w.Checked == true).ToList();
                    bsDeptInfo.DataSource = listDept;
                }
            }
        }

        private void dataGridView1_Leave(object sender, EventArgs e)
        {
            //m_mthChooseDeptInfo();
        }

        private void dataGridView1_MouseLeave(object sender, EventArgs e)
        {
            m_mthChooseDeptInfo();
        }
    }
}
