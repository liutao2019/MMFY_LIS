using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 操作结果
    /// </summary>
    [Serializable]
    public class EntityOperateResult : EntityBase
    {
        public Dictionary<string,object> ReturnResult { get; set; }
        public string OperationName { get; set; }

        public EntityOperateResultData Data { get; set; }
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
                foreach (EntityOperateError item in this.Message)
                {
                    if (item.ErrorLevel == EnumOperateErrorLevel.Error
                        || item.ErrorCode == EnumOperateErrorCode.Exception)
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
        public List<EntityOperateError> Message { get; set; }

        ///// <summary>
        ///// 自定义消息
        ///// </summary>
        //public List<EntityOperationError> CustomMessage { get; set; }

        private EntityOperateError GetSingleMessage(EnumOperateErrorCode code)
        {
            foreach (EntityOperateError err in Message)
            {
                if (err.ErrorCode == code)
                {
                    return err;
                }
            }

            return null;
        }

        private EntityOperateError GetSingleCustomMessage(string message_key)
        {
            if (message_key != null && message_key.Trim() == string.Empty)
            {
                message_key = string.Empty;
            }

            foreach (EntityOperateError err in this.Message)
            {
                if (err.ErrorCode == EnumOperateErrorCode.CustomMessage && err.CustomMessageKey == message_key)
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
                return GetSingleMessage(EnumOperateErrorCode.Exception) != null;
            }
        }

        public void AddMessage(EnumOperateErrorCode code, string msg, EnumOperateErrorLevel lv)
        {
            if (lv == EnumOperateErrorLevel.None)
            {
                return;
            }

            this.canContinue = this.canContinue && (lv != EnumOperateErrorLevel.Error);

            EntityOperateError err = GetSingleMessage(code);

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
                err = new EntityOperateError();
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
        public void AddMessage(EnumOperateErrorCode type, EnumOperateErrorLevel lv)
        {
            AddMessage(type, string.Empty, lv);
        }

        public void AddCustomMessage(string message_key, string title, string msg, EnumOperateErrorLevel lv)
        {
            if (lv == EnumOperateErrorLevel.None)
            {
                return;
            }

            if (message_key != null && message_key.Trim() == string.Empty)
            {
                message_key = null;
            }

            this.canContinue = this.canContinue && (lv != EnumOperateErrorLevel.Error);

            EntityOperateError err = GetSingleCustomMessage(message_key);

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
                err = new EntityOperateError();
                err.ErrorCode = EnumOperateErrorCode.CustomMessage;
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
        public void Merge(EntityOperateResult source)
        {
            if (source != null)
            {
                foreach (EntityOperateError err in source.Message)
                {
                    if (err.ErrorCode == EnumOperateErrorCode.CustomMessage)
                    {
                        this.AddCustomMessage(err.CustomMessageKey, err.CustomMessageTitle, err.Param, err.ErrorLevel);
                    }
                    else
                    {
                        this.AddMessage(err.ErrorCode, err.Param, err.ErrorLevel);
                    }
                }

                //foreach (EntityOperationError err in source.CustomMessage)
                //{
                //    this.AddCustomMessage(err.CustomMessageKey, err.CustomMessageTitle, err.Param, err.ErrorLevel);
                //}
            }
        }

        #region .ctor
        /// <summary>
        /// .ctor
        /// </summary>
        public EntityOperateResult()
        {
            this.Data = new EntityOperateResultData();
            this.canContinue = true;
            this.Message = new List<EntityOperateError>();
            //this.CustomMessage = new List<EntityOperationError>();

            //this.AddCustomMessage("111", "aaa", "bbb", EnumOperationErrorLevel.Warn);
        }
        #endregion

        public string Key { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", this.Data.Patient.RepId, this.Data.Patient.RepSid, this.Data.Patient.PidName);
        }
    }
}
