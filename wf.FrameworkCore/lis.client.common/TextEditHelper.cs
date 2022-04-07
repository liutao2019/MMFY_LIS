using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraEditors;

namespace dcl.client.common
{
    public class TextEditHelper
    {
        /// <summary>
        /// 在Text中插入字符
        /// </summary>
        /// <param name="str"></param>
        public static void InsertStringIntoMemoEdit(TextEdit textEdit, string str)
        {
            int start = textEdit.SelectionStart;
            textEdit.Text = textEdit.Text.Insert(start, str);
            textEdit.Select(start + str.Length, 0);
            textEdit.Focus();
        }
    }
}
