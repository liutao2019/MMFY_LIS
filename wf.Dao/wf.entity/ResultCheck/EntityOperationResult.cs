using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace dcl.entity
{
    [Serializable]
    public class EntityOperationResult
    {
        public DataSet ReturnResult { get; set; }

        public Dictionary<string, object> DclReturnResult { get; set; }
        public string OperationName { get; set; }

        public EntityOperationResultData Data { get; set; }
        public object OperationResultData { get; set; }

        public bool Success
        {
            get
            {
                return (Message.Count == 0);
            }
        }

        public bool HasError
        {
            get
            {
                foreach (EntityOperationError item in this.Message)
                {
                    if (item.ErrorLevel == EnumOperationErrorLevel.Error
                        || item.ErrorCode == EnumOperationErrorCode.Exception)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private bool canContinue;

        /// <summary>
        /// 是否能继续操作
        /// </summary>
        public bool CanContinue
        {
            get
            {
                return this.canContinue;
            }
        }

        /// <summary>
        /// 错误列表
        /// </summary>
        public List<EntityOperationError> Message { get; set; }

        ///// <summary>
        ///// 自定义消息
        ///// </summary>
        //public List<EntityOperationError> CustomMessage { get; set; }

        private EntityOperationError GetSingleMessage(EnumOperationErrorCode code)
        {
            foreach (EntityOperationError err in Message)
            {
                if (err.ErrorCode == code)
                {
                    return err;
                }
            }

            return null;
        }

        private EntityOperationError GetSingleCustomMessage(string message_key)
        {
            if (message_key != null && message_key.Trim() == string.Empty)
            {
                message_key = string.Empty;
            }

            foreach (EntityOperationError err in this.Message)
            {
                if (err.ErrorCode == EnumOperationErrorCode.CustomMessage && err.CustomMessageKey == message_key)
                {
                    return err;
                }
            }

            return null;
        }

        public bool HasExcaptionError
        {
            get
            {
                return GetSingleMessage(EnumOperationErrorCode.Exception) != null;
            }
        }

        public void AddMessage(EnumOperationErrorCode code, string msg, EnumOperationErrorLevel lv)
        {
            if (lv == EnumOperationErrorLevel.None)
            {
                return;
            }

            this.canContinue = this.canContinue && (lv != EnumOperationErrorLevel.Error);

            EntityOperationError err = GetSingleMessage(code);

            if (err != null)
            {
                if (string.IsNullOrEmpty(err.Param))
                {
                    err.Param = msg;
                }
                else
                {
                    err.Param += "," + msg;
                }
            }
            else
            {
                err = new EntityOperationError();
                err.ErrorCode = code;
                err.Param = msg;
                Message.Add(err);
            }
            err.ErrorLevel = lv;
        }


        /// <summary>
        /// 添加操作消息
        /// </summary>
        /// <param name="type"></param>
        public void AddMessage(EnumOperationErrorCode type, EnumOperationErrorLevel lv)
        {
            AddMessage(type, string.Empty, lv);
        }

        public void AddCustomMessage(string message_key, string title, string msg, EnumOperationErrorLevel lv)
        {
            if (lv == EnumOperationErrorLevel.None)
            {
                return;
            }

            if (message_key != null && message_key.Trim() == string.Empty)
            {
                message_key = null;
            }

            this.canContinue = this.canContinue && (lv != EnumOperationErrorLevel.Error);

            EntityOperationError err = GetSingleCustomMessage(message_key);

            if (err != null)
            {
                if (string.IsNullOrEmpty(err.Param))
                {
                    err.Param = msg;
                }
                else
                {
                    err.Param += "," + msg;
                }
            }
            else
            {
                err = new EntityOperationError();
                err.ErrorCode = EnumOperationErrorCode.CustomMessage;
                err.Param = msg;
                err.CustomMessageKey = message_key;
                err.CustomMessageTitle = title;
                Message.Add(err);
            }
            err.ErrorLevel = lv;
        }

        /// <summary>
        /// 合并
        /// </summary>
        /// <param name="source"></param>
        public void Merge(EntityOperationResult source)
        {
            if (source != null)
            {
                foreach (EntityOperationError err in source.Message)
                {
                    if (err.ErrorCode == EnumOperationErrorCode.CustomMessage)
                    {
                        this.AddCustomMessage(err.CustomMessageKey, err.CustomMessageTitle, err.Param, err.ErrorLevel);
                    }
                    else
                    {
                        this.AddMessage(err.ErrorCode, err.Param, err.ErrorLevel);
                    }
                }
            }
        }

        #region .ctor
        /// <summary>
        /// .ctor
        /// </summary>
        public EntityOperationResult()
        {
            this.Data = new EntityOperationResultData();
            this.canContinue = true;
            this.Message = new List<EntityOperationError>();
        }
        #endregion

        public string Key { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", this.Data.Patient.RepId, this.Data.Patient.RepSid, this.Data.Patient.PidName);
        }
    }
}
