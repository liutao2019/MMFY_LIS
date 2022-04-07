using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Lib.LogManager;
using dcl.common;
using dcl.entity;
using dcl.dao.interfaces;

namespace dcl.svr.samstock
{
    public class SampStoreDetailBIZ : dcl.servececontract.ISampStoreDetail
    {
        public string BatchHandData(List<EntitySampStoreDetail> table, string strSsid, string strRackID, string cuvcode, string operatorName, string operatorID, string opPlace, int rowhander, int colHander, string rack_Barcode)
        {
            try
            {
                SampStoreRecordBIZ StoreRecordBIZ = new SampStoreRecordBIZ();
                if (table.Count == 0)
                {
                    return "无数据可以归档";
                }

                List<string> barcodeList = new List<string>();
                List<EntitySampStoreDetail> returnDT = new List<EntitySampStoreDetail>();
                foreach (EntitySampStoreDetail row in table)
                {
                    if (row.RepBarCode == null || string.IsNullOrEmpty(row.RepBarCode.ToString())
                        || (row.DetBarCode != null && !string.IsNullOrEmpty(row.DetBarCode.ToString())))
                        continue;
                    if (!barcodeList.Contains(row.RepBarCode.ToString()))
                    {
                        barcodeList.Add(row.RepBarCode.ToString());
                        returnDT.Add(row);
                    }
                }

                if (returnDT.Count == 0)
                {
                    return "该批次查询数据条码已经归档或者无条码号";
                }
                int maxSeq = 1;

                List<EntitySampStoreDetail> dtNow = GetRackDetail(strSsid);
                int samDetailCount = 0;
                if (dtNow != null && dtNow.Count > 0)
                {
                    samDetailCount = dtNow.Count;
                    object objmaxSeq = dtNow.OrderBy(i => i.DetSeqno).FirstOrDefault().DetSeqno;
                    if (objmaxSeq == DBNull.Value)
                    {
                        maxSeq = 1;
                    }
                    else
                    {
                        maxSeq = Convert.ToInt32(objmaxSeq) + 1;
                    }
                }

                List<EntityDicTubeRack> dtRackSpec = StoreRecordBIZ.GetCuvShelf();
                List<EntityDicTubeRack> rows = dtRackSpec.Where(i => i.RackCode == cuvcode).ToList();


                int x = Convert.ToInt32(rows[0].RackXAmount);
                int y = Convert.ToInt32(rows[0].RackYAmount);

                if (rowhander > 0 || colHander > 1)
                {
                    int seq = rowhander * x + colHander;

                    if (maxSeq == seq + 1)
                    {
                        return "该位置已经归档,请重新选择试管架孔";
                    }
                    maxSeq = seq;
                }





                if (maxSeq > (x * y))
                {
                    return "请更换试管架";
                }

                bool isBreak = false;
                string txtNumX;
                string txtNumY;
                for (int i = 0; i < returnDT.Count; i++)
                {

                    EntitySampStoreDetail row = returnDT[i];
                    if (maxSeq > (x * y))
                    {
                        isBreak = true;
                        break;
                    }
                    if (maxSeq < x)
                    {
                        txtNumX = (maxSeq / x + 1).ToString();
                    }
                    else if (maxSeq == x)
                    {
                        txtNumX = (maxSeq / x).ToString();
                    }
                    else if (maxSeq % x == 0)
                    {
                        txtNumX = (maxSeq / x).ToString();
                    }
                    else
                    {
                        txtNumX = (maxSeq / x + 1).ToString();
                    }

                    txtNumY = maxSeq % x == 0 ? x.ToString() : (maxSeq % x).ToString();


                    if (dtNow != null && dtNow.Count > 0 && dtNow.Where(e => e.DetX == int.Parse(txtNumX) && e.DetY == int.Parse(txtNumY)).ToList().Count > 0)
                    {
                        continue;
                    }

                    EntitySampStoreDetail entity = new EntitySampStoreDetail();
                    entity.DetId = strSsid;

                    entity.DetBarCode = row.RepBarCode.ToString();
                    entity.DetX = Convert.ToInt32(txtNumX);
                    entity.DetY = Convert.ToInt32(txtNumY);
                    entity.DetSeqno = maxSeq;
                    entity.DetStatus = 5; //未审核
                    maxSeq = maxSeq + 1;
                    InsertRackDetail(entity);

                    string remark = string.Format("归档架子号:{0},孔号:{1}X{2}", rack_Barcode, txtNumX, txtNumY);
                    StoreRecordBIZ.InsertBcSign(operatorName, operatorID, entity.DetBarCode, "110", remark, opPlace);

                    samDetailCount += 1;


                }

                EntitySampStoreRack entitySamRack = new EntitySampStoreRack();
                entitySamRack.SrId = strSsid;
                entitySamRack.SrStatus = 5; //未审核
                entitySamRack.SrAmount = samDetailCount;
                entitySamRack.SrRackId = strRackID;
                StoreRecordBIZ.ModifySamStoreRack(entitySamRack);

                EntityDicSampTubeRack entityRack = new EntityDicSampTubeRack();
                entityRack.RackId = strRackID;
                entityRack.RackStatus = 5;
                StoreRecordBIZ.ModifyRackStatus(entityRack);
                if (isBreak)
                {
                    return "该试管架已满，请更换试管架";
                }
                return string.Empty;

            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 BatchHandData", ex);
                return ex.Message;
            }
        }

