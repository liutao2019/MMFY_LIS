using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevExpress.XtraCharts;
using dcl.entity;

namespace dcl.client.qc
{
    public class QCValueAudit
    {
        public QCValueAudit()
        {
            WestgardType = false;
            DateFile = false;
        }

        public List<EntityObrQcResult> QCValue { get; set; }

        public List<EntityDicQcRule> QCRule { get; set; }

        public Boolean WestgardType { get; set; }

        public QCImageType ImageType { get; set; }

        public DateTime DtBeginTime { get; set; }

        public DateTime DtEndTime { get; set; }

        public Boolean DateFile { get; set; }

        private List<EntityObrQcResult> dtCalculation = new List<EntityObrQcResult>();

        private String ruleName = String.Empty;

        public List<XYDiagramPane> DiagramPane { get; set; }

        public ChartControl QCControl { get; set; }

        /// <summary>
        /// 审核数据
        /// </summary>
        /// <param name="drQcValue"></param>
        /// <param name="qcr_key"></param>
        public List<EntityObrQcResult> QcAudit()
        {
            dtCalculation = new List<EntityObrQcResult>();//审核后放入表中以统计平均
            for (int i = 0; i < QCValue.Count; i++)
            {
                if (QCValue[i].QresAuditFlag == 0 && //未审核
                    QCValue[i].NSD != null &&   //NSD的值
                    QCValue[i].QresDisplay == 0 &&  //有效数据
                    (QCValue[i].QresRunawayFlag != "2"))//失控数据
                {

                    double sd = Convert.ToDouble(QCValue[i].NSD);
                    for (int j = 0; j < QCRule.Count; j++)
                    {
                        EntityDicQcRule drQc_rule = QCRule[j];

                        if (WestgardType && j > 0 && drQc_rule.RulSdAmount > 0)//启用westgard 必须先违背警告才判定其他规则
                        {
                            if (QCValue[i].QresRunawayFlag != "1")
                                continue;
                        }

                        //单水平规则审核
                        if (Convert.ToInt32(drQc_rule.RulMAmount) == 1 && drQc_rule.RulIsMoreLevel != 1)
                        {
                            if (SingleLevelAudit(i, j) == QCValueType.OutOfControl)
                                break;
                            else
                                continue;
                        }
                        //R-4S规则审核
                        else if (drQc_rule.RulName.ToString().Trim() == "R-4S")
                        {
                            if (R4SAudit(i) == QCValueType.OutOfControl)
                                break;
                        }
                        //多水平规则审核
                        else
                            MultiLevelAudit(i, j);
                    }
                }

                if (DateFile)
                {
                    DateTime dtQcValueTime = Convert.ToDateTime(QCValue[i].QresDate);
                    if (dtQcValueTime > DtBeginTime && dtQcValueTime < DtEndTime)
                        dtCalculation.Add(QCValue[i]);
                }
                else
                    dtCalculation.Add(QCValue[i]);
            }
            return dtCalculation;
        }

        /// <summary>
        /// 半定量审核
        /// </summary>
        public List<EntityObrQcResult> SemiQuantitativeAudit()
        {
            List<EntityObrQcResult> dtCalculation = new List<EntityObrQcResult>();
            foreach (EntityObrQcResult drQcValue in QCValue)
            {
                if (drQcValue.MatMaxValue != null &&
                    drQcValue.MatMinValue != null)
                {
                    double maxValue = Convert.ToDouble(drQcValue.MatMaxValue);
                    double minValue = Convert.ToDouble(drQcValue.MatMinValue);
                    double value = 0;
                    if (double.TryParse(drQcValue.FinalValue.ToString(), out value))
                    {
                        if (value > maxValue)
                        {
                            drQcValue.QresRunawayRule = "Max↑";
                            drQcValue.QresRunawayFlag = "2";
                        }
                        if (value < minValue)
                        {
                            drQcValue.QresRunawayRule = "Min↓";
                            drQcValue.QresRunawayFlag = "2";
                        }
                        if (DateFile)
                        {
                            DateTime dtQcValueTime = Convert.ToDateTime(drQcValue.QresDate);
                            if (dtQcValueTime > DtBeginTime && dtQcValueTime < DtEndTime)
                                dtCalculation.Add(drQcValue);
                        }
                        else
                            dtCalculation.Add(drQcValue);
                    }
                }
            }
            return dtCalculation;

        }

        /// <summary>
        /// R-4S规则判断
        /// </summary>
        /// <param name="RowIndex">质控数据所在索引</param>
        /// <returns></returns>
        private QCValueType R4SAudit(int RowIndex)
        {
            if (RowIndex > 0 && QCValue[RowIndex].QresItmId.ToString() != QCValue[RowIndex - 1].QresItmId.ToString())
                return QCValueType.Normal;
            if (RowIndex - 1 >= 0 && QCValue[RowIndex].NSD != null && QCValue[RowIndex - 1].NSD != null
                && Math.Abs(Convert.ToDouble(QCValue[RowIndex].NSD) - Convert.ToDouble(QCValue[RowIndex - 1].NSD)) > 4)
            {
                QCValue[RowIndex].QresRunawayFlag = "2";
                QCValue[RowIndex].QresRunawayRule = ruleName + "R-4S";
                if (dtCalculation[dtCalculation.Count - 1].QresRunawayFlag.ToString() != "2")
                {
                    dtCalculation[dtCalculation.Count - 1].QresRunawayFlag = "2";
                    dtCalculation[dtCalculation.Count - 1].QresRunawayRule = ruleName + "R-4S";
                }
                return QCValueType.OutOfControl;
            }
            return QCValueType.Normal;
        }

