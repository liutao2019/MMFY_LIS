namespace dcl.client.frame
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class AppError : dcl.client.frame.Form1
    {
        private IContainer components = null;
        private static AppError frm;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        public TextBox textBox1;
        public TextBox textBox2;

        public AppError()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        public static AppError getErrFrm()
        {
            if (frm == null)
            {
                frm = new AppError();
            }
            return frm;
        }

        private void InitializeComponent()
        {
            this.tabControl1 = new TabControl();
            this.tabPage1 = new TabPage();
            this.textBox1 = new TextBox();
            this.tabPage2 = new TabPage();
            this.textBox2 = new TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            base.SuspendLayout();
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = DockStyle.Fill;
            this.tabControl1.Location = new Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new Size(0x124, 0xb3);
            this.tabControl1.TabIndex = 0;
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.Location = new Point(4, 0x15);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new Padding(3);
            this.tabPage1.Size = new Size(0x11c, 0x9a);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "系统错误";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.textBox1.BackColor = SystemColors.ActiveBorder;
            this.textBox1.BorderStyle = BorderStyle.None;
            this.textBox1.Dock = DockStyle.Fill;
            this.textBox1.Font = new Font("宋体", 10.5f, FontStyle.Regular, GraphicsUnit.Point, 0x86);
            this.textBox1.ForeColor = Color.Black;
            this.textBox1.Location = new Point(3, 3);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new Size(0x116, 0x94);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "系统异常，请不要关闭此对话框同时联系管理员帮助查错";
            this.tabPage2.Controls.Add(this.textBox2);
            this.tabPage2.Location = new Point(4, 0x15);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new Padding(3);
            this.tabPage2.Size = new Size(0x11c, 0x9a);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "详细错误原因";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.textBox2.Dock = DockStyle.Fill;
            this.textBox2.ForeColor = Color.FromArgb(0xc0, 0, 0);
            this.textBox2.Location = new Point(3, 3);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new Size(0x116, 0x94);
            this.textBox2.TabIndex = 2;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x124, 0xb3);
            base.Controls.Add(this.tabControl1);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "AppError";
            base.ShowInTaskbar = false;
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "错误";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            base.ResumeLayout(false);
        }

        public static void show(Exception ex)
        {
            AppError error = getErrFrm();
            error.textBox2.Text = ex.ToString();
            if (error.tabControl1.TabPages.Count < 2)
            {
                error.tabControl1.TabPages.Add(error.tabPage2);
            }
            error.ShowDialog();
        }

        public static void show(string error, ArrayList ex)
        {
            AppError error2 = getErrFrm();
            error2.textBox1.Text = error;
            for (int i = 0; i < (ex.Count - 1); i++)
            {
                error2.textBox1.Text = error2.textBox1.Text + "      " + ex[i].ToString();
            }
            if (error2.tabControl1.TabPages.Count > 1)
            {
                error2.tabControl1.TabPages.Remove(error2.tabPage2);
            }
            error2.ShowDialog();
        }

        public static void show(string error, Exception ex)
        {
            AppError error2 = getErrFrm();
            error2.textBox1.Text = error;
            error2.textBox2.Text = ex.ToString();
            if (error2.tabControl1.TabPages.Count < 2)
            {
                error2.tabControl1.TabPages.Add(error2.tabPage2);
            }
            error2.ShowDialog();
        }

        public static void show(string error, string ex)
        {
            AppError error2 = getErrFrm();
            error2.Text = "提示";
            error2.tabPage1.Text = "";
            error2.textBox1.Text = error;
            if (error2.tabControl1.TabPages.Count > 1)
            {
                error2.tabControl1.TabPages.Remove(error2.tabPage2);
            }
            error2.ShowDialog();
        }
    }
}

