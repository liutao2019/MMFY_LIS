using dcl.dao.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using dcl.entity;
using dcl.dao.core;
using System.Data;
using dcl.common;

namespace dcl.dao.clab
{
    [Export("wf.plugin.wf", typeof(IDaoNoticeManage))]
    public class DaoNoticeManage : IDaoNoticeManage
    {
        public EntityResponse SearchMessUserRoleDepart(EntitySysUser user)
        {
            EntityResponse result = new EntityResponse();
            try
            {
                DBManager helper = new DBManager();
                List<Object> listObj = new List<Object>();

                string userInfoId = "";
                if (user != null)
                {
                    userInfoId = user.UserId;
                }

                //--==== SysMessage 别名表 ====
                string sqlSysMessage = string.Format(@"select  -2 MessageId,
null MessageType,
'收件箱('+ CONVERT(VARCHAR(12),count(1))+')' MessageTitle,
0 MessageOwerType,
null CreateDate,null ReadDateYN 
from sysmessage WHERE (MessageOwer =  '{0}') And MessageOwerType='-2' 
union all 
select  -1 MessageId,null MessageType,
'发件箱('+ CONVERT(VARCHAR(12),count(1))+')' MessageTitle,
0 MessageOwerType,null CreateDate,null ReadDateYN 
from sysmessage WHERE (MessageOwer =  '{0}') And MessageOwerType='-1' 
union all 
select  MessageId,MessageType,MessageTitle,MessageOwerType,
CONVERT(VARCHAR(20),CreateDate,120) CreateDate,
(Case When ReadDate is Null Then '否' ELSE '是' End) ReadDateYN 
from sysmessage 
WHERE (MessageOwer =  '{0}')", userInfoId);

                //--====PowerUserInfo 别名=====
                string sqlUserInfo = string.Format(@"select  Dpro_id  userId,
Dpro_id,
Dpro_name,
-1 Buser_id,
'物理组: '+ Dpro_name Buser_name 
from  Dict_profession  where Dpro_type=1 and Dict_profession.del_flag=0 
union 
select  Dpro_id+'^'+Base_user.Buser_id  userId,
Dict_profession.Dpro_id, Dict_profession.Dpro_name,
Base_user.Buser_id,Base_user.Buser_name 
from Base_user                    
join Base_user_lab on Base_user.Buser_id = Base_user_lab.Bul_Buser_id 
join  Dict_profession on Base_user_lab.Bul_lab_id =  Dict_profession.Dpro_id 
where Dpro_type=1 and Base_user.del_flag=0     ");

                // --=======PowerRoleUser 别名表====----
                string sqlRoleUser = string.Format(@" select cast(Base_role.Brole_id as varchar)  userId, 
cast(Brole_id as varchar) Brole_id,
Base_role.Brole_name,
-1 user_id,
'角色: '+ Base_role.Brole_name user_name 
from Base_role 
union 
select  cast(Base_role.Brole_id as varchar) +'^'+ Base_user.Buser_id userId,
cast(Base_role.Brole_id as varchar) role_id,
Base_role.Brole_name,Base_user.Buser_id,Base_user.Buser_name 
from Base_role 
join Base_user_role on Base_role.Brole_id =Base_user_role.Bur_Brole_id 
join Base_user on Base_user_role.Bur_Buser_id =Base_user.Buser_id  ");

                //--=====PowerUserDepart 别名表===--
                string sqlUserDepart = string.Format(@" select  Ddept_id userId,
Ddept_id as Bud_Ddept_id,
Ddept_name , 
-1 Bud_Buser_id,
'科室：'+Ddept_name Buser_name  
from Dict_dept 
union 
select  Ddept_id +'^'+ Base_user.Buser_id userId,
Ddept_id,Ddept_name,Base_user.Buser_id,Base_user.Buser_name 
from Dict_dept 
join poweruserdepart on Dict_dept.Ddept_id =poweruserdepart.departId 
join Base_user on poweruserdepart.userInfoId =Base_user.Buser_id ");

                DataTable dtSysMessage = helper.ExecuteDtSql(sqlSysMessage);
                List<EntitySysMessage> listSysMessage = EntityManager<EntitySysMessage>.ConvertToList(dtSysMessage).OrderBy(i => i.MessageId).ToList();

                DataTable dtUserInfo = helper.ExecuteDtSql(sqlUserInfo);
                List<EntitySysUser> listUserInfo = EntityManager<EntitySysUser>.ConvertToList(dtUserInfo).OrderBy(i => i.UserId).ToList();

                DataTable dtRoleUser = helper.ExecuteDtSql(sqlRoleUser);
                List<EntitySysRole> listRoleUser = EntityManager<EntitySysRole>.ConvertToList(dtRoleUser).OrderBy(i => i.UserId).ToList();

                DataTable dtUserDepart = helper.ExecuteDtSql(sqlUserDepart);
                List<EntityUserDept> listUserDepart = EntityManager<EntityUserDept>.ConvertToList(dtUserDepart).OrderBy(i => i.UserId).ToList();

                listObj.Add(listSysMessage);
                listObj.Add(listUserInfo);
                listObj.Add(listRoleUser);
                listObj.Add(listUserDepart);

                result.Scusess = true;
                result.SetResult(listObj);
                return result;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                result.Scusess = false;
                return result;
            }
        }


        public bool SaveSysMessage(List<EntitySysMessage> message)
        {
            try
            {
                foreach (var info in message)
                {
                    info.CreateDate = DateTime.Now;

                    if (info.MessageOwerType == -1)
                    {
                        info.ReadDate = DateTime.Now;
                    }

                    Save(info);
                }

                //AnnuncemenCache.Current.Refresh();

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool DeleteSysMessage(List<EntitySysMessage> message)
        {
            try
            {
                foreach (var info in message)
                {
                    Delete(info.MessageId);
                }
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public List<EntitySysMessage> UpdateAndSearchSysMessage(int messageId)
        {
            try
            {
                DBManager helper = new DBManager();

                //设置为已读
                string sqlUpdate = String.Format(@"Update SysMessage set ReadDate = '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE (MessageId = {0})", messageId);
                //读取数据
                string sqlSearch = String.Format(@"select * from sysmessage WHERE (MessageId =  '{0}')", messageId);

                helper.ExecSql(sqlUpdate);

                DataTable dtMess = helper.ExecuteDtSql(sqlSearch);
                List<EntitySysMessage> listMess = EntityManager<EntitySysMessage>.ConvertToList(dtMess).OrderBy(i => i.MessageId).ToList();
                
                //AnnuncemenCache.Current.Refresh();

                return listMess;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntitySysMessage>();
            }
        }

        public bool Save(EntitySysMessage message)
        {
            try
            {
                //int id = IdentityHelper.GetMedIdentity("sysmessage ");

                DBManager helper = new DBManager();

                Dictionary<string, object> values = new Dictionary<string, object>();
                //values.Add("MessageId", id); //字段自增，不能赋值
                values.Add("MessageType", message.MessageType);
                values.Add("MessageTitle", message.MessageTitle);
                values.Add("MessageContent", message.MessageContent);
                values.Add("MessageOwer", message.MessageOwer);
                values.Add("MessageOwerType", message.MessageOwerType);
                values.Add("MessageFromId", message.MessageFromId);
                values.Add("MessageFrom", message.MessageFrom);
                values.Add("MessageTo", message.MessageTo);
                values.Add("CreateDate", message.CreateDate?.ToString("yyyy-MM-dd HH:mm:ss"));
                values.Add("ReadDate", message.ReadDate?.ToString("yyyy-MM-dd HH:mm:ss"));

                helper.InsertOperation("sysmessage ", values);

                //message.MessageId = id;

                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

        public bool Delete(int messageId)
        {
            try
            {
                DBManager helper = new DBManager();
                string sql = string.Format(@"delete from sysmessage where MessageId={0} ", messageId);
                helper.ExecSql(sql);
                return true;
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return false;
            }
        }

    }
}
