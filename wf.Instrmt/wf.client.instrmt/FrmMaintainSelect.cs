using System;
using System.Windows.Forms;
using dcl.client.frame;
using dcl.client.common;
using dcl.client.wcf;
using dcl.entity;

namespace dcl.client.instrmt
{
    public partial class FrmMaintainSelect : ConCommon
    {
        public FrmMaintainSelect()
        {
            InitializeComponent();
        }

        private void FrmMaintainSelect_Load(object sender, EventArgs e)
        {
            deStartTime.EditValue = DateTime.Now.AddMonths(-1).Date;
            deEndTime.EditValue = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);
            lueCType.SelectByID(LocalSetting.Current.Setting.CType_id);
        }
        
        private void cmbSelectType_SelectedValueChanged(object sender, EventArgs e)
        {
            setControl(cmbSelectType.Text == "维修信息");
        }

        private void setControl(bool isTrue)
        {
            cmbType.Properties.ReadOnly = isTrue;
            cmbOperate.Properties.ReadOnly = isTrue;
            txtTime.Properties.ReadOnly = isTrue;
            gcMaintRegis.Visible = !isTrue;
            gcServicing.Visible = isTrue;

            gcMaintRegis.Dock = isTrue ? DockStyle.Top : DockStyle.Fill;
            gcServicing.Dock = isTrue ? DockStyle.Fill : DockStyle.Top;
        }

        //查询按钮事件
        private void btnSelect_Click(object sender, EventArgs e)
        {
            EntityDicInstrmtMaintainRegistration registration = new EntityDicInstrmtMaintainRegistration();
            registration.DeStartTime = deStartTime.DateTime;
            registration.DeEndTime = deEndTime.DateTime;
            registration.MaiItrId = lueInstrmt.valueMember;
            registration.ProId =  lueCType.valueMember;

            EntityDicItrInstrumentServicing servicing = new EntityDicItrInstrumentServicing();
            servicing.DeStartTime = deStartTime.DateTime;
            servicing.DeEndTime = deEndTime.DateTime;
            servicing.StrCtypeId = lueCType.valueMember;
            servicing.SerItrId = lueInstrmt.valueMember;
            
            ProxyItrInstrumentRegistration proxyRegistration = new ProxyItrInstrumentRegistration();
            ProxyItrInstrumentServicing proxyServicing = new ProxyItrInstrumentServicing();

            if (cmbSelectType.Text == "保养信息")
            {
                registration.StrOperate = cmbOperate.Text;
                registration.StrTimeType = cmbType.Text;
                registration.Hour = Convert.ToInt32(txtTime.EditValue);
                
                gcMaintRegis.DataSource = proxyRegistration.Service.GetMaintainData(registration);

                gvMaintRegis.ExpandAllGroups();
            }
            else
            {
                gcServicing.DataSource = proxyServicing.Service.GetServicingData(servicing);
                gvServicing.ExpandAllGroups();
            }
        }
        

        //根据物理组来过滤仪器
        private void lueCType_ValueChanged(object sender, control.ValueChangeEventArgs args)
        {
            if (!string.IsNullOrEmpty(lueCType.valueMember))
            {
                lueInstrmt.SetFilter(lueInstrmt.getDataSource().FindAll(w => w.ItrLabId == lueCType.valueMember));
            }
            else
            {
                lueInstrmt.SetFilter(lueInstrmt.getDataSource());
            }
        }
        
    }
}
