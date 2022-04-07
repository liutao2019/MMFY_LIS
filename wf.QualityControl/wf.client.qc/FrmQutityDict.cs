using System;
using System.Windows.Forms;
using dcl.client.frame;
using System.Reflection;

namespace dcl.client.qc
{
    public partial class FrmQutityDict : FrmCommon
    {
        public FrmQutityDict()
        {
            InitializeComponent();
        }

        private void Nav_LinkClick(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (e == null)
                return;
            string FrmName = e.Link?.Item?.Name;
            string FrmCaption = e.Link?.Item?.Caption;
            Control curConDict = null;
            foreach (Control ctrl in this.groupControl1.Controls)
            {
                if (ctrl.Name == FrmName && FrmName != "ConType")
                {
                    curConDict = ctrl;
                    break;
                }
            }
            if (curConDict == null)
            {
                //如果还没创建
                Assembly assembly = Assembly.GetExecutingAssembly();
                Type type;

                type = assembly.GetType("dcl.client.qc." + FrmName, false, true);

                if (type == null)
                {
                    lis.client.control.MessageDialog.Show(FrmName + "不存在，无法加载，请联系管理员重新配置");
                    return;
                }
                BindingFlags bflags = BindingFlags.DeclaredOnly | BindingFlags.Public
                                                            | BindingFlags.NonPublic | BindingFlags.Instance;
                Object obj = null;
                try
                {
                    obj = type.InvokeMember("UserControl", bflags | BindingFlags.CreateInstance, null, null, null);
                }
                catch (Exception ex)
                {
                    AppError.show(ex);
                }
                if (obj == null) return;

                curConDict = (Control)obj;

                //barAction.DoRefresh();
                this.groupControl1.Controls.Add(curConDict);
                curConDict.Dock = DockStyle.Fill;
            }


            curConDict.BringToFront();
            groupControl1.Text = FrmCaption;
            this.Text = FrmCaption;
        }
    }
}
