using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.servececontract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace dcl.svr.cache
{
    public class CacheDataBIZ : ICacheData
    {
        public EntityResponse GetCacheData(String cacheName)
        {
            EntityResponse response = new EntityResponse();

            switch (cacheName)
            {
                case "EntityDicSample":
                    response = Search<EntityDicSample>("cache");
                    break;
                case "EntityDicPubOrganize":
                    response = Search<EntityDicPubOrganize>(new EntityRequest());
                    break;
                case "EntityDicOrigin":
                    response = Search<EntityDicOrigin>(new EntityRequest());
                    break;
                case "EntityDicSState":
                    response = Search<EntityDicSState>(new EntityRequest());
                    break;
                case "EntityDicDoctor":
                    response = Search<EntityDicDoctor>(new EntityRequest());
                    break;
                case "EntityDicCheckPurpose":
                    response = Search<EntityDicCheckPurpose>(new EntityRequest());
                    break;
                case "EntityDicInstrument":
                    response = SearchInstrumentInfo();
                    break;
                case "EntityDicPubProfession":
                    response = Search<EntityDicPubProfession>();
                    break;
                case "EntityDicPubDept":
                    response = Search<EntityDicPubDept>();
                    break;
                case "EntityReportStat":
                    response = Search<EntityReportStat>();
                    break;
                case "EntityDicPubInsurance":
                    response = Search<EntityDicPubInsurance>();
                    break;
                case "EntityDicPubIdent":
                    response = Search<EntityDicPubIdent>();
                    break;
                case "EntityDicPubIcd":
                    response = Search<EntityDicPubIcd>();
                    break;
                case "EntityDicMicAntidetail":
                    response = SearchMicAntiDetail();
                    break;
                case "EntityDicMicAntitype":
                    response = Search<EntityDicMicAntitype>();
                    break;
                case "EntityDicMicBacttype":
                    response = Search<EntityDicMicBacttype>();
                    break;
                case "EntityDicTestTube":
                    response = Search<EntityDicTestTube>();
                    break;
                case "EntityDicItmReftype":
                    response = Search<EntityDicItmReftype>();
                    break;
                case "EntityDicItmItem":
                    response = SearchItmItem();//Search<EntityDicItmItem>(null);
                    break;
                case "EntityDicCombine"://组合暂不控制
                    string strHospitalId = string.Empty;// ConfigurationManager.AppSettings["HospitalId"];
                    response = GetCombineCache(strHospitalId);
                    break;
                case "EntityDicSampRemark":
                    response = Search<EntityDicSampRemark>();
                    break;
                case "EntityDicComReptime":
                    response = Search<EntityDicComReptime>();
                    break;
                case "EntitySysUser":
                    response = SearchUserInfo();
                    break;
                case "EntityDicItrCombine":
                    response = Search<EntityDicItrCombine>();
                    break;
                case "EntityDicItmCombine":
                    response = Search<EntityDicItmCombine>();
                    break;
                case "EntityDicItemSample":
                    response = Search<EntityDicItemSample>("cache");
                    break;
                case "EntityDicResultTips":
                    response = Search<EntityDicResultTips>();
                    break;
                case "EntityDicPubEvaluate":
                    response = SearchPubEvaluate();
                    break;
                case "EntityDicSampReturn":
                    response = Search<EntityDicSampReturn>();
                    break;
                case "EntityDicMicroscope":
                    response = Search<EntityDicMicroscope>();
                    break;
                case "EntityDicCombineDetail":
                    response = SearchCombineDetail();
                    break;
                case "EntityDicItmRefdetail":
                    response = Search<EntityDicItmRefdetail>("cache");
                    break;
                case "EntityDicMicBacteria":
                    response = Search<EntityDicMicBacteria>();
                    break;
                case "EntityDicMicAntibio":
                    response = Search<EntityDicMicAntibio>();
                    break;
                case "EntityDicElisaSort":
                    response = Search<EntityDicElisaSort>();
                    break;
                case "EntityDicElisaStatus":
                    response = Search<EntityDicElisaStatus>();
                    break;
                case "EntityDicElisaItem":
                    response = Search<EntityDicElisaItem>();
                    break;
                case "EntityDicElisaCriter":
                    response = Search<EntityDicElisaCriter>();
                    break;
                case "EntityDicElisaCalu":
                    response = Search<EntityDicElisaCalu>();
                    break;
                case "EntityDefItmProperty":
                    response = Search<EntityDefItmProperty>();
                    break;
                case "EntityOaAnnouncementReceive":
                    response = SearchAnnouncementInfo();
                    break;
                case "EntityUserRole":
                    response = GetAllUserRole();
                    break;
                case "EntitySysRoleFunction":
                    response = GetFuncsByRoleId();
                    break;
                case "EntitySysFunction":
                    response = GetFuncList();
                    break;
                case "EntitySysRole":
                    response = GetSysRoleList();
                    break;
                case "EntityDicItmCalu":
                    response = Search<EntityDicItmCalu>();
                    break;
                case "EntityDicTubeRack":
                    response = Search<EntityDicTubeRack>();
                    break;
                case "EntityDicMicSmear":
                    response = GetDicMicSmear();
                    break;
                case "EntityDicUtgentValue":
                    response = Search<EntityDicUtgentValue>("0");
                    break;
                case "EntitySysItfInterface":
                    response = GetSysInterface();
                    break;
                case "EntitySysItfContrast":
                    response = GetSysContrast();
                    break;
                case "EntityTypeBarcode":
                    response = SearchTypeBarcode();
                    break;
                case "EntityDicPubStatus":
                    response = Search<EntityDicPubStatus>();
                    break;
                case "EntitySysParameter":
                    response = GetSysParCache();
                    break;
                case "EntityUserInstrmt":
                    response = GetUserInstrmtCache();
                    break;
                case "EntityDicItmCheck":
                    response = Search<EntityDicItmCheck>("cache");
                    break;
                case "EntityDicItmCheckDetail":
                    response = Search<EntityDicItmCheckDetail>("cache");
                    break;
                case "EntityDictHarvester":
                    response = Search<EntityDictHarvester>();
                    break;
                case "EntityDicTemperature":
                    response = Search<EntityDicTemperature>();
                    break;
                case "EntityDicMicTemplate":
                    response = Search<EntityDicMicTemplate>("cache");
                    break;
                case "EntityDicAntibioType":
                    response = Search<EntityDicAntibioType>();
                    break;
                case "EntityDefItmResultTips":
                    response = Search<EntityDefItmResultTips>();//结果提示标志
                    break;
                case "EntityServerSetting":
                    response = GetServerSetting();
                    break;
                case "EntityDicRealTimeReportStat":
                    response = Search<EntityDicRealTimeReportStat>();
                    break;
                case "EntityDicNobactCom":
                    response = Search<EntityDicNobactCom>("Cache");
                    break;
                case "EntityDicReaUnit":
                    response = Search<EntityDicReaUnit>();
                    break;
                case "EntityDicReaProduct":
                    response = Search<EntityDicReaProduct>();
                    break;
                case "EntityDicReaClaimant":
                    response = Search<EntityDicReaClaimant>();
                    break;
                case "EntityDicReaDept":
                    response = Search<EntityDicReaDept>();
                    break;
                case "EntityDicReaGroup":
                    response = Search<EntityDicReaGroup>();
                    break;
                case "EntityDicReaStoreCondition":
                    response = Search<EntityDicReaStoreCondition>();
                    break;
                case "EntityDicReaStorePos":
                    response = Search<EntityDicReaStorePos>();
                    break;
                case "EntityDicReaReturn":
                    response = Search<EntityDicReaReturn>();
                    break;
                case "EntityDicReaSupplier":
                    response = Search<EntityDicReaSupplier>();
                    break;
                case "EntityReaSetting":
                    response = GetResSetting();
                    break;
                default:
                    break;
            }

            return response;
        }
        private EntityResponse GetResSetting()
        {
            EntityResponse response = new EntityResponse();

            IDaoReagentSetting dao = DclDaoFactory.DaoHandler<IDaoReagentSetting>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.SearchReaSettingAll());
            }

            return response;
        }

        /// <summary>
        /// 获取服务器配置信息
        /// </summary>
        /// <returns></returns>
        private EntityResponse GetServerSetting()
        {
            EntityResponse response = new EntityResponse();

            EntityServerSetting setting = new EntityServerSetting();

            setting.SystemType = (ConfigurationManager.AppSettings["SystemType"] ?? "");
            setting.ExtDataInterface = (ConfigurationManager.AppSettings["DCL.ExtDataInterface"] ?? "");


            response.SetResult(new List<EntityServerSetting>() { setting });

            return response;
        }


        private EntityResponse GetDicMicSmear()
        {
            EntityResponse response = new EntityResponse();

            IDaoDicMicSmear dao = DclDaoFactory.DaoHandler<IDaoDicMicSmear>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(new object()));
            }

            return response;
        }

        //本地配置—条码确认目的地下拉控件缓存读取数据方法
        private EntityResponse SearchTypeBarcode()
        {
            EntityResponse response = new EntityResponse();

            IDaoTypeBarcode dao = DclDaoFactory.DaoHandler<IDaoTypeBarcode>();
            string hosID = ConfigurationManager.AppSettings["HospitalID"];
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.SearchTypeBarcode(hosID));
            }

            return response;
        }

        private EntityResponse Search<T>(Object obj = null)
        {
            EntityResponse response = new EntityResponse();

            IDaoDic<T> dao = DclDaoFactory.DaoHandler<IDaoDic<T>>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;

                List<T> listT = dao.Search(obj);

                string strHospitalId = ConfigurationManager.AppSettings["HospitalId"];
                if (!string.IsNullOrEmpty(strHospitalId))
                {
                    Object value = listT;
                    string strName = typeof(T).Name;

                    switch (strName)
                    {
                        case "EntityDicDoctor":
                            List<EntityDicDoctor> list = listT as List<EntityDicDoctor>;
                            value = list.FindAll(w => w.DoctorHospital == strHospitalId);
                            break;
                        case "EntityDicPubDept":
                            List<EntityDicPubDept> listDept = listT as List<EntityDicPubDept>;
                            value = listDept.FindAll(w => w.DeptHospital == strHospitalId);
                            break;
                        case "EntityDicPubProfession":
                            List<EntityDicPubProfession> listProfession = listT as List<EntityDicPubProfession>;
                            value = listProfession.FindAll(w => w.ProOrgId == strHospitalId);
                            break;
                        default:
                            break;
                    }

                    response.SetResult(value);
                }
                else
                    response.SetResult(listT);
            }
            return response;
        }
        /// <summary>
        /// 获取用户的缓存
        /// </summary>
        /// <returns></returns>
        private EntityResponse SearchUserInfo()
        {
            EntityResponse response = new EntityResponse();

            IDaoSysUser dao = DclDaoFactory.DaoHandler<IDaoSysUser>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;

                List<EntitySysUser> listUser = dao.Search(null);

                string strHospitalId = ConfigurationManager.AppSettings["HospitalId"];
                if (!string.IsNullOrEmpty(strHospitalId))
                    listUser = listUser.FindAll(w => w.UserOrgId == strHospitalId);

                response.SetResult(listUser);
            }
            return response;
        }

        /// <summary>
        /// 获取仪器的缓存
        /// </summary>
        /// <returns></returns>
        private EntityResponse SearchInstrumentInfo()
        {
            EntityResponse response = new EntityResponse();

            IDaoDicInstrument dao = DclDaoFactory.DaoHandler<IDaoDicInstrument>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;

                List<EntityDicInstrument> list = dao.Search(null);

                string strHospitalId = ConfigurationManager.AppSettings["HospitalId"];
                if (!string.IsNullOrEmpty(strHospitalId))
                {
                    EntityResponse type = GetCacheData("EntityDicPubProfession");
                    List<EntityDicPubProfession> listRv = type.GetResult() as List<EntityDicPubProfession>;
                    list = list.FindAll(w => listRv.FindIndex(z => z.ProId == w.ItrLabId && z.ProType == 1) > -1);
                }

                response.SetResult(list);
            }
            return response;
        }


        /// <summary>
        /// 获取组合明细的缓存
        /// </summary>
        /// <returns></returns>
        private EntityResponse SearchCombineDetail()
        {
            EntityResponse response = new EntityResponse();

            IDaoDicCombineDetail dao = DclDaoFactory.DaoHandler<IDaoDicCombineDetail>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(null));
            }
            return response;
        }

        /// <summary>
        /// 获取描述评价的缓存
        /// </summary>
        /// <returns></returns>
        private EntityResponse SearchPubEvaluate()
        {
            EntityResponse response = new EntityResponse();

            IDaoDicPubEvaluate dao = DclDaoFactory.DaoHandler<IDaoDicPubEvaluate>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.Search(null));
            }
            return response;
        }
        /// <summary>
        /// 获取公告的缓存
        /// </summary>
        /// <returns></returns>
        private EntityResponse SearchAnnouncementInfo()
        {
            EntityResponse response = new EntityResponse();

            IDaoOaAnnouncement dao = DclDaoFactory.DaoHandler<IDaoOaAnnouncement>();
            if (dao == null)
            {
                response.Scusess = false;
                response.ErroMsg = "未找到数据访问";
            }
            else
            {
                response.Scusess = true;
                response.SetResult(dao.GetAnnouncementCache());
            }
            return response;
        }

        private EntityResponse GetAllUserRole()
        {
            EntityResponse response = new EntityResponse();
            List<EntityUserRole> listUserRole = new List<EntityUserRole>();
            IDaoUserRole dao = DclDaoFactory.DaoHandler<IDaoUserRole>();
            if (dao != null)
            {
                listUserRole = dao.GetAllUserRole();
                response.Scusess = true;
                response.SetResult(listUserRole);
            }
            return response;
        }
        /// <summary>
        /// 获取角色功能缓存
        /// </summary>
        /// <returns></returns>
        private EntityResponse GetFuncsByRoleId()
        {
            EntityResponse response = new EntityResponse();
            List<EntitySysRoleFunction> listRoleFunc = new List<EntitySysRoleFunction>();
            IDaoSysRoleFunc dao = DclDaoFactory.DaoHandler<IDaoSysRoleFunc>();
            if (dao != null)
            {
                listRoleFunc = dao.GetFuncsByRoleId("");
                response.Scusess = true;
                response.SetResult(listRoleFunc);
            }
            return response;
        }

        private EntityResponse GetFuncList()
        {
            EntityResponse response = new EntityResponse();
            List<EntitySysFunction> listSysFunc = new List<EntitySysFunction>();
            IDaoSysFunction dao = DclDaoFactory.DaoHandler<IDaoSysFunction>();
            if (dao != null)
            {
                listSysFunc = dao.GetFuncList();
                response.Scusess = true;
                response.SetResult(listSysFunc);
            }
            return response;
        }

        private EntityResponse GetSysRoleList()
        {
            EntityResponse response = new EntityResponse();
            List<EntitySysRole> listSysRole = new List<EntitySysRole>();
            IDaoSysRole dao = DclDaoFactory.DaoHandler<IDaoSysRole>();
            if (dao != null)
            {
                listSysRole = dao.GetAllInfo();
                response.Scusess = true;
                response.SetResult(listSysRole);
            }
            return response;
        }

        /// <summary>
        /// 获取项目字典缓存
        /// </summary>
        private EntityResponse SearchItmItem()
        {
            EntityResponse result = new EntityResponse();
            List<EntityDicItmItem> listItmItem = new List<EntityDicItmItem>();
            IDaoDicItmItem dao = DclDaoFactory.DaoHandler<IDaoDicItmItem>();
            if (dao == null)
            {
                result.Scusess = false;
                result.ErroMsg = "未找到数据访问(项目字典)";
            }
            else
            {
                string hosID = ConfigurationManager.AppSettings["HospitalID"];
                listItmItem = dao.Search(null, hosID);
                result.Scusess = true;
                result.SetResult(listItmItem);
            }
            return result;
        }

        /// <summary>
        /// 获取接口缓存
        /// </summary>
        /// <returns></returns>
        private EntityResponse GetSysInterface()
        {
            EntityResponse result = new EntityResponse();
            List<EntitySysItfInterface> listItmItem = new List<EntitySysItfInterface>();
            IDaoSysItfInterface dao = DclDaoFactory.DaoHandler<IDaoSysItfInterface>();
            if (dao == null)
            {
                result.Scusess = false;
                result.ErroMsg = "未找到数据访问";
            }
            else
            {
                listItmItem = dao.GetSysInterface("cache");
                result.Scusess = true;
                result.SetResult(listItmItem);
            }
            return result;
        }

        /// <summary>
        /// 获取数据对照缓存
        /// </summary>
        /// <returns></returns>
        private EntityResponse GetSysContrast()
        {
            EntityResponse result = new EntityResponse();
            List<EntitySysItfContrast> listItmItem = new List<EntitySysItfContrast>();
            IDaoSysItfContrast dao = DclDaoFactory.DaoHandler<IDaoSysItfContrast>();
            if (dao == null)
            {
                result.Scusess = false;
                result.ErroMsg = "未找到数据访问";
            }
            else
            {
                listItmItem = dao.GetSysContrast();
                result.Scusess = true;
                result.SetResult(listItmItem);
            }
            return result;
        }

        /// <summary>
        /// 获取系统配置缓存
        /// </summary>
        /// <returns></returns>
        private EntityResponse GetSysParCache()
        {
            EntityResponse result = new EntityResponse();
            List<EntitySysParameter> listPar = new List<EntitySysParameter>();
            IDaoSysParameter dao = DclDaoFactory.DaoHandler<IDaoSysParameter>();
            if (dao == null)
            {
                result.Scusess = false;
                result.ErroMsg = "未找到数据访问";
            }
            else
            {
                listPar = dao.GetSysParaCache();
                result.Scusess = true;
                result.SetResult(listPar);
            }
            return result;
        }

        /// <summary>
        /// 获取用户仪器缓存
        /// </summary>
        /// <returns></returns>
        private EntityResponse GetUserInstrmtCache()
        {
            EntityResponse result = new EntityResponse();
            List<EntityUserInstrmt> listItr = new List<EntityUserInstrmt>();
            IDaoUserManage dao = DclDaoFactory.DaoHandler<IDaoUserManage>();
            if (dao == null)
            {
                result.Scusess = false;
                result.ErroMsg = "未找到数据访问";
            }
            else
            {
                listItr = dao.GetUserInstrmtCache();
                result.Scusess = true;
                result.SetResult(listItr);
            }
            return result;
        }


        /// <summary>
        /// 获取抗生素缓存
        /// </summary>
        private EntityResponse SearchMicAntiDetail()
        {
            EntityResponse result = new EntityResponse();
            List<EntityDicMicAntidetail> listAntiDetail = new List<EntityDicMicAntidetail>();
            IDaoDicMicAntidetail dao = DclDaoFactory.DaoHandler<IDaoDicMicAntidetail>();
            if (dao == null)
            {
                result.Scusess = false;
                result.ErroMsg = "未找到数据访问(项目字典)";
            }
            else
            {
                listAntiDetail = dao.Search(null);
                result.Scusess = true;
                result.SetResult(listAntiDetail);
            }
            return result;
        }

        /// <summary>
        /// 获取组合缓存
        /// </summary>
        /// <returns></returns>
        private EntityResponse GetCombineCache(string hospitalId)
        {
            EntityResponse result = new EntityResponse();
            List<EntityDicCombine> listCombine = new List<EntityDicCombine>();
            IDaoDicCombine dao = DclDaoFactory.DaoHandler<IDaoDicCombine>();
            if (dao == null)
            {
                result.Scusess = false;
                result.ErroMsg = "未找到数据访问";
            }
            else
            {
                listCombine = dao.Search(hospitalId);
                result.Scusess = true;
                result.SetResult(listCombine);
            }
            return result;
        }
    }
}
