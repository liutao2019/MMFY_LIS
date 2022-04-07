using dcl.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace dcl.hl7
{
    public class POOR_IN200901UV21 : IMessageAnalysis
    {
        public List<EntitySampOrderHL7> MessageAnalysis(string msgXml)
        {

            List<EntitySampOrderHL7> listHl7 = new List<EntitySampOrderHL7>();
            EntitySampOrderHL7 NW = new EntitySampOrderHL7();
            try
            {
                //oml_r = Lis.HL7V3.XmlHelper.XmlDeserialize<Lis.HL7V3.POOR_IN200901UV.POOR_IN200901UV>(msg);
                XmlDocument doc_rxml = new XmlDocument();

                doc_rxml.LoadXml(msgXml);

                NW.EnumOrderType = EnumSampOrderType.NW;//默认新增
                //<!-- 消息交互类型 @code: 新增 :new 删除：delete -->
                //<code code="update"></code>
                string tempAttV = XMLHelper.GetAttValueForDoc(doc_rxml, "/POOR_IN200901UV/controlActProcess/code", "code");

                if (tempAttV == "delete")
                {
                    NW.EnumOrderType = EnumSampOrderType.CA;//撤销
                }
                else if (tempAttV == "update")
                {
                    NW.EnumOrderType = EnumSampOrderType.RU;//修改
                }

                //获取病人各种id号
                XmlNode node_patient_id = XMLHelper.GetSingleNodeForDoc(doc_rxml, "/POOR_IN200901UV/controlActProcess/subject/placerGroup/subject/patient/id");

                if (node_patient_id != null && node_patient_id.ChildNodes.Count > 0)
                {
                    //<!--域ID -->
                    //<item root="1.2.156.45535072844010611A1001.1.2.1.2" extension="111" />
                    //<!--患者ID -->
                    //<item root="1.2.156.45535072844010611A1001.1.2.1.3" extension="001407878200" />
                    //<!--就诊号 -->
                    //<item root="1.2.156.45535072844010611A1001.1.2.1.12" extension="4153754" />

                    foreach (XmlNode nodeFht in node_patient_id.ChildNodes)
                    {
                        if (nodeFht != null && nodeFht.Attributes != null && nodeFht.Name == "item" && nodeFht.Attributes["root"].Value == "1.2.156.45535072844010611A1001.1.2.1.3")
                        {
                            NW.PidSocialNo = nodeFht.Attributes["extension"].Value;
                        }
                        if (nodeFht != null && nodeFht.Attributes != null && nodeFht.Name == "item" && nodeFht.Attributes["root"].Value == "1.2.156.45535072844010611A1001.1.2.1.12")
                        {
                            NW.PidInNo = nodeFht.Attributes["extension"].Value;
                        }
                    }
                }

                //<!-- 病区 床号 -->
                XmlNode node_patient_addr_item = XMLHelper.GetSingleNodeForDoc(doc_rxml, "/POOR_IN200901UV/controlActProcess/subject/placerGroup/subject/patient/addr/item");
                if (node_patient_addr_item != null && node_patient_addr_item.ChildNodes.Count > 0)
                {
                    //<!-- 病区名 病区编码 -->
                    //<part type="BNR" value="10C" code="1000068" codeSystem="1.2.156.112678.1.1.33" />
                    //<!--床号 -->
                    //<part type="CAR" value="002" />

                    foreach (XmlNode nodeFht in node_patient_addr_item.ChildNodes)
                    {
                        if (nodeFht != null && nodeFht.Attributes != null && nodeFht.Name == "part" && nodeFht.Attributes["type"].Value == "BNR")
                        {
                            NW.PidDeptWard = nodeFht.Attributes["code"].Value;
                        }
                        if (nodeFht != null && nodeFht.Attributes != null && nodeFht.Name == "part" && nodeFht.Attributes["type"].Value == "CAR")
                        {
                            NW.PidBedNo = nodeFht.Attributes["value"].Value;
                        }
                    }
                }

                //<!--个人信息 -->
                //<patientPerson classCode="PSN">
                //<!-- 身份证号 -->
                //<id>
                //    <item extension="15001198807080982" root="1.2.156.112678.1.2.1.9" />
                //</id>
                XmlNode node_patientPerson_idItem = XMLHelper.GetSingleNodeForDoc(doc_rxml, "/POOR_IN200901UV/controlActProcess/subject/placerGroup/subject/patient/patientPerson/id/item");
                if (node_patientPerson_idItem != null && node_patientPerson_idItem.Attributes != null)
                {
                    NW.PidIdentityCard = node_patientPerson_idItem.Attributes["extension"].Value;
                }

                //<!--患者姓名 -->
                //            <name xsi:type="BAG_EN">
                //                <item>
                //                    <part value="张三" />
                //                </item>
                //            </name>
                XmlNode node_patientPerson_nameItem = XMLHelper.GetSingleNodeForDoc(doc_rxml, "/POOR_IN200901UV/controlActProcess/subject/placerGroup/subject/patient/patientPerson/name/item/part");
                if (node_patientPerson_nameItem != null && node_patientPerson_nameItem.Attributes != null)
                {
                    NW.PidName = node_patientPerson_nameItem.Attributes["value"].Value;
                }

                //<!-- 联系电话 -->				
                XmlNode node_patientPerson_telecom = XMLHelper.GetSingleNodeForDoc(doc_rxml, "/POOR_IN200901UV/controlActProcess/subject/placerGroup/subject/patient/patientPerson/telecom");
                if (node_patientPerson_telecom != null && node_patientPerson_telecom.ChildNodes.Count > 0)
                {
                    //<!-- 联系电话 -->
                    //<item use="EC" value="13128870214"></item>
                    //<!--移动电话 -->
                    //<item use="MC" value="010-6632134"></item>

                    foreach (XmlNode nodeFht in node_patientPerson_telecom.ChildNodes)
                    {
                        if (nodeFht != null && nodeFht.Attributes != null && nodeFht.Name == "item" && nodeFht.Attributes["use"].Value == "EC")
                        {
                            NW.PidTel = nodeFht.Attributes["value"].Value;
                        }
                        if (nodeFht != null && nodeFht.Attributes != null && nodeFht.Name == "item" && nodeFht.Attributes["use"].Value == "MC")
                        {
                            //NW.PidBedNo = nodeFht.Attributes["value"].Value;
                        }
                    }
                }

                //<!--性别代码 -->
                //<administrativeGenderCode code="1"
                //    codeSystem="1.2.156.112678.1.1.3" />
                XmlNode node_patientPerson_administrativeGenderCode = XMLHelper.GetSingleNodeForDoc(doc_rxml, "/POOR_IN200901UV/controlActProcess/subject/placerGroup/subject/patient/patientPerson/administrativeGenderCode");
                if (node_patientPerson_administrativeGenderCode != null && node_patientPerson_administrativeGenderCode.Attributes != null)
                {
                    if (node_patientPerson_administrativeGenderCode.Attributes["code"].Value == "1")
                    {
                        NW.PidSex = "男";
                    }
                    else if (node_patientPerson_administrativeGenderCode.Attributes["code"].Value == "2")
                    {
                        NW.PidSex = "女";
                    }
                    else
                    {
                        NW.PidSex = "";
                    }
                }

                //<!--出生日期 -->
                //<birthTime value="20000101">
                //    <!--年龄 -->
                //    <originalText value="12" />
                //</birthTime>
                XmlNode node_patientPerson_birthTime = XMLHelper.GetSingleNodeForDoc(doc_rxml, "/POOR_IN200901UV/controlActProcess/subject/placerGroup/subject/patient/patientPerson/birthTime");
                if (node_patientPerson_birthTime != null && node_patientPerson_birthTime.Attributes != null)
                {
                    string dateStr = node_patientPerson_birthTime.Attributes["value"].Value;
                    DateTime PidAge = DateTime.Now;
                    //19890119000000要改为19890119
                    if (!string.IsNullOrEmpty(dateStr) && dateStr.Length == 8)
                    {
                        if (DateTime.TryParseExact(dateStr, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out PidAge))
                            NW.PidBirthday = PidAge.ToString();
                    }
                    else if (!string.IsNullOrEmpty(dateStr) && dateStr.Length == 12)
                    {
                        if (DateTime.TryParseExact(dateStr, "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out PidAge))
                            NW.PidBirthday = PidAge.ToString();
                    }
                    else if (!string.IsNullOrEmpty(dateStr) && dateStr.Length == 14)
                    {
                        if (DateTime.TryParseExact(dateStr, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out PidAge))
                            NW.PidBirthday = PidAge.ToString();
                    }
                    else if (!string.IsNullOrEmpty(dateStr))
                    {
                        if (DateTime.TryParse(dateStr, out PidAge))
                            NW.PidBirthday = PidAge.ToString();
                    }

                    //XmlNode node_patientPerson_originalText = XMLHelper.GetSingleNodeForDoc(doc_rxml, "/POOR_IN200901UV/controlActProcess/subject/placerGroup/subject/patient/patientPerson/originalText");
                    //if (node_patientPerson_originalText != null && node_patientPerson_originalText.Attributes != null)
                    //{
                    //    NW.PidAge = node_patientPerson_originalText.Attributes["value"].Value;
                    //}

                    foreach (XmlNode node_patientPerson_birthTime_cld in node_patientPerson_birthTime.ChildNodes)
                    {
                        if (node_patientPerson_birthTime_cld != null && node_patientPerson_birthTime_cld.Name == "originalText"
                            && node_patientPerson_birthTime_cld.Attributes != null
                            && node_patientPerson_birthTime_cld.Attributes["value"] != null)
                        {
                            NW.PidAge = node_patientPerson_birthTime_cld.Attributes["value"].Value;
                        }
                    }
                }

                //病人科室
                XmlNode node_providerOrganization = XMLHelper.GetSingleNodeForDoc(doc_rxml, "/POOR_IN200901UV/controlActProcess/subject/placerGroup/subject/patient/providerOrganization");
                if (node_providerOrganization != null && node_providerOrganization.ChildNodes.Count > 0)
                {
                    foreach (XmlNode nodeFht in node_providerOrganization.ChildNodes)
                    {
                        //<!--病人科室编码-->
                        //<id>
                        //    <item extension="1409889" root="1.2.156.112678.1.1.1"/>
                        //</id>
                        //<!--病人科室名称 -->
                        //<name xsi:type="BAG_EN">
                        //    <item>
                        //        <part value="检验科" />
                        //    </item>
                        //</name>

                        if (nodeFht != null && nodeFht.Attributes != null && nodeFht.Name == "id" && nodeFht.ChildNodes.Count > 0)
                        {
                            NW.PidDeptCode = nodeFht.ChildNodes[0].Attributes["extension"].Value;
                        }
                        if (nodeFht != null && nodeFht.Attributes != null && nodeFht.Name == "name" && nodeFht.ChildNodes.Count > 0)
                        {
                            NW.PidDeptName = nodeFht.ChildNodes[0].ChildNodes[0].Attributes["value"].Value;
                        }
                    }
                }

                //<!-- 开单医生编码 -->
                //<id>
                //    <item extension="03421" root="1.2.156.112678.1.1.2"></item>
                //</id>
                XmlNode node_author_assignedEntity_idItem = XMLHelper.GetSingleNodeForDoc(doc_rxml, "/POOR_IN200901UV/controlActProcess/subject/placerGroup/author/assignedEntity/id/item");
                if (node_author_assignedEntity_idItem != null && node_author_assignedEntity_idItem.Attributes != null)
                {
                    NW.PidDoctorCode = node_author_assignedEntity_idItem.Attributes["extension"].Value;
                }

                //<!--开单医生姓名 必须项已使用 -->
                //<name xsi:type="BAG_EN">
                //    <item>
                //        <part value="李四"></part>
                //    </item>
                //</name>
                XmlNode node_author_assignedEntity_nameItemPart = XMLHelper.GetSingleNodeForDoc(doc_rxml, "/POOR_IN200901UV/controlActProcess/subject/placerGroup/author/assignedEntity/assignedPerson/name/item/part");
                if (node_author_assignedEntity_nameItemPart != null && node_author_assignedEntity_nameItemPart.Attributes != null)
                {
                    NW.PidDoctorName = node_author_assignedEntity_nameItemPart.Attributes["value"].Value;
                }

                //<!-- 申请科室编码 必须已使用 -->
                //<id>
                //    <item extension="140988948" root="1.2.156.112678.1.1.1"></item>
                //</id>
                XmlNode node_author_assignedEntity_representedOrganizationIdItem = XMLHelper.GetSingleNodeForDoc(doc_rxml, "/POOR_IN200901UV/controlActProcess/subject/placerGroup/author/assignedEntity/representedOrganization/id/item");
                if (node_author_assignedEntity_representedOrganizationIdItem != null && node_author_assignedEntity_representedOrganizationIdItem.Attributes != null)
                {
                    NW.PidDeptCode = node_author_assignedEntity_representedOrganizationIdItem.Attributes["extension"].Value;
                }

                //<!-- 申请科室名称 -->
                //<name xsi:type="BAG_EN">
                //    <item use="ABC">
                //        <part value="检验科微生物室" />
                //    </item>
                //</name>
                XmlNode node_author_assignedEntity_representedOrganizationNameItemPart = XMLHelper.GetSingleNodeForDoc(doc_rxml, "/POOR_IN200901UV/controlActProcess/subject/placerGroup/author/assignedEntity/representedOrganization/name/item/part");
                if (node_author_assignedEntity_representedOrganizationNameItemPart != null && node_author_assignedEntity_representedOrganizationNameItemPart.Attributes != null)
                {
                    NW.PidDeptName = node_author_assignedEntity_representedOrganizationNameItemPart.Attributes["value"].Value;
                }

                //<!--就诊 -->
                XmlNode node_componentOf1_encountercode = XMLHelper.GetSingleNodeForDoc(doc_rxml, "/POOR_IN200901UV/controlActProcess/subject/placerGroup/componentOf1/encounter/code");
                if (node_componentOf1_encountercode != null && node_componentOf1_encountercode.Attributes != null)
                {
                    //<!--就诊类别编码-->
                    //<code code="01" codeSystem="1.2.156.112678.1.1.80"> 
                    //01 门诊
                    //0201 急诊
                    //02 住院
                    //03 普通体检

                    //<!-- 就诊类别名称 -->
                    //<displayName value="门诊/住院/体检" />
                    if (node_componentOf1_encountercode.Attributes["code"].Value == "02")
                    {
                        NW.PidSrcId = "108";
                    }
                    else if (node_componentOf1_encountercode.Attributes["code"].Value == "03")
                    {
                        NW.PidSrcId = "109";
                    }
                    else
                    {
                        NW.PidSrcId = "107";
                    }
                }

                //<!-- 就诊次数 -->
                //<item extension="2" root="1.2.156.45535072844010611A1001.1.2.1.7"/>
                //<!-- 就诊流水号 -->
                //<item extension="123456" root="1.2.156.45535072844010611A1001.1.2.1.6"/>
                XmlNodeList nodeLt_componentOf1_encounterIdItem = XMLHelper.GetNodesForDoc(doc_rxml, "/POOR_IN200901UV/controlActProcess/subject/placerGroup/componentOf1/encounter/id/item");
                if (nodeLt_componentOf1_encounterIdItem != null && nodeLt_componentOf1_encounterIdItem.Count > 0)
                {
                    foreach (XmlNode nodeFht in nodeLt_componentOf1_encounterIdItem)
                    {
                        if (nodeFht != null && nodeFht.Attributes != null && nodeFht.Attributes["root"].Value == "1.2.156.45535072844010611A1001.1.2.1.7")
                        {
                            int tempIntV = 0;
                            if (int.TryParse(nodeFht.Attributes["extension"].Value, out tempIntV))
                            {
                                NW.PidAdmissTimes = tempIntV;//就诊次数
                            }
                        }
                        if (nodeFht != null && nodeFht.Attributes != null && nodeFht.Attributes["root"].Value == "1.2.156.45535072844010611A1001.1.2.1.6")
                        {
                            //if (NW.PidSrcId != "107")
                            //就诊流水号
                            NW.PidPatNo = nodeFht.Attributes["extension"].Value;
                        }
                    }
                }




                //<!-- 诊断(原因) -->
                XmlNode node_componentOf1_observationDxcodeDisplayName = XMLHelper.GetSingleNodeForDoc(doc_rxml, "/POOR_IN200901UV/controlActProcess/subject/placerGroup/componentOf1/encounter/pertinentInformation1/observationDx/code/displayName");
                if (node_componentOf1_observationDxcodeDisplayName != null && node_componentOf1_observationDxcodeDisplayName.Attributes != null)
                {
                    //<!-- 诊断类别 必须项已使用 -->
                    //<code code="0100" codeSystem="1.2.156.112678.1.1.29" >
                    //    <!--诊断类别名称 -->
                    //    <displayName value="门诊诊断" />		
                    //</code>

                    NW.PidDiag = node_componentOf1_observationDxcodeDisplayName.Attributes["value"].Value;
                }

                XmlNode node_componentOf1_observationDxValueDisplayName = XMLHelper.GetSingleNodeForDoc(doc_rxml, "/POOR_IN200901UV/controlActProcess/subject/placerGroup/componentOf1/encounter/pertinentInformation1/observationDx/value/displayName");
                if (node_componentOf1_observationDxValueDisplayName != null && node_componentOf1_observationDxValueDisplayName.Attributes != null)
                {
                    //<value code="22974" codeSystem="1.2.156.112678.1.1.30">
                    //<!-- 疾病名称 -->
                    //<displayName value="角膜白斑"/>
                    //</value>
                    NW.PidDiag = node_componentOf1_observationDxValueDisplayName.Attributes["value"].Value;
                }


                #region 申请单与医嘱信息


                XmlNodeList component2_ListNode = XMLHelper.GetNodesForDoc(doc_rxml, "/POOR_IN200901UV/controlActProcess/subject/placerGroup/component2");
                //遍历申请号信息
                foreach (XmlNode component2_node in component2_ListNode)
                {
                    foreach (XmlNode component2_ChildA in component2_node.ChildNodes)
                    {
                        //observationRequest
                        if (component2_ChildA != null && component2_ChildA.Name == "observationRequest")
                        {
                            //遍历里面的子节点
                            //<!--申请单信息Begin -->
                            //<observationRequest classCode="OBS" moodCode="RQO">

                            foreach (XmlNode component2_ChildAA in component2_ChildA.ChildNodes)
                            {
                                //<!--检验申请单编号 必须项已使用 -->
                                //<id>
                                //    <!--检验申请单编号 必须项已使用 HIS APPLY_ID -->
                                //    <item extension="12009320" root="1.2.156.45535072844010611A1001.1.2.1.21" />		
                                //</id>
                                if (component2_ChildAA != null && component2_ChildAA.Name == "id")
                                {
                                    foreach (XmlNode component2_ChildAAA in component2_ChildAA.ChildNodes)
                                    {
                                        if (component2_ChildAAA != null
                                           && component2_ChildAAA.Name == "item"
                                           && component2_ChildAAA.Attributes["root"] != null
                                           && component2_ChildAAA.Attributes["root"].Value != null
                                           && component2_ChildAAA.Attributes["root"].Value == "1.2.156.45535072844010611A1001.1.2.1.21"
                                           && component2_ChildAAA.Attributes["extension"] != null)
                                        {
                                            NW.SampApplyNo = component2_ChildAAA.Attributes["extension"].Value;
                                        }
                                    }
                                }

                                //<!-- 检验申请日期 -->
                                //<effectiveTime xsi:type="IVL_TS">
                                //    <any value="20120322"></any>
                                //</effectiveTime>

                                if (component2_ChildAA != null && component2_ChildAA.Name == "effectiveTime")
                                {
                                    //开单时间
                                    string dateString = component2_ChildAA.ChildNodes[0].Attributes["value"].Value;
                                    DateTime SampOccDate = DateTime.Now;
                                    if (!string.IsNullOrEmpty(dateString) && DateTime.TryParseExact(dateString, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out SampOccDate))
                                    {
                                        NW.SampOccDate = SampOccDate;
                                    }
                                    else if (!string.IsNullOrEmpty(dateString) && DateTime.TryParseExact(dateString, "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out SampOccDate))
                                    {
                                        NW.SampOccDate = SampOccDate;
                                    }
                                    else if (!string.IsNullOrEmpty(dateString) && DateTime.TryParseExact(dateString, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out SampOccDate))
                                    {
                                        NW.SampOccDate = SampOccDate;
                                    }
                                    else if (!string.IsNullOrEmpty(dateString) && DateTime.TryParse(dateString, out SampOccDate))
                                    {
                                        NW.SampOccDate = SampOccDate;
                                    }
                                    else
                                    {
                                        NW.SampOccDate = SampOccDate;
                                    }
                                }

                                #region 医嘱号信息

                                if (component2_ChildAA != null && component2_ChildAA.Name == "component2")
                                {
                                    EntitySampOrderHL7 NWAdd = new EntitySampOrderHL7();
                                    NWAdd.SampUrgentFlag = "0";

                                    XmlNode component2_ChildAB = null;

                                    for (int j = 0; j < component2_ChildAA.ChildNodes.Count; j++)
                                    {
                                        if (component2_ChildAA.ChildNodes[j] != null && component2_ChildAA.ChildNodes[j].Name == "observationRequest")
                                        {
                                            component2_ChildAB = component2_ChildAA.ChildNodes[j];
                                        }
                                    }

                                    foreach (XmlNode component2_ChildB in component2_ChildAB.ChildNodes)
                                    {
                                        //<!--医嘱项目信息Begin -->
                                        //<observationRequest classCode="OBS" 
                                        #region 医嘱项目信息Begin observationRequest里的子节点

                                        //<!-- 医嘱号 必须项已使用 -->
                                        //<id>
                                        //    <item extension="35566"></item>
                                        //</id>
                                        if (component2_ChildB != null && component2_ChildB.Name == "id")
                                        {
                                            NWAdd.OrderSn = component2_ChildB.ChildNodes[0].Attributes["extension"].Value;
                                        }

                                        //<!-- 检验项目编码 必须项使用 -->
                                        //<code code="1300200001"  codeSystem="1.2.156.112678.1.1.46">
                                        //    <displayName value="生化五項"/>
                                        //</code>
                                        if (component2_ChildB != null && component2_ChildB.Name == "code")
                                        {
                                            NWAdd.OrderCode = component2_ChildB.Attributes["code"].Value;
                                            NWAdd.OrderName = component2_ChildB.ChildNodes[0].Attributes["value"].Value;
                                        }

                                        //<!-- 采集部位 -->
                                        //<targetSiteCode>
                                        //<item code="2">
                                        //  <displayName value="左眼"/>
                                        // </item>
                                        // </targetSiteCode>
                                        if (component2_ChildB != null && component2_ChildB.Name == "targetSiteCode")
                                        {
                                            foreach (XmlNode xmltargetSiteCodeChs in component2_ChildB.ChildNodes)
                                            {
                                                if (xmltargetSiteCodeChs != null && xmltargetSiteCodeChs.Name != null)
                                                {
                                                    if (xmltargetSiteCodeChs.Name == "item")
                                                    {
                                                        NWAdd.SampRemContent = xmltargetSiteCodeChs.Attributes["code"].Value;
                                                        //foreach (XmlNode xmltargetSiteCodeChsdisplayName in xmltargetSiteCodeChs.ChildNodes)
                                                        //{
                                                        //    if (xmltargetSiteCodeChsdisplayName != null && xmltargetSiteCodeChsdisplayName.Name != null)
                                                        //    {
                                                        //        if (xmltargetSiteCodeChsdisplayName.Name == "displayName"
                                                        //            && xmltargetSiteCodeChsdisplayName.Attributes != null
                                                        //            && xmltargetSiteCodeChsdisplayName.Attributes["value"] != null)
                                                        //        {
                                                        //            NWAdd.SampRemContent = xmltargetSiteCodeChsdisplayName.Attributes["value"].Value;
                                                        //        }
                                                        //    }
                                                        //}
                                                    }
                                                }
                                            }
                                        }


                                        if (component2_ChildB != null && component2_ChildB.Name == "specimen")
                                        {
                                            //<!--标本类型编码 血清/血浆/尿 标本类别代码 -->
                                            //<code code="01" codeSystem="1.2.156.112678.1.1.45">
                                            //    <!--标本类型名称 -->
                                            //    <displayName value="血" />
                                            //</code>

                                            XmlNode node_specimenNatural_code = XMLHelper.GetChildNodeByPath(doc_rxml, component2_ChildB, "specimen/specimenNatural/code");

                                            if (node_specimenNatural_code != null && node_specimenNatural_code.Attributes != null)
                                            {
                                                NWAdd.SampSamId = node_specimenNatural_code.Attributes["code"].Value;

                                                for (int j = 0; j < node_specimenNatural_code.ChildNodes.Count; j++)
                                                {
                                                    if (node_specimenNatural_code.ChildNodes[j] != null && node_specimenNatural_code.ChildNodes[j].Name == "displayName")
                                                    {
                                                        NWAdd.SampSamName = node_specimenNatural_code.ChildNodes[j].Attributes["value"].Value;
                                                    }
                                                }

                                            }

                                        }


                                        if (component2_ChildB != null && component2_ChildB.Name == "component2")
                                        {
                                            //<!-- 是否标记 -->
                                            //<placerGroup>
                                            #region 遍历各种标记

                                            XmlNode node_placerGroup = XMLHelper.GetChildNodeByPath(doc_rxml, component2_ChildB, "placerGroup");
                                            if (node_placerGroup != null && node_placerGroup.ChildNodes.Count > 0)
                                            {
                                                foreach (XmlNode node_placerGroup_pertinentInformation in node_placerGroup.ChildNodes)
                                                {
                                                    //只读取pertinentInformation
                                                    if (node_placerGroup_pertinentInformation != null && node_placerGroup_pertinentInformation.Name == "pertinentInformation")
                                                    {
                                                        XmlNode node_pertinentInformation_code = XMLHelper.GetChildNodeByPath(doc_rxml, node_placerGroup_pertinentInformation, "observation/code");
                                                        XmlNode node_pertinentInformation_value = XMLHelper.GetChildNodeByPath(doc_rxml, node_placerGroup_pertinentInformation, "observation/value");

                                                        #region 加急标记
                                                        //加急标记
                                                        //<code code="03" codeSystem="1.2.156.45535072844010611A1001.1.1.84">
                                                        //<displayName value="加急" />
                                                        //</code>
                                                        //<value xsi:type="BL" value="" />
                                                        if (node_pertinentInformation_code != null && node_pertinentInformation_code.Attributes["code"] != null
                                                            && node_pertinentInformation_code.Attributes["code"].Value == "03")
                                                        {
                                                            if (node_pertinentInformation_value != null && node_pertinentInformation_value.Attributes["value"] != null
                                                            && node_pertinentInformation_value.Attributes["value"].Value == "1")
                                                            {
                                                                NWAdd.SampUrgentFlag = "1";//1=true
                                                            }
                                                            else
                                                            {
                                                                NWAdd.SampUrgentFlag = "0";
                                                            }
                                                        }
                                                        #endregion

                                                        #region 收费状态标识
                                                        //<code code="0102" 
                                                        //codeSystem="1.2.156.112635.1.1.120">
                                                        //<displayName value="收费状态标识"/>
                                                        //</code>
                                                        //<text value="未收费"/>
                                                        //<value xsi:type="ST" value="0"/>
                                                        if (node_pertinentInformation_code != null && node_pertinentInformation_code.Attributes["code"] != null
                                                            && node_pertinentInformation_code.Attributes["code"].Value == "0102")
                                                        {
                                                            if (node_pertinentInformation_value != null && node_pertinentInformation_value.Attributes["value"] != null
                                                            && node_pertinentInformation_value.Attributes["value"].Value == "0")
                                                            {
                                                                NWAdd.DelFlag = "1";//1=true
                                                            }
                                                            else
                                                            {
                                                                NWAdd.DelFlag = "0";
                                                            }
                                                        }
                                                        #endregion
                                                    }
                                                }
                                            }

                                            #endregion


                                        }
                                        #endregion
                                    }

                                    #region 病人信息赋值

                                    NWAdd.PidAge = NW.PidAge;
                                    NWAdd.PidBedNo = NW.PidBedNo;
                                    NWAdd.PidDeptCode = NW.PidDeptCode;
                                    NWAdd.PidDeptName = NW.PidDeptName;
                                    //NWAdd.DelFlag = NW.DelFlag;
                                    NWAdd.PidDeptWard = NW.PidDeptWard;
                                    NWAdd.PidDiag = NW.PidDiag;
                                    NWAdd.PidDoctorCode = NW.PidDoctorCode;
                                    NWAdd.PidDoctorName = NW.PidDoctorName;
                                    NWAdd.PidExamCompany = NW.PidExamCompany;
                                    NWAdd.PidInNo = NW.PidInNo;
                                    NWAdd.PidName = NW.PidName;
                                    NWAdd.SampOccDate = NW.SampOccDate;
                                    NWAdd.PidSrcId = NW.PidSrcId;
                                    NWAdd.PidPatNo = NW.PidPatNo;
                                    NWAdd.PidSex = NW.PidSex;
                                    NWAdd.PidSocialNo = NW.PidSocialNo;
                                    NWAdd.EnumOrderType = NW.EnumOrderType;
                                    NWAdd.PidIdentityCard = NW.PidIdentityCard;
                                    NWAdd.PidTel = NW.PidTel;
                                    NWAdd.PidAdmissTimes = NW.PidAdmissTimes;
                                    NWAdd.PidBirthday = NW.PidBirthday;
                                    NWAdd.SampApplyNo = NW.SampApplyNo;
                                    #endregion


                                    if (NWAdd.PidSrcId == "107" || NWAdd.PidSrcId == "109")
                                    {
                                        NWAdd.SampStatusId = "1";//默认为1
                                    }
                                    else
                                    {
                                        NWAdd.SampStatusId = "0";
                                    }

                                    listHl7.Add(NWAdd);
                                }

                                #endregion
                            }
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {

            }

            return listHl7;

        }
    }
}

