using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraNavBar;
using DevExpress.XtraBars;
using dcl.client.common;
using dcl.client.frame;
using System.Reflection;
using dcl.client.wcf;
using Lib.DataInterface.Implement;

namespace dcl.client.interfaces
{
    public partial class FrmHIS : FrmCommon
    {
        public FrmHIS()
        {
            InitializeComponent();
        }

        private void navBarControl1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            if (e.Link.Item.Name.ToLower() == "btnconnectionnew" ||
                 e.Link.Item.Name.ToLower() == "btncommandnew"
               )
            {
                return;
            }

            Control curConDict = null;
            String conName = e.Link.Item.Name.Substring(3);


            foreach (Control con in this.groupControl1.Controls)
            {
                if (con.Name == conName)
                {
                    curConDict = con;
                    break;
                }
            }
            if (curConDict == null)
            { //如果还没创建
                Assembly assembly = Assembly.GetExecutingAssembly();
                Type type = assembly.GetType("dcl.client.interfaces." + conName, false, true);
                if (type == null)
                {
                    lis.client.control.MessageDialog.Show(conName + "不存在，无法加载，请联系管理员重新配置");
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

                // barAction.Refresh(); 
                this.groupControl1.Controls.Add(curConDict);
                curConDict.Dock = DockStyle.Fill;
            }

            curConDict.BringToFront();
            groupControl1.Text = e.Link.Caption;
            this.Text = e.Link.Caption;
        }

        private void btnConnectionNew_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            //注释 2018-06-11
            frmDataInterfaceConnectionEditor frm = new frmDataInterfaceConnectionEditor(EnumDataAccessMode.Custom);
            frm.ShowDialog();
        }

        private void btnCommandNew_LinkClicked(object sender, NavBarLinkEventArgs e)
        {
            try
            {
                List<string> listBarcodeBinding = new List<string>();
                listBarcodeBinding.Add("bc_in_no");
                listBarcodeBinding.Add("bc_pid");
                listBarcodeBinding.Add("bc_yz_id");
                listBarcodeBinding.Add("bc_his_code");
                listBarcodeBinding.Add("bc_his_name");
                listBarcodeBinding.Add("bc_name");
                listBarcodeBinding.Add("op_code");
                listBarcodeBinding.Add("op_time");
                listBarcodeBinding.Add("bc_times");

                Dictionary<string, string[]> list = new Dictionary<string, string[]>();
                list.Add("条码_门诊_下载后", listBarcodeBinding.ToArray());
                list.Add("条码_住院_下载后", listBarcodeBinding.ToArray());
                list.Add("条码_体检_下载后", listBarcodeBinding.ToArray());
                list.Add("条码_其他_下载后", listBarcodeBinding.ToArray());

                list.Add("条码_门诊_签收后", listBarcodeBinding.ToArray());
                list.Add("条码_住院_签收后", listBarcodeBinding.ToArray());
                list.Add("条码_体检_签收后", listBarcodeBinding.ToArray());
                list.Add("条码_其他_签收后", listBarcodeBinding.ToArray());

                list.Add("条码_门诊_删除后", listBarcodeBinding.ToArray());
                list.Add("条码_住院_删除后", listBarcodeBinding.ToArray());
                list.Add("条码_体检_删除后", listBarcodeBinding.ToArray());
                list.Add("条码_其他_删除后", listBarcodeBinding.ToArray());

                list.Add("二审后_危急值", new string[] { });
                list.Add("二审后_门诊_危急值", new string[] { });
                list.Add("二审后_住院_危急值", new string[] { });
                list.Add("二审后_体检_危急值", new string[] { });
                list.Add("二审后_其他_危急值", new string[] { });

                //list.Add("二审后_门诊_成功后", new string[] { });
                //list.Add("二审后_住院_成功后", new string[] { });
                //list.Add("二审后_体检_成功后", new string[] { });
                //list.Add("二审后_其他_成功后", new string[] { });


                List<string> listLabBinding = new List<string>();
                listLabBinding.Add("pat_id");
                listLabBinding.Add("pat_name");
                listLabBinding.Add("pat_in_no");
                listLabBinding.Add("pat_c_name");
                listLabBinding.Add("pat_report_code");
                listLabBinding.Add("pat_report_date");
                listLabBinding.Add("op_time");

                list.Add("检验_门诊_二审后", listLabBinding.ToArray());
                list.Add("检验_住院_二审后", listLabBinding.ToArray());
                list.Add("检验_体检_二审后", listLabBinding.ToArray());
                list.Add("检验_其他_二审后", listLabBinding.ToArray());

                list.Add("@ID号转换", new string[] { });

                list.Add("@身份验证", new string[] { });
                frmDataInterfaceCommandEditor frm = new frmDataInterfaceCommandEditor(EnumDataAccessMode.Custom);
                frm.SetGroupFieldStyle(list, null, true, true);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}