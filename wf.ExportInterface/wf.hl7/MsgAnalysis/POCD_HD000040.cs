using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace dcl.hl7
{
    public class POCD_HD000040
    {
        public string CreateReportMessage(EntityQcResultList pidResult, EntitySampMain sampMain)
        {
            string strApp = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

            string fileXmlTemplate = strApp + "HL7Xmls\\BS319_检验报告信息模板.xml";

            if (!System.IO.File.Exists(fileXmlTemplate))
            {
                throw new Exception("找不到模板文件:" + fileXmlTemplate);
            }

            EntityPidReportMain patient = pidResult.listPatients[0];

            XmlDocument doc_wxml = new XmlDocument();
            doc_wxml.Load(fileXmlTemplate);

            XmlNode node_main_id = XMLHelper.GetChildNodeByPath(doc_wxml, doc_wxml.DocumentElement, "id");
            if (node_main_id != null && node_main_id.Attributes != null)
            {
                node_main_id.Attributes["extension"].Value = patient.RepId;
            }

            XmlNode node_effectiveTime = XMLHelper.GetChildNodeByPath(doc_wxml, doc_wxml.DocumentElement, "effectiveTime");
            if (node_effectiveTime != null && node_effectiveTime.Attributes != null)
            {
                node_effectiveTime.Attributes["value"].Value = DateTime.Now.ToString("yyyyMMdd");
            }

            XmlNode node_versionNumber = XMLHelper.GetChildNodeByPath(doc_wxml, doc_wxml.DocumentElement, "versionNumber");
            if (node_versionNumber != null && node_versionNumber.Attributes != null)
            {
                node_versionNumber.Attributes["value"].Value = "0";
            }

            #region recordTarget 文档记录对象
            //<!-- 文档记录对象 -->
            //<recordTarget>
            XmlNode node_recordTarget = XMLHelper.GetChildNodeByPath(doc_wxml, doc_wxml.DocumentElement, "recordTarget");
            if (node_recordTarget != null)
            {
                List<XmlNode> node_recordTarget_IDs = XMLHelper.GetChildNodeListByPath(doc_wxml, node_recordTarget, "patientRole/id");
                if (node_recordTarget_IDs.Count > 0)
                {
                    // <!-- 域ID -->
                    //<id root="1.2.156.112678.1.2.1.2" extension="222" /> 
                    //<!-- 患者ID -->
                    //<id root="1.2.156.112678.1.2.1.3" extension="333" /> 
                    //<!-- 就诊号 -->
                    //<id root="1.2.156.112678.1.2.1.12" extension="121212" /> 
                    foreach (XmlNode nodeFh in node_recordTarget_IDs)
                    {
                        if (nodeFh != null && nodeFh.Attributes != null && nodeFh.Attributes["root"].Value == "1.2.156.45535072844010611A1001.1.2.1.3")
                        {
                            nodeFh.Attributes["extension"].Value = patient.PidSocialNo;
                        }
                        if (nodeFh != null && nodeFh.Attributes != null && nodeFh.Attributes["root"].Value == "1.2.156.45535072844010611A1001.1.2.1.12")
                        {
                            nodeFh.Attributes["extension"].Value = patient.PidInNo;
                        }
                    }
                }

                // <!-- 病人名称 -->
                //<name>李四</name>
                XmlNode node_patient_name = XMLHelper.GetChildNodeByPath(doc_wxml, node_recordTarget, "patientRole/patient/name");
                if (node_patient_name != null)
                {
                    node_patient_name.InnerText = patient.PidName;
                }
                //<!-- 性别编码/性别名称 -->
                //<administrativeGenderCode code="M" codeSystem="1.2.156.112678.1.1.3" displayName="男" />
                XmlNode node_patient_administrativeGenderCode = XMLHelper.GetChildNodeByPath(doc_wxml, node_recordTarget, "patientRole/patient/administrativeGenderCode");
                if (node_patient_administrativeGenderCode != null)
                {
                    node_patient_administrativeGenderCode.Attributes["code"].Value = patient.PidSex;
                    node_patient_administrativeGenderCode.Attributes["displayName"].Value = patient.PidSexName;
                }
                //<!-- 出生日期 -->
                //<birthTime value="20000101" />
                XmlNode node_birthTime = XMLHelper.GetChildNodeByPath(doc_wxml, node_recordTarget, "patientRole/patient/birthTime");
                if (node_birthTime != null && node_birthTime.Attributes != null)
                {
                    if (patient.PidBirthday != null)
                    {
                        node_birthTime.Attributes["value"].Value = Convert.ToDateTime(patient.PidBirthday.Value).ToString("yyyyMMdd");
                    }
                }
            }
            #endregion

            #region author 报告人信息

            //<!-- 报告人信息 -->
            //<author>
            XmlNode node_author = XMLHelper.GetChildNodeByPath(doc_wxml, doc_wxml.DocumentElement, "author");
            if (node_author != null)
            {
                // <!-- 报告日期 -->
                //<time value="201112311010" />
                XmlNode node_author_time = XMLHelper.GetChildNodeByPath(doc_wxml, node_author, "time");
                if (node_author_time != null)
                {
                    node_author_time.Attributes["value"].Value = patient.RepAuditDate.Value.ToString("yyyyMMddHHmm");
                }

                //  <!-- 报告人编码 -->
                //<id root="1.2.156.112678.1.1.2" extension="KP00017" />	
                XmlNode node_author_id = XMLHelper.GetChildNodeByPath(doc_wxml, node_author, "assignedAuthor/id");
                if (node_author_id != null)
                {
                    node_author_id.Attributes["extension"].Value = patient.RepAuditUserId;
                }

                //<!-- 报告人名称 -->
                //<name>张三</name>
                XmlNode node_author_name = XMLHelper.GetChildNodeByPath(doc_wxml, node_author, "assignedAuthor/assignedPerson/name");
                if (node_author_name != null)
                {
                    node_author_name.InnerText = patient.PidChkName;
                }

            }

            #endregion

            #region authenticator 审核人信息

            //<!-- 审核人信息 -->
            //<authenticator>
            XmlNode node_authenticator = XMLHelper.GetChildNodeByPath(doc_wxml, doc_wxml.DocumentElement, "authenticator");
            if (node_authenticator != null)
            {
                // <!-- 审核日期 -->
                //<time value="201112311010" />
                XmlNode node_authenticator_time = XMLHelper.GetChildNodeByPath(doc_wxml, node_authenticator, "time");
                if (node_authenticator_time != null)
                {
                    node_authenticator_time.Attributes["value"].Value = patient.RepReportDate.Value.ToString("yyyyMMddHHmm");
                }

                //  <!-- 审核者编码 -->
                //<id root="1.2.156.112678.1.1.2" extension="KP00017" />	
                XmlNode node_authenticator_id = XMLHelper.GetChildNodeByPath(doc_wxml, node_authenticator, "assignedEntity/id");
                if (node_authenticator_id != null)
                {
                    node_authenticator_id.Attributes["extension"].Value = patient.RepReportUserId;
                }

                //<!-- 审核者名称 -->
                //<name>张三</name>
                XmlNode node_authenticator_name = XMLHelper.GetChildNodeByPath(doc_wxml, node_authenticator, "assignedEntity/assignedPerson/name");
                if (node_authenticator_name != null)
                {
                    node_authenticator_name.InnerText = patient.RepReportUserName;
                }
            }

            #endregion

            #region participant 集合


            List<XmlNode> nodeLt_participant = XMLHelper.GetChildNodeListByPath(doc_wxml, doc_wxml.DocumentElement, "participant");
            //<!-- 送检医生信息 -->
            //<participant typeCode="DIST">


            //<!-- 检验科室信息(执行科室) -->
            //<participant typeCode="PRF">


            //<!-- 申请科室信息 -->
            //<participant typeCode="AUT">
            XmlNode node_participant_AUT = nodeLt_participant.Find(nodeI => (nodeI != null && nodeI.Attributes["typeCode"].Value == "AUT"));
            if (node_participant_AUT != null)
            {
                //<!-- 申请时间 -->
                //<time value="申请时间"/>
                XmlNode node_participant_AUT_time = XMLHelper.GetChildNodeByPath(doc_wxml, node_participant_AUT, "time");
                if (node_participant_AUT_time != null)
                {
                    //申请日期为空时取pat_date
                    if (patient.SampReceiveDate != null)
                    {
                        node_participant_AUT_time.Attributes["value"].Value = patient.SampReceiveDate.Value.ToString("yyyyMMddHHmm");
                    }
                    else
                    {
                        node_participant_AUT_time.Attributes["value"].Value = patient.RepInDate.Value.ToString("yyyyMMddHHmm");
                    }
                }

                //<!-- 申请科室编码 -->
                //<id root="1.2.156.112678.1.1.1" extension="1234"/>
                XmlNode node_participant_AUT_id = XMLHelper.GetChildNodeByPath(doc_wxml, node_participant_AUT, "associatedEntity/scopingOrganization/id");
                if (node_participant_AUT_id != null)
                {
                    node_participant_AUT_id.Attributes["extension"].Value = patient.PidDeptId;
                }
                //<!-- 申请科室名称 -->
                //<name>申请科室名称</name>
                XmlNode node_participant_AUT_name = XMLHelper.GetChildNodeByPath(doc_wxml, node_participant_AUT, "associatedEntity/scopingOrganization/name");
                if (node_participant_AUT_name != null)
                {
                    node_participant_AUT_name.InnerText = patient.PidDeptName;
                }

            }

            #endregion

            #region inFulfillmentOf 关联医嘱信息

            //<!-- 关联医嘱信息 -->
            //<inFulfillmentOf>

            XmlNode node_inFulfillmentOf = XMLHelper.GetChildNodeByPath(doc_wxml, doc_wxml.DocumentElement, "inFulfillmentOf");
            if (node_inFulfillmentOf != null)
            {
                XmlNode node_inFulfillmentOf_order = XMLHelper.GetChildNodeByPath(doc_wxml, node_inFulfillmentOf, "order");
                XmlNode node_inFulfillmentOf_orderID = XMLHelper.GetChildNodeByPath(doc_wxml, node_inFulfillmentOf, "order/id");
                XmlNode node_inFulfillmentOf_orderIDClone = null;
                if (node_inFulfillmentOf_orderID != null)
                {
                    node_inFulfillmentOf_orderIDClone = node_inFulfillmentOf_orderID.CloneNode(false);
                }

                if (node_inFulfillmentOf_order != null)
                {
                    //清除原来的医嘱信息
                    node_inFulfillmentOf_order.RemoveAll();

                    if (pidResult.listRepDetail != null && pidResult.listRepDetail.Count > 0 && node_inFulfillmentOf_orderIDClone != null)
                    {
                        // <!-- 关联医嘱号(可多个) -->
                        //<id extension="1000" />
                        //<id extension="2000" />
                        for (int j = 0; j < pidResult.listRepDetail.Count; j++)
                        {
                            XmlNode node_inFulfillmentOf_orderIDCloneNw = node_inFulfillmentOf_orderIDClone.Clone();
                            node_inFulfillmentOf_orderIDCloneNw.Attributes["extension"].Value = pidResult.listRepDetail[j].OrderSn;
                            node_inFulfillmentOf_order.AppendChild(node_inFulfillmentOf_orderIDCloneNw);
                        }
                    }
                }
            }


            #endregion

            #region componentOf 文档中医疗卫生事件的就诊场景

            //<!-- 文档中医疗卫生事件的就诊场景 -->
            //<componentOf>
            XmlNode node_componentOf = XMLHelper.GetChildNodeByPath(doc_wxml, doc_wxml.DocumentElement, "componentOf");
            if (node_componentOf != null)
            {
                //<!-- 就诊次数 -->
                //<id root="1.2.156.112678.1.2.1.7" extension="3"/>
                //<!-- 就诊流水号 -->
                //<id root="1.2.156.112678.1.2.1.6" extension="123456789"/>
                List<XmlNode> node_componentOf_IDs = XMLHelper.GetChildNodeListByPath(doc_wxml, node_componentOf, "encompassingEncounter/id");
                if (node_componentOf_IDs.Count > 0)
                {
                    foreach (XmlNode nodeFh in node_componentOf_IDs)
                    {
                        if (nodeFh != null && nodeFh.Attributes != null && nodeFh.Attributes["root"].Value == "1.2.156.45535072844010611A1001.1.2.1.6")
                        {
                            //就诊流水号
                            nodeFh.Attributes["extension"].Value = patient.RepInputId;
                        }
                        if (nodeFh != null && nodeFh.Attributes != null && nodeFh.Attributes["root"].Value == "1.2.156.45535072844010611A1001.1.2.1.7")
                        {
                            //就诊次数
                            nodeFh.Attributes["extension"].Value = patient.PidAddmissTimes.ToString();
                        }
                    }
                }

                //<!-- 就诊类别编码/就诊类别名称 -->
                //<code code="03" codeSystem="1.2.156.112678.1.1.80" displayName="门诊" />
                XmlNode node_componentOf_code = XMLHelper.GetChildNodeByPath(doc_wxml, node_componentOf, "encompassingEncounter/code");
                if (node_componentOf_code != null)
                {
                    //01 门诊
                    //0201 急诊
                    //03 住院
                    //0401 普通体检
                    switch (patient.PidSrcId)
                    {
                        case "107":
                            node_componentOf_code.Attributes["code"].Value = "01";
                            node_componentOf_code.Attributes["displayName"].Value = "门诊";
                            break;
                        case "108":
                            node_componentOf_code.Attributes["code"].Value = "03";
                            node_componentOf_code.Attributes["displayName"].Value = "住院";
                            break;
                        case "109":
                            node_componentOf_code.Attributes["code"].Value = "0401";
                            node_componentOf_code.Attributes["displayName"].Value = "普通体检";
                            break;
                        default: break;
                    }
                }
                //<!-- 病床号 -->
                //<wholeOrganization classCode="ORG" determinerCode="INSTANCE">
                //<id extension="001"/>
                XmlNode node_componentOf_wholeOrganization_id = XMLHelper.GetChildNodeByPath(doc_wxml, node_componentOf, "encompassingEncounter/location/healthCareFacility/serviceProviderOrganization/asOrganizationPartOf/wholeOrganization/id");

                if (node_componentOf_wholeOrganization_id != null)
                {
                    node_componentOf_wholeOrganization_id.Attributes["extension"].Value = patient.PidBedNo;
                }
            }

            #endregion

            #region MyRegion

            //<!-- 结构化信息 -->
            //<component>
            List<XmlNode> node_componentLt = XMLHelper.GetChildNodeListByPath(doc_wxml, doc_wxml.DocumentElement, "component/structuredBody/component");
            if (node_componentLt.Count > 0)
            {
                #region 检验章节(Labs section) node_componentLt[1]

                XmlNode node_component_Res = node_componentLt[1];
                if (node_component_Res != null)
                {
                    #region 报告备注

                    //<!-- 报告备注/检验备注/结果提示 -->
                    //<content ID="a1">检验报告上的备注内容填在这</content>
                    //<!-- 技术备注 -->
                    //<content ID="a2"></content>
                    //<!-- 表现现象(指报告的一些备注内容) -->
                    //<content ID="a3"></content>
                    //<!-- HIS相关备注(门诊申请备注, 住院医嘱嘱托。例如: 透析前, 透析后) -->
                    //<content ID="a4"></content>
                    List<XmlNode> node_component_Res_contentLt = XMLHelper.GetChildNodeListByPath(doc_wxml, node_component_Res, "section/text/content");
                    if (node_component_Res_contentLt.Count > 0)
                    {
                        foreach (XmlNode nodeFh in node_component_Res_contentLt)
                        {
                            if (nodeFh != null && nodeFh.Attributes != null && nodeFh.Attributes["ID"].Value == "a1")
                            {
                                nodeFh.InnerText = patient.RepRemark;
                            }
                        }
                    }
                    #endregion

                    #region 结果

                    //<!-- 相关信息 -->
                    //<entryRelationship typeCode="COMP">
                    XmlNode node_entryRelationship1 = XMLHelper.GetChildNodeByPath(doc_wxml, node_component_Res, "section/entry/observation/entryRelationship");

                    //<!-- 图片信息(要求编码为BASE64), @mediaType: 图片格式(JPG格式: image/jpeg PDF格式为: application/pdf) -->
                    //<value xsi:type="ED" mediaType=""></value>
                    if (node_entryRelationship1 != null)
                    {
                        XmlNode node_entryRelationship1_value = XMLHelper.GetChildNodeByPath(doc_wxml, node_entryRelationship1, "organizer/component/observationMedia/value");
                        if (node_entryRelationship1_value != null)
                        {
                            node_entryRelationship1_value.Attributes["mediaType"].Value = "application/pdf";
                        }
                    }

                    //项目结果节点
                    XmlNode node_entryRelationship2 = node_entryRelationship1.NextSibling;

                    XmlNode node_entryRelationship3 = XMLHelper.GetChildNodeListByPath(doc_wxml, node_component_Res, "section/entry/observation/entryRelationship")[2];

                    //克隆结果节点
                    XmlNode node_entryRelationship2_clone = node_entryRelationship2.CloneNode(true);
                    //删除旧项目结果节点
                    node_entryRelationship1.ParentNode.RemoveChild(node_entryRelationship2);

                    //在此节点后,添加增节点
                    XmlNode node_entryRelationship2ref = node_entryRelationship1;

                    foreach (EntityPidReportDetail detail in pidResult.listRepDetail)
                    {
                        //创建新节点 组合信息
                        XmlNode node_entryRelationship2_Nw = node_entryRelationship2_clone.CloneNode(true);

                        //<!-- 检验项编码/检验项名称(血细胞分析24项类型编码) -->
                        //<code code="1000" codeSystem="1.2.156.112678.1.1.46" displayName="血细胞分析24项" />
                        XmlNode node_entryRelationship2_NwCode = XMLHelper.GetChildNodeByPath(doc_wxml, node_entryRelationship2_Nw, "organizer/code");
                        if (node_entryRelationship2_NwCode != null)
                        {
                            node_entryRelationship2_NwCode.Attributes["code"].Value = detail.OrderCode;
                            node_entryRelationship2_NwCode.Attributes["displayName"].Value = detail.PatComName;
                        }

                        //旧component
                        XmlNode node_entryRelationship2_NwcomponentOld = XMLHelper.GetChildNodeByPath(doc_wxml, node_entryRelationship2_Nw, "organizer/component");
                        //克隆component
                        XmlNode node_entryRelationship2_NwcomponentClone = node_entryRelationship2_NwcomponentOld.CloneNode(true);
                        //删除 旧component
                        node_entryRelationship2_NwcomponentOld.ParentNode.RemoveChild(node_entryRelationship2_NwcomponentOld);


                        List<EntityObrResult> listResult = pidResult.listResulto.FindAll(w => w.ItmComId == detail.ComId);

                        int sequenceNumber = 0;
                        foreach (EntityObrResult item in listResult)
                        {
                            #region 添加项目明细结果

                            sequenceNumber++;
                            //建新component
                            XmlNode node_entryRelationship2_NwcomponentNew = node_entryRelationship2_NwcomponentClone.CloneNode(true);

                            //<!-- 显示序号 -->
                            //<sequenceNumber value="显示序号"/>
                            XmlNode node_NwcomponentNew_sequenceNumber = XMLHelper.GetChildNodeByPath(doc_wxml, node_entryRelationship2_NwcomponentNew, "sequenceNumber");
                            node_NwcomponentNew_sequenceNumber.Attributes["value"].Value = sequenceNumber.ToString();


                            //<!-- 检验子项编码/检验子项简称/检验子项全称(白细胞检验类型编码) -->
                            //<code code="检验子项编码" codeSystem="1.2.156.112678.1.1.108" displayName="检验子项简称">
                            //  <!-- @displayName是简称, originalText是全称 -->
                            //  <originalText>检验子项全称</originalText>
                            XmlNode node_NwcomponentNew_code = XMLHelper.GetChildNodeByPath(doc_wxml, node_entryRelationship2_NwcomponentNew, "observation/code");
                            node_NwcomponentNew_code.Attributes["code"].Value = item.ItmId;// dr["res_itm_id"].ToString();
                            node_NwcomponentNew_code.Attributes["displayName"].Value = item.ItmEname;// dr["res_itm_ecd"].ToString();
                            XmlNode node_NwcomponentNew_code_originalText = XMLHelper.GetChildNodeByPath(doc_wxml, node_entryRelationship2_NwcomponentNew, "observation/code/originalText");
                            node_NwcomponentNew_code_originalText.InnerText = item.ItmName;// dr["itm_name"].ToString();

                            // <!-- 结果是否正常-, 偏高↑或偏低↓ -->
                            //<interpretationCode code="高低值判断编码" displayName="高低值判断内容">
                            //  <originalText>高低值判断内容</originalText>
                            //  <translation code="数值标识" />
                            //</interpretationCode>
                            //<interpretationCode code="危险值判断编码" displayName="危险值判断内容">
                            //  <translation code="危险标识" />
                            //</interpretationCode>
                            XmlNode node_interpretationCode1 = XMLHelper.GetChildNodeByPath(doc_wxml, node_entryRelationship2_NwcomponentNew, "observation/interpretationCode");

                            XmlNode node_interpretationCode2 = node_interpretationCode1.NextSibling;

                            //<!-- 参考范围值 -->
                            //<referenceRange>
                            //  <observationRange>
                            //    <code code="范围类型编码"  displayName="范围类型名称"/>
                            //    <!--参考范围 文本说明-->
                            //    <text></text>
                            //    <value xsi:type="IVL_PQ" unit="参考范围值单位">
                            //      <low value="参考范围低值" />
                            //      <high value="参考范围高值" />
                            //    </value>
                            //  </observationRange>
                            //</referenceRange>
                            XmlNode node_referenceRange = XMLHelper.GetChildNodeByPath(doc_wxml, node_entryRelationship2_NwcomponentNew, "observation/referenceRange");

                            //<!-- 检验结果(结果只采用PQ, ST, SC类型) -->
                            //<!--
                            //PQ: <value xsi:type="PQ" value="19.1" unit="10^9/L" />  数值类型的结果+单位(没有单位去掉@unit, 没有结果去掉@value)
                            //ST: <value xsi:type="ST">阳性(+)</value>  文本类型结果
                            //SC: <value xsi:type="SC" code="个/LPF">未见</value> 文本类型结果+单位
                            //-->
                            //<value xsi:type="PQ" value="检验结果" unit="检验结果单位" />
                            XmlNode node_NwcomponentNew_value = XMLHelper.GetChildNodeByPath(doc_wxml, node_entryRelationship2_NwcomponentNew, "observation/value");

                            double doub_resValue = 0;
                            //数值型结果
                            if (item.ObrValue.Length > 0 && double.TryParse(item.ObrValue, out doub_resValue) && true)
                            {
                                if (item.ObrUnit.Length <= 0)
                                {
                                    node_NwcomponentNew_value.Attributes.Remove(node_NwcomponentNew_value.Attributes["unit"]);
                                }
                                else
                                {
                                    node_NwcomponentNew_value.Attributes["unit"].Value = item.ObrUnit;
                                }
                                //结果值
                                node_NwcomponentNew_value.Attributes["value"].Value = item.ObrValue;
                                //异常提示编码
                                node_interpretationCode1.Attributes["code"].Value = item.RefFlag;// dr["res_ref_flag"].ToString();
                                node_interpretationCode1.Attributes["displayName"].Value = item.ResTips;// dr["res_ref_flag_name2"].ToString();
                                XmlNode node_interpretationCode1_originalText = XMLHelper.GetChildNodeByPath(doc_wxml, node_interpretationCode1, "originalText");
                                node_interpretationCode1_originalText.InnerText = item.ResTips;// dr["res_ref_flag_descrip"].ToString();

                                //删除危急值异常提示
                                node_interpretationCode2.ParentNode.RemoveChild(node_interpretationCode2);

                                //<!-- 范围类型编码(固定值 - @code: 01参考范围值, 02危险范围值)
                                //<code code="范围类型编码"  displayName="范围类型名称"/>
                                XmlNode node_referenceRange_code = XMLHelper.GetChildNodeByPath(doc_wxml, node_referenceRange, "observationRange/code");
                                node_referenceRange_code.Attributes["code"].Value = "01";
                                node_referenceRange_code.Attributes["displayName"].Value = "参考范围值";

                                //<value xsi:type="IVL_PQ" unit="参考范围值单位">
                                //  <low value="参考范围低值" />
                                //  <high value="参考范围高值" />
                                //</value>
                                XmlNode node_referenceRange_value = XMLHelper.GetChildNodeByPath(doc_wxml, node_referenceRange, "observationRange/value");
                                node_referenceRange_value.Attributes["unit"].Value = item.ObrUnit;// dr["res_unit"].ToString();
                                XmlNode node_referenceRange_valueLow = XMLHelper.GetChildNodeByPath(doc_wxml, node_referenceRange_value, "low");
                                node_referenceRange_valueLow.Attributes["value"].Value = item.RefLowerLimit;// dr["res_ref_l"].ToString();
                                XmlNode node_referenceRange_valueHigh = XMLHelper.GetChildNodeByPath(doc_wxml, node_referenceRange_value, "high");
                                node_referenceRange_valueHigh.Attributes["value"].Value = item.RefUpperLimit;// dr["res_ref_h"].ToString();

                            }
                            else
                            {
                                //非数值结果
                                if (item.ObrValue.Length > 0)
                                {
                                    node_NwcomponentNew_value.Attributes["xsi:type"].Value = "SC";
                                    node_NwcomponentNew_value.Attributes.Remove(node_NwcomponentNew_value.Attributes["unit"]);
                                    node_NwcomponentNew_value.Attributes.Remove(node_NwcomponentNew_value.Attributes["value"]);
                                    XmlAttribute xmlAttribute = doc_wxml.CreateAttribute("code");
                                    xmlAttribute.Value = item.ObrUnit;
                                    node_NwcomponentNew_value.Attributes.Append(xmlAttribute);
                                    //((XmlElement)node_NwcomponentNew_value).SetAttribute("code", dr["res_unit"].ToString());
                                    node_NwcomponentNew_value.InnerText = item.ObrValue;
                                }
                                else
                                {
                                    node_NwcomponentNew_value.Attributes["xsi:type"].Value = "ST";
                                    node_NwcomponentNew_value.Attributes.Remove(node_NwcomponentNew_value.Attributes["unit"]);
                                    node_NwcomponentNew_value.Attributes.Remove(node_NwcomponentNew_value.Attributes["value"]);
                                    node_NwcomponentNew_value.InnerText = item.ObrValue;// dr["res_chr"].ToString();
                                }
                                //删除异常提示
                                node_interpretationCode1.ParentNode.RemoveChild(node_interpretationCode1);

                                //删除危急值异常提示
                                node_interpretationCode2.ParentNode.RemoveChild(node_interpretationCode2);

                                //删除参考值
                                node_referenceRange.ParentNode.RemoveChild(node_referenceRange);

                            }

                            //添加新component到新entryRelationship2
                            node_entryRelationship2_NwCode.ParentNode.AppendChild(node_entryRelationship2_NwcomponentNew);

                            #endregion
                        }

                        //添加新节点
                        node_entryRelationship2ref = node_entryRelationship2ref.ParentNode.InsertAfter(node_entryRelationship2_Nw, node_entryRelationship2ref);
                    }


                    #endregion

                    #region entryRelationship 标本及其图像信息

                    //<!-- 标本及其图像信息 -->
                    //<entryRelationship typeCode="SAS" inversionInd="true">
                    if (node_entryRelationship3 != null && node_entryRelationship3.Attributes != null && node_entryRelationship3.Attributes["typeCode"].Value == "SAS")
                    {
                        //<!-- 标本采集日期(采血时间) -->
                        //<effectiveTime value="201112311100" />
                        XmlNode node_entryRelationship3_effectiveTime = XMLHelper.GetChildNodeByPath(doc_wxml, node_entryRelationship3, "procedure/effectiveTime");
                        node_entryRelationship3_effectiveTime.Attributes["value"].Value = patient.SampApplyDate.Value.ToString("yyyyMMddHHmm");

                        //<!-- 标本条码号 -->
                        //<id extension="标本条码号" />
                        XmlNode node_entryRelationship3_specimenID = XMLHelper.GetChildNodeByPath(doc_wxml, node_entryRelationship3, "procedure/specimen/specimenRole/id");
                        node_entryRelationship3_specimenID.Attributes["extension"].Value = patient.RepBarCode;

                        //<!-- 标本类型编码/标本类型名称(标本来源) -->
                        //<code code="标本类型编码" codeSystem="1.2.156.112678.1.1.45" displayName="标本类型名称" />
                        XmlNode node_entryRelationship3_specimenCode = XMLHelper.GetChildNodeByPath(doc_wxml, node_entryRelationship3, "procedure/specimen/specimenRole/specimenPlayingEntity/code");
                        node_entryRelationship3_specimenCode.Attributes["code"].Value = patient.PidSamId;
                        node_entryRelationship3_specimenCode.Attributes["displayName"].Value = patient.SamName;

                        string strCollectionUserId = string.Empty;
                        string strCollectionUserName = string.Empty;
                        string strReceiverUserId = string.Empty;
                        string strReceiverUserName = string.Empty;
                        string strSampTubCode = string.Empty;
                        string strSampTubName = string.Empty;
                        if (sampMain != null)
                        {
                            strCollectionUserId = sampMain.CollectionUserId;
                            strCollectionUserName = sampMain.CollectionUserName;
                            strReceiverUserId = sampMain.ReceiverUserId;
                            strReceiverUserName = sampMain.ReceiverUserName;
                            strSampTubCode = sampMain.SampTubCode;
                            strSampTubName = sampMain.SampTubName;
                        }
                        //<!-- 采集人编码 -->
                        //<id root="1.2.156.112678.1.1.2" extension="采集人编码" />
                        XmlNode node_entryRelationship3_performerID = XMLHelper.GetChildNodeByPath(doc_wxml, node_entryRelationship3, "procedure/performer/assignedEntity/id");
                        node_entryRelationship3_performerID.Attributes["extension"].Value = strCollectionUserId;

                        // <!-- 采集人名称 -->
                        //<name>采集人名称</name>
                        XmlNode node_entryRelationship3_performerName = XMLHelper.GetChildNodeByPath(doc_wxml, node_entryRelationship3, "procedure/performer/assignedEntity/assignedPerson/name");
                        node_entryRelationship3_performerName.InnerText = strCollectionUserName;

                        //<!-- 标本接收人信息 -->
                        //<participant typeCode="RCV">
                        //<!-- 标本容器信息 -->
                        //<participant typeCode="SBJ">
                        List<XmlNode> node_entryRelationship3_participantLt = XMLHelper.GetChildNodeListByPath(doc_wxml, node_entryRelationship3, "procedure/participant");
                        if (node_entryRelationship3_participantLt.Count > 0)
                        {
                            foreach (XmlNode nodeFh in node_entryRelationship3_participantLt)
                            {
                                if (nodeFh != null && nodeFh.Attributes != null && nodeFh.Attributes["typeCode"].Value == "RCV")
                                {
                                    //<!-- 接收时间/送检时间 -->
                                    //<time value="接收时间" />
                                    if (patient.SampReciveDate != null)
                                    {
                                        XmlNode node_entryRelationship3_participanttime = XMLHelper.GetChildNodeByPath(doc_wxml, nodeFh, "time");
                                        node_entryRelationship3_participanttime.Attributes["value"].Value = patient.SampReciveDate.Value.ToString("yyyyMMddHHmm");
                                    }
                                    //<!-- 接收人编码 -->
                                    //<id root="1.2.156.112678.1.1.2" extension="接收人编码" />
                                    XmlNode node_entryRelationship3_participantid = XMLHelper.GetChildNodeByPath(doc_wxml, nodeFh, "participantRole/id");
                                    node_entryRelationship3_participantid.Attributes["extension"].Value = strReceiverUserId;
                                    //<!-- 接收人名称 -->
                                    //<name>接收人名称</name>
                                    XmlNode node_entryRelationship3_participantname = XMLHelper.GetChildNodeByPath(doc_wxml, nodeFh, "participantRole/playingEntity/name");
                                    node_entryRelationship3_participantname.InnerText = strReceiverUserName;

                                }
                                if (nodeFh != null && nodeFh.Attributes != null && nodeFh.Attributes["typeCode"].Value == "SBJ")
                                {
                                    //<!-- 容器编码/容器名称 -->
                                    //<code code="容器编码" displayName="容器名称" />
                                    XmlNode node_entryRelationship3_participantcode = XMLHelper.GetChildNodeByPath(doc_wxml, nodeFh, "participantRole/playingDevice/code");
                                    node_entryRelationship3_participantcode.Attributes["code"].Value = strSampTubCode;
                                    node_entryRelationship3_participantcode.Attributes["displayName"].Value = strSampTubName;
                                }
                            }

                        }
                    }

                    #endregion
                }

                #endregion

                #region 诊断章节(Diagnosis section) node_componentLt[2]

                XmlNode node_component_Diagnosis = node_componentLt[2];

                //<title>诊断</title> 
                XmlNode node_component_Diagnosis_title = XMLHelper.GetChildNodeByPath(doc_wxml, node_component_Diagnosis, "section/title");
                node_component_Diagnosis_title.InnerText = patient.PidDiag;

                // <!-- 诊断类别编码/诊断类别名称 -->
                //<code code="诊断类别编码" codeSystem="1.2.156.112678.1.1.29" displayName="诊断类别名称" />
                XmlNode node_component_Diagnosis_code = XMLHelper.GetChildNodeByPath(doc_wxml, node_component_Diagnosis, "section/entry/act/entryRelationship/observation/code");
                node_component_Diagnosis_code.Attributes["code"].Value = "";
                node_component_Diagnosis_code.Attributes["displayName"].Value = patient.PidDiag;

                #endregion
            }

            #endregion

            return XMLHelper.XMLDocumentToString(doc_wxml);
        }
    }
}
