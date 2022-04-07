using System;
using System.Collections.Generic;

using System.Text;
using System.Data;
using dcl.client.common;
using System.Diagnostics;
using dcl.root.logon;
using dcl.common;
using dcl.entity;
using System.Linq;
using dcl.client.cache;

namespace dcl.client.result.CommonPatientInput
{
    public class DictItem
    {
        public List<EntityDicItemSample> DictItemSam_schema = null;
        public List<EntityDicItmRefdetail> DictItemRef_schema = null;

        public DictItem()
        {
            DictItemSam_schema = EntityManager<EntityDicItemSample>.ListClone(this.Dict_item_sam);
            DictItemRef_schema = EntityManager<EntityDicItmRefdetail>.ListClone(this.Dict_item_ref);
        }

        public static DictItem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DictItem();
                }
                return _instance;
            }
        }

        private static DictItem _instance;

        /// <summary>
        /// 项目基本信息表
        /// </summary>
        public List<EntityDicItmItem> Dict_item
        {
            get
            {
                List<EntityDicItmItem> list = CacheClient.GetCache<EntityDicItmItem>();
                return list;
            }
        }

        /// <summary>
        /// 项目样本表
        /// </summary>
        public List<EntityDicItemSample> Dict_item_sam
        {
            get
            {
                List<EntityDicItemSample> list = CacheClient.GetCache<EntityDicItemSample>();
                return list;
            }
        }

        /// <summary>
        /// 项目参考值表
        /// </summary>
        public List<EntityDicItmRefdetail> Dict_item_ref
        {
            get
            {
                List<EntityDicItmRefdetail> list = CacheClient.GetCache<EntityDicItmRefdetail>();
                return list;
            }
        }

        /// <summary>
        /// 项目特征表
        /// </summary>
        public List<EntityDefItmProperty> DclItmProp
        {
            get
            {
                List<EntityDefItmProperty> listItmProp = CacheClient.GetCache<EntityDefItmProperty>();
                return listItmProp;
            }
        }
        public List<EntityDicUtgentValue> Dict_item_UtgentValue
        {
            get
            {
                List<EntityDicUtgentValue> list = CacheClient.GetCache<EntityDicUtgentValue>();
                return list;
            }
        }

        /// <summary>
        /// 根据项目ID获取单个项目
        /// </summary>
        /// <param name="item_id"></param>
        /// <returns></returns>
        public EntityDicItmItem GetItem(string item_id)
        {
            List<EntityDicItmItem> list = this.Dict_item.Where(w => w.ItmId == item_id).ToList();

            if (list.Count > 0)
            {
                return list[0];
            }
            else
            {
                return null;
            }
        }


        void FillUrgentValue(EntityDicItmRefdetail itmRef, string sam_id, int ageMinute, string sex, string dep_code, string patDiag)
        {
            if (itmRef != null)
            {
                if (string.IsNullOrEmpty(dep_code))
                    dep_code = "-1";
                if (string.IsNullOrEmpty(patDiag))
                    patDiag = "-1";

                if (string.IsNullOrEmpty(sam_id))
                    sam_id = "-1";

                if (sex == "1" || sex == "男")
                    sex = "男";
                else if (sex == "2" || sex == "女")
                    sex = "女";
                else
                    sex = string.Empty;

                try
                {
                    List<EntityDicUtgentValue> list = this.Dict_item_UtgentValue.Where(w => w.UgtItmId == itmRef.ItmId && w.UgtSamId == sam_id
                                                                 && ageMinute >= Convert.ToInt32(w.UgtAgeL) && ageMinute <= Convert.ToInt32(w.UgtAgeH) && (w.UgtSex == sex || w.UgtSex == "0"
                                                                  || w.UgtSex == null) && (w.UgtDepCode == dep_code || w.UgtSex == "-1") && w.UgtIcdName.Contains(patDiag)).ToList();
                    if (list.Count > 0)
                    {
                        itmRef.ItmDangerUpperLimit = list[0].UgtPanH;
                        itmRef.ItmDangerUpperLimitCal = list[0].UgtPanH;
                        itmRef.ItmDangerUpperLimit = list[0].UgtPanL;
                        itmRef.ItmDangerUpperLimitCal = list[0].UgtPanL;
                        itmRef.ItmMaxValue = list[0].UgtMax;
                        itmRef.ItmMaxValueCal = list[0].UgtMax;
                        itmRef.ItmMinValue = list[0].UgtMin;
                        itmRef.ItmMinValueCal = list[0].UgtMin;
                    }
                }
                catch(Exception ex)
                {
                    Lib.LogManager.Logger.LogException(ex);
                }
                

            }
        }
        /// <summary>
        /// 根据获取项目ID集合获取多个项目
        /// </summary>
        /// <param name="itemsID"></param>
        /// <returns></returns>
        //public DataRow[] GetItems(IEnumerable<string> itemsID)
        //{
        //    if (IEnumerableUtil.Count(itemsID) > 0)
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        foreach (string itemID in itemsID)
        //        {
        //            sb.Append(string.Format(",'{0}'", itemID));
        //        }
        //        sb.Remove(0, 1);
        //        DataRow[] drs = Dict_item.Select(string.Format("itm_id in ()", sb.ToString()));
        //        return drs;
        //    }
        //    else
        //    {
        //        return Dict_item.Select("item_id = '-1'");
        //    }
        //}

        /// <summary>
        /// 获取单个项目的参考值
        /// </summary>
        /// <param name="itm_id">项目ID</param>
        /// <param name="sam_id">样本类型ID</param>
        /// <param name="ageMinute">年龄</param>
        /// <param name="sex"></param>
        /// <param name="sex">性别 "1"=男 "2"=女 其它值=男</param>
        /// <returns></returns>
        public EntityDicItmRefdetail GetItemRef(string itm_id, string sam_id, int ageMinute, string sex, string depcode, string patDiag)
        {
            if (sex == null)
            {
                sex = string.Empty;
            }

            int age = ageMinute;
            //if (age == 0)
            //{
            //    age = -1;
            //}


            List<EntityDicItmRefdetail> listItemRef = this.Dict_item_ref.Where(w => w.ItmId == itm_id &&
            w.ItmSamId == sam_id && age >= w.ItmAgeLowerLimit && age <= w.ItmAgeUpperLimit && (w.ItmSex == sex || w.ItmSex == "0")).OrderByDescending(w => w.ItmSex).ToList();
            if (listItemRef.Count > 0)
            {
                EntityDicItmRefdetail ItemRef = listItemRef[0];
                FillUrgentValue(ItemRef, sam_id, age, sex, depcode, patDiag);
                return ItemRef;
            }
            else
            {
                return new EntityDicItmRefdetail();
            }
        }

        /// <summary>
        /// 获取多个项目的参考值
        /// </summary>
        /// <param name="items_id">项目ID集合</param>
        /// <param name="sam_id">样本类型ID</param>
        /// <param name="ageMinute">年龄</param>
        /// <param name="getConfigOnNullSex"></param>
        /// <param name="sex">性别 "1"=男 "2"=女 其它值=默认</param>
        /// <param name="sam_rem">标本备注</param>
        /// <param name="itr_id"></param>
        /// <returns></returns>
        /// <returns></returns>
        public List<EntityDicItmRefdetail> GetItemsRef(IEnumerable<string> items_id, string sam_id, int ageMinute, string sex, string sam_rem, string itr_id, string dep_id, string patDiag)
        {
            if (sex == null)
            {
                sex = string.Empty;
            }

            if (sam_rem == null)
                sam_rem = "";

            int age = ageMinute;

            List<string> listItmids = new List<string>();
            foreach (string itm_id in items_id)
            {
                listItmids.Add(itm_id);
            }

            string select = string.Empty;
            List<string> listItemSam_id = new List<string>();
            List<EntityDicItmRefdetail> listReturn = new List<EntityDicItmRefdetail>();
            List<string> listSamIds = new List<string>();
            listSamIds.Add(sam_id);
            listSamIds.Add("-1");
            if (!string.IsNullOrEmpty(itr_id))
            {

                List<EntityDicItemSample> listItemSam = (from x in Dict_item_sam where listItmids.Contains(x.ItmId) && listSamIds.Contains(x.ItmSamId) && x.ItmItrId==itr_id select x).ToList();

                if (listItemSam.Count > 0)
                {
                    for (int i = 0; i < listItemSam.Count; i++)
                    {
                        EntityDicItemSample drItem = listItemSam[i];
                        listItemSam_id.Add(drItem.ItmId);
                    }
                }
            }

            List<string> listsb = new List<string>();
            foreach (string itm_id in items_id)
            {
                if (!listItemSam_id.Contains(itm_id))
                    listsb.Add(itm_id);
            }
            if (listsb.Count > 0)
            {
                try
                {
                    List<EntityDicItmRefdetail> dvItem = (from x in Dict_item_ref
                                                          where listsb.Contains(x.ItmId) && listSamIds.Contains(x.ItmSamId) &&
                                     age >= x.ItmAgeLowerMinute && age <= x.ItmAgeUpperMinute && (x.ItmSex == sex || x.ItmSex == "0") &&
                                   (x.ItmSamRemark == "" || x.ItmSamRemark == null || x.ItmSamRemark == sam_rem) && (x.ItmItrId == itr_id || x.ItmItrId == "-1")
                                   && x.ItmRefFlag == 0
                                                          select x).OrderByDescending(w => w.ItmSamId).ThenByDescending(w => w.ItmSex).ThenByDescending(w => w.ItmSamRemark).ThenByDescending(w => w.ItmItrId).ToList();

                    if (dvItem.Count > 0)
                    {
                        for (int i = 0; i < dvItem.Count; i++)
                        {
                            EntityDicItmRefdetail drItem = dvItem[i];
                            FillUrgentValue(drItem, sam_id, age, sex, dep_id, patDiag);
                            FillItemRef(drItem);
                            listReturn.Add(drItem);

                        }
                    }

                }
                catch (Exception ex)
                {

                }
            }


            List<string> listsb2 = new List<string>();
            foreach (string itm_id in listItemSam_id)
            {
                listsb2.Add(itm_id);
            }
            if (listsb2.Count > 0)
            {

                try
                {
                    List<EntityDicItmRefdetail> dvItem = (from x in Dict_item_ref
                                                          where listsb2.Contains(x.ItmId) && listSamIds.Contains(x.ItmSamId) &&
                                  age >= x.ItmAgeLowerMinute && age <= x.ItmAgeUpperMinute && (x.ItmSex == sex || x.ItmSex == "0") &&
                                (x.ItmSamRemark == "" || x.ItmSamRemark == null || x.ItmSamRemark == sam_rem) && (x.ItmItrId == itr_id || x.ItmItrId == "-1")
                                && x.ItmRefFlag == 0
                                                          select x).OrderByDescending(w => w.ItmSamId).ThenByDescending(w => w.ItmSex).ThenByDescending(w => w.ItmSamRemark).ThenByDescending(w => w.ItmItrId).ToList();

                    if (dvItem.Count > 0)
                    {
                        for (int i = 0; i < dvItem.Count; i++)
                        {
                            EntityDicItmRefdetail drItem = dvItem[i];
                            FillUrgentValue(drItem, sam_id, age, sex, dep_id, patDiag);
                            FillItemRef(drItem);
                            listReturn.Add(drItem);
                        }
                    }

                }
                catch (Exception ex)
                {

                }
            }
            return listReturn;

        }
        /// <summary>
        /// 填充参考值
        /// </summary>
        /// <param name="refDetail"></param>
        public void FillItemRef(EntityDicItmRefdetail refDetail)
        {
            #region 格式化参考值
            string str_itm_ref_l = string.Empty;
            string str_itm_ref_h = string.Empty;

            if (!string.IsNullOrEmpty(refDetail.ItmLowerLimitValueCal) && refDetail.ItmLowerLimitValueCal.Trim(null) != string.Empty)
            {
                str_itm_ref_l = refDetail.ItmLowerLimitValueCal.Trim(null);

                if (str_itm_ref_l.StartsWith(">") || str_itm_ref_l.StartsWith(">=") || str_itm_ref_h.StartsWith("≥"))
                {
                    str_itm_ref_l = str_itm_ref_l.Replace(">=", "").Replace(">", "").Replace("≥", "");

                    refDetail.ItmLowerLimitValueCal = str_itm_ref_l; // 2010-7-20
                                                                     //drRef[field_refh] = string.Empty;
                }
                else if (str_itm_ref_l.StartsWith("<") || str_itm_ref_l.StartsWith("<=") || str_itm_ref_l.StartsWith("≤"))
                {
                    str_itm_ref_h = str_itm_ref_l.Replace("<=", "").Replace("<", "").Replace("≤", "");

                    refDetail.ItmLowerLimitValueCal = string.Empty;// 2010-7-20
                    refDetail.ItmUpperLimitValueCal = str_itm_ref_h;
                }
            }

            if (!string.IsNullOrEmpty(refDetail.ItmUpperLimitValueCal) && refDetail.ItmUpperLimitValueCal.Trim(null) != string.Empty)
            {
                str_itm_ref_h = refDetail.ItmUpperLimitValueCal.Trim(null);


                if (str_itm_ref_h.StartsWith(">") || str_itm_ref_h.StartsWith(">=") || str_itm_ref_h.StartsWith("≥"))
                {
                    str_itm_ref_l = str_itm_ref_l.Replace(">=", "").Replace(">", "").Replace("≥", "");
                    refDetail.ItmLowerLimitValueCal = str_itm_ref_l; // 2010-7-20
                    refDetail.ItmUpperLimitValueCal = string.Empty;
                }
                else if (str_itm_ref_h.StartsWith("<") || str_itm_ref_h.StartsWith("<=") || str_itm_ref_l.StartsWith("≤"))
                {
                    str_itm_ref_h = str_itm_ref_h.Replace("<=", "").Replace("<", "").Replace("≤", "");
                    // drRef[field_refl] = string.Empty;// 2010-7-20
                    refDetail.ItmUpperLimitValueCal = str_itm_ref_h;
                }
            }
            #endregion

            #region 格式化阈值
            string str_itm_min = string.Empty;
            string str_itm_max = string.Empty;

            if (!string.IsNullOrEmpty(refDetail.ItmMinValueCal))
            {
                if (refDetail.ItmMinValueCal.Trim(null) != string.Empty)
                {
                    str_itm_min = refDetail.ItmMinValueCal.Trim(null);

                    if (str_itm_min.StartsWith(">") || str_itm_min.StartsWith(">=") || str_itm_max.StartsWith("≥"))
                    {
                        str_itm_min = str_itm_min.Replace(">=", "").Replace(">", "").Replace("≥", "");
                        refDetail.ItmMinValueCal = str_itm_min;
                    }
                    else if (str_itm_min.StartsWith("<") || str_itm_min.StartsWith("<=") || str_itm_min.StartsWith("≤"))
                    {
                        str_itm_max = str_itm_min.Replace("<=", "").Replace("<", "").Replace("≤", "");
                        refDetail.ItmMaxValueCal = str_itm_max;
                        refDetail.ItmMinValueCal = string.Empty;
                    }
                }
            }

            if (!string.IsNullOrEmpty(refDetail.ItmMaxValueCal))
            {
                if (refDetail.ItmMaxValueCal.Trim(null) != string.Empty)
                {
                    str_itm_max = refDetail.ItmMaxValueCal.Trim(null);


                    if (str_itm_max.StartsWith(">") || str_itm_max.StartsWith(">=") || str_itm_max.StartsWith("≥"))
                    {
                        str_itm_min = str_itm_min.Replace(">=", "").Replace(">", "").Replace("≥", "");
                        refDetail.ItmMinValueCal = str_itm_min;
                        refDetail.ItmMaxValueCal = string.Empty;
                    }
                    else if (str_itm_max.StartsWith("<") || str_itm_max.StartsWith("<=") || str_itm_min.StartsWith("≤"))
                    {
                        str_itm_max = str_itm_max.Replace("<=", "").Replace("<", "").Replace("≤", "");
                        refDetail.ItmMaxValueCal = str_itm_max;
                    }
                }
            }
            #endregion

            #region 格式化危急值
            string str_pan_l = string.Empty;
            string str_pan_h = string.Empty;

            if (!string.IsNullOrEmpty(refDetail.ItmDangerLowerLimitCal) && refDetail.ItmDangerLowerLimitCal.Trim(null) != string.Empty)
            {
                str_pan_l = refDetail.ItmDangerLowerLimitCal.Trim(null);

                if (str_pan_l.StartsWith(">") || str_pan_l.StartsWith(">=") || str_pan_h.StartsWith("≥"))
                {
                    str_pan_l = str_pan_l.Replace(">=", "").Replace(">", "").Replace("≥", "");
                    refDetail.ItmDangerLowerLimitCal = str_pan_l;
                }
                else if (str_pan_l.StartsWith("<") || str_pan_l.StartsWith("<=") || str_pan_l.StartsWith("≤"))
                {
                    str_pan_h = str_pan_l.Replace("<=", "").Replace("<", "").Replace("≤", "");
                    refDetail.ItmDangerUpperLimitCal = str_pan_h;
                    refDetail.ItmDangerLowerLimitCal = string.Empty;
                }
            }

            if (!string.IsNullOrEmpty(refDetail.ItmDangerUpperLimitCal) && refDetail.ItmDangerUpperLimitCal.Trim(null) != string.Empty)
            {
                str_pan_h = refDetail.ItmDangerUpperLimitCal.Trim(null);


                if (str_pan_h.StartsWith(">") || str_pan_h.StartsWith(">=") || str_pan_h.StartsWith("≥"))
                {
                    str_pan_l = str_pan_h.Replace(">=", "").Replace(">", "").Replace("≥", "");
                    refDetail.ItmDangerLowerLimitCal = str_pan_l;
                    refDetail.ItmDangerUpperLimitCal = string.Empty;
                }
                else if (str_pan_h.StartsWith("<") || str_pan_h.StartsWith("<=") || str_pan_l.StartsWith("≤"))
                {
                    str_pan_h = str_pan_h.Replace("<=", "").Replace("<", "").Replace("≤", "");
                    refDetail.ItmDangerUpperLimitCal = str_pan_h;
                }
            }
            #endregion
        }
        /// <summary>
        /// 获取项目代码对应的项目特征列表
        /// </summary>
        /// <param name="_itm_ecd"></param>
        /// <returns></returns>
        public List<EntityDefItmProperty> GetItmProp(string itm_id)
        {
            List<EntityDefItmProperty> list = new List<EntityDefItmProperty>();
            foreach (EntityDefItmProperty entity in DclItmProp.FindAll(w => w.PtyItmId == itm_id))
            {
                list.Add(entity);
            }
            foreach (EntityDefItmProperty entity in DclItmProp.FindAll(w => w.PtyItmFlag == 1))
            {
                list.Add(entity);
            }
            return list;
        }

        /// <summary>
        /// 获取项目代码对应的项目特征列表
        /// </summary>
        /// <param name="_itm_ecd"></param>
        /// <returns></returns>
        public List<EntityDefItmProperty> GetDclItmProp(string itm_id)
        {
            List<EntityDefItmProperty> list = new List<EntityDefItmProperty>();
            foreach (EntityDefItmProperty itmProp in DclItmProp.Where(w => w.PtyItmId == itm_id).OrderBy(w => w.PtySortNo))
            {
                list.Add(itmProp);
            }

            foreach (EntityDefItmProperty itmProp in DclItmProp.Where(w => w.PtyItmFlag == 1).OrderBy(w => w.PtySortNo))
            {
                list.Add(itmProp);
            }

            return list;
        }
        public string GetItmProp(string itm_id, string in_code)
        {
            if (string.IsNullOrEmpty(in_code))
            {
                return string.Empty;
            }

            string incode = SQLFormater.FormatSQL(in_code);

            List<EntityDefItmProperty> listItmProps = DclItmProp.FindAll(i => i.PtyCCode == incode && (i.PtyItmId == itm_id || (i.PtyItmFlag != null && i.PtyItmFlag.Value == 1)));

            if (listItmProps.Count > 0)
            {
                if (!Compare.IsEmpty(listItmProps[0].PtyItmProperty))
                {
                    return listItmProps[0].PtyItmProperty;
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 根据样本类型ID获取多个项目的样本信息
        /// </summary>
        /// <param name="items_id"></param>
        /// <param name="sam_id"></param>
        /// <returns></returns>
        public List<EntityDicItemSample> GetItemSam(IEnumerable<string> items_id, string sam_id)
        {
            List<EntityDicItemSample> list = EntityManager<EntityDicItemSample>.ListClone(Dict_item_sam);

            if (IEnumerableUtil.Count(items_id) > 0)
            {
                List<string> listsb = new List<string>();
                foreach (string itm_id in items_id)
                {
                    listsb.Add(itm_id);
                }
                List<string> listItmSamId = new List<string>();
                listItmSamId.Add(sam_id);
                listItmSamId.Add("-1");
                //                string select = string.Format(@"
                //itm_id in ({0}) and itm_sam_id in ('{1}','-1')
                //", sb.ToString(), sam_id);


                //                itm_id in ({0})
                //and itm_sam_id in('{1}','-1')
                //and  {2}>=itm_age_ls
                //and  {2}<=itm_age_hs
                //and (itm_sex='{3}' or itm_sex='0')
                //and (itm_sam_rem = '' or itm_sam_rem is null or itm_sam_rem='{4}')
                //and itm_flag='0'"

                //DataView dvItem = new DataView(Dict_item_sam);
                //dvItem.RowFilter = select;
                //dvItem.Sort = "itm_sam_id desc,itm_sex desc,itm_sam_rem desc";

                ////DataRow[] drs = this.Dict_item_sam.Select(select);

                //foreach (DataRow dr in drs)
                //{
                //    dt.ImportRow(dr);
                //}

                List<EntityDicItemSample> dvItem = (from x in Dict_item_sam where listsb.Contains(x.ItmId) && listItmSamId.Contains(x.ItmSamId) select x).OrderByDescending(w => w.ItmSamId).ToList();
                //DataTable dtReturn = this.Dict_item_sam.Clone();
                if (dvItem.Count > 0)
                {
                    for (int i = 0; i < dvItem.Count; i++)
                    {
                        EntityDicItemSample drItem = dvItem[i];
                        //ItemRefFormatter.Format(drItem);
                        list.Add(drItem);
                    }
                }

            }
            return list;
        }
    }
}
