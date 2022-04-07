using dcl.entity;

using dcl.svr.result;
using dcl.svr.sample;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Services;


namespace dcl.pub.wcf
{
    /// <summary>
    /// UploadData 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class UploadData : System.Web.Services.WebService
    {

        [WebMethod(Description = "上传病人资料和结果")]
        public string UploadPatInfoAndResult(byte[] patBuffer, byte[] buffer)
        {
            string result = "";
            try
            {
                bool success = false;
                EntityPidReportMain patient = new EntityPidReportMain();
                List<EntityPidReportMain> list = new List<EntityPidReportMain>();
                MemoryStream patStream = new MemoryStream();
                patStream = new MemoryStream(patBuffer);
                IFormatter formatter = new BinaryFormatter();
                //反序列化
                patient = (EntityPidReportMain)formatter.Deserialize(patStream);
                patStream.Close();
                EntityPidReportMain patientQuery = new PidReportMainBIZ().GetPatientsByBarCode(patient.RepBarCode);
                if (string.IsNullOrEmpty(patientQuery.RepBarCode))
                {
                    result = string.Format("上传病人资料失败! 条码号{0}不存在!", patient.RepBarCode);
                    return result;
                }
                patientQuery.RepSid = patient.RepSid;
                patientQuery.RepItrId = patient.RepItrId;
                patientQuery.RepInDate = patient.RepInDate;
                patientQuery.RepId = patient.RepId;
                patientQuery.RepStatus = 0;
                list.Add(patientQuery);
                if (new PidReportMainBIZ().ExsitSid(patientQuery.RepSid, patientQuery.RepItrId, patientQuery.RepInDate.Value))
                {
                    result = string.Format("上传病人资料失败! 样本号{0}已存在", patientQuery.RepSid);
                    return result;
                }
                else
                {
                    success = new PidReportMainBIZ().InsertNewPatient(list);
                    if (!success)
                    {
                        result = string.Format("上传病人资料失败! 条码号{0}插入病人信息失败", patientQuery.RepBarCode);
                        return result;
                    }
                    else {
                        EntitySampOperation operation = new EntitySampOperation();
                        operation.OperationStatus = "4";
                        operation.OperationStatusName = "送达";
                        operation.OperationTime = DateTime.Now;
                        operation.OperationID ="";
                        operation.OperationName = "";
                        operation.OperationIP = "";
                        operation.OperationWorkId = "";
                        operation.Remark = "单机版客户端上传";
                        new SampMainBIZ().UpdateSampMainStatusByBarId(operation, patient.RepBarCode);
                    }
                }
                MemoryStream stream = new MemoryStream();
                stream = new MemoryStream(buffer);
                List<EntityObrResult> listResult = new List<EntityObrResult>();
                //反序列化
                listResult = (List<EntityObrResult>)formatter.Deserialize(stream);
                stream.Close();
                if (listResult != null && listResult.Count > 0)
                {
                    foreach (EntityObrResult obrResult in listResult)
                    {
                        new ObrResultBIZ().InsertObrResult(obrResult);
                    }
                }


            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            return result;
        }
    }

}
