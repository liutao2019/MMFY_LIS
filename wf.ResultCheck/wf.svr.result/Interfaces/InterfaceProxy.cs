using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.pub.entities;
using System.Data;

namespace dcl.svr.result
{
    public class InterfaceProxy
    {
        public static InterfacePatientInfo GetInterfacePatient(string code, string interfaceID, NetInterfaceType interfaceType)
        {
            InterfacePatientInfo patinfo = null;
            if (interfaceType == NetInterfaceType.Interface)
            {
                NetPatientInterface proxy = new NetPatientInterface();
                DataSet dsData = proxy.Get(code, interfaceID);

                //if (dsData != null && dsData.Tables.Count > 0)
                //{
                patinfo = ConvertUtil.ToPatients(dsData);
                if (patinfo != null && patinfo.Name != null)
                {
                    patinfo.Name = patinfo.Name.Trim();
                }
                //}
                //else//如果在院网接口中取不到病人资料，则在检验系统中取病人基本信息
                //{
                //patinfo
                //}
            }
            else if (interfaceType == NetInterfaceType.Barcode)
            {
                BarcodePatientInterface proxy = new BarcodePatientInterface();
                patinfo = proxy.Get(code, interfaceID);
            }
            else if (interfaceType==NetInterfaceType.SampleId)
            {

                BarcodePatientInterface proxy = new BarcodePatientInterface();
                string[] strCodeArr=code.Split(';');
                if (strCodeArr.Length>1)
                {
                    string strBarcode = proxy.GetBarcodeBySamIdAndItrId(strCodeArr[0], strCodeArr[1]);
                    patinfo = proxy.Get(strBarcode, interfaceID);
                }
                else
                {
                    return null;
                }
               
                
            }
            else if (interfaceType == NetInterfaceType.Filtercode)//筛查号
            {
                //根据筛查号获取病人信息
                FiltercodePatientInterface proxy = new FiltercodePatientInterface();
                patinfo = proxy.Get(code, interfaceID);
            }
            else if (interfaceType == NetInterfaceType.TelPhone)//手机号
            {
                //根据手机号获取病人信息
                TelPhonePatientInterface proxy = new TelPhonePatientInterface();
                patinfo = proxy.Get(code, interfaceID);
            }
            else
            {
                return null;
            }
            return patinfo;
        }
    }
}
