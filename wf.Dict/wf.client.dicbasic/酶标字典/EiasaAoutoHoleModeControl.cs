using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using dcl.client.elisa;

namespace dcl.client.dicbasic
{
    public partial class EiasaAoutoHoleModeControl : UserControl
    {
        #region 全局变量
        SampleControl SamleCon = null;
        #endregion
        public EiasaAoutoHoleModeControl()
        {
            InitializeComponent();
        }

        public EiasaAoutoHoleModeControl(SampleControl p_SamleCon)
        {
            InitializeComponent();
            SamleCon = p_SamleCon;
        }

        private void radioButtonDefu_CheckedChanged(object sender, EventArgs e)
        {
            if (SamleCon==null)
            {
                return;
            }
            
            if (this.rbnMouseAuto.Checked)
            {
                this.SamleCon.blnAoutoSetSeq = true;
            }
            else
            {
                this.SamleCon.blnAoutoSetSeq = false; 
            }

        }

        private void btnAoutoSeq_Click(object sender, EventArgs e)
        {
            int intRows = 0;
            int intCols = 0;
            int intResult = 0;
            if (this.rbnLater.Checked)
            {
                if (!string.IsNullOrEmpty(this.txtLaterCol.Text.Trim()) && !string.IsNullOrEmpty(this.txtLaterRow.Text.Trim()))
                {
                    if (int.TryParse(this.txtLaterCol.Text.Trim(), out intResult)&&int.TryParse(this.txtLaterRow.Text.Trim(), out intResult))
                    {
                        intRows = Int32.Parse(this.txtLaterRow.Text.Trim());
                        intCols = Int32.Parse(this.txtLaterCol.Text.Trim());

                    }
                    else
                    {
                        lis.client.control.MessageDialog.Show("请输入正确的行数与列数！", "提示信息");
                        return;
                    }


                }

                //填充限制模板数量
                this.SamleCon.m_mthLaterAllFill(intRows, intCols);
            }
            else if (this.rbnLong.Checked)
            {
               
                

               
                 if (!string.IsNullOrEmpty(this.txtLongCol.Text.Trim())&&!string.IsNullOrEmpty(this.txtLongRow.Text.Trim()))
                 {
                     if (int.TryParse(this.txtLongCol.Text.Trim(),out intResult)&&int.TryParse(this.txtLongRow.Text.Trim(),out intResult))
	
                     {
                       intRows = Int32.Parse(this.txtLongRow.Text.Trim());
                       intCols = Int32.Parse(this.txtLongCol.Text.Trim());
	
                     }
                     else
	
                     {
                          lis.client.control.MessageDialog.Show("请输入正确的行数与列数！", "提示信息");
                           return;
                     }
                                
                    
                 }
                
               //填充限制模板数量
                this.SamleCon.m_mthLongAllFill(intRows,intCols);
            }
        }

       
    }
}
