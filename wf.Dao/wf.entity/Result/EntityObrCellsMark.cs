using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    [Serializable]
    public class EntityObrCellsMark
    {
        /// <summary>
        /// 细胞控件名称
        /// </summary>
        public string CellName { get; set; }
        /// <summary>
        /// 细胞名称
        /// </summary>
        public string CellActualName { get; set; }
        /// <summary>
        /// 编码ID
        /// </summary>
        public string ItemId { get; set; }
        /// <summary>
        /// 矩形
        /// </summary>
        public Rectangle rectangle { get; set; }

        /// <summary>
        ///骨髓记录
        /// </summary>   
        public string ObrBoneValue { get; set; }

        /// <summary>
        ///血片记录
        /// </summary>   
        public string ObrBldValue { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Counts { get; set; }

        /// <summary>
        /// 用户鼠标点击的点
        /// </summary>
        public Point markPoint { get; set; }
        /// <summary>
        /// 标记起点
        /// </summary>
        public Point startPoint { get; set; }
        /// <summary>
        /// 标记终点
        /// </summary>
        public Point endPoint { get; set; }
        /// <summary>
        /// 是否为用户手动勾选
        /// </summary>
        public bool IsUserDefine { get; set; }
        /// <summary>
        /// 轮廓区域面积
        /// </summary>
        public double Area { get; set; }

        /// <summary>
        ///数值是否需要进行转换计算
        /// </summary>   
        public bool IsValueNeedCalculate { get; set; }
    }
}
