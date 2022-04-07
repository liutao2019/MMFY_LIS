using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using FssyZnfzDataHelper;
using Lib.ProxyFactory;
using dcl.pub.entities;
using dcl.pub.entities.dict;
using dcl.root.logon;
using dcl.root.dac;
using dcl.entity;

namespace dcl.svr.resultcheck
{

    public class FssyZnfzDataSender
    {
        EntityPidReportMain _patInfo;
         public FssyZnfzDataSender(EntityPidReportMain patInfo)
        {
            this._patInfo = patInfo;
        }

        public void Execute()
        {

            if (_patInfo.PidSrcId == "107" && !string.IsNullOrEmpty(_patInfo.RepBarCode))
            {
                Thread t = new Thread(ThredExecute);

                t.Start();
            }
        }

        void ThredExecute()
        {
            try
            {

                string znfzWebserviceUrl = ConfigurationManager.AppSettings["ZNFZWebserviceUrl"];

                if (!string.IsNullOrEmpty(znfzWebserviceUrl))
                {
                    XmlDocument xmldoc = new XmlDocument();

                    XmlDeclaration xmldecl;
                    xmldecl = xmldoc.CreateXmlDeclaration("1.0", "utf-8", null);
                    xmldoc.AppendChild(xmldecl);

                    XmlElement xmlelem = xmldoc.CreateElement("Response");
                    xmldoc.AppendChild(xmlelem);

                    XmlElement xeBarCode = xmldoc.CreateElement("FunctionID");
                    xeBarCode.InnerText = "00013";
                    xmlelem.AppendChild(xeBarCode);

                    XmlElement xeType;
                    XmlElement xeSub;

                     string sqlSelect = string.Format(@"select * from bc_cname   where bc_del <> '1' and bc_bar_code='{0}' ", _patInfo.RepBarCode);

        

                     DBHelper helper = new DBHelper();
                     DataTable result = helper.GetTable(sqlSelect);


                    foreach (DataRow mi in result.Rows)
                    {
                        xeType = xmldoc.CreateElement("TestInfo");
                        xmlelem.AppendChild(xeType);

                        xeSub = xmldoc.CreateElement("VisitID");
                        xeSub.InnerText = _patInfo.RepBarCode;
                        xeType.AppendChild(xeSub);

                        xeSub = xmldoc.CreateElement("TestTime");
                        xeSub.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        xeType.AppendChild(xeSub);

                        xeSub = xmldoc.CreateElement("PatientID");
                        xeSub.InnerText = _patInfo.PidInNo;
                        xeType.AppendChild(xeSub);

                        xeSub = xmldoc.CreateElement("PatientName");
                        xeSub.InnerText = _patInfo.PidName;
                        xeType.AppendChild(xeSub);

                        xeSub = xmldoc.CreateElement("DeptID");
                        xeSub.InnerText = _patInfo.PidDeptId;
                        xeType.AppendChild(xeSub);

                        xeSub = xmldoc.CreateElement("DeptName");

                        EntityDicPubDept dep =
                            dcl.svr.cache.DictDepartCache.Current.DclCache.Find(
                                a => a.DeptCode == _patInfo.PidDeptId || a.DeptId == _patInfo.PidDeptId);
                        xeSub.InnerText = (dep == null ? "" : dep.DeptName);
                        xeType.AppendChild(xeSub);

                        xeSub = xmldoc.CreateElement("TestID");
                        xeSub.InnerText = mi["bc_his_code"].ToString();
                        xeType.AppendChild(xeSub);

                        xeSub = xmldoc.CreateElement("TestDesc");
                        xeSub.InnerText = mi["bc_his_name"].ToString();
                        xeType.AppendChild(xeSub);

                        xeSub = xmldoc.CreateElement("ExamID");
                        xeSub.InnerText = _patInfo.RepId;
                        xeType.AppendChild(xeSub);
                    }

                    

                    DataSender sender = new DataSender();

                    sender.SendataToWs(xmldoc.InnerXml);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteException(this.GetType().Name, "执行FssyZnfzDataSender", ex.ToString());
            }
        }

    }
}
