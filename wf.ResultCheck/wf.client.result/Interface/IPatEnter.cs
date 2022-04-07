using System;
using System.Collections.Generic;
using dcl.client.frame.runsetting;

using dcl.entity;

namespace dcl.client.result.Interface
{
    public interface IPatEnter
    {
        /// <summary>
        /// 工具条按键配置
        /// </summary>
        string[] ToolBarStyle { get; }

        /// <summary>
        /// 应用式样
        /// </summary>
        /// <param name="UserCustomSetting"></param>
        void ApplyCustomSetting(PatInputRuntimeSetting UserCustomSetting);

        /// <summary>
        /// 仪器数据类型
        /// </summary>
        string ItrDataType { get; }

        /// <summary>
        /// 组合编辑器
        /// </summary>
        ICombineEditor CombineEditor { get; }

        /// <summary>
        /// 获取病人资料（病人基本信息、检验组合、结果）
        /// </summary>
        /// <param name="patID"></param>
        /// <returns>病人基本资料</returns>
        void LoadPatientData(string patID, ref EntityPidReportMain patient, ref List<EntityPidReportDetail> listPatCombine);

        /// <summary>
        /// 物理组改变
        /// </summary>
        /// <param name="typeID"></param>
        void TypeChanged(string typeID);


        void PatDateChanged(DateTime dt);

        /// <summary>
        /// 仪器改变
        /// </summary>
        /// <param name="itr_id"></param>
        void InstructmentChanged(string itr_id);

        /// <summary>
        /// 重置数据/界面数据
        /// </summary>
        void Reset();

        void ResReset();

        /// <summary>
        /// 新增
        /// </summary>
        void AddNew();

        /// <summary>
        /// 设置仪器默认检验组合
        /// </summary>
        /// <param name="itr_id"></param>
        void SetItrDefaultCombine(string itr_id);

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="dsData"></param>
        /// <returns>返回当前保存后的病人ID</returns>
     //   string Save(DataSet dataSet);

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="dsData"></param>
        /// <returns>返回当前保存后的病人ID</returns>
        string Save(EntityPidReportMain patient);

        /// <summary>
        /// 性别改变
        /// </summary>
        /// <param name="pat_sex"></param>
        void SexChanged(string pat_sex);

        /// <summary>
        /// 病人ID(界面录入的ID)改变
        /// </summary>
        void PatIDChanged(string PatIDType, string PatID);

        /// <summary>
        /// 病人ID类型改变
        /// </summary>
        void PatIDTypeChanged(string PatIDType);

        void DepChanged(string depid);
       

        ///// <summary>
        ///// 删除
        ///// </summary>
        ///// <param name="pat_id"></param>
        //bool Delete(string pat_id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="listPat"></param>
        List<EntityOperationResult> DeleteBatch(EntityRemoteCallClientInfo caller, List<string> listPat);

        /// <summary>
        /// 显示结果视窗(普通录入/细菌报告共有)
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itr_id"></param>
        void ResultView(DateTime date, string itr_id);

        /// <summary>
        /// 显示质控图
        /// </summary>
        /// <param name="date"></param>
        /// <param name="itr_id"></param>
        void QualityImageView(DateTime date, string itr_id, string itr_mid);

        ///// <summary>
        ///// 打印
        ///// </summary>
        ///// <param name="pat_id"></param>
        //void PatPrint(string pat_id);

        ///// <summary>
        ///// 打印预览
        ///// </summary>
        ///// <param name="pat_id"></param>
        //void PatPrintPreview(string pat_id);

        /// <summary>
        /// 年龄改变
        /// </summary>
        /// <param name="ageMinute"></param>
        void PatAgeChanged(int ageMinute);


        /// <summary>
        /// 样本改变
        /// </summary>
        /// <param name="sam_id"></param>
        void SampleChanged(string sam_id);


        /// <summary>
        /// 临床诊断改变
        /// </summary>
        /// <param name="sam_rem"></param>
        void PatDiagChanged(string patDiag);

        /// <summary>
        /// 设置结果列焦点
        /// </summary>
        void SetColumnFocus();

        #region  2010-6-9
        /// <summary>
        /// 是否没有手工项目结果
        /// </summary>
        /// <returns></returns>
        bool HasNotManualResult();
        /// <summary>
        /// 当样本号改变时是否检查
        /// </summary>
        bool ShouldCheckWhenPatSidLeave { get; }

        /// <summary>
        /// 当样本号改变时
        /// </summary>
        /// <param name="p"></param>
        void PatSIDChanged(string pat_id, bool merge);

        /// <summary>
        /// 审核前检查当前结果
        /// </summary>
        /// <param name="pat_id"></param>
        /// <param name="isAudit"></param>
        bool CheckResultBeforeAction(string pat_id,bool isAudit);
        #endregion


    }
}
