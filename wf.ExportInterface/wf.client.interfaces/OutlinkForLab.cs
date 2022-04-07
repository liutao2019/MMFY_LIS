using System;
using dcl.common;
using System.Data;
using dcl.common.extensions;
using Lib.LogManager;
using dcl.entity;

namespace dcl.client.interfaces
{
    public class OutlinkForLab
    {
        public static InterfacePatientInfo GetZYPatient(string patientID)
        {
            InterfacePatientInfo patient = new InterfacePatientInfo();
            try
            {


                OutlinkClient client = new OutlinkClient();
                DataTable dtResult = client.GetZYPatient(patientID);
                if (Extensions.IsEmpty(dtResult))
                    return null;

                DataRow row = dtResult.Rows[0];
                patient.PatientsIDType = "106";
                patient.PatientsID = GetRowValue(row, "PATNO");
                patient.Name = GetRowValue(row, "PATNM");
                if (row["AGE"] == null || string.IsNullOrEmpty(row["AGE"].ToString()))
                    patient.AgeValue = "";

                string age = GetRowValue(row, "AGE");
                if (age.IndexOf("年") >= 0)
                {
                    age = age.Replace('年', 'Y').Replace('月', 'M').Replace('日', 'D').Replace('时', 'H');//新outlink修改了返回年龄格式    2010-7-1 by li
                    patient.AgeValue = age + "0I";
                }
                else
                    patient.AgeValue = age + "Y0M0D0H0I";

                patient.BedNumber = GetRowValue(row, "BEDNO");
                patient.Tel = GetRowValue(row, "TEL");
                patient.Address = GetRowValue(row, "ADDR");

                string sexCode = "";
                string sexName = "";
                GetCodeAndName(GetRowValue(row, "SEX"), ref sexCode, ref sexName);
                SetSex(patient, sexCode);
                string warCode = "";
                string wardName = "";
                GetCodeAndName(GetRowValue(row, "DEP"), ref warCode, ref wardName);
                patient.WardCode = warCode;
                patient.WardName = wardName;

                string senderDeptCode = "";
                string senderDeptName = "";
                GetCodeAndName(GetRowValue(row, "STADEP"), ref senderDeptCode, ref senderDeptName);
                patient.SenderDeptCode = senderDeptCode;
                patient.SenderDeptName = senderDeptName;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }

            return patient;
        }

        public static InterfacePatientInfo GetMZPatient(string patientID)
        {
            InterfacePatientInfo patient = new InterfacePatientInfo();
            try
            {
                OutlinkClient client = new OutlinkClient();
                DataTable dtResult = client.GetMZPatient(patientID);
                if (Extensions.IsEmpty(dtResult))
                    return null;

                DataRow row = dtResult.Rows[0];
                patient.PatientsIDType = "107";
                patient.PatientsID = patientID;
                patient.Name = GetRowValue(row, "PATNM");
                if (row["AGE"] == null || string.IsNullOrEmpty(row["AGE"].ToString()))
                    patient.AgeValue = "";
                string age = GetRowValue(row, "AGE");
                if (age.IndexOf("年") >= 0)
                {
                    age = age.Replace('年', 'Y').Replace('月', 'M').Replace('日', 'D').Replace('时', 'H');//新outlink修改了返回年龄格式    2010-7-1 by li
                    patient.AgeValue = age + "0I";
                }
                else
                    patient.AgeValue = age + "Y0M0D0H0I";
                patient.BedNumber = GetRowValue(row, "BEDNO");
                patient.Tel = GetRowValue(row, "TEL");
                patient.Address = GetRowValue(row, "ADDR");

                string sexCode = "";
                string sexName = "";
                GetCodeAndName(GetRowValue(row, "SEX"), ref sexCode, ref sexName);
                SetSex(patient, sexCode);
                patient.SenderDeptCode = GetRowValue(row, "DEPTNO");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
            return patient;
        }

        private static void SetSex(InterfacePatientInfo patient, string sexCode)
        {
            if (sexCode == "0")
                patient.Sex = "2";
            else if (sexCode == "1")
                patient.Sex = "1";
            else
                patient.Sex = "0";
        }

        private static string GetRowValue(DataRow row, string p)
        {
            if (row == null || row.Table == null || row.Table.Columns == null || !row.Table.Columns.Contains(p))
                return "";
            return row[p].ToString();
        }


        private static void GetCodeAndName(string p, ref string code, ref string name)
        {
            if (string.IsNullOrEmpty(p))
                return;

            string[] codeAndName = p.Split('_');
            if (codeAndName == null || codeAndName.Length == 0)
                return;

            if (codeAndName.Length == 1)
                code = codeAndName[0];
            if (codeAndName.Length == 2)
            {
                code = codeAndName[0];
                name = codeAndName[1];

            }
        }


    }
}