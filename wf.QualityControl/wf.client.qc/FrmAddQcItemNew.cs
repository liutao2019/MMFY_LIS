using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using dcl.client.frame;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using dcl.entity;
using dcl.client.wcf;
using System.Linq;

namespace dcl.client.qc
{
    public partial class FrmAddQcItemNew : FrmCommon
    {
        string where;
        string parId;
        string insId;

        //新增全局变量 质控项目数据
        public List<EntityDicQcMateriaDetail> listMateriaDetail;
        //新增全局服务变量，调用数据层操作方法
        private ProxyFastAddQcItem proxyAddQcItem = new ProxyFastAddQcItem();


        /// <summary>
        /// 委托
        /// </summary>
        /// <param name="e"></param>
        public delegate void ClikeHander(ClickEventArgs e);

        /// <summary>
        /// 事件
        /// </summary>
        public event ClikeHander clikcA;



        public FrmAddQcItemNew()
        {
            InitializeComponent();
        }

        public FrmAddQcItemNew(string where, string parId, string insId)
        {
            InitializeComponent();
            this.where = where;
            this.parId = parId;
            this.insId = insId;
        }

        public event ClikeHander checkQcItem;

        //全局变量 用于保存所有项目的值
        private List<EntityDicQcItem> listDicQCItem = new List<EntityDicQcItem>();

        private void FrmQcCopy_Load(object sender, EventArgs e)
        {
            sysAddItem.SetToolButtonStyle(new string[] { sysAddItem.BtnSave.Name, sysAddItem.BtnReset.Name });//显示的按钮
            gridheadercheckbox = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();//用于全选按钮

            EntityResponse result = new EntityResponse();
            result = proxyAddQcItem.Service.SearchItemQcParQcRule(listMateriaDetail, insId);
            List<Object> listObj = result.GetResult() as List<Object>;

            //List<EntityDicQcItem> listItem = new List<EntityDicQcItem>();
            List<EntityDicQcMateriaDetail> listMd = new List<EntityDicQcMateriaDetail>();
            List<EntityDicQcRule> listRule = new List<EntityDicQcRule>();
            if (listObj.Count > 0)
            {
                listDicQCItem = listObj[0] as List<EntityDicQcItem>;
                listMd = listObj[1] as List<EntityDicQcMateriaDetail>;
                listRule = listObj[2] as List<EntityDicQcRule>;
            }

            this.gcItem.DataSource = listDicQCItem.OrderBy(w => w.ItmId).ToList();//质控项目
            this.gcQcItem.DataSource = listMd; //绑定质控项目
            cklRules.DataSource = listRule;//质控规则关联

            //默认质控规则选中
            for (int j = 0; j < this.cklRules.ItemCount; j++)
            {
                cklRules.SetItemChecked(j, true);
            }
        }

        #region CheckBoxOnGridHeader

