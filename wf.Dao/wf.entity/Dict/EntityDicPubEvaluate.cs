using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    /// <summary>
    /// 描述评价字典 
    /// 旧表名:Dic_pub_evaluate 新表名:Dict_evaluate
    /// </summary>
    [Serializable()]
    public class EntityDicPubEvaluate : EntityBase
    {
        public EntityDicPubEvaluate()
        {
            Checked = false;
        }

        /// <summary>
        ///编码
        /// </summary>                       
        public String EvaId { get; set; }

        /// <summary>
        ///输入码
        /// </summary>                       
        public String EvaCcode { get; set; }

        /// <summary>
        ///描述内容
        /// </summary>                       
        public String EvaContent { get; set; }

        /// <summary>
        ///组别
        /// </summary>                       
        public String EvaProid { get; set; }

        /// <summary>
        /// 类别 0-描述(报告评价) 1-处理意见 2-细菌备注 3-描述报告 4-菌落计数 5-质控失控原因
        /// 6-质控解决措施 7-危急类型 8-标本超时签收理由 9-标本超时拒绝理由 10-自编危急范文
        /// 11-细菌临时报告结论 12-危急值信息原因 13-危急值反馈信息 14-交接班字典信息 
        /// 15-危急值记录信息原因 16-临床危急值备注 17-处理结果 18-审核结果 19-院感监测对象
        /// 20-复查意见 21-质控结果分析与处理 22-骨髓象描述  23-保养仪器描述
        /// </summary>                       
        public String EvaFlag { get; set; }

        /// <summary>
        ///排序
        /// </summary>                       
        public Int32 EvaSortNo { get; set; }

        /// <summary>
        ///拥有者(用户ID)
        /// </summary>                       
        public String EvaUserId { get; set; }

        /// <summary>
        ///仪器ID
        /// </summary>                       
        public String EvaItrId { get; set; }

        /// <summary>
        ///标题
        /// </summary>                       
        public String EvaTitle { get; set; }

        /// <summary>
        ///标本ID
        /// </summary>                       
        public String EvaSamId { get; set; }

        #region 附加字段
        /// <summary>
        /// 标本
        /// </summary>
        public String EvaSamName { get; set; }

        /// <summary>
        ///模板类别
        /// </summary>                       
        public String EvaFlagText { get; set; }

        /// <summary>
        ///类别名称
        /// </summary>                       
        public String EvaFlagName {
            get
            {
                if (EvaFlag == "0")
                    return "报告评价";
                else if (EvaFlag == "1")
                    return "处理意见";
                else if (EvaFlag == "2")
                    return "细菌备注";
                else if (EvaFlag == "3")
                    return "描述报告";
                else if (EvaFlag == "4")
                    return "菌落计数";
                else if (EvaFlag == "5")
                    return "质控失控原因";
                else if (EvaFlag == "6")
                    return "质控解决措施";
                else if (EvaFlag == "7")
                    return "危急类型";
                else if (EvaFlag == "8")
                    return "标本超时签收理由";
                else if (EvaFlag == "9")
                    return "标本超时拒绝理由";
                else if (EvaFlag == "10")
                    return "自编危急范文";
                else if (EvaFlag == "11")
                    return "细菌临时报告结论";
                else if (EvaFlag == "12")
                    return "危急值信息原因";
                else if (EvaFlag == "13")
                    return "危急值反馈信息";
                else if (EvaFlag == "14")
                    return "交接班字典信息";
                else if (EvaFlag == "15")
                    return "危急值记录信息原因";
                else if (EvaFlag == "16")
                    return "临床危急值备注";
                else if (EvaFlag == "17")
                    return "处理结果";
                else if (EvaFlag == "18")
                    return "审核结果";
                else if (EvaFlag == "19")
                    return "院感监测对象";
                else if (EvaFlag == "20")
                    return "复查意见";
                else if (EvaFlag == "21")
                    return "质控结果分析与处理";
                else if (EvaFlag == "22")
                    return "骨髓描述";
                else if (EvaFlag == "23")
                    return "保养仪器描述";
                else return string.Empty;   
            }

        }

        public String SpId
        {
            get
            {
                return EvaContent;
            }
        }
        /// <summary>
        /// 是否选中
        /// </summary>
        public Boolean Checked { get; set; } 
     
        #endregion
    }
}
