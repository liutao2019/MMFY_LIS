using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.wcf;
using dcl.root.logon;
using dcl.common;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using dcl.client.common;
using dcl.entity;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;

namespace dcl.client.dicbasic
{

    public partial class FrmDictMitmNoResultView : FrmCommon
    {
        public string Itr_id { get; set; }
        private bool bSave = true;

        //全局变量，存放项目的信息
        private List<EntityDicItmItem> itmList = new List<EntityDicItmItem>();

        public FrmDictMitmNoResultView()
        {
            InitializeComponent();
            gridheadercheckbox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            sysToolBar1.BtnExport.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(BtnExport_Click);
            
            PoxyMitmNoResultView proxy = new PoxyMitmNoResultView();
            EntityResponse item = proxy.Service.SearchItem(new EntityRequest());
            List<EntityDicItmItem> itmCombine = new List<EntityDicItmItem>();
            itmCombine = item.GetResult() as List<EntityDicItmItem>;
            itmList = itmCombine.Where(w=>w.ItmDelFlag=="0").ToList(); //给全局变量赋值,用来筛选掉停用的标志
            this.bsItem.DataSource = itmList;
        }

        /// <summary>
        /// 导入字典按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                gridViewSingle.CloseEditor();

                Itr_id = txtItr.valueMember;
                toSave();
                if (bSave)
                {
                    this.DialogResult = DialogResult.Yes;
                }
                bSave = true;
            }
            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show("导入项目通道码字典失败：" + ex.Message, "提示");
            }

        }

        /// <summary>
        /// 获取所有已勾选的项目通道码对应的DataTable
        /// </summary>
        /// <returns></returns>
        private List<EntityDicMachineCode> FilterCnoDataTable() 
        {
            try
            {
                gridViewSingle.CloseEditor();
                
                List<EntityDicObrResultOriginal> table = bindingSource1.DataSource as List<EntityDicObrResultOriginal>;

                if (table.Count <= 0)
                {
                    lis.client.control.MessageDialog.Show("未找到仪器通道码信息", "提示");
                    txtItr.Focus();
                    return null;
                }
                
                List<EntityDicObrResultOriginal> drFilter = table.Where(w => w.ResSelect == "1" && w.Msg != "").ToList(); 

                //选中通道码的所有行
                List<EntityDicMachineCode> dtResultFilter = new List<EntityDicMachineCode>();

                //判断是否有勾选通道码
                if (drFilter.Count != 0)
                {
                    #region 构造dtResultFilter
                    bool bRepeat = false;
                    
                    foreach (var row in drFilter) 
                    {
                        if (row != null && row.ResSelect == "1"&&row.Msg.ToString()!="")
                        {
                            EntityDicMachineCode newRow = new EntityDicMachineCode();

                            newRow.MacId = "";
                            newRow.ItrId = txtItr.valueMember;
                            newRow.MacCode = row.ObrMacCode;

                            string itm_id = row.ItmID;

                            newRow.MacItmId = itm_id;
                            if (string.IsNullOrEmpty(itm_id))
                            {
                                lis.client.control.MessageDialog.Show("项目不能为空", "提示");
                                break;
                            }
                            newRow.MacItmEcd = GetEcdById(itm_id);
                            if (string.IsNullOrEmpty(row.MitDec.ToString()))
                            {
                                newRow.MacDecPlace = 0;
                            }
                            else
                            {
                                newRow.MacDecPlace = Convert.ToDecimal(row.MitDec);
                            }
                            if (string.IsNullOrEmpty(row.MitPos.ToString()))
                            {
                                newRow.MacPosition = 0;
                            }
                            else
                            {
                                newRow.MacPosition = Convert.ToDecimal(row.MitPos);
                            }
                            //if (string.IsNullOrEmpty(row.MitRlen.ToString()))
                            //{
                            //    newRow.MacResLen = 0;
                            //}
                            //else
                            //{
                            newRow.MacResLen = row.MitRlen;
                            //}
                            if (string.IsNullOrEmpty(row.MitType.ToString()))
                            {
                                newRow.MacType = String.Empty;
                            }
                            else
                            {
                                newRow.MacType = row.MitType;
                            }
                            //if (string.IsNullOrEmpty(row["mit_flag"].ToString()))
                            //{
                            //    newRow["mit_flag"] = DBNull.Value;
                            //}
                            //else
                            //{
                            newRow.MacFlag = row.MitFlag;
                            //}
                            newRow.DelFlag = "0";
                            if (dtResultFilter.Count == 0)
                            {
                                dtResultFilter.Add(newRow);
                            }
                            else
                            {
                                foreach (var info in dtResultFilter)
                                {
                                    //判断是否有勾选重复的通道码
                                    if (info.MacCode == newRow.MacCode)
                                    {
                                        bRepeat = true;
                                        break;
                                    }
                                }
                                if (!bRepeat)
                                {
                                    dtResultFilter.Add(newRow);
                                }
                                bRepeat = false;
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    lis.client.control.MessageDialog.Show("请勾选需要导入的通道码", "提示");
                }
                return dtResultFilter;
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// 根据itm_id获取itm_ecd
        /// </summary>
        /// <param name="itm_id"></param>
        /// <returns></returns>
        private string GetEcdById(string itm_id)
        {
            if (!string.IsNullOrEmpty(itm_id))
            {
                string itm_ecd = string.Empty;
                
                List<EntityDicItmItem> dr = itmList.Where(w => w.ItmId == itm_id).ToList();
                if (dr != null && dr.Count > 0)
                {
                    itm_ecd = dr[0].ItmEcode;
                }
                return itm_ecd;
            }
            else
            {
                return string.Empty;
            }
        }

        #region CheckBoxOnGridHeader 全选按钮

        public void gridViewPatientList_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column != null && e.Column.Name == "col_selected")
            {
                e.Info.InnerElements.Clear();
                e.Painter.DrawObject(e.Info);
                DrawCheckBoxOnHeader(e.Graphics, e.Bounds, bGridHeaderCheckBoxState);

                e.Handled = true;
            }
        }

        DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit gridheadercheckbox;
        private void DrawCheckBoxOnHeader(Graphics g, Rectangle r, bool check)
        {


            DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo info;
            DevExpress.XtraEditors.Drawing.CheckEditPainter painter;
            DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs args;

            info = gridheadercheckbox.CreateViewInfo() as DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo;
            painter = gridheadercheckbox.CreatePainter() as DevExpress.XtraEditors.Drawing.CheckEditPainter;
            info.EditValue = check;
            info.Bounds = r;
            info.CalcViewInfo(g);
            args = new DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs(info, new DevExpress.Utils.Drawing.GraphicsCache(g), r);
            painter.Draw(args);
            args.Cache.Dispose();

        }
        private bool bGridHeaderCheckBoxState = false;
        private void gridViewSingle_MouseDown(object sender, MouseEventArgs e)
        {
            Point pt = this.gridViewSingle.GridControl.PointToClient(Control.MousePosition);
            GridHitInfo info = this.gridViewSingle.CalcHitInfo(pt);
            if (info.InColumn && info.Column.Name == "col_selected")
            {
                bGridHeaderCheckBoxState = !bGridHeaderCheckBoxState;
                this.gridViewSingle.InvalidateColumnHeader(this.gridViewSingle.Columns["col_selected"]);
                SelectAllPatientInGrid(bGridHeaderCheckBoxState);
            }
        }
        //全选按钮方法
        private void SelectAllPatientInGrid(bool selectAll)
        {
            List<EntityDicObrResultOriginal> listSelAll = this.bindingSource1.DataSource as List<EntityDicObrResultOriginal>;
            //List<EntityDicObrResultOriginal> listSelAll = this.gridControlSingle.DataSource as List<EntityDicObrResultOriginal>;
            
            foreach (var info in listSelAll)
            {
                info.ResSelect= bGridHeaderCheckBoxState ? "1" : "0";
            }
            this.gridControlSingle.DataSource = listSelAll;
            this.gridControlSingle.RefreshDataSource();
            
        }
        #endregion

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshData()
        {
            try
            {
                PoxyMitmNoResultView proxy = new PoxyMitmNoResultView();
                
                string tempSid= this.txtSID.Text;
                MemoryStream stream = new MemoryStream();
                byte[] buffer = proxy.Service.GetAllBuffer(this.txtDate.DateTime, this.txtItr.valueMember, 2, txtSID.Text);
                //解压压缩流
                byte[] bytes = InflateData(buffer);
                stream = new MemoryStream(bytes);
                IFormatter formatter = new BinaryFormatter();
                List<EntityDicObrResultOriginal> listResult = new List<EntityDicObrResultOriginal>();
                //反序列化
                listResult = (List<EntityDicObrResultOriginal>)formatter.Deserialize(stream);
                stream.Close();
                //获取数据
              //  List<EntityDicObrResultOriginal> dtResult = proxy.Service.GetInstructmentResult2(this.txtDate.DateTime, this.txtItr.valueMember, 2, tempSid);
                
                foreach (var info in listResult) 
                {
                    info.ResSelect = "0";
                }
                this.gridViewSingle.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.None;

                int trindex = this.gridViewSingle.TopRowIndex;

                //绑定网格
                this.bindingSource1.DataSource = listResult;

                this.gridViewSingle.TopRowIndex = trindex;

            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "获取实时结果出错", ex.ToString());
            }
        }

        /// <summary>
        /// 解压压缩数据
        /// </summary>
        /// <param name="compressedData"></param>
        /// <returns></returns>
        public byte[] InflateData(byte[] compressedData)
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

        /// <summary>
        /// 离开样本号刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSID_Leave(object sender, EventArgs e)
        {
            RefreshData();
        }

        /// <summary>
        /// 样本号回车刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                RefreshData();
            }
        }
        
        /// <summary>
        /// 保存通道码字典
        /// </summary>
        private void toSave()
        {
            try
            {
                List<EntityDicMachineCode> dtChange = FilterCnoDataTable();

                if (dtChange == null || dtChange.Count == 0)
                {
                    bSave = false;
                    return;
                }
                
                PoxyMitmNoResultView proxy = new PoxyMitmNoResultView();

                EntityResponse result = new EntityResponse();
                result = proxy.Service.SaveOrUpdateMitmNo(dtChange);

                if (result.Scusess)
                {
                    lis.client.control.MessageDialog.ShowAutoCloseDialog("保存成功！");
                }
                
            }

            catch (Exception ex)
            {
                lis.client.control.MessageDialog.Show("保存异常:" + ex.Message, "信息");
                return;
            }
        }

        private void FrmDictMitmNoResultView_Load(object sender, EventArgs e)
        {
            sysToolBar1.BtnExport.Caption = "导入字典";

            sysToolBar1.SetToolButtonStyle(
                new string[] { sysToolBar1.BtnExport.Name },
                    new string[] { "F8" }
                    );
            this.Visible = true;
            txtDate.EditValue = DbType.DateTime;
            txtDate.EditValue = DateTime.Today;
            txtItr.Focus();
        }

        /// <summary>
        /// 仪器数据改变，重新刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void txtItr_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            RefreshData();

            #region old
            //string itr_id = txtItr.valueMember;
            //DataRow drItr = DictInstrmt.Instance.GetItr(itr_id);
            //bsItem.Filter = "itm_ptype='" + drItr["itr_ptype"].ToString() + "'";
            #endregion 
        }
    }
}
