namespace dcl.client.frame
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public class Form1 : System.Windows.Forms.Form
    {
        private const int IME_CHOTKEY_SHAPE_TOGGLE = 0x11;
        private const int IME_CMODE_FULLSHAPE = 8;
        private bool m_FirstStart = true;
        public string module_code = "";

        [DllImport("imm32.dll")]
        private static extern IntPtr ImmGetContext(IntPtr hwnd);
        [DllImport("imm32.dll")]
        private static extern bool ImmGetConversionStatus(IntPtr himc, ref int lpdw, ref int lpdw2);
        [DllImport("imm32.dll")]
        private static extern bool ImmGetOpenStatus(IntPtr himc);
        [DllImport("imm32.dll")]
        private static extern bool ImmSetOpenStatus(IntPtr himc, bool b);
        [DllImport("imm32.dll")]
        private static extern int ImmSimulateHotKey(IntPtr hwnd, int lngHotkey);
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            IntPtr himc = ImmGetContext(base.Handle);
            if (ImmGetOpenStatus(himc))
            {
                int lpdw = 0;
                int num2 = 0;
                if (ImmGetConversionStatus(himc, ref lpdw, ref num2))
                {
                    ImmSimulateHotKey(base.Handle, 0x11);
                }
            }
            if (this.m_FirstStart)
            {
                this.SetFormImeToHangul(this);
                this.m_FirstStart = false;
            }
        }

        private void p_Control_Enter(object sender, EventArgs e)
        {
            (sender as Control).ImeMode = ImeMode.Hangul;
        }

        private void p_Control_KeyDown(object sender, KeyEventArgs e)
        {
            (sender as Control).ImeMode = ImeMode.Hangul;
        }

        private void p_Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            int index = "０１２３４５６７８９。．".IndexOf(e.KeyChar);
            if (index > -1)
            {
                e.KeyChar = "0123456789.."[index];
            }
        }

        private void SetControlImeToHangul(Control p_Control)
        {
            if (p_Control.HasChildren)
            {
                foreach (Control control in p_Control.Controls)
                {
                    this.SetControlImeToHangul(control);
                }
            }
            p_Control.KeyDown += new KeyEventHandler(this.p_Control_KeyDown);
            p_Control.KeyPress += new KeyPressEventHandler(this.p_Control_KeyPress);
            p_Control.Enter += new EventHandler(this.p_Control_Enter);
        }

        private void SetFormImeToHangul(System.Windows.Forms.Form p_Form)
        {
            foreach (Control control in p_Form.Controls)
            {
                this.SetControlImeToHangul(control);
            }
        }
    }
}

