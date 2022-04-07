
using dcl.entity;

namespace dcl.interfaces
{
    public interface IPatients
    {
        InterfacePatientInfo GetPatientFromInterface(PatientInterfaceInfo patient);
    }

    public abstract class AbstractPatients : IPatients
    {

        #region IPatients 成员

        public virtual InterfacePatientInfo GetPatientFromInterface(PatientInterfaceInfo patient)
        {
            return null;
        }

        #endregion
    }

    public class NormalPatients : AbstractPatients
    {

    }

    public class OutlinkPatinets : AbstractPatients
    {
        public override InterfacePatientInfo GetPatientFromInterface(PatientInterfaceInfo patient)
        {
            if (patient.NoCode.Trim().ToLower() == "barcode")
            {
                return null;
            }
            else if (patient.NoCode.Trim().ToLower() == "inpatient")//住院
            {
                return dcl.client.interfaces.OutlinkForLab.GetZYPatient(patient.PatientID);
            }
            else if (patient.NoCode.Trim().ToLower() == "outpatient")//门诊
            {
                return dcl.client.interfaces.OutlinkForLab.GetMZPatient(patient.PatientID);
            }

            return null;
        }
    }

    /// <summary>
    /// 病人接口参数
    /// </summary>
    public class PatientInterfaceInfo
    {
        public string PatientID { get; set; }
        public string InterfaceID { get; set; }
        public string NoCode { get; set; }
        public NetInterfaceType NetInterfaceType { get; set; }

        public PatientInterfaceInfo(string noCode, string patientID, string interfaceID,NetInterfaceType netInterfaceType)
        {
            NoCode = noCode;
            PatientID = patientID;
            InterfaceID = interfaceID;
            NetInterfaceType = netInterfaceType;
        }
    }
}
