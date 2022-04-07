namespace Lib.DataInterface
{
    partial class frmCommandTest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCommandTest));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.pName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pDirection = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.bsParameterDirection = new System.Windows.Forms.BindingSource(this.components);
            this.pDataType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.bsPDataType = new System.Windows.Forms.BindingSource(this.components);
            this.pLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsParameter = new System.Windows.Forms.BindingSource(this.components);
            this.tabExecuteResult = new System.Windows.Forms.TabControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabSetting = new System.Windows.Forms.TabControl();
            this.tbConnection = new System.Windows.Forms.TabPage();
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.txtConnLoginPass = new System.Windows.Forms.TextBox();
            this.lblConnLoginPass = new System.Windows.Forms.Label();
            this.txtConnLoginName = new System.Windows.Forms.TextBox();
            this.lblConnLoginName = new System.Windows.Forms.Label();
            this.txtConnCatelog = new System.Windows.Forms.TextBox();
            this.lblConnCatelog = new System.Windows.Forms.Label();
            this.txtConnDialet = new System.Windows.Forms.ComboBox();
            this.lblConnDialet = new System.Windows.Forms.Label();
            this.txtConnDriver = new System.Windows.Forms.ComboBox();
            this.lblConnDriver = new System.Windows.Forms.Label();
            this.txtConnType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtConnAddress = new System.Windows.Forms.TextBox();
            this.lblConnAddress = new System.Windows.Forms.Label();
            this.tbCommand = new System.Windows.Forms.TabPage();
            this.txtCmdExeType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCmdCommandText = new System.Windows.Forms.RichTextBox();
            this.txtCmdCommandType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbParameter = new System.Windows.Forms.TabPage();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsOpen = new System.Windows.Forms.ToolStripButton();
            this.tsSave = new System.Windows.Forms.ToolStripButton();
            this.tsRun = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsParameterDirection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPDataType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsParameter)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabSetting.SuspendLayout();
            this.tbConnection.SuspendLayout();
            this.tbCommand.SuspendLayout();
            this.tbParameter.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 10.5F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pName,
            this.pDirection,
            this.pDataType,
            this.pLength,
            this.pValue});
            this.dataGridView1.DataSource = this.bsParameter;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(715, 203);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            // 
            // pName
            // 
            this.pName.DataPropertyName = "Name";
            this.pName.HeaderText = "参数名";
            this.pName.Name = "pName";
            // 
            // pDirection
            // 
            this.pDirection.DataPropertyName = "Direction";
            this.pDirection.DataSource = this.bsParameterDirection;
            this.pDirection.HeaderText = "输出方向";
            this.pDirection.Name = "pDirection";
            this.pDirection.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.pDirection.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.pDirection.Width = 120;
            // 
            // pDataType
            // 
            this.pDataType.DataPropertyName = "DataType";
            this.pDataType.DataSource = this.bsPDataType;
            this.pDataType.HeaderText = "数据类型";
            this.pDataType.Name = "pDataType";
            this.pDataType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.pDataType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.pDataType.Width = 150;
            // 
            // pLength
            // 
            this.pLength.DataPropertyName = "Length";
            this.pLength.HeaderText = "长度";
            this.pLength.Name = "pLength";
            this.pLength.Width = 50;
            // 
            // pValue
            // 
            this.pValue.DataPropertyName = "Value";
            this.pValue.HeaderText = "当前值";
            this.pValue.Name = "pValue";
            this.pValue.Width = 200;
            // 
            // tabExecuteResult
            // 
            this.tabExecuteResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabExecuteResult.Location = new System.Drawing.Point(0, 0);
            this.tabExecuteResult.Name = "tabExecuteResult";
            this.tabExecuteResult.SelectedIndex = 0;
            this.tabExecuteResult.Size = new System.Drawing.Size(729, 274);
            this.tabExecuteResult.TabIndex = 1;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabSetting);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabExecuteResult);
            this.splitContainer1.Size = new System.Drawing.Size(729, 516);
            this.splitContainer1.SplitterDistance = 237;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 2;
            // 
            // tabSetting
            // 
            this.tabSetting.Controls.Add(this.tbConnection);
            this.tabSetting.Controls.Add(this.tbCommand);
            this.tabSetting.Controls.Add(this.tbParameter);
            this.tabSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabSetting.Font = new System.Drawing.Font("宋体", 10.5F);
            this.tabSetting.Location = new System.Drawing.Point(0, 0);
            this.tabSetting.Name = "tabSetting";
            this.tabSetting.SelectedIndex = 0;
            this.tabSetting.Size = new System.Drawing.Size(729, 237);
            this.tabSetting.TabIndex = 1;
            this.tabSetting.SelectedIndexChanged += new System.EventHandler(this.tabSetting_SelectedIndexChanged);
            // 
            // tbConnection
            // 
            this.tbConnection.Controls.Add(this.btnTestConnection);
            this.tbConnection.Controls.Add(this.txtConnLoginPass);
            this.tbConnection.Controls.Add(this.lblConnLoginPass);
            this.tbConnection.Controls.Add(this.txtConnLoginName);
            this.tbConnection.Controls.Add(this.lblConnLoginName);
            this.tbConnection.Controls.Add(this.txtConnCatelog);
            this.tbConnection.Controls.Add(this.lblConnCatelog);
            this.tbConnection.Controls.Add(this.txtConnDialet);
            this.tbConnection.Controls.Add(this.lblConnDialet);
            this.tbConnection.Controls.Add(this.txtConnDriver);
            this.tbConnection.Controls.Add(this.lblConnDriver);
            this.tbConnection.Controls.Add(this.txtConnType);
            this.tbConnection.Controls.Add(this.label5);
            this.tbConnection.Controls.Add(this.txtConnAddress);
            this.tbConnection.Controls.Add(this.lblConnAddress);
            this.tbConnection.Location = new System.Drawing.Point(4, 24);
            this.tbConnection.Name = "tbConnection";
            this.tbConnection.Size = new System.Drawing.Size(721, 209);
            this.tbConnection.TabIndex = 1;
            this.tbConnection.Text = " 连  接 ";
            this.tbConnection.UseVisualStyleBackColor = true;
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTestConnection.Location = new System.Drawing.Point(13, 176);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(75, 23);
            this.btnTestConnection.TabIndex = 14;
            this.btnTestConnection.Text = "测试连接";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // txtConnLoginPass
            // 
            this.txtConnLoginPass.Location = new System.Drawing.Point(545, 70);
            this.txtConnLoginPass.Name = "txtConnLoginPass";
            this.txtConnLoginPass.PasswordChar = '*';
            this.txtConnLoginPass.Size = new System.Drawing.Size(103, 23);
            this.txtConnLoginPass.TabIndex = 13;
            // 
            // lblConnLoginPass
            // 
            this.lblConnLoginPass.AutoSize = true;
            this.lblConnLoginPass.Location = new System.Drawing.Point(475, 76);
            this.lblConnLoginPass.Name = "lblConnLoginPass";
            this.lblConnLoginPass.Size = new System.Drawing.Size(77, 14);
            this.lblConnLoginPass.TabIndex = 12;
            this.lblConnLoginPass.Text = "登录密码：";
            // 
            // txtConnLoginName
            // 
            this.txtConnLoginName.Location = new System.Drawing.Point(358, 70);
            this.txtConnLoginName.Name = "txtConnLoginName";
            this.txtConnLoginName.Size = new System.Drawing.Size(103, 23);
            this.txtConnLoginName.TabIndex = 11;
            // 
            // lblConnLoginName
            // 
            this.lblConnLoginName.AutoSize = true;
            this.lblConnLoginName.Location = new System.Drawing.Point(299, 76);
            this.lblConnLoginName.Name = "lblConnLoginName";
            this.lblConnLoginName.Size = new System.Drawing.Size(63, 14);
            this.lblConnLoginName.TabIndex = 10;
            this.lblConnLoginName.Text = "登录名：";
            // 
            // txtConnCatelog
            // 
            this.txtConnCatelog.Location = new System.Drawing.Point(90, 70);
            this.txtConnCatelog.Name = "txtConnCatelog";
            this.txtConnCatelog.Size = new System.Drawing.Size(206, 23);
            this.txtConnCatelog.TabIndex = 8;
            // 
            // lblConnCatelog
            // 
            this.lblConnCatelog.AutoSize = true;
            this.lblConnCatelog.Location = new System.Drawing.Point(10, 76);
            this.lblConnCatelog.Name = "lblConnCatelog";
            this.lblConnCatelog.Size = new System.Drawing.Size(63, 14);
            this.lblConnCatelog.TabIndex = 9;
            this.lblConnCatelog.Text = "数据库：";
            // 
            // txtConnDialet
            // 
            this.txtConnDialet.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtConnDialet.FormattingEnabled = true;
            this.txtConnDialet.Location = new System.Drawing.Point(494, 9);
            this.txtConnDialet.Name = "txtConnDialet";
            this.txtConnDialet.Size = new System.Drawing.Size(103, 22);
            this.txtConnDialet.TabIndex = 3;
            // 
            // lblConnDialet
            // 
            this.lblConnDialet.AutoSize = true;
            this.lblConnDialet.Location = new System.Drawing.Point(409, 13);
            this.lblConnDialet.Name = "lblConnDialet";
            this.lblConnDialet.Size = new System.Drawing.Size(91, 14);
            this.lblConnDialet.TabIndex = 7;
            this.lblConnDialet.Text = "数据库类型：";
            // 
            // txtConnDriver
            // 
            this.txtConnDriver.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtConnDriver.FormattingEnabled = true;
            this.txtConnDriver.Location = new System.Drawing.Point(288, 9);
            this.txtConnDriver.Name = "txtConnDriver";
            this.txtConnDriver.Size = new System.Drawing.Size(103, 22);
            this.txtConnDriver.TabIndex = 2;
            // 
            // lblConnDriver
            // 
            this.lblConnDriver.AutoSize = true;
            this.lblConnDriver.Location = new System.Drawing.Point(205, 13);
            this.lblConnDriver.Name = "lblConnDriver";
            this.lblConnDriver.Size = new System.Drawing.Size(91, 14);
            this.lblConnDriver.TabIndex = 6;
            this.lblConnDriver.Text = "数据库驱动：";
            // 
            // txtConnType
            // 
            this.txtConnType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtConnType.FormattingEnabled = true;
            this.txtConnType.Location = new System.Drawing.Point(90, 9);
            this.txtConnType.Name = "txtConnType";
            this.txtConnType.Size = new System.Drawing.Size(103, 22);
            this.txtConnType.TabIndex = 4;
            this.txtConnType.SelectedIndexChanged += new System.EventHandler(this.txtConnType_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 14);
            this.label5.TabIndex = 5;
            this.label5.Text = "连接类型：";
            // 
            // txtConnAddress
            // 
            this.txtConnAddress.Location = new System.Drawing.Point(90, 41);
            this.txtConnAddress.Name = "txtConnAddress";
            this.txtConnAddress.Size = new System.Drawing.Size(558, 23);
            this.txtConnAddress.TabIndex = 0;
            // 
            // lblConnAddress
            // 
            this.lblConnAddress.AutoSize = true;
            this.lblConnAddress.Location = new System.Drawing.Point(10, 44);
            this.lblConnAddress.Name = "lblConnAddress";
            this.lblConnAddress.Size = new System.Drawing.Size(91, 14);
            this.lblConnAddress.TabIndex = 1;
            this.lblConnAddress.Text = "数据库地址：";
            // 
            // tbCommand
            // 
            this.tbCommand.Controls.Add(this.txtCmdExeType);
            this.tbCommand.Controls.Add(this.label3);
            this.tbCommand.Controls.Add(this.label2);
            this.tbCommand.Controls.Add(this.txtCmdCommandText);
            this.tbCommand.Controls.Add(this.txtCmdCommandType);
            this.tbCommand.Controls.Add(this.label1);
            this.tbCommand.Location = new System.Drawing.Point(4, 24);
            this.tbCommand.Name = "tbCommand";
            this.tbCommand.Size = new System.Drawing.Size(721, 209);
            this.tbCommand.TabIndex = 2;
            this.tbCommand.Text = " 命  令 ";
            this.tbCommand.UseVisualStyleBackColor = true;
            // 
            // txtCmdExeType
            // 
            this.txtCmdExeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtCmdExeType.FormattingEnabled = true;
            this.txtCmdExeType.Location = new System.Drawing.Point(317, 9);
            this.txtCmdExeType.Name = "txtCmdExeType";
            this.txtCmdExeType.Size = new System.Drawing.Size(140, 22);
            this.txtCmdExeType.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 14);
            this.label3.TabIndex = 5;
            this.label3.Text = "执行命令：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(243, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 14);
            this.label2.TabIndex = 4;
            this.label2.Text = "执行方式：";
            // 
            // txtCmdCommandText
            // 
            this.txtCmdCommandText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCmdCommandText.Location = new System.Drawing.Point(9, 62);
            this.txtCmdCommandText.Name = "txtCmdCommandText";
            this.txtCmdCommandText.Size = new System.Drawing.Size(700, 144);
            this.txtCmdCommandText.TabIndex = 2;
            this.txtCmdCommandText.Text = "";
            // 
            // txtCmdCommandType
            // 
            this.txtCmdCommandType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txtCmdCommandType.FormattingEnabled = true;
            this.txtCmdCommandType.Location = new System.Drawing.Point(82, 9);
            this.txtCmdCommandType.Name = "txtCmdCommandType";
            this.txtCmdCommandType.Size = new System.Drawing.Size(140, 22);
            this.txtCmdCommandType.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "命令类型：";
            // 
            // tbParameter
            // 
            this.tbParameter.Controls.Add(this.dataGridView1);
            this.tbParameter.Location = new System.Drawing.Point(4, 24);
            this.tbParameter.Name = "tbParameter";
            this.tbParameter.Padding = new System.Windows.Forms.Padding(3);
            this.tbParameter.Size = new System.Drawing.Size(721, 209);
            this.tbParameter.TabIndex = 0;
            this.tbParameter.Text = " 参  数 ";
            this.tbParameter.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 541);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(729, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsOpen,
            this.tsSave,
            this.tsRun});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(729, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsOpen
            // 
            this.tsOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsOpen.Image = ((System.Drawing.Image)(resources.GetObject("tsOpen.Image")));
            this.tsOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsOpen.Name = "tsOpen";
            this.tsOpen.Size = new System.Drawing.Size(23, 22);
            this.tsOpen.ToolTipText = "打开";
            this.tsOpen.Click += new System.EventHandler(this.tsOpen_Click);
            // 
            // tsSave
            // 
            this.tsSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsSave.Image = ((System.Drawing.Image)(resources.GetObject("tsSave.Image")));
            this.tsSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsSave.Name = "tsSave";
            this.tsSave.Size = new System.Drawing.Size(23, 22);
            this.tsSave.ToolTipText = "保存导出";
            this.tsSave.Click += new System.EventHandler(this.tsSave_Click);
            // 
            // tsRun
            // 
            this.tsRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsRun.Image = ((System.Drawing.Image)(resources.GetObject("tsRun.Image")));
            this.tsRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsRun.Name = "tsRun";
            this.tsRun.Size = new System.Drawing.Size(23, 22);
            this.tsRun.Text = "toolStripButton1";
            this.tsRun.ToolTipText = "执行";
            this.tsRun.Visible = false;
            this.tsRun.Click += new System.EventHandler(this.tsRun_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // frmCommandTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 563);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("宋体", 10.5F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmCommandTest";
            this.Text = "接口测试";
            this.Load += new System.EventHandler(this.frmCommandTest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsParameterDirection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsPDataType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsParameter)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabSetting.ResumeLayout(false);
            this.tbConnection.ResumeLayout(false);
            this.tbConnection.PerformLayout();
            this.tbCommand.ResumeLayout(false);
            this.tbCommand.PerformLayout();
            this.tbParameter.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TabControl tabExecuteResult;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsRun;
        private System.Windows.Forms.TabControl tabSetting;
        private System.Windows.Forms.TabPage tbConnection;
        private System.Windows.Forms.TabPage tbCommand;
        private System.Windows.Forms.TabPage tbParameter;
        private System.Windows.Forms.ComboBox txtCmdCommandType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox txtCmdCommandText;
        private System.Windows.Forms.ComboBox txtCmdExeType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtConnAddress;
        private System.Windows.Forms.Label lblConnAddress;
        private System.Windows.Forms.ComboBox txtConnType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox txtConnDialet;
        private System.Windows.Forms.ComboBox txtConnDriver;
        private System.Windows.Forms.Label lblConnDriver;
        private System.Windows.Forms.Label lblConnDialet;
        private System.Windows.Forms.TextBox txtConnCatelog;
        private System.Windows.Forms.Label lblConnCatelog;
        private System.Windows.Forms.TextBox txtConnLoginPass;
        private System.Windows.Forms.Label lblConnLoginPass;
        private System.Windows.Forms.TextBox txtConnLoginName;
        private System.Windows.Forms.Label lblConnLoginName;
        private System.Windows.Forms.ToolStripButton tsOpen;
        private System.Windows.Forms.ToolStripButton tsSave;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.BindingSource bsParameterDirection;
        private System.Windows.Forms.BindingSource bsParameter;
        private System.Windows.Forms.BindingSource bsPDataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn pName;
        private System.Windows.Forms.DataGridViewComboBoxColumn pDirection;
        private System.Windows.Forms.DataGridViewComboBoxColumn pDataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn pLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn pValue;
    }
}