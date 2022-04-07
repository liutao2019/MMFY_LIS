using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 统计分析的查询条件
    /// </summary>
    [Serializable]
    public class EntityStatisticsQC : EntityBase
    {
        /// <summary>
        ///时间类型
        /// </summary>   
        public string TimeType { get; set; }

        /// <summary>
        ///开始时间
        /// </summary>   
        public string DateEditStart { get; set; }
        /// <summary>
        /// 排除回退条码的
        /// </summary>
        public Boolean cbWithoutReturn { get; set; }

        /// <summary>
        ///结束时间
        /// </summary>   
        public String DateEditEnd { get; set; }

        /// <summary>
        ///选择序号
        /// </summary>   
        public String SelectedIndex { get; set; }

        /// <summary>
        ///开始样本号
        /// </summary>   
        public String EditYBStart { get; set; }

        /// <summary>
        ///结束样本号
        /// </summary>   
        public String EditYBEnd { get; set; }

        /// <summary>
        ///开始年龄
        /// </summary>   
        public String EditAgeStart { get; set; }

        /// <summary>
        ///结束年龄
        /// </summary>   
        public String EditAgeEnd { get; set; }

        /// <summary>
        ///报告代码
        /// </summary>   
        public String ReportCode { get; set; }
        /// <summary>
        /// 分组标识
        /// </summary>
        public List<string> typeList { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public String GroupName { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public String Group { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public String Order { get; set; }

        /// <summary>
        /// 细菌统计类型
        /// </summary>
        public String BacilliType { get; set; }

        /// <summary>
        /// 细菌药敏结果
        /// </summary>
        public String CmbResults { get; set; }

        /// <summary>
        ///性别
        /// </summary>   
        public String Sex { get; set; }

        /// <summary>
        ///所有条件的where
        /// </summary>   
        public String Where { get; set; }

        /// <summary>
        ///所有条件的where
        /// </summary>   
        public String SubWhere { get; set; }

        /// <summary>
        ///所有条件的where
        /// </summary>   
        public String ItemWhere { get; set; }

        /// <summary>
        /// (质控)所有条件where
        /// </summary>
        public String QcWhere { get; set; }

        /// <summary>
        /// 所选仪器列表
        /// </summary>
        public List<EntityDicInstrument> ItrList { get; set; }

        /// <summary>
        /// 全选仪器列表
        /// </summary>
        public List<EntityDicInstrument> ItrAllList { get; set; }
        /// <summary>
        /// 所选供货商列表
        /// </summary>
        public List<EntityDicReaSupplier> ReaSupList { get; set; }

        /// <summary>
        /// 全选供货商列表
        /// </summary>
        public List<EntityDicReaSupplier> ReaSupAllList { get; set; }

        /// <summary>
        ///院区id
        /// </summary>   
        public String OrgId { get; set; }

        /// <summary>
        /// 所选仪器ID列表
        /// </summary>
        public String ItrIdString
        {
            get
            {
                string ItrId = string.Empty;
                if (ItrList != null)
                {
                    foreach (var item in ItrList)
                    {
                        ItrId += "'" + item.ItrId + "',";
                    }
                    ItrId = ItrId.Remove(ItrId.LastIndexOf(","));
                }
                return ItrId;
            }
        }

        /// <summary>
        /// 所选仪器名称列表
        /// </summary>
        public String ItrNameString
        {
            get
            {
                string ItrName = string.Empty;
                if (ItrList != null)
                {
                    if (ItrAllList != null)
                    {
                        ItrName = "仪器:(全部);";
                    }
                    else
                    {
                        ItrName += "仪器:(";
                        foreach (var item in ItrList)
                        {
                            ItrName += item.ItrName + ",";
                        }
                        ItrName = ItrName.Remove(ItrName.LastIndexOf(","));
                        ItrName += ");";
                    }
                }
                return ItrName;
            }
        }
        /// <summary>
        /// 所选供货商ID列表
        /// </summary>
        public String ReaSupIdString
        {
            get
            {
                string ReaSupId = string.Empty;
                if (ReaSupList != null)
                {
                    foreach (var item in ReaSupList)
                    {
                        ReaSupId += "'" + item.Rsupplier_id + "',";
                    }
                    ReaSupId = ReaSupId.Remove(ReaSupId.LastIndexOf(","));
                }
                return ReaSupId;
            }
        }

        /// <summary>
        /// 所选供货商名称列表
        /// </summary>
        public String ReaSupNameString
        {
            get
            {
                string ReaSupName = string.Empty;
                if (ReaSupList != null)
                {
                    if (ReaSupAllList != null)
                    {
                        ReaSupName = "供货商:(全部);";
                    }
                    else
                    {
                        ReaSupName += "供货商:(";
                        foreach (var item in ReaSupList)
                        {
                            ReaSupName += item.Rsupplier_name + ",";
                        }
                        ReaSupName = ReaSupName.Remove(ReaSupName.LastIndexOf(","));
                        ReaSupName += ");";
                    }
                }
                return ReaSupName;
            }
        }
        /// <summary>
        /// 所选试剂列表
        /// </summary>
        public List<EntityReaSetting> ReagentList { get; set; }

        /// <summary>
        /// 全选选试剂列表
        /// </summary>
        public List<EntityReaSetting> ReagentAllList { get; set; }
        /// <summary>
        /// 所选试剂ID列表
        /// </summary>
        public String ReagentIdString
        {
            get
            {
                string ReagentId = string.Empty;
                if (ReagentList != null)
                {
                    foreach (var item in ReagentList)
                    {
                        ReagentId += "'" + item.Drea_id + "',";
                    }
                    ReagentId = ReagentId.Remove(ReagentId.LastIndexOf(","));
                }
                return ReagentId;
            }
        }

        /// <summary>
        /// 所选试剂名称列表
        /// </summary>
        public String ReagentNameString
        {
            get
            {
                string ReagentName = string.Empty;
                if (ReagentList != null)
                {
                    if (ReagentAllList != null)
                    {
                        ReagentName = "试剂:(全部);";
                    }
                    else
                    {
                        ReagentName += "试剂:(";
                        foreach (var item in ReagentList)
                        {
                            ReagentName += item.Drea_name + ",";
                        }
                        ReagentName = ReagentName.Remove(ReagentName.LastIndexOf(","));
                        ReagentName += ");";
                    }
                }
                return ReagentName;
            }
        }
        /// <summary>
        /// 所选组别列表
        /// </summary>
        public List<EntityDicReaGroup> GroupList { get; set; }
        /// <summary>
        /// 全选组别列表
        /// </summary>
        public List<EntityDicReaGroup> GroupAllList { get; set; }
        /// <summary>
        /// 所选组别ID列表
        /// </summary>
        public String GroupIdString
        {
            get
            {
                string GroupId = string.Empty;
                if (GroupList != null)
                {
                    foreach (var item in GroupList)
                    {
                        GroupId += "'" + item.Rea_group_id + "',";
                    }
                    GroupId = GroupId.Remove(GroupId.LastIndexOf(","));
                }
                return GroupId;
            }
        }

        /// <summary>
        /// 所选组别名称列表
        /// </summary>
        public String GroupNameString
        {
            get
            {
                string GroupName = string.Empty;
                if (GroupList != null)
                {
                    if (GroupAllList != null)
                    {
                        GroupName = "组别:(全部);";
                    }
                    else
                    {
                        GroupName += "组别:(";
                        foreach (var item in GroupList)
                        {
                            GroupName += item.Rea_group + ",";
                        }
                        GroupName = GroupName.Remove(GroupName.LastIndexOf(","));
                        GroupName += ");";
                    }
                }
                return GroupName;
            }
        }

        /// <summary>
        /// 所选生产厂商列表
        /// </summary>
        public List<EntityDicReaProduct> PdtList { get; set; }
        /// <summary>
        /// 全选生产厂商列表
        /// </summary>
        public List<EntityDicReaProduct> PdtAllList { get; set; }
        /// <summary>
        /// 所选生产厂商ID列表
        /// </summary>
        public String PdtIdString
        {
            get
            {
                string PdtId = string.Empty;
                if (PdtList != null)
                {
                    foreach (var item in PdtList)
                    {
                        PdtId += "'" + item.Rpdt_id + "',";
                    }
                    PdtId = PdtId.Remove(PdtId.LastIndexOf(","));
                }
                return PdtId;
            }
        }

        /// <summary>
        /// 所选生产厂商名称列表
        /// </summary>
        public String PdtNameString
        {
            get
            {
                string PdtName = string.Empty;
                if (PdtList != null)
                {
                    if (PdtAllList != null)
                    {
                        PdtName = "生产厂商:(全部);";
                    }
                    else
                    {
                        PdtName += "生产厂商:(";
                        foreach (var item in PdtList)
                        {
                            PdtName += item.Rpdt_name + ",";
                        }
                        PdtName = PdtName.Remove(PdtName.LastIndexOf(","));
                        PdtName += ");";
                    }
                }
                return PdtName;
            }
        }
        /// <summary>
        /// 所选科室列表
        /// </summary>
        public List<EntityDicPubDept> DeptList { get; set; }

        /// <summary>
        /// 全选选科室列表
        /// </summary>
        public List<EntityDicPubDept> DeptAllList { get; set; }
        /// <summary>
        /// 所选科室ID列表
        /// </summary>
        public String DeptIdString
        {
            get
            {
                string DeptId = string.Empty;
                if (DeptList != null)
                {
                    foreach (var item in DeptList)
                    {
                        DeptId += "'" + item.DeptId + "',";
                    }
                    DeptId = DeptId.Remove(DeptId.LastIndexOf(","));
                }
                return DeptId;
            }
        }

        /// <summary>
        /// 所选科室名称列表
        /// </summary>
        public String DeptNameString
        {
            get
            {
                string DeptName = string.Empty;
                if (DeptList != null)
                {
                    if (DeptAllList != null)
                    {
                        DeptName = "科室:(全部);";
                    }
                    else
                    {
                        DeptName += "科室:(";
                        foreach (var item in DeptList)
                        {
                            DeptName += item.DeptName + ",";
                        }
                        DeptName = DeptName.Remove(DeptName.LastIndexOf(","));
                        DeptName += ");";
                    }
                }
                return DeptName;
            }
        }
        /// <summary>
        /// 所选诊断列表
        /// </summary>
        public List<EntityDicPubIcd> DiagList { get; set; }
        /// <summary>
        /// 全选诊断列表
        /// </summary>
        public List<EntityDicPubIcd> DiagAllList { get; set; }
        /// <summary>
        /// 所选诊断ID列表
        /// </summary>
        public String DiagIdString
        {
            get
            {
                string DiagId = string.Empty;
                if (DiagList != null)
                {
                    foreach (var item in DiagList)
                    {
                        DiagId += "'" + item.IcdName + "',";
                    }
                    DiagId = DiagId.Remove(DiagId.LastIndexOf(","));
                }
                return DiagId;
            }
        }

        /// <summary>
        /// 所选诊断名称列表
        /// </summary>
        public String DiagNameString
        {
            get
            {
                string DiagName = string.Empty;
                if (DiagList != null)
                {
                    if (DiagAllList != null)
                    {
                        DiagName = "诊断:(全部);";
                    }
                    else
                    {
                        DiagName += "诊断:(";
                        foreach (var item in DiagList)
                        {
                            DiagName += item.IcdName + ",";
                        }
                        DiagName = DiagName.Remove(DiagName.LastIndexOf(","));
                        DiagName += ");";
                    }
                }
                return DiagName;
            }
        }

        /// <summary>
        /// 所选标本列表
        /// </summary>
        public List<EntityDicSample> SampleList { get; set; }
        /// <summary>
        /// 全选标本列表
        /// </summary>
        public List<EntityDicSample> SampleAllList { get; set; }
        /// <summary>
        /// 所选标本ID列表
        /// </summary>
        public String SamIdString
        {
            get
            {
                string SamId = string.Empty;
                if (SampleList != null)
                {
                    foreach (var item in SampleList)
                    {
                        SamId += "'" + item.SamId + "',";
                    }
                    SamId = SamId.Remove(SamId.LastIndexOf(","));
                }
                return SamId;
            }
        }

        /// <summary>
        /// 所选标本名称列表
        /// </summary>
        public String SampleNameString
        {
            get
            {
                string SampleName = string.Empty;
                if (SampleList != null)
                {
                    if (SampleAllList != null)
                    {
                        SampleName = "标本:(全部);";
                    }
                    else
                    {
                        SampleName += "标本:(";
                        foreach (var item in SampleList)
                        {
                            SampleName += item.SamName + ",";
                        }
                        SampleName = SampleName.Remove(SampleName.LastIndexOf(","));
                        SampleName += ");";
                    }
                }
                return SampleName;
            }
        }

        /// <summary>
        /// 所选实验组列表
        /// </summary>
        public List<EntityDicPubProfession> PhyList { get; set; }
        /// <summary>
        /// 全选实验组列表
        /// </summary>
        public List<EntityDicPubProfession> PhyAllList { get; set; }
        /// <summary>
        /// 所选实验组ID列表
        /// </summary>
        public String PhyIdString
        {
            get
            {
                string PhyId = string.Empty;
                if (PhyList != null)
                {
                    foreach (var item in PhyList)
                    {
                        PhyId += "'" + item.ProId + "',";
                    }
                    PhyId = PhyId.Remove(PhyId.LastIndexOf(","));
                }
                return PhyId;
            }
        }
        /// <summary>
        /// 所选实验组名称列表
        /// </summary>
        public String PhyNameString
        {
            get
            {
                string PhyName = string.Empty;
                if (PhyList != null)
                {
                    if (PhyAllList != null)
                    {
                        PhyName = "实验组:(全部);";
                    }
                    else
                    {
                        PhyName += "实验组:(";
                        foreach (var item in PhyList)
                        {
                            PhyName += item.ProName + ",";
                        }
                        PhyName = PhyName.Remove(PhyName.LastIndexOf(","));
                        PhyName += ");";
                    }
                }
                return PhyName;
            }
        }

        /// <summary>
        /// 所选专业组列表
        /// </summary>
        public List<EntityDicPubProfession> SepList { get; set; }
        /// <summary>
        /// 全选专业组列表
        /// </summary>
        public List<EntityDicPubProfession> SepAllList { get; set; }
        /// <summary>
        /// 所选专业组ID列表
        /// </summary>
        public String SepIdString
        {
            get
            {
                string SepId = string.Empty;
                if (SepList != null)
                {
                    foreach (var item in SepList)
                    {
                        SepId += "'" + item.ProId + "',";
                    }
                    SepId = SepId.Remove(SepId.LastIndexOf(","));
                }
                return SepId;
            }
        }
        /// <summary>
        /// 所选专业组名称列表
        /// </summary>
        public String SepNameString
        {
            get
            {
                string SepName = string.Empty;
                if (SepList != null)
                {
                    if (SepAllList != null)
                    {
                        SepName = "专业组:(全部);";
                    }
                    else
                    {
                        SepName += "专业组:(";
                        foreach (var item in SepList)
                        {
                            SepName += item.ProName + ",";
                        }
                        SepName = SepName.Remove(SepName.LastIndexOf(","));
                        SepName += ");";
                    }
                }
                return SepName;
            }
        }

        /// <summary>
        /// 所选检验者列表
        /// </summary>
        public List<EntitySysUser> ChkDocList { get; set; }
        /// <summary>
        /// 全选检验者列表
        /// </summary>
        public List<EntitySysUser> ChkDocAllList { get; set; }
        /// <summary>
        /// 所选检验者ID列表
        /// </summary>
        public String ChkDocIdString
        {
            get
            {
                string ChkDocId = string.Empty;
                if (ChkDocList != null)
                {
                    foreach (var item in ChkDocList)
                    {
                        ChkDocId += "'" + item.UserId + "',";
                    }
                    ChkDocId = ChkDocId.Remove(ChkDocId.LastIndexOf(","));
                }
                return ChkDocId;
            }
        }
        /// <summary>
        /// 所选检验者名称列表
        /// </summary>
        public String ChkDocNameString
        {
            get
            {
                string ChkDocName = string.Empty;
                if (ChkDocList != null)
                {
                    if (ChkDocAllList != null)
                    {
                        ChkDocName = "检验者:(全部);";
                    }
                    else
                    {
                        ChkDocName += "检验者:(";
                        foreach (var item in ChkDocList)
                        {
                            ChkDocName += item.UserName + ",";
                        }
                        ChkDocName = ChkDocName.Remove(ChkDocName.LastIndexOf(","));
                        ChkDocName += ");";
                    }
                }
                return ChkDocName;
            }
        }
        /// <summary>
        /// 所选报告者列表
        /// </summary>
        public List<EntitySysUser> AuditList { get; set; }
        /// <summary>
        /// 全选报告者列表
        /// </summary>
        public List<EntitySysUser> AuditAllList { get; set; }
        /// <summary>
        /// 所选报告者ID列表
        /// </summary>
        public String AuditIdString
        {
            get
            {
                string AuditId = string.Empty;
                if (AuditList != null)
                {
                    foreach (var item in AuditList)
                    {
                        AuditId += "'" + item.UserLoginid + "',";
                    }
                    AuditId = AuditId.Remove(AuditId.LastIndexOf(","));
                }
                return AuditId;
            }
        }

        /// <summary>
        /// 所选报告者名称列表
        /// </summary>
        public String AuditNameString
        {
            get
            {
                string AuditName = string.Empty;
                if (AuditList != null)
                {
                    if (AuditAllList != null)
                    {
                        AuditName = "报告者:(全部);";
                    }
                    else
                    {
                        AuditName += "报告者:(";
                        foreach (var item in AuditList)
                        {
                            AuditName += item.UserName + ",";
                        }
                        AuditName = AuditName.Remove(AuditName.LastIndexOf(","));
                        AuditName += ");";
                    }
                }
                return AuditName;
            }
        }

        /// <summary>
        /// 所选申请者列表
        /// </summary>
        public List<EntityDicDoctor> SendDocList { get; set; }
        /// <summary>
        /// 全选申请者列表
        /// </summary>
        public List<EntityDicDoctor> SendDocAllList { get; set; }
        /// <summary>
        /// 所选申请者ID列表
        /// </summary>
        public String SendDocIdString
        {
            get
            {
                string SendDocId = string.Empty;
                if (SendDocList != null)
                {
                    foreach (var item in SendDocList)
                    {
                        SendDocId += "'" + item.DoctorId + "',";
                    }
                    SendDocId = SendDocId.Remove(SendDocId.LastIndexOf(","));
                }
                return SendDocId;
            }
        }
        /// <summary>
        /// 所选申请者名称列表
        /// </summary>
        public String SendDocNameString
        {
            get
            {
                string SendDocName = string.Empty;
                if (SendDocList != null)
                {
                    if (SendDocAllList != null)
                    {
                        SendDocName = "申请者:(全部);";
                    }
                    else
                    {
                        SendDocName += "申请者:(";
                        foreach (var item in SendDocList)
                        {
                            SendDocName += item.DoctorName + ",";
                        }
                        SendDocName = SendDocName.Remove(SendDocName.LastIndexOf(","));
                        SendDocName += ");";
                    }
                }
                return SendDocName;
            }
        }
        /// <summary>
        /// 所选组合列表
        /// </summary>
        public List<EntityDicCombine> CombineList { get; set; }
        /// <summary>
        /// 全选组合列表
        /// </summary>
        public List<EntityDicCombine> CombineAllList { get; set; }
        /// <summary>
        /// 所选组合ID列表
        /// </summary>
        public String ComIdString
        {
            get
            {
                string ComId = string.Empty;
                if (CombineList != null)
                {
                    foreach (var item in CombineList)
                    {
                        ComId += "'" + item.ComId + "',";
                    }
                    ComId = ComId.Remove(ComId.LastIndexOf(","));
                }
                return ComId;
            }
        }
        /// <summary>
        /// 所选组合名称列表
        /// </summary>
        public String ComNameString
        {
            get
            {
                string ComName = string.Empty;
                if (CombineList != null)
                {
                    if (CombineAllList != null)
                    {
                        ComName = "组合:(全部);";
                    }
                    else
                    {
                        ComName += "组合:(";
                        foreach (var item in CombineList)
                        {
                            ComName += item.ComName + ",";
                        }
                        ComName = ComName.Remove(ComName.LastIndexOf(","));
                        ComName += ");";
                    }
                }
                return ComName;
            }
        }

        /// <summary>
        /// 所选病人来源列表
        /// </summary>
        public List<EntityDicOrigin> OriginList { get; set; }
        /// <summary>
        /// 全选病人来源列表
        /// </summary>
        public List<EntityDicOrigin> OriginAllList { get; set; }
        /// <summary>
        /// 所选病人来源ID列表
        /// </summary>
        public String OriIdString
        {
            get
            {
                string OriId = string.Empty;
                if (OriginList != null)
                {
                    foreach (var item in OriginList)
                    {
                        OriId += "'" + item.SrcId + "',";
                    }
                    OriId = OriId.Remove(OriId.LastIndexOf(","));
                }
                return OriId;
            }
        }
        /// <summary>
        /// 所选病人来源名称列表
        /// </summary>
        public String OriNameString
        {
            get
            {
                string OriName = string.Empty;
                if (OriginList != null)
                {
                    if (OriginAllList != null)
                    {
                        OriName = "病人来源:(全部);";
                    }
                    else
                    {
                        OriName += "病人来源:(";
                        foreach (var item in OriginList)
                        {
                            OriName += item.SrcName + ",";
                        }
                        OriName = OriName.Remove(OriName.LastIndexOf(","));
                        OriName += ");";
                    }
                }
                return OriName;
            }
        }
        /// <summary>
        /// 所选急查列表
        /// </summary>
        public List<EntityMark> MarkList { get; set; }
        /// <summary>
        /// 所选急查ID列表
        /// </summary>
        public String MarkIdString
        {
            get
            {
                string MarkId = string.Empty;
                if (MarkList != null)
                {
                    foreach (var item in MarkList)
                    {
                        MarkId += "'" + item.SpId + "',";
                    }
                    MarkId = MarkId.Remove(MarkId.LastIndexOf(","));
                }
                return MarkId;
            }
        }
        /// <summary>
        /// 所选急查名称
        /// </summary>
        public String MarkNameString
        {
            get
            {
                string OriName = string.Empty;
                if (MarkList != null)
                {
                    OriName += "急查:(";
                    foreach (var item in MarkList)
                    {
                        OriName += item.MarkName + ",";
                    }
                    OriName = OriName.Remove(OriName.LastIndexOf(","));
                    OriName += ");";
                }
                return OriName;
            }
        }

        /// <summary>
        /// 所选性别列表
        /// </summary>
        public List<EntitySex> SexList { get; set; }
        /// <summary>
        /// 所选性别ID列表
        /// </summary>
        public String SexIdString
        {
            get
            {
                string SexId = string.Empty;
                if (SexList != null)
                {
                    foreach (var item in SexList)
                    {
                        SexId += "'" + item.SpId + "',";
                    }
                    SexId = SexId.Remove(SexId.LastIndexOf(","));
                }
                return SexId;
            }
        }
        /// <summary>
        /// 所选性别列表
        /// </summary>
        public String SexNameString
        {
            get
            {
                string SexName = string.Empty;
                if (SexList != null)
                {
                    SexName += "性别:(";
                    foreach (var item in SexList)
                    {
                        SexName += item.SexName + ",";
                    }
                    SexName = SexName.Remove(SexName.LastIndexOf(","));
                    SexName += ");";
                }
                return SexName;
            }
        }

        /// <summary>
        /// 所选结果提示列表
        /// </summary>
        public List<EntityDicResultTips> ResultTipsList { get; set; }
        /// <summary>
        /// 全选结果提示列表
        /// </summary>
        public List<EntityDicResultTips> ResultTipsAllList { get; set; }
        /// <summary>
        /// 所选结果提示ID列表
        /// </summary>
        public String TipIdString
        {
            get
            {
                string TipId = string.Empty;
                if (ResultTipsList != null)
                {
                    foreach (var item in ResultTipsList)
                    {
                        TipId += "'" + item.TipId + "',";
                    }
                    TipId = TipId.Remove(TipId.LastIndexOf(","));
                }
                return TipId;
            }
        }

        /// <summary>
        /// 所选结果提示名称列表
        /// </summary>
        public String TipNameString
        {
            get
            {
                string TipName = string.Empty;
                if (ResultTipsList != null)
                {
                    if (ResultTipsAllList != null)
                    {
                        TipName = "结果提示:(全部);";
                    }
                    else
                    {
                        TipName += "结果提示:(";
                        foreach (var item in ResultTipsList)
                        {
                            TipName += item.TipValue + ",";
                        }
                        TipName = TipName.Remove(TipName.LastIndexOf(","));
                        TipName += ");";
                    }
                }
                return TipName;
            }
        }

        /// <summary>
        /// 所选抗生素列表
        /// </summary>
        public List<EntityDicMicAntibio> AntibioList { get; set; }
        /// <summary>
        /// 全选抗生素列表
        /// </summary>
        public List<EntityDicMicAntibio> AntibioAllList { get; set; }
        /// <summary>
        /// 所选抗生素ID列表
        /// </summary>
        public String AntiIdString
        {
            get
            {
                string AntiId = string.Empty;
                if (AntibioList != null)
                {
                    foreach (var item in AntibioList)
                    {
                        AntiId += "'" + item.AntId + "',";
                    }
                    AntiId = AntiId.Remove(AntiId.LastIndexOf(","));
                }
                return AntiId;
            }
        }
        /// <summary>
        /// 所选抗生素名称列表
        /// </summary>
        public String AntiNameString
        {
            get
            {
                string AntiName = string.Empty;
                if (AntibioList != null)
                {
                    if (AntibioAllList != null)
                    {
                        AntiName = "抗生素:(全部);";
                    }
                    else
                    {
                        AntiName += "抗生素:(";
                        foreach (var item in AntibioList)
                        {
                            AntiName += item.AntCname + ",";
                        }
                        AntiName = AntiName.Remove(AntiName.LastIndexOf(","));
                        AntiName += ");";
                    }
                }
                return AntiName;
            }
        }

        /// <summary>
        /// 所选细菌列表
        /// </summary>
        public List<EntityDicMicBacteria> BacteriaList { get; set; }
        /// <summary>
        /// 全选细菌列表
        /// </summary>
        public List<EntityDicMicBacteria> BacteriaAllList { get; set; }
        /// <summary>
        /// 所选细菌ID列表
        /// </summary>
        public String BacIdString
        {
            get
            {
                string BacId = string.Empty;
                if (BacteriaList != null)
                {
                    foreach (var item in BacteriaList)
                    {
                        BacId += "'" + item.BacId + "',";
                    }
                    BacId = BacId.Remove(BacId.LastIndexOf(","));
                }
                return BacId;
            }
        }
        /// <summary>
        /// 所选细菌名称列表
        /// </summary>
        public String BacNameString
        {
            get
            {
                string BacName = string.Empty;
                if (BacteriaList != null)
                {
                    if (BacteriaAllList != null)
                    {
                        BacName = "细菌名称:(全部);";
                    }
                    else
                    {
                        BacName += "细菌名称:(";
                        foreach (var item in BacteriaList)
                        {
                            BacName += item.BacCname + ",";
                        }
                        BacName = BacName.Remove(BacName.LastIndexOf(","));
                        BacName += ");";
                    }
                }
                return BacName;
            }
        }

        /// <summary>
        /// 所选细菌菌类列表
        /// </summary>
        public List<EntityDicMicBacttype> BacttypeList { get; set; }
        /// <summary>
        /// 全选细菌菌类列表
        /// </summary>
        public List<EntityDicMicBacttype> BacttypeAllList { get; set; }
        /// <summary>
        /// 所选细菌菌类ID列表
        /// </summary>
        public String BactypeIdString
        {
            get
            {
                string BactypeId = string.Empty;
                if (BacttypeList != null)
                {
                    foreach (var item in BacttypeList)
                    {
                        BactypeId += "'" + item.BtypeId + "',";
                    }
                    BactypeId = BactypeId.Remove(BactypeId.LastIndexOf(","));
                }
                return BactypeId;
            }
        }
        /// <summary>
        /// 所选细菌菌类名称列表
        /// </summary>
        public String BactypeNameString
        {
            get
            {
                string BactypeName = string.Empty;
                if (BacttypeList != null)
                {
                    if (BacttypeAllList != null)
                    {
                        BactypeName = "菌类名称:(全部);";
                    }
                    else
                    {
                        BactypeName += "菌类名称:(";
                        foreach (var item in BacttypeList)
                        {
                            BactypeName += item.BtypeCname + ",";
                        }
                        BactypeName = BactypeName.Remove(BactypeName.LastIndexOf(","));
                        BactypeName += ");";
                    }
                }
                return BactypeName;
            }
        }

        /// <summary>
        /// 所选标本备注列表
        /// </summary>
        public List<EntityDicSampRemark> SampRemarkList { get; set; }
        /// <summary>
        /// 全选标本备注列表
        /// </summary>
        public List<EntityDicSampRemark> SampRemarkAllList { get; set; }
        /// <summary>
        /// 所选标本备注ID列表
        /// </summary>
        public String SampRemarkIdString
        {
            get
            {
                string remId = string.Empty;
                if (SampRemarkList != null)
                {
                    foreach (var item in SampRemarkList)
                    {
                        remId += "'" + item.RemId + "',";
                    }
                    remId = remId.Remove(remId.LastIndexOf(","));
                }
                return remId;
            }
        }
        /// <summary>
        /// 所选标本备注名称列表
        /// </summary>
        public String SampRemNameString
        {
            get
            {
                string RemName = string.Empty;
                if (SampRemarkList != null)
                {
                    if (SampRemarkAllList != null)
                    {
                        RemName = "标本备注:(全部);";
                    }
                    else
                    {
                        RemName += "标本备注:(";
                        foreach (var item in SampRemarkList)
                        {
                            RemName += item.RemContent + ",";
                        }
                        RemName = RemName.Remove(RemName.LastIndexOf(","));
                        RemName += ");";
                    }
                }
                return RemName;
            }
        }


        /// <summary>
        /// 所选标本状态列表
        /// </summary>
        public List<EntityDicSState> SampStateList { get; set; }
        /// <summary>
        /// 全选标本状态列表
        /// </summary>
        public List<EntityDicSState> SampStateAllList { get; set; }
        /// <summary>
        /// 所选标本状态ID列表
        /// </summary>
        public String SampStateIdString
        {
            get
            {
                string statuId = string.Empty;
                if (SampStateList != null)
                {
                    foreach (var item in SampStateList)
                    {
                        statuId += "'" + item.StauId + "',";
                    }
                    statuId = statuId.Remove(statuId.LastIndexOf(","));
                }
                return statuId;
            }
        }
        /// <summary>
        /// 所选标本状态名称列表
        /// </summary>
        public String SampStateNameString
        {
            get
            {
                string stateName = string.Empty;
                if (SampStateList != null)
                {
                    if (SampStateAllList != null)
                    {
                        stateName = "标本状态:(全部);";
                    }
                    else
                    {
                        stateName += "标本状态:(";
                        foreach (var item in SampStateList)
                        {
                            stateName += item.StauName + ",";
                        }
                        stateName = stateName.Remove(stateName.LastIndexOf(","));
                        stateName += ");";
                    }
                }
                return stateName;
            }
        }

        #region 试剂管理
        /// <summary>
        ///试剂操作类型
        /// </summary>   
        public string ReagentType { get; set; }

        public bool WithoutTime { get; set; }
        #endregion
    }
}
