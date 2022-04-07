using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using dcl.servececontract;
using dcl.svr.dicbasic;
using Lib.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.svr.sample
{
    /// <summary>
    /// 排样登记
    /// </summary>
    public class SecondSignBIZ : ISecondSign
    {
        public bool DeleteShelfBarcode(long RegSn)
        {
            bool ret = false;
            SampRegisterBIZ RegisterBIZ = new SampRegisterBIZ();
            if (RegisterBIZ != null)
                ret = RegisterBIZ.DeleteShelfBarcode(RegSn);

            return ret;
        }

        public int SaveShelfBarcode(EntitySampRegister data)
        {
            int ret = 0;
            SampRegisterBIZ RegisterBIZ = new SampRegisterBIZ();
            if (RegisterBIZ != null)
                ret = RegisterBIZ.SaveShelfBarcode(data);

            return ret;
        }

        public List<EntitySampRegister> GetSampRegister(long RegSn)
        {
            List<EntitySampRegister> SampRegisterList = new List<EntitySampRegister>();
            try
            {
                SampRegisterBIZ RegisterBIZ = new SampRegisterBIZ();
                if (RegisterBIZ != null)
                {
                    SampRegisterList = RegisterBIZ.GetSampRegister(RegSn);

                }
            }
            catch (Exception ex)
            {
                Logger.LogException("GetSampRegister", ex);
            }
            return SampRegisterList;
        }

        public List<EntitySampRegister> GetCuvetteRegisteredBarcodeInfo(string deptid, DateTime depTime)
        {
            List<EntitySampRegister> SampRegisterList = new List<EntitySampRegister>();
            try
            {
                SampRegisterBIZ RegisterBIZ = new SampRegisterBIZ();
                if (RegisterBIZ != null)
                {
                    SampRegisterList = RegisterBIZ.GetCuvetteRegisteredBarcodeInfo(deptid, depTime);

                }
            }
            catch (Exception ex)
            {
                Logger.LogException("GetCuvetteRegisteredBarcodeInfo", ex);
            }
            return SampRegisterList;
        }

        public List<EntityDicInstrument> GetInstrumentByComIds(List<string> ComIdList)
        {
            List<EntityDicInstrument> ItrList = new List<EntityDicInstrument>();
            try
            {
                InstrmtBIZ ItrBIZ = new InstrmtBIZ();
                if (ItrBIZ != null)
                {
                    ItrList = ItrBIZ.GetInstrumentByComIds(ComIdList);

                }
            }
            catch (Exception ex)
            {
                Logger.LogException("GetInstrumentByComIds", ex);
            }
            return ItrList;
        }

        public EntityDCLPrintData GetReportData(EntityStatisticsQC StatQc)
        {
            EntityDCLPrintData PrintData = new EntityDCLPrintData();
            try
            {
                SampRegisterBIZ RegisterBIZ = new SampRegisterBIZ();
                if (RegisterBIZ != null)
                {
                    PrintData = RegisterBIZ.GetReportData(StatQc);

                }
            }
            catch (Exception ex)
            {
                Logger.LogException("GetReportData", ex);
            }
            return PrintData;
        }

        public int GetSampRegisterMaxId()
        {
            int ret = 0;
            SampRegisterBIZ RegisterBIZ = new SampRegisterBIZ();
            if (RegisterBIZ != null)
                ret = RegisterBIZ.GetSampRegisterMaxId();

            return ret;
        }

        public void GetLastBarcodeAction(string barcode, out string name, out string time, out string remark, int status)
        {
            SampProcessDetailBIZ ProcessDetailBIZ = new SampProcessDetailBIZ();
            List<EntitySampProcessDetail> dsBarcode = new List<EntitySampProcessDetail>();
            if (ProcessDetailBIZ != null)
            {
                dsBarcode = ProcessDetailBIZ.GetSampProcessDetail(barcode);
            }
            if (dsBarcode != null && dsBarcode.Count > 0)
            {
                List<EntitySampProcessDetail> dt = dsBarcode;
                if (dt.Count > 0)
                {
                    List<EntitySampProcessDetail> dr = dt.Where(i => i.ProcStatus == status.ToString()).ToList();

                    if (dr.Count > 0)
                    {
                        name = dr[dr.Count - 1].ProcUsername.ToString();
                        time = dr[dr.Count - 1].ProcDate.ToString();
                        remark = dr[dr.Count - 1].ProcContent.ToString();
                    }
                    else
                    {
                        name = string.Empty;
                        time = string.Empty;
                        remark = string.Empty;
                    }
                }
                else
                {
                    name = string.Empty;
                    time = string.Empty;
                    remark = string.Empty;
                }
            }
            else
            {
                name = string.Empty;
                time = string.Empty;
                remark = string.Empty;
            }
        }
    }
}
