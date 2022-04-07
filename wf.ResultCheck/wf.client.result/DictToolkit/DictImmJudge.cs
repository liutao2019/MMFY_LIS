using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using dcl.client.common;
using dcl.common;
using dcl.entity;
using dcl.client.cache;

namespace dcl.client.result.DictToolkit
{
    public class DictImmJudge
    {
        public static DictImmJudge Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DictImmJudge();
                }
                return _instance;
            }
        }

        private static DictImmJudge _instance;

        public List<EntityDicElisaCriter> dtDictImmJudge
        {
            get
            {
                return CacheClient.GetCache<EntityDicElisaCriter>();
            }
        }


        public string GetJudge(string itm_id, string input)
        {
            string output = string.Empty;

            decimal decInputValue = 0;

            if (decimal.TryParse(input, out decInputValue))
            {
                List<EntityDicElisaCriter> drs = dtDictImmJudge.FindAll(i => i.CriItmId == itm_id);

                if (drs.Count > 0)
                {
                    DataTable dtCal = new DataTable();

                    foreach (EntityDicElisaCriter dr in drs)
                    {
                        string strImjValue = string.Empty;

                        if (string.IsNullOrEmpty(dr.CriExpression))
                        {
                            strImjValue = dr.CriValue.ToString();
                        }
                        else
                        {
                            strImjValue = dr.CriExpression;
                        }
                        decimal decImjValue = 0;

                        //弱阳性判断
                        if (!Compare.IsNullOrDBNull(dr.CriWposLowerLimit)
                            && !Compare.IsNullOrDBNull(dr.CriWposUpperLimit))
                        {
                            try
                            {
                                string formula_bpos_1 = decInputValue + ">=" + dr.CriWposLowerLimit;
                                string formula_bpos_2 = decInputValue + "<=" + dr.CriWposUpperLimit;

                                if (Convert.ToBoolean(dtCal.Compute(formula_bpos_1, string.Empty))
                                    && Convert.ToBoolean(dtCal.Compute(formula_bpos_2, string.Empty)))
                                {
                                    return "弱阳性(±)";
                                }
                            }
                            catch
                            {
                            }
                        }

                        if (decimal.TryParse(strImjValue, out decImjValue))
                        {
                            //定性判定
                            string imj_res = dr.CriResult;
                            string imj_express = dr.CriJudge;

                            string formula = decInputValue + imj_express + strImjValue;


                            //阴阳性判断
                            try
                            {
                                object objResult = dtCal.Compute(formula, string.Empty);

                                if (objResult.GetType() == typeof(bool))
                                {
                                    if (Convert.ToBoolean(objResult) == true)
                                    {
                                        if (imj_res == "pos")
                                        {
                                            return "阳性(+)";
                                        }
                                        else
                                        {
                                            return "阴性(-)";
                                        }
                                    }
                                    else
                                    {
                                        if (imj_res == "pos")
                                        {
                                            return "阴性(-)";
                                        }
                                        else
                                        {
                                            return "阳性(+)";
                                        }
                                    }
                                }

                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }

            return output;
        }

    }
}
