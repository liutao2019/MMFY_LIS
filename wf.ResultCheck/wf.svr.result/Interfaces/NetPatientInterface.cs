using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using dcl.pub.entities;
using dcl.common;
using dcl.common.extensions;
using lis.dto;
using dcl.svr.sample;
using dcl.svr.interfaces;
using Lis.HQHisInterface.HISRequest;
using Lis.HQHisInterface;
using dcl.root.logon;
using lis.common.extensions;

namespace dcl.svr.result
{
    public class NetPatientInterface : IPatientInterface<DataSet>
    {
        /// <summary>
        /// 虚拟数据：
        /// false = 使用院网接口
        /// true = 构造虚拟数据
        /// </summary>
        private bool bVirtualMode = false;
        TestsMock testMock;
        #region IPatientInterface 成员

        public DataSet Get(string code, string interfaceID)
        {
            DataTable dtPat = InterfaceDataStruct.Instance.GetStruct_Patients();

            //调用接口
            BCHISInterfacesBIZ InterfacesBIZ = new BCHISInterfacesBIZ();


            DataSet dsPatients;

            if (!bVirtualMode)
            {
#if DEBUG
                //模拟条码下载返回的字符串结果
                //testMock = TestsMock.InitForNormal();
                //if (testMock.ShouldTestDownLoaPatientInfo)
                //{
                //    dsPatients = testMock.MockDownloadPatientInfo(interfaceID, code);
                //}
                //else
#endif
                    if (dcl.svr.cache.CacheSysConfig.Current.GetSystemConfig("Interface_HospitalInterfaceMode") == "中医大附属第一医院")
                    {
                        HISRequestBuilder request = null;
                        dsPatients = new DataSet();

                        if (interfaceID == "10006")//门诊院网接口
                        {
                            HISRequest_MZ_GetPatientInfo req = new HISRequest_MZ_GetPatientInfo();
                            req.Id_Code = code;
                            request = req;
                        }
                        else if (interfaceID == "10008")//住院院网接口
                        {
                            HISRequest_ZY_GetPatientInfo req = new HISRequest_ZY_GetPatientInfo();
                            req.inpatient_no = code;
                            request = req;
                        }
                        else
                        {
                            dsPatients = InterfacesBIZ.ExecuteInterface(interfaceID, code);
                        }

                        string reqString = string.Empty;
                        try
                        {
                            reqString = request.Build();
                            string reponse = HqServiceHelper.GetProxy().Call(request);
                            Logger.WriteException(this.GetType().Name, "中医大调用his接口", string.Format("请求字符\r\n{0}\r\n\r\n返回字符\r\n{1}", request.Build(), reponse));

                            DataTable table = HISInterface.XmlToTable(reponse);
                            dsPatients.Tables.Add(table);
                        }
                        catch (Exception ex)
                        {
                            Logger.WriteException(this.GetType().Name, "中医大接口获取数据,Get", ex.ToString());
                            //throw;
                        }
                    }
                    else
                    {
                        dsPatients = InterfacesBIZ.ExecuteInterface(interfaceID, code);
                    }
            }
            else
            {
                #region 模拟院网数据
                dsPatients = new DataSet();
                DataTable tableVirtualPatient = new DataTable();

                //列定义
                //姓名
                string col_patientsName = "xm";

                //性别
                string col_sex = "xb";

                //科室代码
                string col_deptcode = "ksdm";

                //科室名称
                string col_deptname = "ksmc";

                //开单医生工号
                string col_doctcode = "kdysgh";

                //开单医生姓名
                string col_doctname = "kdys";

                //医嘱号\流水号
                string col_adviceid = "lsh";

                //
                string col_his_com_id = "xmdm";

                tableVirtualPatient.Columns.Add(col_patientsName);
                tableVirtualPatient.Columns.Add(col_sex);
                tableVirtualPatient.Columns.Add(col_deptcode);
                tableVirtualPatient.Columns.Add(col_deptname);
                tableVirtualPatient.Columns.Add(col_doctcode);
                tableVirtualPatient.Columns.Add(col_doctname);
                tableVirtualPatient.Columns.Add(col_adviceid);
                tableVirtualPatient.Columns.Add(col_his_com_id);


                DataRow rowVirtualPatient = tableVirtualPatient.NewRow();
                rowVirtualPatient[col_patientsName] = "测试病人";
                rowVirtualPatient[col_sex] = "1";
                rowVirtualPatient[col_adviceid] = "123abc";
                rowVirtualPatient[col_his_com_id] = "00290";

                tableVirtualPatient.Rows.Add(rowVirtualPatient);
                dsPatients.Tables.Add(tableVirtualPatient);
                #endregion
            }




            if (dsPatients == null || dsPatients.Tables.Count == 0)
                return null;

            DataTable dtPatients = dsPatients.Tables[0];
            if (Extensions.IsEmpty(dtPatients))
                return null;

            //对照表
            BCContrastBIZ ContrastBIZ = new BCContrastBIZ();
            DataSet dsContrast = ContrastBIZ.Search(string.Format("con_interface_id = {0}", interfaceID));
            if (dsContrast.IsEmpty())
                return null;

            DataTable dtContrast = dsContrast.Tables[BarcodeTable.Contrast.TableName];
            if (dtContrast.IsEmpty())
                return null;


            //对照表转换

            foreach (DataRow patientRow in dtPatients.Rows)
            {
                DataRow row = dtPat.NewRow();
                for (int i = 0; i < dtContrast.Rows.Count; i++)
                {
                    if (
                        Compare.IsEmpty(dtContrast.Rows[i][BarcodeTable.Contrast.HisColumns])
                        || Compare.IsEmpty(dtContrast.Rows[i][BarcodeTable.Contrast.LisColumns])
                        )
                        continue;

                    string lisColName = dtContrast.Rows[i][BarcodeTable.Contrast.LisColumns].ToString();
                    string hisColName = dtContrast.Rows[i][BarcodeTable.Contrast.HisColumns].ToString();

                    if (patientRow.Table.Columns.Contains(hisColName))
                    {
                        if (Compare.IsEmpty(dtContrast.Rows[i][BarcodeTable.Contrast.Rule]))
                        {
                            //如果是DateTime类型，值又为空
                            if (dtContrast.Rows[i][BarcodeTable.Contrast.DataType] != null &&
                                dtContrast.Rows[i][BarcodeTable.Contrast.DataType] != DBNull.Value &&
                                dtContrast.Rows[i][BarcodeTable.Contrast.DataType].ToString() == "System.DateTime"
                                && (patientRow[hisColName] == null || patientRow[hisColName] == DBNull.Value || string.IsNullOrEmpty(patientRow[hisColName].ToString()))
                                )
                                row[lisColName] = DBNull.Value;
                            else
                                row[lisColName] = patientRow[hisColName];
                        }
                        else
                        {
                            //如果是DateTime类型，值又为空
                            if (dtContrast.Rows[i][BarcodeTable.Contrast.DataType] != null &&
                                dtContrast.Rows[i][BarcodeTable.Contrast.DataType] != DBNull.Value &&
                                dtContrast.Rows[i][BarcodeTable.Contrast.DataType].ToString() == "System.DateTime"
                                && (patientRow[hisColName] == null || patientRow[hisColName] == DBNull.Value || string.IsNullOrEmpty(patientRow[hisColName].ToString()))
                                )
                                row[lisColName] = DBNull.Value;
                            else
                            {
                                //规则转换
                                IRule rule = IRule.CreateRule(dtContrast.Rows[i][BarcodeTable.Contrast.Rule].ToString());
                                row[lisColName] = rule.ConvertRule(patientRow[hisColName].ToString());
                            }
                        }
                    }
                }
                dtPat.Rows.Add(row);
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dtPat);
            return ds;
        }

        #endregion
    }
}
