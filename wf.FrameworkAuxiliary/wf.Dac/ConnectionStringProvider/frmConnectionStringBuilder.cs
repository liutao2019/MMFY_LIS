using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace Lib.DAC
{
    public partial class frmConnectionStringBuilder : Form
    {
        public frmConnectionStringBuilder(bool showOutputTextbox)
        {
            InitializeComponent();
            if (!showOutputTextbox)
            {
                this.txtOutputString.Visible = false;
                this.Height -= this.txtOutputString.Height;
            }
        }

        private void frmConnectionStringBuilder_Load(object sender, EventArgs e)
        {
            this.txtDriver.Items.AddRange(Enum.GetNames(typeof(Lib.DAC.EnumDbDriver)));
            this.txtDriver.SelectedItem = Lib.DAC.EnumDbDriver.MSSql.ToString();

            this.txtDialet.Items.AddRange(Enum.GetNames(typeof(Lib.DAC.EnumDataBaseDialet)));
            this.txtDialet.SelectedItem = Lib.DAC.EnumDataBaseDialet.SQL2005.ToString();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            EnumDbDriver driver = DbDriverHelper.GetDriverTypeByName(this.txtDriver.SelectedItem.ToString());
            EnumDataBaseDialet dialet = DbDialetHelper.GetDialetTypeByName(this.txtDialet.SelectedItem.ToString());

            Lib.DAC.ConnectionStringProvider.ConnectionStringBuilder builder = Lib.DAC.ConnectionStringProvider.ConnectionStrBuilderHelper.CreateConnStrBuilder(this.txtDriver.SelectedItem.ToString(), this.txtDialet.SelectedItem.ToString());
            builder.Server = this.txtServer.Text;
            builder.DbName = this.txtDataBase.Text;
            builder.LoginName = this.txtLoginName.Text;
            builder.LoginPassword = this.txtLoginPassword.Text;

            this.txtOutputString.Text = builder.Build();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }
    }
}
