using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lis.client.control
{
    public interface IsignInterface
    {
        string OperatorID { get; set; }
        string OperatorName { get; set; }
        string FuncInfoID { get; set; }
        string OperatorSftId { get; set; }
        string FuncCode { get; set; }
        string ModuleName { get; set; }
        bool Power { get; set; } 
        string PowerName { get; set; }
        string Pat_i_code { get; set; }
        string PassWord { get; set; }

        /// <summary>
        /// 内容返回
        /// </summary>
        string Comment { get; set; }

        /// <summary>
        /// 验证方式
        /// </summary>
        string CheckType { get; set; }
        /// <summary>
        /// CA签名认证操作
        /// </summary>
        Lis.Client.CASign.FrmUserInfo CAUserInfo { get; set; }
        /// <summary>
        /// 是否CA电子签名验证模式
        /// </summary>
        string strCASignMode { get; set; }

        bool CheckUserLogin();
    }
}
