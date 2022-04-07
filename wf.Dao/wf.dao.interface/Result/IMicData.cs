using dcl.dao.core;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.interfaces
{
    /// <summary>
    /// 获取细菌报告一些特殊sql
    /// </summary>
    public interface IMicData : IDaoBase
    {
        List<EntityDicMicAntidetail> GetMicAntidetailList(string bacID);
        List<EntityPidReportMain> PatientQuery(EntityPatientQC patientCondition);

        string GetPatDate_ByItrSID(DateTime date, string itr_id, string currentSID);
        List<EntityDicMicSmear> GetDicMicSmearByComID(string strComIDs);


        List<EntityMicViewData> GetMicViewList(DateTime dt, string itrid);

        string GetAntiResult(List<string> repId);
    }
}
