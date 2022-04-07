using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace lis.client.control
{
    public class MessageDialog
    {
        private MessageDialog()
        {

        }

        public static DialogResult Show(string message, string caption, MessageBoxButtons buttons)
        {
            return Show(message, caption, buttons, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult Show(string message, string caption)
        {
            return Show(message, caption, MessageBoxButtons.OK);
        }

        public static DialogResult Show(string message, MessageBoxButtons buttons)
        {
            return Show(message, "提示", buttons);
        }


        public static DialogResult Show(string message, MessageBoxButtons buttons, MessageBoxDefaultButton defButton)
        {
            return Show(message, "提示", buttons, MessageBoxIcon.None, defButton);
        }

        public static DialogResult Show(string message)
        {
            return Show(message, "提示", MessageBoxButtons.OK);
        }

        public static DialogResult Show(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon, MessageBoxDefaultButton defButton)
        {
            frmMessageDialog frm = new frmMessageDialog(message, caption, buttons, icon, defButton);
            frm.AutoClose = false;
            DialogResult result = frm.ShowDialog();
            return result;
        }

        #region 打开自动关闭窗体
        public static void ShowAutoCloseDialog(string message)
        {
            frmMessageDialog frm = new frmMessageDialog(message, string.Empty, MessageBoxButtons.OK);
            frm.AutoClose = true;
            frm.ShowDialog();
        }

        public static void ShowAutoCloseDialog(string message, decimal time)
        {
            frmMessageDialog frm = new frmMessageDialog(message, string.Empty, MessageBoxButtons.OK, time);
            frm.AutoClose = true;
            frm.ShowDialog();
        }
        #endregion
    }
}