        private void gvItem_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column != null && e.Column.Name == "itm_select")
            {
                e.Info.InnerElements.Clear();
                e.Painter.DrawObject(e.Info);
                DrawCheckBoxOnHeader(e.Graphics, e.Bounds, bGridHeaderCheckBoxState);

                e.Handled = true;
            }
        }

        DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit gridheadercheckbox;// = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
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
        protected virtual void gridControl1_MouseDown(object sender, MouseEventArgs e)
        {

            Point pt = this.gvItem.GridControl.PointToClient(Control.MousePosition);
            GridHitInfo info = this.gvItem.CalcHitInfo(pt);
            if (info.InColumn && info.Column.Name == "itm_select")
            {
                bGridHeaderCheckBoxState = !bGridHeaderCheckBoxState;
                this.gvItem.InvalidateColumnHeader(this.gvItem.Columns["itm_select"]);
                SelectAllPatientInGrid(bGridHeaderCheckBoxState);
            }
        }

        //全选事件
        private void SelectAllPatientInGrid(bool selectAll)
        {
            //for (int i = 0; i < this.gvItem.RowCount; i++)
            //{
            //    if (bGridHeaderCheckBoxState)
            //    {
            //        this.gvItem.GetDataRow(i)["ItmSelect"] = 1; 
            //    }
            //    else
            //    {
            //        this.gvItem.GetDataRow(i)["ItmSelect"] = 0;
            //    }
            //}

            //this.gvItem.CloseEditor();
            List<EntityDicQcItem> listItem = this.gcItem.DataSource as List<EntityDicQcItem>;

            if (listItem == null)
                return;

            foreach (var info in listItem)
            {
                if (bGridHeaderCheckBoxState)
                {
                    info.ItmSelect = 1;
                }
                else
                {
                    info.ItmSelect = 0;
                }
            }
            this.gcItem.DataSource = listItem;
            this.gcItem.RefreshDataSource();
        }
        #endregion


        /// <summary>
        /// 质控项检索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_EditValueChanged(object sender, EventArgs e)
        {
            List<EntityDicQcItem> searchItem = new List<EntityDicQcItem>();
            if (txtSearch.EditValue.ToString() != "")
            {
                string where = txtSearch.EditValue.ToString().Replace("'", "''");
                searchItem = listDicQCItem.Where(w => w.ItmName.Contains(where) ||
                                             w.ItmEcode.Contains(where.ToUpper()) ||
                                             w.PyCode.Contains(where.ToUpper()) ||
                                             w.WbCode.Contains(where.ToUpper())).ToList();
                gcItem.DataSource = searchItem;
            }
            else
                gcItem.DataSource = listDicQCItem;
        }

        private void sysCopy_BtnCopyClick(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 增加质控物项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddQcItem_Click(object sender, EventArgs e)
        {
            List<EntityDicQcItem> listItem = gcItem.DataSource as List<EntityDicQcItem>; //得到项目数据源

            List<EntityDicQcMateriaDetail> listQcItem = gcQcItem.DataSource as List<EntityDicQcMateriaDetail>; //得到增加的质控项目源

            List<EntityDicQcItem> itemSelect = listItem.Where(w => w.ItmSelect == 1).ToList(); //得到选中的数据

            if (itemSelect.Count > 0)
            {
                for (int i = 0; i < itemSelect.Count; i++)
                {
                    if (listQcItem.Where(w => w.MatItmId == itemSelect[i].ItmId).ToList().Count == 0)
                    {
                        //增加质控项目
                        EntityDicQcMateriaDetail detail = new EntityDicQcMateriaDetail();
                        detail.MatItrId = insId;
                        detail.MatItmId = itemSelect[i].ItmId;
                        detail.MatId = parId;
                        detail.MatItmX = null;
                        detail.MatItmSd = null;
                        detail.MatItmCv = null;
                        detail.MatItmCcv = null;
                        detail.QcrItmName = itemSelect[i].ItmEcode;
                        detail.PyCode = itemSelect[i].PyCode;
                        detail.WbCode = itemSelect[i].WbCode;
                        detail.ItmName = itemSelect[i].ItmName;

                        listQcItem.Add(detail);

                        listItem.Remove(itemSelect[i]);//删除已增加的项目

                        gvItem.RefreshData();
                        gvQcItem.RefreshData();
                    }
                }
            }
        }

        /// <summary>
        /// 删除添加的指控项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelQcItem_Click(object sender, EventArgs e)
        {
            deleteQcItem();
        }

        /// <summary>
        /// 删除项目方法
        /// </summary>
        private void deleteQcItem()
        {
            List<EntityDicQcItem> listItem = gcItem.DataSource as List<EntityDicQcItem>; //得到项目数据源
            List<EntityDicQcMateriaDetail> listQcItem = gcQcItem.DataSource as List<EntityDicQcMateriaDetail>; //得到增加的质控项目源
            EntityDicQcMateriaDetail eyQcItem = gvQcItem.GetFocusedRow() as EntityDicQcMateriaDetail;

            if (eyQcItem != null)
            {
                EntityDicQcItem eyDQcItem = new EntityDicQcItem();
                eyDQcItem.ItmSelect = 0;
                eyDQcItem.ItmId = eyQcItem.MatItmId;
                eyDQcItem.ItmName = eyQcItem.ItmName;
                eyDQcItem.ItmEcode = eyQcItem.QcrItmName;
                eyDQcItem.PyCode = eyQcItem.PyCode;
                eyDQcItem.WbCode = eyQcItem.WbCode;

                listItem.Add(eyDQcItem);

                listQcItem.Remove(eyQcItem);

                gvItem.RefreshData();
                gvQcItem.RefreshData();
            }
        }


        private void FrmAddQcItem_FormClosed(object sender, FormClosedEventArgs e)
        {
            ClickEventArgs cea = new ClickEventArgs();
            cea.State = 1;
            checkQcItem(cea);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysAddItem_OnBtnSaveClicked(object sender, EventArgs e)
        {
            sysAddItem.Focus();

            List<EntityDicQcMateriaDetail> listQcItem = gcQcItem.DataSource as List<EntityDicQcMateriaDetail>;

            //验证项目数值是否符合标准
            if (listQcItem.Count > 0)
            {
                foreach (var infoQcItem in listQcItem)
                {
                    try
                    {
                        if (Convert.ToDouble(infoQcItem.MatItmX) <= 0)
                        {
                            lis.client.control.MessageDialog.Show("靶值不能小于等于0！", "提示");
                            return;
                        }
                    }
                    catch (Exception)
                    {
                        lis.client.control.MessageDialog.Show("请输入正确的靶值！", "提示");
                    }
                    try
                    {
                        if (Convert.ToDouble(infoQcItem.MatItmSd) <= 0)
                        {
                            lis.client.control.MessageDialog.Show("标准差不能小于等于0！", "提示");
                            return;
                        }
                    }
                    catch (Exception)
                    {
                        lis.client.control.MessageDialog.Show("请输入正确的标准差！", "提示");
                        return;
                    }
                    if (infoQcItem.MatItmCcv != null && infoQcItem.MatItmCcv.ToString().Trim() != "")
                    {
                        try
                        {
                            if (Convert.ToDouble(infoQcItem.MatItmCcv) <= 0)
                            {
                                lis.client.control.MessageDialog.Show("CCV不能小于等于0！", "提示");
                            }
                        }
                        catch (Exception)
                        {
                            lis.client.control.MessageDialog.Show("请输入正确的CCV", "提示");
                            return;
                        }
                    }
                    if (infoQcItem.MatItmCv != null && infoQcItem.MatItmCv.ToString().Trim() != "")
                    {
                        try
                        {
                            Convert.ToDouble(infoQcItem.MatItmCv);
                        }
                        catch (Exception)
                        {
                            lis.client.control.MessageDialog.Show("请输入正确的CV", "提示");
                            return;
                        }
                    }
                    infoQcItem.MatReadValidDate = DateTime.Now; //赋值当前时间，不然保存时会报错
                }

                List<EntityDicQcMateriaRule> listMR = new List<EntityDicQcMateriaRule>();

                for (int i = 0; i < cklRules.CheckedItems.Count; i++)
                {
                    EntityDicQcMateriaRule eyRule = new EntityDicQcMateriaRule();
                    eyRule.RulId = cklRules.CheckedItems[i].ToString();
                    listMR.Add(eyRule);
                }

                proxyAddQcItem.Service.SaveQcMateriaDetailOrRule(listQcItem, listMR);
            }

            this.Close();
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sysAddItem_BtnResetClick(object sender, EventArgs e)
        {
            List<EntityDicQcItem> listItem = gcItem.DataSource as List<EntityDicQcItem>; //得到项目数据源

            bGridHeaderCheckBoxState = false;//清除全选状态
            foreach (var infoItem in listItem)
            {
                infoItem.ItmSelect = 0;//设置项目为未选状态
            }

            List<EntityDicQcMateriaDetail> listQcItem = gcQcItem.DataSource as List<EntityDicQcMateriaDetail>;//得到增加的质控项目源

            int count = listQcItem.Count - 1;
            for (int i = count; i >= 0; i--)//删除选中的项目，放回项目列表，供下次选则
            {
                EntityDicQcItem eyDQcItem = new EntityDicQcItem();
                eyDQcItem.ItmSelect = 0;
                eyDQcItem.ItmId = listQcItem[i].MatItmId;
                eyDQcItem.ItmName = listQcItem[i].ItmName;
                eyDQcItem.ItmEcode = listQcItem[i].QcrItmName;
                eyDQcItem.PyCode = listQcItem[i].PyCode;
                eyDQcItem.WbCode = listQcItem[i].WbCode;

                listItem.Add(eyDQcItem);

                listQcItem.Remove(listQcItem[i]);

                gvItem.RefreshData();
                gvQcItem.RefreshData();
            }
            if (listItem.Count > 0)
            {
                gvItem.FocusedRowHandle = 0;//重置后，焦点放在第一条数据
            }
            this.txtSearch.Focus();
        }

        /// <summary>
        /// 双击添加项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcItem_DoubleClick(object sender, EventArgs e)
        {
            if (gvItem.GetFocusedRow() != null)
            {
                List<EntityDicQcItem> listItem = gcItem.DataSource as List<EntityDicQcItem>; //得到项目数据源
                List<EntityDicQcMateriaDetail> listQcItem = gcQcItem.DataSource as List<EntityDicQcMateriaDetail>; //得到增加的质控项目源
                EntityDicQcItem eyQcItem = gvItem.GetFocusedRow() as EntityDicQcItem;//得到选中的数据

                if (listQcItem.Where(w => w.MatItmId == eyQcItem.ItmId).ToList().Count == 0)
                {
                    //增加质控项目
                    EntityDicQcMateriaDetail detail = new EntityDicQcMateriaDetail();
                    detail.MatItrId = insId;
                    detail.MatItmId = eyQcItem.ItmId;
                    detail.MatId = parId;
                    detail.MatItmX = null;
                    detail.MatItmSd = null;
                    detail.MatItmCv = null;
                    detail.MatItmCcv = null;
                    detail.QcrItmName = eyQcItem.ItmEcode;
                    detail.PyCode = eyQcItem.PyCode;
                    detail.WbCode = eyQcItem.WbCode;
                    detail.ItmName = eyQcItem.ItmName;

                    listQcItem.Add(detail);

                    listItem.Remove(eyQcItem);//删除已增加的项目

                    gvItem.RefreshData();
                    gvQcItem.RefreshData();
                }
            }
        }

        /// <summary>
        /// 删除添加的项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gcQcItem_DoubleClick(object sender, EventArgs e)
        {
            deleteQcItem();
        }

        private void txtSd_EditValueChanged(object sender, EventArgs e)
        {
            EntityDicQcMateriaDetail entityMD = gvQcItem.GetFocusedRow() as EntityDicQcMateriaDetail;
            if (entityMD != null)
            {
                string stSD = gvQcItem.EditingValue.ToString();
                if (entityMD.MatItmX != null && entityMD.MatItmX.ToString().Trim() != "" && stSD.Trim() != "")
                {
                    try
                    {
                        //entityMD.MatItmCv = ((Convert.ToDouble(stSD) / Convert.ToDouble(entityMD.MatItmX)) * 100).ToString("0.000");
                        entityMD.MatItmCv = Convert.ToDecimal(Math.Round((Convert.ToDouble(stSD) / Convert.ToDouble(entityMD.MatItmX) * 100), 3));//取小数点后三位
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
            }
        }

        private void txtCV_EditValueChanged(object sender, EventArgs e)
        {
            //DataRow dr = gvQcItem.GetFocusedDataRow();
            EntityDicQcMateriaDetail entityMD = gvQcItem.GetFocusedRow() as EntityDicQcMateriaDetail;
            if (entityMD != null)
            {
                string stCV = gvQcItem.EditingValue.ToString();
                if (entityMD.MatItmX != null && entityMD.MatItmX.ToString().Trim() != "" && stCV.Trim() != "")
                {
                    try
                    {
                        entityMD.MatItmSd = Convert.ToDecimal(Math.Round((Convert.ToDouble(stCV) * Convert.ToDouble(entityMD.MatItmX) / 100), 3));//取小数点后三位
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
            }
        }

    }


    public class ClickEventArgs : EventArgs
    {
        public int State
        {
            get;
            set;
        }
    }
}
