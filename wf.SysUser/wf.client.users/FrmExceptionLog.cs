using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using System.IO;
using dcl.entity;
using System.Linq;

namespace dcl.client.users
{
    public partial class FrmExceptionLog : FrmCommon
    {
        string directoryPath = Application.StartupPath + "\\log";

        public FrmExceptionLog()
        {
            InitializeComponent();
        }

        private void FrmExceptionLog_Load(object sender, EventArgs e)
        {
            //初始化工具条
            sysToolBar1.SetToolButtonStyle(new string[] { sysToolBar1.BtnSearch.Name, sysToolBar1.BtnDelete.Name, sysToolBar1.BtnClose.Name });

            //默认时间为今天
            timeFrom.EditValue = DateTime.Now.AddDays(-3);
            timeTo.EditValue = DateTime.Now;
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnCloseClicked(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 查询日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnSearchClicked(object sender, EventArgs e)
        {
            LoadData();

            sysToolBar1.LogMessage = String.Format("从{0}到{1}", timeFrom.Text, timeTo.Text);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                DateTime from = Convert.ToDateTime(timeFrom.Text);
                DateTime to = Convert.ToDateTime(timeTo.Text);

                if (Directory.Exists(directoryPath))
                {
                    DataTable dtFiles = new DataTable();
                    dtFiles.Columns.Add("Time");
                    dtFiles.Columns.Add("Module");
                    dtFiles.Columns.Add("FileName");

                    string[] fileNames = Directory.GetFiles(directoryPath);
                    foreach (string fileName in fileNames)
                    {
                        string file = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                        string[] fileInfo = file.Split('_');

                        string thisTime = fileInfo[0];

                        DateTime tmpTime = new DateTime(int.Parse(thisTime.Substring(0, 4)), int.Parse(thisTime.Substring(4, 2)), int.Parse(thisTime.Substring(6, 2)));

                        if (tmpTime < from || tmpTime > to)
                        {
                            continue;
                        }

                        DataRow dr = dtFiles.NewRow();
                        dr["FileName"] = fileName;
                        dr["Time"] = tmpTime.ToString("yyyy-MM-dd");
                        string module = fileInfo[2].Substring(0, fileInfo[2].Length - 4);

                        List<EntitySysFunction> listAllFunc = UserInfo.entityUserInfo.AllFunc;
                        List<EntitySysFunction> drs = listAllFunc.Where(w=>w.FuncCode==module && w.FuncChildName=="").ToList();
                        if (module != "" && drs.Count > 0)
                        {
                            dr["Module"] = "[" + drs[0].FuncName + "]" + module;
                        }
                        else
                        {
                            if (module == "lis.client.FrmMain")
                            {
                                dr["Module"] = "[主窗体]" + module;
                            }
                            else
                            {
                                dr["Module"] = module;
                            }
                        }
                        dtFiles.Rows.Add(dr);
                    }

                    gdSysLog.DataSource = dtFiles;

                    gridView1_FocusedRowChanged(null, null);
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysToolBar1_OnBtnDeleteClicked(object sender, EventArgs e)
        {
            string timespan = String.Format("从{0}到{1}", timeFrom.Text, timeTo.Text);

            DialogResult dresult = MessageBox.Show("确认要清除" + timespan + "的所有异常日志吗?", PowerMessage.BASE_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            switch (dresult)
            {
                case DialogResult.OK:
                    {
                        DateTime from = Convert.ToDateTime(timeFrom.Text);
                        DateTime to = Convert.ToDateTime(timeTo.Text);

                        if (Directory.Exists(directoryPath))
                        {
                            string[] fileNames = Directory.GetFiles(directoryPath);
                            foreach (string fileName in fileNames)
                            {
                                string file = fileName.Substring(fileName.LastIndexOf("\\") + 1);
                                string[] fileInfo = file.Split('_');

                                string thisTime = fileInfo[0];

                                DateTime tmpTime = new DateTime(int.Parse(thisTime.Substring(0, 4)), int.Parse(thisTime.Substring(4, 2)), int.Parse(thisTime.Substring(6, 2)));

                                if (tmpTime >= from && tmpTime <= to)
                                {
                                    File.Delete(fileName);
                                }

                            }
                        }
                        LoadData();
                        break;
                    }
                        case DialogResult.Cancel:
                    return;
            }

            sysToolBar1.LogMessage = timespan;
        }

        /// <summary>
        /// 显示详细内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                string fileName = dr["FileName"].ToString();
                StreamReader reader = new StreamReader(fileName,Encoding.Default);
                txtDetail.Text = reader.ReadToEnd();
                reader.Close();
            }
            catch
            {
                txtDetail.Text = "";
            }
        }
    }
}
