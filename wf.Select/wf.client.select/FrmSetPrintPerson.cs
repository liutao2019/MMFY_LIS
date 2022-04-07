using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using lis.client.control;
using DevExpress.XtraEditors;

namespace dcl.client.resultquery
{
    public partial class FrmSetPrintPerson : XtraForm
    {
        /// <summary>
        /// 保存信息数据表
        /// </summary>
        private DataTable dtSave = null;

        /// <summary>
        /// 打印者
        /// </summary>
        public string outPrintPersonName { get; set; }

        public FrmSetPrintPerson()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            saveXml();
            this.Close();
        }

        private void FrmSetPrintPerson_Load(object sender, EventArgs e)
        {
            if (dtSave == null)
            {
                dtSave = new DataTable("dtSave");
                dtSave.Columns.Add("printName",Type.GetType("System.String"));
            }

            readXml_Load();

            bsdata.DataSource = dtSave;

            if (outPrintPersonName != null) txtName.Text = outPrintPersonName;

            txtName.Focus();
            txtName.SelectAll();
        }

        /// <summary>
        /// 加载xml配置信息
        /// </summary>
        private void readXml_Load()
        {
            string filepath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "FrmSetPrintPerson.XML";

            try
            {
                if (!File.Exists(filepath))
                {
                    return; 
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(filepath);
                DataSet ds = new DataSet();
                System.Xml.XmlNodeReader xmlReader = new System.Xml.XmlNodeReader(doc);
                ds.ReadXml(xmlReader);//把xml字符串生成DataSet
                xmlReader.Close();
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0
                        && ds.Tables[0].Columns.Contains("printName"))
                    {
                        //DataView dv = ds.Tables[0].DefaultView;
                        //dv.Sort = "printName asc";
                        DataRow[] arrDr = ds.Tables[0].Select("1=1", "printName asc");
                        foreach (DataRow dr in arrDr)
                        {
                            //过滤重复的
                            if (!string.IsNullOrEmpty(dr["printName"].ToString())
                                &&dtSave.Select("printName='" + dr["printName"].ToString() + "'").Length <= 0)
                            {
                                dtSave.Rows.Add(new object[] { dr["printName"].ToString() });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException("设置打印者信息", ex);
            }
        }

        /// <summary>
        /// 保存xmlMSG配置文件
        /// </summary>
        private void saveXml()
        {
            if (!string.IsNullOrEmpty(txtName.Text)&& dtSave != null
                && dtSave.Select("printName='" + txtName.Text + "'").Length <= 0)
            {
                dtSave.Rows.Add(new object[] { txtName.Text });
            }

            if (!string.IsNullOrEmpty(outPrintPersonName) && dtSave != null
                && dtSave.Select("printName='" + outPrintPersonName + "'").Length <= 0)
            {
                dtSave.Rows.Add(new object[] { outPrintPersonName });
            }

            if (dtSave.Rows.Count > 0)
            {
                string filepath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "FrmSetPrintPerson.XML";
                try
                {
                    DataSet ds = new DataSet("DS");
                    ds.Tables.Add(dtSave.DefaultView.Table);
                    if (ds.Tables[0] != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (File.Exists(filepath))
                            {
                                File.Delete(filepath);
                            }
                            ds.WriteXml(filepath, XmlWriteMode.IgnoreSchema);
                            ds.Clear();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Lib.LogManager.Logger.LogException("设置打印者信息", ex);
                }
            }
        }

        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            saveXml();
            if (UpdateOutPrintPerson != null)
            {
                UpdateOutPrintPerson(txtName.Text);
            }
            this.Close();
        }

        /// <summary>
        /// 根据输入筛选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        bool isdoubleclickGgv = false;

        /// <summary>
        /// 双击选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvData_DoubleClick(object sender, EventArgs e)
        {
            if (bsdata.Position >= 0&&dgvData.CurrentRow != null)
            {
                isdoubleclickGgv = true;
                DataRowView drvObj = dgvData.CurrentRow.DataBoundItem as DataRowView;
                txtName.Text = drvObj.Row["printName"].ToString();
            }
            
            isdoubleclickGgv = false;
        }

        /// <summary>
        /// 委托
        /// </summary>
        public delegate void UpdateOutPrintPersonHandler(string str);

        /// <summary>
        /// 事件
        /// </summary>
        public event UpdateOutPrintPersonHandler UpdateOutPrintPerson;

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            FrmCheckPassword frm = new FrmCheckPassword();
            if (frm.ShowDialog() == DialogResult.OK)//身份验证
            {
                txtName.Text = frm.OperatorName;
                saveXml();
                readXml_Load();
            }
        }
    }
}
