using System;
using System.Collections.Generic;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.root.logon;
using dcl.client.wcf;
using dcl.entity;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;
using dcl.client.cache;

namespace dcl.client.result
{
    public partial class FrmRealTimeResultView : FrmCommon
    {
        /// <summary>
        /// 获取仪器结果计时器
        /// </summary>
        Timer timer1;

        /// <summary>
        /// 工具条式样
        /// </summary>
        public string[] ToolBarStyle
        {
            get
            {
                return
                    new string[] {
                                     "BtnClose",
                                    };
            }
        }

        #region .ctor
        public FrmRealTimeResultView()
        {
            InitializeComponent();

            this.txtDate.EditValue = DateTime.Now;
        }

        public FrmRealTimeResultView(DateTime date, string itr_id)
        {
            InitializeComponent();

            this.txtDate.EditValue = date;
            EntityDicInstrument drItr = DictInstrmt.Instance.GetItr(itr_id);
            if (drItr != null)
            {
                this.txtItr.displayMember = drItr.ItrEname.ToString();
                this.txtItr.valueMember = itr_id;
            }
        }
        #endregion

        /// <summary>
        /// 窗体加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmResultoView_Load(object sender, EventArgs e)
        {
            InitTimer();

            sysToolBar1.BtnExport.Caption = "导出";

            sysToolBar1.SetToolButtonStyle(
                new string[] { sysToolBar1.BtnRefresh.Name, sysToolBar1.BtnExport.Name },
                    new string[] { "F6", "F8" }
                    );
            sysToolBar1.OnBtnRefreshClicked += btnRefresh_Click;
        }

        private void InitTimer()
        {
            this.sysToolBar1.SetToolButtonStyle(ToolBarStyle);

            RefreshData();

            if (timer1 == null)
            {
                timer1 = new Timer();
                timer1.Interval = 6000;
                timer1.Tick += new EventHandler(timer_Tick);
            }
            else
            {
                timer1.Stop();
            }

            timer1.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            //仪器不为空时才访问数据
            if (this.txtItr.valueMember != null && this.txtItr.valueMember.Trim() != string.Empty)
            {
                if (chkAutoRefresh.Checked)
                    RefreshData();
            }
        }

        private void RefreshData()
        {
            try
            {
                int result_type = Convert.ToInt32(rdoResultType.EditValue);
                PoxyMitmNoResultView proxy = new PoxyMitmNoResultView();
                MemoryStream stream = new MemoryStream();
                byte[] buffer = proxy.Service.GetAllBuffer(this.txtDate.DateTime, this.txtItr.valueMember, result_type, txtSID.Text);
                //解压压缩流
                byte[] bytes = InflateData(buffer);
                stream = new MemoryStream(bytes);
                IFormatter formatter = new BinaryFormatter();
                List<EntityDicObrResultOriginal> listResult = new List<EntityDicObrResultOriginal>();
                //反序列化
                listResult = (List<EntityDicObrResultOriginal>)formatter.Deserialize(stream);
                stream.Close();
                //获取数据
                //List<EntityDicObrResultOriginal> listResult = new PoxyMitmNoResultView().Service.GetInstructmentResult2(this.txtDate.DateTime, this.txtItr.valueMember, result_type, txtSID.Text);
                //DataTable dtResult = PatientEnterClient.NewInstance.GetInstructmentResult2(this.txtDate.DateTime, this.txtItr.valueMember, result_type, GetFilter());
                this.gridView1.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.None;

                int trindex = this.gridView1.TopRowIndex;

                //绑定网格
                this.bindingSource1.DataSource = listResult;

                this.gridView1.TopRowIndex = trindex;

            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "获取实时结果出错", ex.ToString());
            }
        }
        /// <summary>
        /// 解压压缩数据流
        /// </summary>
        /// <param name="compressedData"></param>
        /// <returns></returns>
        public  byte[] InflateData(byte[] compressedData)
        {
            if (compressedData == null) return null;

            //  initialize the default lenght to the compressed data length times 2
            int deflen = compressedData.Length * 2;
            byte[] buffer = null;

            using (MemoryStream stream = new MemoryStream(compressedData))
            {
                using (DeflateStream inflatestream = new DeflateStream(stream, CompressionMode.Decompress))
                {
                    using (MemoryStream uncompressedstream = new MemoryStream())
                    {
                        using (BinaryWriter writer = new BinaryWriter(uncompressedstream))
                        {
                            int offset = 0;
                            while (true)
                            {
                                byte[] tempbuffer = new byte[deflen];

                                int bytesread = inflatestream.Read(tempbuffer, offset, deflen);

                                writer.Write(tempbuffer, 0, bytesread);

                                if (bytesread < deflen || bytesread == 0) break;
                            }   // end while

                            uncompressedstream.Seek(0, SeekOrigin.Begin);
                            buffer = uncompressedstream.ToArray();
                        }
                    }
                }
            }

            return buffer;
        }
        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmRealTimeResultView_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {

                //注销计时器
                if (timer1 != null)
                {
                    timer1.Stop();
                    timer1.Dispose();
                    timer1 = null;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "FrmRealTimeResultView_FormClosed", ex.ToString());
            }
        }

        /// <summary>
        /// 仪器改变,重新刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void txtItr_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            RefreshData();
        }

        /// <summary>
        /// 日期控件离开,重新刷新数据(暂未做日期改变才刷新数据)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDate_Leave(object sender, EventArgs e)
        {
            RefreshData();
        }

        /// <summary>
        /// 手动刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void chkIncludeNonItrResult_CheckedChanged(object sender, EventArgs e)
        {
            RefreshData();
        }


        private void txtSID_Leave(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void txtSID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                RefreshData();
            }
        }

        private void rdoResultType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void sysToolBar1_OnBtnExportClicked(object sender, EventArgs e)
        {
            setExcel(gridControl1);
        }

        /// <summary>
        /// 导出Excel方法
        /// </summary>
        /// <param name="gcExcel"></param>
        private void setExcel(DevExpress.XtraGrid.GridControl gcExcel)
        {
            if (gcExcel.DataSource != null)
            {
                SaveFileDialog ofd = new SaveFileDialog();
                ofd.DefaultExt = "xls";
                ofd.Filter = "Excel文件(*.xls)|*.xls";
                ofd.Title = "导出到Excel";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (ofd.FileName.Trim() == string.Empty)
                    {
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("文件名不能为空！");
                        return;
                    }

                    try
                    {
                        gcExcel.ExportToXls(ofd.FileName.Trim());
                        //gcExcel.ExportToXls(ofd.FileName.Trim());       
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("导出成功！");
                    }
                    catch (Exception ex)
                    {
                        dcl.root.logon.Logger.WriteException(this.GetType().Name, "导出", ex.ToString());
                        lis.client.control.MessageDialog.ShowAutoCloseDialog("导出失败！");
                    }
                }

            }
        }
    }
}
