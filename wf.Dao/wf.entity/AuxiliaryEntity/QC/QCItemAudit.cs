using System;
using System.Collections.Generic;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public class QCItemAudit
    {
        public QCItemAudit()
        {
        }

        private string itemId;
        private DateTime stateTime;
        private DateTime endTime;
        private string qcParDetailId;
        private bool isCheckGrubbs;
        private string itrId;

        /// <summary>
        /// 仪器ID
        /// </summary>
        public string ItrId
        {
            get { return itrId; }
            set { itrId = value; }
        }

        /// <summary>
        /// 是否启用即刻发判断
        /// </summary>
        public bool IsCheckGrubbs
        {
            get { return isCheckGrubbs; }
            set { isCheckGrubbs = value; }
        }

        /// <summary>
        /// 项目id
        /// </summary>
        public string ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StateTime
        {
            get { return stateTime; }
            set { stateTime = value; }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

        /// <summary>
        /// 批号
        /// </summary>
        public string QcParDetailId
        {
            get { return qcParDetailId; }
            set { qcParDetailId = value; }
        }

    }
}
