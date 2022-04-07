using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC
{
    /// <summary>
    /// SqlString注释
    /// </summary>
    [Serializable]
    public class SqlStringComment : ISqlStringPart
    {
        private string _comment = null;
        private bool _isBlock;

        public SqlStringComment(string comment, bool isBlock)
        {
            this._comment = comment;
            this._isBlock = isBlock;
            this.CanRemove = true;
        }

        /// <summary>
        /// 是否为块注释(/*  */)
        /// </summary>
        public bool IsBlockComment
        {
            get
            {
                return this._isBlock;
            }
        }

        /// <summary>
        /// 注释是否可移除
        /// </summary>
        public bool CanRemove { get; set; }

        public override string ToString()
        {
            return this._comment;
        }
    }
}
