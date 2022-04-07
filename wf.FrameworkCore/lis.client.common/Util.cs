using System.Collections.Generic;
using dcl.client.frame;
using dcl.common;
using dcl.client.cache;
using dcl.entity;

namespace dcl.client.common
{
    public class Util
    {
        public static EntityRemoteCallClientInfo ToCallerInfo()
        {
            EntityRemoteCallClientInfo caller = new EntityRemoteCallClientInfo();
            caller.LoginID = UserInfo.loginID;
            caller.LoginName = UserInfo.userName;
            caller.OperationName = UserInfo.userName;
            caller.IPAddress = IPUtility.GetIP();
            //caller.Remarks = LocalSetting.Current.Setting.Description;
            caller.Time = ServerDateTime.GetServerDateTime();
            caller.UserID = UserInfo.userInfoId;
            caller.Location = LocalSetting.Current.Setting.FullDescription;
         
            return caller;
        }

        public static EntityRemoteCallClientInfo ToCallerInfo(string loginID, string userID, string loginName,string OperatorSftId="")
        {
            EntityRemoteCallClientInfo caller = new EntityRemoteCallClientInfo();
            caller.LoginID = loginID;
            caller.LoginName = loginName;
            caller.OperationName = loginName; //常规(自动审核):将用户名赋给操作人 
            caller.UserID = userID;
            caller.IPAddress = IPUtility.GetIP();
            caller.Time = ServerDateTime.GetServerDateTime();
            //caller.Remarks = LocalSetting.Current.Setting.Description;
            caller.Location = LocalSetting.Current.Setting.FullDescription;
            caller.OperatorSftId = OperatorSftId;
            return caller;
        }

        public static EntityRemoteCallClientInfo ToCallerInfo(string loginID, string userID, string loginName, List<string> unSendCriticalMessagePatIDs,string OperatorSftId="")
        {
            EntityRemoteCallClientInfo caller = new EntityRemoteCallClientInfo();
            caller.LoginID = loginID;
            caller.LoginName = loginName;
            caller.UserID = userID;
            caller.IPAddress = IPUtility.GetIP();
            caller.Time = ServerDateTime.GetServerDateTime();
            caller.OperationName = loginName;
            //caller.Remarks = LocalSetting.Current.Setting.Description;
            caller.Location = LocalSetting.Current.Setting.FullDescription;
            caller.OperatorSftId = OperatorSftId;
            if (unSendCriticalMessagePatIDs != null && unSendCriticalMessagePatIDs.Count > 0)
            {
                caller.UnSendCriticalMessagePatIDs = unSendCriticalMessagePatIDs;
            }
            

            return caller;
        }
    }
}
