using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using System.Reflection;
using System.Windows.Forms;
using dcl.client.frame;
using System.IO;
using System.Text;

namespace wf.ClientEntrance
{
    partial class FrmAbout : FrmCommon
    {
        private bool isNoLogo = false;
        public FrmAbout()
        {
            isNoLogo =true;
            InitializeComponent();
            this.Text = String.Format("关于 {0}", isNoLogo?"医学检验信息系统":AssemblyTitle);
            this.labelProductName.Text = isNoLogo ? "医学检验信息系统" : AssemblyProduct;
            this.labelVersion.Text = String.Format("版本 {0}", AssemblyVersion);
            this.labelCompanyName.Text = isNoLogo?"":AssemblyCompany;
            txtCopyRight.Text = AssemblyDescription;
            //this.textBoxDescription.Text = AssemblyDescription;
        }

        #region 程序集属性访问器

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {

                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }

                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion

        private void FrmAbout_Load(object sender, EventArgs e)
        {
            txtCopyRight.BackColor = this.txtUpdate.BackColor = this.BackColor;
            //
            txtUpdate.Text = ReadFromFile("update.txt");
        }

        private string ReadFromFile(string fileName)
        {
            string result = "";
            try
            {
                if (File.Exists(fileName))
                {
                    result = File.ReadAllText(fileName, Encoding.UTF8);

                    string[] strs = result.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    StringBuilder sb = new StringBuilder();
                    foreach (string str in strs)
                    {
                        if (str.Trim().StartsWith("@") || str.Trim().StartsWith("#"))
                        {

                        }
                        else
                        {
                            sb.Append(str + "\r\n");
                        }
                    }
                    result = sb.ToString();
                }
            }
            catch
            {
                return "";
            }

            return result;
        }
    }
}
