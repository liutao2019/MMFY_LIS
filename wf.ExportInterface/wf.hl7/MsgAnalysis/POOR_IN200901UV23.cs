using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace dcl.hl7
{
    public class POOR_IN200901UV23
    {
        public string CreateOperationMessage(EntitySampOperation operation, EntitySampMain sampMain)
        {
            string strApp = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            string fileXmlTemplate = strApp + "HL7Xmls\\BS004_医嘱执行状态信息样例.xml";

            if (!System.IO.File.Exists(fileXmlTemplate))
            {
                throw new Exception("找不到模板文件:" + fileXmlTemplate);
            }

            //  EntityPidReportMain patient = pidResult.listPatients[0];

            XmlDocument doc_wxml = new XmlDocument();
            doc_wxml.Load(fileXmlTemplate);

            XmlNode node_main_id = XMLHelper.GetChildNodeByPath(doc_wxml, doc_wxml.DocumentElement, "id");
            if (node_main_id != null && node_main_id.Attributes != null)
            {
                node_main_id.Attributes["extension"].Value = "BS004";
            }

            XmlNode node_effectiveTime = XMLHelper.GetChildNodeByPath(doc_wxml, doc_wxml.DocumentElement, "creationTime");
            if (node_effectiveTime != null && node_effectiveTime.Attributes != null)
            {
                node_effectiveTime.Attributes["value"].Value = DateTime.Now.ToString("yyyyMMdd");
            }


            //<!-- 接受者 -->
            //<receiver>
            XmlNode node_receiver = XMLHelper.GetChildNodeByPath(doc_wxml, doc_wxml.DocumentElement, "receiver/device/id/item");
            if (node_receiver != null && (node_receiver.Attributes["root"].Value == "1.2.156.45535072844010611A1001.1.1.19"))
            {
                node_receiver.Attributes["extension"].Value = sampMain.ReceiverUserName;
            }

            //<!-- 送达者 -->
            //<sender >
            XmlNode node_sender = XMLHelper.GetChildNodeByPath(doc_wxml, doc_wxml.DocumentElement, "sender/device/id/item");
            if (node_sender != null && (node_receiver.Attributes["root"].Value == "1.2.156.45535072844010611A1001.1.1.19"))
            {
                node_sender.Attributes["extension"].Value = sampMain.SendUserName;
            }

            #region controlActProcess 封装的消息内容

            //<!--  封装的消息内容(按Excel填写) -->
            //<controlActProcess>
            XmlNode node_controlActProcess = XMLHelper.GetChildNodeByPath(doc_wxml, doc_wxml.DocumentElement, "controlActProcess/subject/placerGroup");
            if (node_controlActProcess != null)
            {
                #region 患者信息
                //  < !--患者信息-- >
                XmlNode node_patient = XMLHelper.GetChildNodeByPath(doc_wxml, node_controlActProcess, "subject/patient");
                if (node_patient != null)
                {
                    List<XmlNode> node_patient_IDs = XMLHelper.GetChildNodeListByPath(doc_wxml, node_patient, "id/item");
                    if (node_patient_IDs.Count > 0)
                    {
                        foreach (XmlNode nodeFh in node_patient_IDs)
                        {
                            if (nodeFh != null && nodeFh.Attributes != null && nodeFh.Attributes["root"].Value == "1.2.156.45535072844010611A1001.1.2.1.2")
                            {
                                //域ID
                                nodeFh.Attributes["extension"].Value = "";
                            }
                            if (nodeFh != null && nodeFh.Attributes != null && nodeFh.Attributes["root"].Value == "1.2.156.45535072844010611A1001.1.2.1.3")
                            {
                                //病人id
                                nodeFh.Attributes["extension"].Value = sampMain.PidInNo;
                            }
                            if (nodeFh != null && nodeFh.Attributes != null && nodeFh.Attributes["root"].Value == "1.2.156.45535072844010611A1001.1.2.1.12")
                            {
                                //就诊号
                                nodeFh.Attributes["extension"].Value = sampMain.PidSocialNo;
                            }
                        }
                    }
                    //科室id
                    XmlNode node_PidDocCode = XMLHelper.GetChildNodeByPath(doc_wxml, node_controlActProcess, "subject/patient/providerOrganization/id/item");
                    if (node_PidDocCode != null && node_PidDocCode.Attributes != null)
                    {
                        node_PidDocCode.Attributes["extension"].Value = sampMain.PidDeptCode;
                    }
                    //科室名称
                    XmlNode node_PidDocName = XMLHelper.GetChildNodeByPath(doc_wxml, node_controlActProcess, "subject/patient/providerOrganization/name/item/part");
                    if (node_PidDocName != null && node_PidDocName.Attributes != null)
                    {
                        node_PidDocName.Attributes["value"].Value = sampMain.PidDeptName;
                    }

                }
                #endregion
                #region 操作人
                //< !--操作人-- >
                XmlNode node_performer = XMLHelper.GetChildNodeByPath(doc_wxml, node_controlActProcess, "performer");
                if (node_performer != null)
                {
                    //操作时间
                    XmlNode node_performer_time = XMLHelper.GetChildNodeByPath(doc_wxml, node_performer, "time/any");
                    if (node_performer_time != null && node_performer_time.Attributes!=null)
                    {
                        node_performer_time.Attributes["value"].Value = DateTime.Now.ToString("yyyyMMdd");
                    }
                    //操作人工号
                    XmlNode node_assignedEntity_id = XMLHelper.GetChildNodeByPath(doc_wxml, node_performer, "assignedEntity/id/item");
                    if (node_assignedEntity_id != null && node_assignedEntity_id.Attributes != null)
                    {
                        node_assignedEntity_id.Attributes["extension"].Value =operation.OperationWorkId;
                    }
                    //操作人姓名
                    XmlNode node_assignedEntity_name = XMLHelper.GetChildNodeByPath(doc_wxml, node_performer, "assignedEntity/assignedPerson/name/item/part");
                    if (node_assignedEntity_name != null && node_assignedEntity_name.Attributes != null)
                    {
                        node_assignedEntity_name.Attributes["value"].Value = operation.OperationName;
                    }
                }
                #endregion

                #region 执行科室
                //< !--执行科室-- >
                XmlNode node_location = XMLHelper.GetChildNodeByPath(doc_wxml, node_controlActProcess, "location");
                if (node_location != null)
                {
                    //执行科室编码
                    XmlNode node_location_id = XMLHelper.GetChildNodeByPath(doc_wxml, node_location, "serviceDeliveryLocation/serviceProviderOrganization/id/item");
                    if (node_location_id != null && node_location_id.Attributes != null)
                    {
                        node_location_id.Attributes["extension"].Value =sampMain.PidDeptCode;
                    }
                    //执行科室
                    XmlNode node_location_name= XMLHelper.GetChildNodeByPath(doc_wxml, node_location, "serviceDeliveryLocation/serviceProviderOrganization/name/item/part");
                    if (node_location_name != null && node_location_name.Attributes != null)
                    {
                        node_location_name.Attributes["value"].Value = sampMain.PidDeptName;
                    }
                }
                #endregion

                #region 医嘱状态
                XmlNode node_component2 = XMLHelper.GetChildNodeByPath(doc_wxml, node_controlActProcess, "component2");
                XmlNode node_exp = node_location.NextSibling;
                if (node_component2 != null)
                {
                    //克隆component
                    XmlNode node_component2Clone = node_exp.NextSibling.CloneNode(true);
                    //删除 旧component
                    node_component2.ParentNode.RemoveChild(node_component2);
                    //在此节点后,添加增节点
                    XmlNode node_componetNext = node_exp;
                    if (sampMain.ListSampDetail!=null && sampMain.ListSampDetail.Count > 0)
                    {
                        foreach (EntitySampDetail detail in sampMain.ListSampDetail)
                        {
                            XmlNode node_component2New = node_component2Clone.CloneNode(true);
                            XmlNode node_sequenceNumber = XMLHelper.GetChildNodeByPath(doc_wxml, node_component2New, "sequenceNumber");
                            int i = 0;
                            node_sequenceNumber.Attributes["value"].Value = i++.ToString();
                            List<XmlNode> node_observationRequest_ids = XMLHelper.GetChildNodeListByPath(doc_wxml, node_component2New, "observationRequest/id/item");
                            if(node_observationRequest_ids!=null && node_observationRequest_ids.Count>0)
                            {
                                foreach (XmlNode nodeFh in node_observationRequest_ids)
                                {
                                    if (nodeFh != null && nodeFh.Attributes != null && nodeFh.Attributes["root"].Value == "1.2.156.45535072844010611A1001.1.2.1.22")
                                    {
                                        //<!-- 医嘱号 -->
                                        nodeFh.Attributes["extension"].Value = detail.OrderSn;
                                    }
                                    if (nodeFh != null && nodeFh.Attributes != null && nodeFh.Attributes["root"].Value == "1.2.156.45535072844010611A1001.1.2.1.21")
                                    {
                                        //<!-- 申请单号 -->
                                        nodeFh.Attributes["extension"].Value = sampMain.SampApplyNo;
                                    }
                                    if (nodeFh != null && nodeFh.Attributes != null && nodeFh.Attributes["root"].Value == "1.2.156.45535072844010611A1001.1.2.1.24")
                                    {
                                        //<!-- 报告号 -->
                                        nodeFh.Attributes["extension"].Value = "";
                                    }
                                    if (nodeFh != null && nodeFh.Attributes != null && nodeFh.Attributes["root"].Value == "1.2.156.45535072844010611A1001.1.2.1.30")
                                    {
                                        //<!-- StudyInstanceUID -->
                                        nodeFh.Attributes["extension"].Value ="";
                                    }
                                }
                            }
                            //标本信息
                            XmlNode node_specimen = XMLHelper.GetChildNodeByPath(doc_wxml, node_component2New, "observationRequest/specimen/specimen");
                            if(node_specimen != null)
                            {
                                //条码号
                                XmlNode node_bar_code= XMLHelper.GetChildNodeByPath(doc_wxml, node_specimen, "id");
                                if (node_bar_code != null && node_bar_code.Attributes != null)
                                {
                                    node_bar_code.Attributes["extension"].Value = sampMain.SampBarCode;
                                }
                                //采集时间
                                XmlNode node_bloodTime = XMLHelper.GetChildNodeByPath(doc_wxml, node_specimen, "subjectOf1/specimenProcessStep/effectiveTime/any");
                                if (node_bloodTime != null && node_bloodTime.Attributes != null)
                                {
                                    if (sampMain.CollectionDate != null)
                                    {
                                        node_bloodTime.Attributes["value"].Value = sampMain.CollectionDate.Value.ToString("yyyyMMdd");
                                    }
                                }
                                //采集人 id
                                XmlNode node_bloodId= XMLHelper.GetChildNodeByPath(doc_wxml, node_specimen, "subjectOf1/specimenProcessStep/performer/assignedEntity/id/item");
                                if (node_bloodId != null && node_bloodId.Attributes != null)
                                {
                                    node_bloodId.Attributes["extension"].Value = sampMain.CollectionUserId;
                                }
                                //采集人姓名
                                XmlNode node_bloodName = XMLHelper.GetChildNodeByPath(doc_wxml, node_specimen, "subjectOf1/specimenProcessStep/performer/assignedEntity/assignedPerson/name/item/part");
                                if (node_bloodName != null && node_bloodName.Attributes != null)
                                {
                                    node_bloodName.Attributes["value"].Value = sampMain.CollectionUserName;
                                }
                            }
                            //医嘱执行状态
                            XmlNode node_processStep = XMLHelper.GetChildNodeByPath(doc_wxml, node_component2New, "observationRequest/component1/processStep");
                            if (node_processStep != null)
                            {
                                string operationStatus = string.Empty;
                                string operationName = string.Empty;
                                switch (operation.OperationStatus)
                                {
                                    case "5":
                                        operationStatus = "140.001";
                                        operationName = "标本已签收";
                                        break;
                                    case "510":
                                        operationStatus = "160.008";
                                        operationName = "退检";
                                        break;
                                    case "60":
                                        operationStatus = "170.002";
                                        operationName = "检验报告已审核";
                                        break;
                                    case "70":
                                        operationStatus = "990.001";
                                        operationName = "报告召回";
                                        break;
                                    default: break;
                                }
                                XmlNode node_processStep_code = XMLHelper.GetChildNodeByPath(doc_wxml, node_processStep, "code");
                                if (node_processStep_code != null && node_processStep_code.Attributes != null)
                                {
                                    node_processStep_code.Attributes["code"].Value = operationStatus;
                                }
                                XmlNode node_processStep_name = XMLHelper.GetChildNodeByPath(doc_wxml, node_processStep, "code/displayName");
                                if (node_processStep_name != null && node_processStep_name.Attributes != null)
                                {
                                    node_processStep_name.Attributes["value"].Value = operationName;
                                }
                            }
                            // node_component2Clone.InsertBefore()
                            //  node_component2.ParentNode.AppendChild(node_component2Clone);
                            //添加新节点
                            node_componetNext = node_componetNext.ParentNode.InsertAfter(node_component2New, node_exp);
                        }
                    }
                 
                }

                #endregion
                #region 就诊
                XmlNode node_componentOf1 = XMLHelper.GetChildNodeByPath(doc_wxml, node_controlActProcess, "componentOf1");
                if (node_componentOf1 != null)
                {
                    List<XmlNode> node_encounter_ids = XMLHelper.GetChildNodeListByPath(doc_wxml, node_componentOf1, "encounter/id/item");
                    if (node_encounter_ids != null && node_encounter_ids.Count > 0)
                    {
                        foreach (XmlNode nodeFh in node_encounter_ids)
                        {
                            if (nodeFh != null && nodeFh.Attributes != null && nodeFh.Attributes["root"].Value == "1.2.156.45535072844010611A1001.1.2.1.7")
                            {
                                //<!-- 就诊次数 -->
                                nodeFh.Attributes["extension"].Value =sampMain.PidAdmissTimes.ToString();
                            }
                            if (nodeFh != null && nodeFh.Attributes != null && nodeFh.Attributes["root"].Value == "1.2.156.45535072844010611A1001.1.2.1.6")
                            {
                                //<!-- 就诊流水号 -->
                                nodeFh.Attributes["extension"].Value = "";
                            }
                        }
                    }
                   //<!--就诊类别编码-->
                    XmlNode node_encounter_code = XMLHelper.GetChildNodeByPath(doc_wxml, node_componentOf1, "encounter/code");
                    if (node_encounter_code != null)
                    {
                        //<!-- 就诊类别名称 -->
                        XmlNode node_encounter_name = XMLHelper.GetChildNodeByPath(doc_wxml, node_encounter_code, "displayName");
                        switch (sampMain.PidSrcId)
                        {
                            case "107":
                                node_encounter_code.Attributes["code"].Value = "01";
                                node_encounter_name.Attributes["value"].Value = "门诊";
                                break;
                            case "108":
                                node_encounter_code.Attributes["code"].Value = "03";
                                node_encounter_name.Attributes["displayName"].Value = "住院";
                                break;
                            case "109":
                                node_encounter_code.Attributes["code"].Value = "0401";
                                node_encounter_name.Attributes["displayName"].Value = "普通体检";
                                break;
                            default: break;
                        }
                    }
                }
                #endregion
            }

            #endregion


            return XMLHelper.XMLDocumentToString(doc_wxml);
        }
    }
}