        /// <summary>
        /// 删除试管存储信息
        /// </summary>
        /// <param name="strSsid"></param>
        /// <param name="rackID"></param>
        /// <param name="barcodeList"></param>
        /// <returns></returns>
        public int DeleteSampStoreDetail(string strSsid, string barcode)
        {
            int intRet = -1;
            try
            {
                IDaoSampStoreDetail dao = DclDaoFactory.DaoHandler<IDaoSampStoreDetail>();

                 intRet = dao.DeleteSampStoreDetail(strSsid, barcode);
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 ModifySamDetail", ex);
            }
            return intRet;
        }

        public List<EntitySampStoreDetail> GetBatchHandData(DateTime date, string BatchItr, string SamFrom, string SamTo, int selectIndex)
        {

            try
            {

                IDaoSampStoreDetail dao = DclDaoFactory.DaoHandler<IDaoSampStoreDetail>();
                List<EntitySampStoreDetail> list = dao.GetSamDetail(date, BatchItr, SamFrom, SamTo, selectIndex);


                List<string> barcodeList = new List<string>();
                List<EntitySampStoreDetail> returnDT = new List<EntitySampStoreDetail>();
                foreach (EntitySampStoreDetail row in list)
                {
                    if (row.DetBarCode == null || row.DetBarCode == "")
                    {
                        row.Checked = true;
                    }

                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Sample_SamStoreNullBarCode") == "是")
                    {
                        returnDT.Add(row);
                    }
                    else
                    {
                        if (row.RepBarCode == null || string.IsNullOrEmpty(row.RepBarCode.ToString()))
                            continue;
                        if (!barcodeList.Contains(row.RepBarCode.ToString()))
                        {
                            barcodeList.Add(row.RepBarCode.ToString());
                            returnDT.Add(row);
                        }
                    }
                }

                return returnDT;

            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 GetBatchHandData", ex);
                return null;
            }
        }

        public List<EntitySampStoreDetail> GetRackDetail(string strSsid)
        {
            List<EntitySampStoreDetail> dt = null;
            List<EntitySampStoreDetail> returnDT = new List<EntitySampStoreDetail>();

            try
            {
                IDaoSampStoreDetail dao = DclDaoFactory.DaoHandler<IDaoSampStoreDetail>();
                dt = dao.GetRackDetail(strSsid);

                List<string> barcodeList = new List<string>();
                foreach (EntitySampStoreDetail row in dt)
                {
                    if (!barcodeList.Contains(row.DetBarCode.ToString()))
                    {
                        barcodeList.Add(row.DetBarCode.ToString());
                        returnDT.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 GetRackDetail", ex);
            }
            return returnDT;
        }

        public int GetSampStoreDetailCount(string strSsid)
        {
            int intRet = -1;
            try
            {
                IDaoSampStoreDetail dao = DclDaoFactory.DaoHandler<IDaoSampStoreDetail>();

                intRet = dao.GetSampStoreDetailCount(strSsid);
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 ModifySamDetail", ex);
            }
            return intRet;
        }

        public int InsertRackDetail(EntitySampStoreDetail entity)
        {
            int intRet = 0;
            try
            {
                IDaoSampStoreDetail dao = DclDaoFactory.DaoHandler<IDaoSampStoreDetail>();
                bool IsExists = dao.AddSampStoreDetail(entity);
                if (IsExists)
                {
                    intRet++;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException("标本管理 InsertRackDetail", ex);
            }

            return intRet;
        }
    }
}
