using dcl.common;
using dcl.dao.interfaces;
using dcl.entity;
using Lib.LogManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.hl7
{
    public class HL7MessageBIZ
    {

        private IMessageAnalysis GetMessageAnalysis(string strMsgType)
        {
            IMessageAnalysis msgAly = null;

            switch (strMsgType)
            {
                case "POOR_IN200901UV21":
                    msgAly = new POOR_IN200901UV21();
                    break;
                default:
                    break;
            }

            return msgAly;
        }

        #region  strTestMsg

        private string strTestMsg = @"<?xml version='1.0' encoding='UTF-8'?>
<POOR_IN200901UV ITSVersion='XML_1.0' xmlns='urn:hl7-org:v3'
	xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'
	xsi:schemaLocation='urn:hl7-org:v3 ../../Schemas/POOR_IN200901UV21.xsd'>
  <!--BS006_检验申请信息样例-->
	<!-- 消息ID -->
	<id extension='BS006' />
	<!-- 消息创建时间 -->
	<creationTime value='20120106110000' />
	<!-- 交互ID -->
	<interactionId root='2.16.840.1.113883.1.6' extension='POOR_IN200901UV21' />
	<!-- 消息用途: P(Production); D(Debugging); T(Training) -->
	<processingCode code='P' />
	<!-- 消息处理模式: A(Archive); I(Initial load); R(Restore from archive); T(Current 
		processing) -->
	<processingModeCode code='R' />
	<!-- 消息应答: AL(Always); ER(Error/reject only); NE(Never) -->
	<acceptAckCode code='NE' />

	<!-- 接受者 -->
	<receiver typeCode='RCV'>
		<device classCode='DEV' determinerCode='INSTANCE'>
			<!-- 接受者ID -->
			<id>
				<item root='1.2.156.45535072844010611A1001.1.1.19' extension=''/>
			</id>
		</device>
	</receiver>

	<!-- 发送者 -->
	<sender typeCode='SND'>
		<device classCode='DEV' determinerCode='INSTANCE'>
			<!-- 发送者ID -->
			<id>
				<item root='1.2.156.45535072844010611A1001.1.1.19' extension=''/>
			</id>
		</device>
	</sender>

	<controlActProcess classCode='CACT' moodCode='EVN'>
		<!-- 消息交互类型 @code: 新增 :new 删除：delete -->
		<code code='update'></code>
		<subject typeCode='SUBJ' xsi:nil='false'>
			<placerGroup classCode='GROUPER' moodCode='RQO'>
				<subject typeCode='SBJ'>
					<patient classCode='PAT'>
						<id>
							<!--域ID -->
							<item root='1.2.156.45535072844010611A1001.1.2.1.2' extension='111' />
							<!--患者ID -->
							<item root='1.2.156.45535072844010611A1001.1.2.1.3' extension='001407878200' />
							<!--就诊号 -->
							<item root='1.2.156.45535072844010611A1001.1.2.1.12' extension='4153754' />
						</id>
						<!-- 病区 床号 -->
						<addr xsi:type='BAG_AD'>
							<item use='TMP'>
								<!-- 病区名 病区编码 -->
								<part type='BNR' value='10C' code='1000068' codeSystem='1.2.156.45535072844010611A1001.1.1.33' />
								<!--床号 -->
								<part type='CAR' value='002' />
							</item>
						</addr>
						<!--个人信息 -->
						<patientPerson classCode='PSN'>
							<!-- 身份证号 -->
							<id>
								<item extension='15001198807080982' root='1.2.156.45535072844010611A1001.1.2.1.9' />
							</id>
							<!--患者姓名 -->
							<name xsi:type='BAG_EN'>
								<item>
									<part value='张三' />
								</item>
							</name>
							<telecom xsi:type='BAG_TEL'>
								<!-- 联系电话 -->
								<item use='EC' value='13128870214'></item>
								<!--移动电话 -->
								<item use='MC' value='010-6632134'></item>
							</telecom>
							<!--性别代码 -->
							<administrativeGenderCode code='1'
								codeSystem='1.2.156.45535072844010611A1001.1.1.3' />
							<!--出生日期 -->
							<birthTime value='20000101'>
								<!--年龄 -->
								<originalText value='12' />
							</birthTime>
							<!--地址 -->
							<addr xsi:type='BAG_AD'>
								<!-- 家庭住址 -->
								<item use='H'>
									<!--地址 -->
									<part type='AL' value='西城区人民医院58号' />
									<!--邮政编码 -->
									<part type='ZIP' value='530022' />
								</item>
							</addr>
							<asEmployee classCode='EMP'>
								<employerOrganization determinerCode='INSTANCE'
									classCode='ORG'>
									<!-- 工作单位代码 -->
									<id>
										<item extension='00100'></item>
									</id>
									<!--工作单位名称 -->
									<name xsi:type='BAG_EN'>
										<item>
											<part value='方正国际' />
										</item>
									</name>
									<!-- 必须项未使用 -->
									<contactParty classCode='CON'></contactParty>
								</employerOrganization>
							</asEmployee>
						</patientPerson>
						<providerOrganization classCode='ORG'
							determinerCode='INSTANCE'>
							<!--病人科室编码-->
							<id>
								<item extension='1409889' root='1.2.156.45535072844010611A1001.1.1.1'/>
							</id>
							<!--病人科室名称 -->
							<name xsi:type='BAG_EN'>
								<item>
									<part value='检验科' />
								</item>
							</name>
							<asOrganizationPartOf classCode='PART'>
								<wholeOrganization determinerCode='INSTANCE' classCode='ORG'>
									<!--医疗机构代码 -->
									<id>
										<item extension='45535072844010611A1001'/>
									</id>
									<!--医疗机构名称 -->
									<name xsi:type='BAG_EN'>
										<item><part value='广州市第十二人民医院' /></item>
									</name>
								</wholeOrganization>
							</asOrganizationPartOf>
						</providerOrganization>
					</patient>
				</subject>
				<author typeCode='AUT'>
					<!-- 开单时间 -->
					<time value='20120413'></time>
					<assignedEntity classCode='ASSIGNED'>
						<!-- 开单医生编码 -->
						<id>
							<item extension='03421' root='1.2.156.45535072844010611A1001.1.1.2'></item>
						</id>
						<assignedPerson determinerCode='INSTANCE'
							classCode='PSN'>
							<!--开单医生姓名 必须项已使用 -->
							<name xsi:type='BAG_EN'>
								<item>
									<part value='李四'></part>
								</item>
							</name>
						</assignedPerson>
						<!-- 申请科室 信息 -->
						<representedOrganization determinerCode='INSTANCE'
							classCode='ORG'>
							<!-- 申请科室编码 必须已使用 -->
							<id>
								<item extension='140988948' root='1.2.156.45535072844010611A1001.1.1.1'></item>
							</id>
							<!-- 申请科室名称 -->
							<name xsi:type='BAG_EN'>
								<item use='ABC'>
									<part value='检验科微生物室' />
								</item>
							</name>
						</representedOrganization>
					</assignedEntity>
				</author>
				<!-- 录入人 -->
				<transcriber typeCode='TRANS'>
					<time>
						<!-- 录入日期 开始时间 -->
						<low value='20120322140500'></low>
						<!-- 录入日期 结束时间 -->
						<high value='20120322140800'></high>
					</time>
					<assignedEntity classCode='ASSIGNED'>
						<!-- 录入人 -->
						<id>
							<item extension='07466' root='1.2.156.45535072844010611A1001.1.1.2'></item>
						</id>
						<assignedPerson determinerCode='INSTANCE'
							classCode='PSN'>
							<!-- 录入人姓名 必须项已使用 -->
							<name xsi:type='BAG_EN'>
								<item use='ABC'>
									<part value='王五' />
								</item>
							</name>
						</assignedPerson>
					</assignedEntity>
				</transcriber>
				<verifier typeCode='VRF' xsi:nil='false'>
					<!--确认时间 -->
					<time value='201205031433' />
					<assignedEntity classCode='ASSIGNED'>
						<!--确认人编码 -->
						<id>
							<item extension='123456789' root='1.2.156.45535072844010611A1001.1.1.2' />
						</id>
						<assignedPerson classCode='PSN' determinerCode='INSTANCE'
							xsi:nil='false'>
							<!--确认人姓名 -->
							<name xsi:type='BAG_EN'>
								<item>
									<part value='确认人姓名' />
								</item>
							</name>
						</assignedPerson>
					</assignedEntity>
				</verifier>
				<!--1.。n 一个检验消息中可以由多个申请单。component2对应一个申请单，有多个申请单时，重复component2 -->
				<component2 typeCode='COMP'><!--申请单信息Begin -->
					<observationRequest classCode='OBS' moodCode='RQO'>
						<!--检验申请单编号 必须项已使用 -->
						<id>
							<!--检验申请单编号 必须项已使用 HIS APPLY_ID -->
							<item extension='12009320' root='1.2.156.45535072844010611A1001.1.2.1.21' />		
						</id>
						<!-- 医嘱类型必须项目已使用 -->
						<code code='03' codeSystem='1.2.156.45535072844010611A1001.1.1.27'>
							<!-- 医嘱类型名称 -->
							<displayName value='化验类' />
						</code>
						<text ></text>
						<!-- 检验申请单状态 必须项未使用 -->
						<statusCode code='active'></statusCode>
						<!-- 检验申请日期 -->
						<effectiveTime xsi:type='IVL_TS'>
							<any value='20120322'></any>
						</effectiveTime>
						<!-- 是否私隐 -->
						<confidentialityCode>
							<item code='0' codeSystem='1.2.156.45535072844010611A1001.1.1.84'></item>
						</confidentialityCode>
						<!--1..n 一个申请单可以包含多个医嘱，每个医嘱对应一个component2,多个医嘱重复component2 -->
						<component2 typeCode='COMP'> 	<!--医嘱项目信息Begin -->
							<observationRequest classCode='OBS' moodCode='RQO'>
								<!-- 医嘱号 必须项已使用 -->
								<id>
									<item extension='35566'></item>
								</id>
								<!-- 检验项目编码 必须项使用 -->
								<code code='1300200001'  codeSystem='1.2.156.45535072844010611A1001.1.1.46'>
									<displayName value='生化五項'/>
								</code>
								<!-- 必须项未使用 -->
								<statusCode />
								<!-- validTimeLow='医嘱开始时间' validTimeHigh='医嘱结束时间'-->
								<effectiveTime xsi:type='QSC_TS' validTimeLow='201203041123' validTimeHigh='201203041130'>
									<!-- 医嘱执行频率编码 -->
									<code code='BID' codeSystem='1.2.156.45535072844010611A1001.1.1.28'>
										<!--医嘱执行频率名称 -->
										<displayName value='2/日(8am-4pm)' />
									</code>
								</effectiveTime>								
								<!-- 检验项目优先级别 -->
								<priorityCode code='1' />
								<!-- 选择血糖或者胰岛素等项目时用 -->
								<methodCode>
									<!-- 检验描述编码 -->
									<item code='0010' codeSystem='1.2.156.45535072844010611A1001.1.1.116'>
										<!-- 检验描述名称 -->
										<displayName value='30分钟'></displayName>
									</item>
								</methodCode>								
								<!-- 采集部位 -->
								<targetSiteCode>
									<item code='11' />
								</targetSiteCode>
								<specimen typeCode='SPC'>
									<specimen classCode='SPEC'>
										<!--标本号/条码号 必须项已使用 -->
										<id extension='00983' />
										<!--标本角色代码（patient,group,blind) 必须项目未使用 -->
										<code />
										<specimenNatural determinerCode='INSTANCE'
											classCode='ENT'>
											<!--标本类型编码 血清/血浆/尿 标本类别代码 -->
											<code code='01' codeSystem='1.2.156.45535072844010611A1001.1.1.45'>
												<!--标本类型名称 -->
												<displayName value='血' />
											</code>
										</specimenNatural>
										<subjectOf1 typeCode='SBJ' contextControlCode='OP'>
											<specimenProcessStep moodCode='EVN'
												classCode='SPECCOLLECT'>
												<!-- 采集日期 -->
												<effectiveTime xsi:type='IVL_TS'>
													<any value='20120222'></any>
												</effectiveTime>
												<subject typeCode='SBJ' contextControlCode='OP'>
													<specimenInContainer classCode='CONT'>
														<container determinerCode='INSTANCE' classCode='CONT'>
															<!--测试项目容器类型-->
															<code code='0237'></code>
															<!-- 测试项目容器 -->
															<desc value='试管' />
														</container>
													</specimenInContainer>
												</subject>
												<performer typeCode='PRF'>
													<assignedEntity classCode='ASSIGNED'>
														<!-- 采集人Id -->
														<id>
															<item extension='0100' root='1.2.156.45535072844010611A1001.1.1.2'></item>
														</id>
														<assignedPerson determinerCode='INSTANCE'
															classCode='PSN'>
															<!-- 采集人姓名 -->
															<name xsi:type='BAG_EN'>
																<item>
																	<part value='张三' />
																</item>
															</name>
															<!-- 采集地点 -->
															<asLocatedEntity classCode='LOCE'>
																<addr xsi:type='BAG_AD'>
																	<item use='WP'>
																		<part type='BNR' value='护士站' />
																	</item>
																</addr>
															</asLocatedEntity>
														</assignedPerson>
													</assignedEntity>
												</performer>
											</specimenProcessStep>
										</subjectOf1>
									</specimen>
								</specimen>
								<!-- 执行科室 -->
								<location typeCode='LOC'>
									<serviceDeliveryLocation classCode='SDLOC'>
										<serviceProviderOrganization
											determinerCode='INSTANCE' classCode='ORG'>
											<!-- 执行科室编码 -->
											<id>
												<item extension='1010700' root='1.2.156.45535072844010611A1001.1.1.1'></item>
											</id>
											<!--执行科室名称 -->
											<name xsi:type='BAG_EN' controlInformationExtension='1010700'>
												<item>
													<part value='检验室' />
												</item>
											</name>
										</serviceProviderOrganization>
									</serviceDeliveryLocation>
								</location>
								<component2>
									<!-- 是否标记 -->
									<placerGroup>
										<!-- 是否皮试 -->
										<pertinentInformation typeCode='PERT'
											contextConductionInd='false'>
											<observation classCode='OBS' moodCode='INT'>
												<code code='01' codeSystem='1.2.156.45535072844010611A1001.1.1.84'>
													<displayName value='皮试' />
												</code>
												<value xsi:type='BL' value='true' />
											</observation>
										</pertinentInformation>
										<!-- 是否加急 -->
										<pertinentInformation typeCode='PERT'
											contextConductionInd='false'>
											<observation classCode='OBS' moodCode='INT'>
												<code code='03' codeSystem='1.2.156.45535072844010611A1001.1.1.84'>
													<displayName value='加急' />
												</code>
												<value xsi:type='BL' value='true' />
											</observation>
										</pertinentInformation>
										<!-- 是否药观 -->
										<pertinentInformation typeCode='PERT'
											contextConductionInd='false'>
											<observation classCode='OBS' moodCode='INT'>
												<code code='04' codeSystem='1.2.156.45535072844010611A1001.1.1.84'>
													<displayName value='药观' />
												</code>
												<value xsi:type='BL' value='false' />
											</observation>
										</pertinentInformation>
										<!-- 先诊疗后付费类型  -->
										<pertinentInformation typeCode='PERT'
											contextConductionInd='false'>
											<observation classCode='OBS' moodCode='INT'>
												<code code='0101' codeSystem='1.2.156.45535072844010611A1001.1.1.120'>
													<displayName value='先诊疗后付费类型' />
												</code>
												<value xsi:type='CD' code='01'>
													<displayName value='银医通'/>
												</value>
											</observation>
										</pertinentInformation>	
										<!-- 收费状态标识  -->
										<pertinentInformation typeCode='PERT'
											contextConductionInd='false'>
											<observation classCode='OBS' moodCode='INT'>
												<code code='0102' codeSystem='1.2.156.45535072844010611A1001.1.1.120'>
													<displayName value='收费状态标识' />
												</code>
												<value xsi:type='ST' value='0' />
											</observation>
										</pertinentInformation>
                                        <!-- HIS执行状态  -->
										<pertinentInformation typeCode='PERT'
											contextConductionInd='false'>
											<observation classCode='OBS' moodCode='INT'>
												<code code='0201' codeSystem='1.2.156.45535072844010611A1001.1.1.120'>
													<displayName value='HIS执行状态' />
												</code>
												<value xsi:type='ST' value='0' />
											</observation>
										</pertinentInformation>											<!-- 业务操作时间  -->
										<pertinentInformation typeCode='PERT'
											contextConductionInd='false'>
											<observation classCode='OBS' moodCode='INT'>
												<code code='0202' codeSystem='1.2.156.45535072844010611A1001.1.1.120'>
													<displayName value='业务操作时间' />
												</code>
												<value xsi:type='ST' value='20150617151212' />
											</observation>
										</pertinentInformation>	
										<!--医嘱时间类型-->
										<pertinentInformation typeCode='PERT' contextConductionInd='false'>
											<observation classCode='OBS ' moodCode='EVN'>
												<code code='0209' codeSystem='1.2.156.45535072844010611A1001.1.1.120'/>
												<!--医嘱时间类型编码 名称-->
												<value xsi:type='CD' code='1' codeSystem='1.2.156.45535072844010611A1001.1.1.82'>
													<displayName value='长期'></displayName>
												</value> 
											</observation>
										</pertinentInformation>	
										<!-- 临床路径项目编号 -->
										<pertinentInformation typeCode='PERT' contextConductionInd='false'>
											<observation classCode='OBS' moodCode='EVN'>
												<code code='0210'  codeSystem='1.2.156.45535072844010611A1001.1.1.120' >
													<displayName value='临床路径项目编号'/>
												</code>
												<value xsi:type='ST' value='11184'/>
											</observation>
										</pertinentInformation>	
														
										<!-- 临床路径项目序号 -->
										<pertinentInformation typeCode='PERT' contextConductionInd='false'>
											<observation classCode='OBS' moodCode='EVN'>
												<code code='0211' codeSystem='1.2.156.45535072844010611A1001.1.1.120'>
													<displayName value='临床路径项目序号'/>
												</code>
												<value xsi:type='ST' value='328521'/>
											</observation>
										</pertinentInformation>							
									</placerGroup>
								</component2>																
								<subjectOf1 typeCode='SUBJ'>
									<valuedItem moodCode='DEF' classCode='INVE'>
										<!--测试项目价格 -->
										<unitPriceAmt>
											<numerator xsi:type='MO' value='200' currency='RMB' />
										</unitPriceAmt>
										<component typeCode='COMP'>
											<valuedUnitItem moodCode='DEF' classCode='INVE'>
												<!-- 必须项未使用 -->
												<unitQuantity />
												<!--耗材价格 -->
												<unitPriceAmt>
													<numerator xsi:type='MO' value='20' currency='RMB' />
												</unitPriceAmt>
											</valuedUnitItem>
										</component>
									</valuedItem>
								</subjectOf1>								
								<!--标本要求 -->
								<subjectOf6 contextConductionInd='false' xsi:nil='false'
									typeCode='SUBJ'>
									<!-- 必须项 未使用 default=false -->
									<seperatableInd value='false' />
									<annotation classCode='ACT' moodCode='EVN'>
										<!-- 备注类型 -->
										<code code='01' codeSystem='1.2.156.45535072844010611A1001.1.2.2.11'></code>
										<!--标本要求 必须项已使用 -->
										<text value='空腹' />
										<!--注意事项状态,模型中要求,值为'completed' -->
										<statusCode code='completed' />
										<!--必须项 未使用 -->
										<author>
											<assignedEntity classCode='ASSIGNED' />
										</author>
									</annotation>
								</subjectOf6>								
							</observationRequest>
						</component2>		<!--医嘱项目信息End -->
						<!--报告备注 -->
						<subjectOf6 contextConductionInd='false' xsi:nil='false'
							typeCode='SUBJ'>
							<!-- 必须项 未使用 default=false -->
							<seperatableInd value='false' />
							<annotation classCode='ACT' moodCode='EVN'>
								<!-- 备注类型 -->
								<code code='03' codeSystem='1.2.156.45535072844010611A1001.1.2.2.11'></code>
								<!--报告备注 必须项已使用 -->
								<text value='报告备注' />
								<!--注意事项状态,模型中要求,值为'completed' -->
								<statusCode code='completed' />
								<!--必须项 未使用 -->
								<author>
									<assignedEntity classCode='ASSIGNED' />
								</author>
							</annotation>
						</subjectOf6>
						<!--备注字段1 药观编码－打印在报告单上，药理机构要求 -->
						<subjectOf6 contextConductionInd='false' xsi:nil='false'
							typeCode='SUBJ'>
							<!-- 必须项 未使用 default=false -->
							<seperatableInd value='false' />
							<annotation classCode='ACT' moodCode='EVN'>
								<!-- 备注类型 -->
								<code code='01' codeSystem='1.2.156.45535072844010611A1001.1.2.2.12'></code>
								<!--药观编码 必须项已使用 -->
								<text value='药观编码' />
								<!--注意事项状态,模型中要求,值为'completed' -->
								<statusCode code='completed' />
								<!--必须项 未使用 -->
								<author>
									<assignedEntity classCode='ASSIGNED' />
								</author>
							</annotation>
						</subjectOf6>
						<!--备注字段2 药观名称－打印在报告单上，药理机构要求 -->
						<subjectOf6 contextConductionInd='false' xsi:nil='false'
							typeCode='SUBJ'>
							<!-- 必须项 未使用 default=false -->
							<seperatableInd value='false' />
							<annotation classCode='ACT' moodCode='EVN'>
								<!-- 备注类型 -->
								<code code='02' codeSystem='1.2.156.45535072844010611A1001.1.2.2.13'></code>
								<!--药观名称 必须项已使用 -->
								<text value='药观名称' />
								<!--注意事项状态,模型中要求,值为'completed' -->
								<statusCode code='completed' />
								<!--必须项 未使用 -->
								<author>
									<assignedEntity classCode='ASSIGNED' />
								</author>
							</annotation>
						</subjectOf6>
						<!--备注字段3 其它HIS要求储存但未确定信息1 -->
						<subjectOf6 contextConductionInd='false' xsi:nil='false'
							typeCode='SUBJ'>
							<!-- 必须项 未使用 default=false -->
							<seperatableInd value='false' />
							<annotation classCode='ACT' moodCode='EVN'>
								<!-- 备注类型 -->
								<code code='04' codeSystem='1.2.156.45535072844010611A1001.1.2.2.14'></code>
								<!--备注字段3 必须项已使用 -->
								<text value='备注字段3' />
								<!--注意事项状态,模型中要求,值为'completed' -->
								<statusCode code='completed' />
								<!--必须项 未使用 -->
								<author>
									<assignedEntity classCode='ASSIGNED' />
								</author>
							</annotation>
						</subjectOf6>
						<!--备注字段4 其它HIS要求储存但未确定信息2 -->
						<subjectOf6 contextConductionInd='false' xsi:nil='false'
							typeCode='SUBJ'>
							<!-- 必须项 未使用 default=false -->
							<seperatableInd value='false' />
							<annotation classCode='ACT' moodCode='EVN'>
								<!-- 备注类型 -->
								<code code='05' codeSystem='1.2.156.45535072844010611A1001.1.2.2.15'></code>
								<!--备注字段4 必须项已使用 -->
								<text value='备注字段4' />
								<!--注意事项状态,模型中要求,值为'completed' -->
								<statusCode code='completed' />
								<!--必须项 未使用 -->
								<author>
									<assignedEntity classCode='ASSIGNED' />
								</author>
							</annotation>
						</subjectOf6>
						<!--备注字段5 其它HIS要求储存但未确定信息3 -->
						<subjectOf6 contextConductionInd='false' xsi:nil='false'
							typeCode='SUBJ'>
							<!-- 必须项 未使用 default=false -->
							<seperatableInd value='false' />
							<annotation classCode='ACT' moodCode='EVN'>
								<!-- 备注类型 -->
								<code code='06' codeSystem='1.2.156.45535072844010611A1001.1.2.2.16'></code>
								<!--备注字段5 必须项已使用 -->
								<text value='备注字段3' />
								<!--注意事项状态,模型中要求,值为'completed' -->
								<statusCode code='completed' />
								<!--必须项 未使用 -->
								<author>
									<assignedEntity classCode='ASSIGNED' />
								</author>
							</annotation>
						</subjectOf6>
					</observationRequest>
				</component2>	<!--申请单信息End -->
				<!--就诊 -->
				<componentOf1 contextConductionInd='false' xsi:nil='false'
					typeCode='COMP'>
					<!--就诊 -->
					<encounter classCode='ENC' moodCode='EVN'>
						<id>
						    <!-- 就诊次数 -->
							<item extension='2' root='1.2.156.45535072844010611A1001.1.2.1.7'/>
							<!-- 就诊流水号 -->
							<item extension='123456' root='1.2.156.45535072844010611A1001.1.2.1.6'/>
						</id>
						
						<!--就诊类别编码-->
						<code codeSystem='1.2.156.45535072844010611A1001.1.1.80' code='01'>
							<!-- 就诊类别名称 -->
							<displayName value='门诊/住院/体检' />
						</code>
						<!--必须项未使用 -->
						<statusCode code='Active' />
						<!--病人 必须项未使用 -->
						<subject typeCode='SBJ'>
							<patient classCode='PAT' />
						</subject>
						<!--就诊机构/科室 -->
						<location typeCode='LOC' xsi:nil='false'>
							<!--必须项未使用 -->
							<time />
							<!--就诊机构/科室 -->
							<serviceDeliveryLocation classCode='SDLOC'>
								<serviceProviderOrganization
									determinerCode='INSTANCE' classCode='ORG'>
									<!--就诊院区编码 -->
									<id>
										<item extension='01' identifierName='新院'></item>
									</id>
									<!--就诊院区名称 -->
									<name xsi:type='BAG_EN'>
										<item>
											<part value='江苏省人民医院'></part>
										</item>
									</name>
								</serviceProviderOrganization>
							</serviceDeliveryLocation>
						</location>
						<!-- 诊断(原因) -->
						<pertinentInformation1 typeCode='PERT'
							xsi:nil='false'>
							<observationDx classCode='OBS' moodCode='EVN'>
								<!-- 诊断类别 必须项已使用 -->
								<code code='0100' codeSystem='1.2.156.45535072844010611A1001.1.1.29' >
									<!--诊断类别名称 -->
									<displayName value='门诊诊断' />		
								</code>						
								<!-- 必须项未使用 -->
								<statusCode code='active' />
								<!-- 疾病代码 必须项已使用 -->
								<value code='A23.901' codeSystem='1.2.156.45535072844010611A1001.1.1.30'>
									<!-- 疾病名称 -->
									<displayName value='感冒' />
								</value>
							</observationDx>
						</pertinentInformation1>
					</encounter>
				</componentOf1>
			</placerGroup>
		</subject>
	</controlActProcess>
</POOR_IN200901UV>";

        #endregion

        public bool SaveOrderInfo(string strMsg, string strMsgType)
        {
            bool result = false;

            //Logger.LogInfo(strMsg);

            try
            {
                //strMsg = strTestMsg;
                IMessageAnalysis msgAly = GetMessageAnalysis(strMsgType);

                if (msgAly != null)
                {
                    List<EntitySampOrderHL7> listSampOrder = msgAly.MessageAnalysis(strMsg);
                    if (listSampOrder.Count > 0)
                    {
                        IDaoSampOrder dao = DclDaoFactory.DaoHandler<IDaoSampOrder>();
                        if (dao != null)
                        {
                            result = dao.SaveSampOrder(listSampOrder);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
            }

            return result;
        }

        public string GetReportInfo(string strRepId, string strMsgType)
        {
            IDaoSampMain dao = DclDaoFactory.DaoHandler<IDaoSampMain>();
            if (dao != null)
            {
                EntityQcResultList pidResult = GetPidResult(strRepId);

                EntitySampMain sampMain = null;
                if (!string.IsNullOrEmpty(pidResult.listPatients[0].RepBarCode))
                {
                    EntitySampQC sampQC = new EntitySampQC();
                    sampQC.ListSampBarId.Add(pidResult.listPatients[0].RepBarCode);

                    List<EntitySampMain> listSampMain = dao.GetSampMain(sampQC);
                    if (listSampMain.Count > 0)
                        sampMain = listSampMain[0];
                }

                return new POCD_HD000040().CreateReportMessage(pidResult, sampMain);
            }
            return string.Empty; ;
        }

        public string GetOperationInfo(EntitySampOperation operation, EntitySampMain sampMain)
        {
            return new POOR_IN200901UV23().CreateOperationMessage(operation, sampMain);
        }

        private EntityQcResultList GetPidResult(string strRepId)
        {
            EntityQcResultList pidResult = null;

            IDaoPidReportMain dao = DclDaoFactory.DaoHandler<IDaoPidReportMain>();
            IDaoPidReportDetail detailDao = DclDaoFactory.DaoHandler<IDaoPidReportDetail>();
            IDaoObrResult resultDao = DclDaoFactory.DaoHandler<IDaoObrResult>();
            IDaoObrResultDesc descDao = DclDaoFactory.DaoHandler<IDaoObrResultDesc>();
            IDaoObrResultAnti antiDao = DclDaoFactory.DaoHandler<IDaoObrResultAnti>();
            IDaoObrResultBact bactDao = DclDaoFactory.DaoHandler<IDaoObrResultBact>();

            if (dao != null &&
                detailDao != null &&
                resultDao != null &&
                descDao != null &&
                antiDao != null &&
                bactDao != null)
            {
                pidResult = new EntityQcResultList();

                EntityPidReportMain patient = dao.GetPatientInfo(strRepId);
                pidResult.listPatients.Add(patient);

                pidResult.listRepDetail = detailDao.GetPidReportDetailByRepId(strRepId);

                if (patient.ItrReportType == "3" || patient.ItrReportType == "4")
                {
                    pidResult.listDesc = descDao.GetObrResultDescById(strRepId);

                    pidResult.listAnti = antiDao.GetAntiResultById(strRepId);

                    if (pidResult.listDesc.Count == 0 && pidResult.listAnti.Count == 0)
                        pidResult.listBact = bactDao.GetBactResultById(strRepId);
                }
                else
                {
                    EntityResultQC resultQc = new EntityResultQC();
                    resultQc.ListObrId.Add(patient.RepId);
                    pidResult.listResulto = resultDao.ObrResultQuery(resultQc);
                }
            }


            return pidResult;
        }

    }
}
