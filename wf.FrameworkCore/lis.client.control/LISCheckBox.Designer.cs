﻿namespace lis.client.control
{
  partial class LISCheckBox
  {
    /// <summary> 
    /// 必需的设计器变量。
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// 清理所有正在使用的资源。
    /// </summary>
    /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region 组件设计器生成的代码

    /// <summary> 
    /// 设计器支持所需的方法 - 不要
    /// 使用代码编辑器修改此方法的内容。
    /// </summary>
    private void InitializeComponent()
    {
      this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
      ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
      this.SuspendLayout();
      // 
      // checkEdit1
      // 
      this.checkEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.checkEdit1.Location = new System.Drawing.Point(0, 0);
      this.checkEdit1.Name = "checkEdit1";
      this.checkEdit1.Properties.Caption = "checkEdit1";
      this.checkEdit1.Size = new System.Drawing.Size(96, 19);
      this.checkEdit1.TabIndex = 0;
      this.checkEdit1.CheckedChanged += new System.EventHandler(this.checkEdit1_CheckedChanged);
      this.checkEdit1.CheckStateChanged += new System.EventHandler(this.checkEdit1_CheckStateChanged);
      this.checkEdit1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LISCheckBox_KeyPress);
      // 
      // LISCheckBox
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.checkEdit1);
      this.Name = "LISCheckBox";
      this.Size = new System.Drawing.Size(96, 23);
      this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.LISCheckBox_KeyPress);
      ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private DevExpress.XtraEditors.CheckEdit checkEdit1;
  }
}