        /// <summary>
        /// 单点质控审核
        /// </summary>
        /// <param name="RowIndex">质控数据所在索引</param>
        /// <param name="RuleIndex">质控规则所在索引</param>
        /// <returns></returns>
        private QCValueType SingleLevelAudit(int RowIndex, int RuleIndex)
        {
            if (QCRule.Count > 0
                && QCValue.Count > 0
                && !string.IsNullOrEmpty(QCRule[RuleIndex].MatSn)
                && !string.IsNullOrEmpty(QCValue[RowIndex].QresMatDetId))
            {
                string qcr_key_rule = QCRule[RuleIndex].MatSn;
                string qcr_key_value = QCValue[RowIndex].QresMatDetId;
                if (qcr_key_rule != qcr_key_value)//检查此质控水平是否使用了当前质控规则
                {
                    return QCValueType.Normal;
                }
            }

            if (Math.Abs(Convert.ToDouble(QCValue[RowIndex].NSD)) > Convert.ToDouble(QCRule[RuleIndex].RulSdAmount))
            {

                if (QCRule[RuleIndex].RulType.ToString() == "失控")
                {
                    QCValue[RowIndex].QresRunawayFlag = "2";
                    QCValue[RowIndex].QresRunawayRule = ruleName + QCRule[RuleIndex].RulName.ToString();
                    return QCValueType.OutOfControl;
                }
                else
                {
                    QCValue[RowIndex].QresRunawayFlag = "1";
                    QCValue[RowIndex].QresRunawayRule = ruleName + QCRule[RuleIndex].RulName.ToString();
                    return QCValueType.Warning;
                }
            }
            return QCValueType.Normal;
        }

        /// <summary>
        /// 多点质控审核
        /// </summary>
        /// <param name="RowIndex">质控数据所在索引</param>
        /// <param name="RuleIndex">质控规则所在索引</param>
        /// <returns></returns>
        private QCValueType MultiLevelAudit(int RowIndex, int RuleIndex)
        {
            if (RowIndex > 0 && QCValue[RowIndex].QresItmId.ToString() != QCValue[RowIndex - 1].QresItmId.ToString())
                return QCValueType.Normal;

            int count = 0;
            int rulseNum = QCRule[RuleIndex].RulNAmount;
            double sd = QCValue[RowIndex].NSD;
            double ruSd = Convert.ToDouble(QCRule[RuleIndex].RulSdAmount);
            string strIsDesc = string.Empty;
            if (Math.Abs(sd) > ruSd)
            {
                count++;
                double thisSdValue = QCValue[RowIndex].NSD;
                for (int k = RowIndex - 1; k >= 0 && k > RowIndex - rulseNum; k--)
                {
                    try
                    {
                        double sdAbsolute = Convert.ToDouble(QCValue[k].NSD);

                        if (QCRule[RuleIndex].RulIsDesc.ToString() == "1")//只判断渐升渐降
                        {
                            if (strIsDesc == string.Empty)
                            {
                                strIsDesc = (thisSdValue - sdAbsolute) > 0 ? "+" : "-";
                                count++;
                                thisSdValue = sdAbsolute;
                            }
                            else
                            {
                                if ((strIsDesc == "+" && (thisSdValue - sdAbsolute) > 0) || (strIsDesc == "-" && (thisSdValue - sdAbsolute) < 0))
                                {
                                    count++;
                                    thisSdValue = sdAbsolute;
                                }
                                else
                                    break;
                            }
                        }
                        else
                        {
                            if (Math.Abs(sdAbsolute) > 0)//上一个值不为均值
                            {
                                if (Convert.ToInt32(QCRule[RuleIndex].RulLevelType) == 1)//判断是否需要同方向
                                {
                                    if ((sdAbsolute > 0 && sd > 0) || (sdAbsolute < 0 && sd < 0))//判断方向
                                    {
                                        if (Math.Abs(sdAbsolute) > ruSd)
                                            count++;
                                    }
                                    else
                                        break;
                                }
                                else
                                {
                                    if (Math.Abs(sdAbsolute) > ruSd)
                                        count++;
                                }
                            }
                            else
                                break;
                        }
                    }
                    catch (Exception) { }
                }

            }
            int m_sum = Convert.ToInt32(QCRule[RuleIndex].RulMAmount);
            if (count >= m_sum)//如果满足失控条件
            {
                if (QCRule[RuleIndex].RulType.ToString() == "失控")
                {
                    QCValue[RowIndex].QresRunawayFlag = "2";
                    QCValue[RowIndex].QresRunawayRule = QCRule[RuleIndex].RulName.ToString();
                    if (QCRule[RuleIndex].RulIsMoreLevel.ToString() == "1" && m_sum <= 2 && dtCalculation[dtCalculation.Count - 1].QresRunawayFlag.ToString() != "2")
                    {
                        dtCalculation[dtCalculation.Count - 1].QresRunawayFlag = "2";
                        dtCalculation[dtCalculation.Count - 1].QresRunawayRule = ruleName + QCRule[RuleIndex].RulName.ToString();
                    }
                    return QCValueType.OutOfControl;
                }
                else
                {
                    QCValue[RowIndex].QresRunawayFlag = "1";
                    QCValue[RowIndex].QresRunawayRule = ruleName + QCRule[RuleIndex].RulName.ToString();
                    return QCValueType.Warning;
                }
            }
            return QCValueType.Normal;
        }
    }

    public enum QCImageType
    {
        LJ,
        Monica,
        SemiQuantitative
    }

    public enum QCValueType
    {
        Normal = 0,
        Warning = 1,
        OutOfControl = 2
    }
}
