using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using dcl.servececontract;
using dcl.entity;
using dcl.common;
using dcl.dao.interfaces;
using System.Configuration;
using dcl.svr.users;

namespace dcl.svr.oa
{
    class OfficeShiftDictBIZ : IOfficeShiftDict
    {
        /// <summary>
        /// 获取科室
        /// </summary>
        /// <returns></returns>
        public List<EntityDicPubDept> GetDepartment()
        {
            string hosID = ConfigurationManager.AppSettings["HospitalID"];
            IDaoDic<EntityDicPubDept> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicPubDept>>();
            return dao.Search(hosID);
        }
        /// <summary>
        /// 获取物理组
        /// </summary>
        /// <returns></returns>
        public List<EntityDicPubProfession> GetPhyic()
        {
            string hosID = ConfigurationManager.AppSettings["HospitalID"];
            IDaoDic<EntityDicPubProfession> dao = DclDaoFactory.DaoHandler<IDaoDic<EntityDicPubProfession>>();
            return dao.Search(hosID).Where(w => w.ProType.ToString().Contains("1")).ToList();
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        public int DeleteDutyRecord(EntityOaDicShift sample)
        {
            IDaoOaDicShift dao = DclDaoFactory.DaoHandler<IDaoOaDicShift>();
            if (dao == null)
            {
                return -1;
            }
            else
            {
                return dao.DeleteDutyRecord(sample);
            }
        }
        /// <summary>
        /// 获得当前存在的班次信息
        /// </summary>
        /// <returns></returns>
        public List<EntityOaDicShift> GetDutyData()
        {
            List<EntityOaDicShift> list = new List<EntityOaDicShift>();
           IDaoOaDicShift dao = DclDaoFactory.DaoHandler<IDaoOaDicShift>();
            if (dao == null)
            {
                return list;
            }
            else
            {
                list = dao.GetDutyData();
            }
            return list;
        }

        /// <summary>
        /// 获得当前最大的ID
        /// </summary>
        /// <returns></returns>
        public string GetMaxDutyID()
        {
            string strMaxID = "";
           IDaoOaDicShift dao = DclDaoFactory.DaoHandler<IDaoOaDicShift>();
            if (dao == null)
            {
                return strMaxID;
            }
            else
            {
                strMaxID = dao.GetMaxDutyID();
            }
            return strMaxID;
        }


        /// <summary>
        /// 插入一条记录
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        public int InsertIntoDuty(EntityOaDicShift sample)
        {
           IDaoOaDicShift dao = DclDaoFactory.DaoHandler<IDaoOaDicShift>();
            if (dao == null)
            {
                return -1;
            }
            else
            {
                return dao.InsertIntoDuty(sample);
            }
        }

        /// <summary>
        /// 修改一条记录
        /// </summary>
        /// <param name="sample"></param>
        /// <returns></returns>
        public int ModifyDutyRecord(EntityOaDicShift sample)
        {
            int intRet = -1;
           IDaoOaDicShift dao = DclDaoFactory.DaoHandler<IDaoOaDicShift>();
            if (dao == null)
            {
                return intRet;
            }
            else
            {
                return dao.ModifyDutyRecord(sample);
            }
        }

        /// <summary>
        /// 获得排班计划表
        /// </summary>
        /// <param name="sDate"></param>
        /// <param name="eDate"></param>
        /// <param name="strType"></param>
        /// <returns></returns>
        public List<EntityOaDicShiftDetail> GetDutyPlan(DateTime sDate, DateTime eDate, string strType)
        {
            OfficeShiftPlanBIZ detail = new OfficeShiftPlanBIZ();
            return detail.GetShiftPlan(sDate, eDate, strType);
        }

        /// <summary>
        /// 登记时插入排班计划
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertDutyPlan(EntityOaDicShiftDetail entity)
        {
            OfficeShiftPlanBIZ detail = new OfficeShiftPlanBIZ();
            return detail.InsertShiftPlan(entity); 
        }

        /// <summary>
        /// 获得用户表数据
        /// </summary>
        /// <param name="p">所选的组别，如果没有选为空就选全部</param>
        /// <returns></returns>
        public List<EntitySysUser> GetUser(string p)
        {
            List<EntitySysUser> list = new List<EntitySysUser>();
            SysUserInfoBIZ userBiz = new SysUserInfoBIZ();
            list = userBiz.GetAllUsers(p);
            return list;
        }


        /// <summary>
        /// 获得排班模板
        /// </summary>
        /// <returns></returns>
        public List<EntityOaShiftTemplate> GetTemplateData()
        {
            List<EntityOaShiftTemplate> list = new List<EntityOaShiftTemplate>();
            IDaoOaDicOaShiftTemplate dao = DclDaoFactory.DaoHandler<IDaoOaDicOaShiftTemplate>();
            if (dao == null)
            {
                return list;
            }
            else
            {
                list = dao.GetTemplateData();
            }
            return list;
        }
    }
}
