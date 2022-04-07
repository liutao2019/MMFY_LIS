using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace dcl.client.elisa
{
    /// <summary>
    /// 画酶标板
    /// </summary>
    public class clsDrawSamleResult
    {
        #region 全局变量
        /// <summary>
        /// 初始化坐标(为左上角坐标为准)
        /// </summary>
        public PointF poiFInt { get; set; }
        /// <summary>
        /// 画线的长度
        /// </summary>
        public float flaHeight { get; set; }
        /// <summary>
        /// 全局画板
        /// </summary>
        public Graphics graphicsAll { get; set; }
        /// <summary>
        /// 全局画刷
        /// </summary>
        public Pen penAll { get; set; }
        /// <summary>
        /// 全局字体
        /// </summary>
        public Font fontAll { get; set; }

        /// <summary>
        /// 酶标板数据
        /// </summary>
        public SampleControl SampleControl1 { get; set; }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public clsDrawSamleResult(SampleControl p_SampleControl)
        {
            //graphicsAll = p_Graphics;
            SampleControl1 = p_SampleControl;
            poiFInt = new PointF(5, 10);

            flaHeight = 43;

            penAll = new Pen(Color.Black, 1);
            fontAll = new Font(new FontFamily("宋体"), 9);
        }
        /// <summary>
        /// 获取特定空位状态的平均值
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public string GetResult(string status)
        {

            string result = string.Empty;
            float total = 0F;
            int num = 0;


     
            int j = 0;

            //开始循环绘出酶标板
            for (int i1 = 0; i1 < SampleControl1.Rows; i1++)
            {
                for (int i2 = 0; i2 < SampleControl1.Columns; i2++)
                {
                    string curStatus = SampleControl1.HoleStatusList[j];
                    if (curStatus == status)
                    {
                        ComplexResultsControl control = SampleControl1.mainControls[j] as ComplexResultsControl;

                        if (!string.IsNullOrEmpty(control.Text))
                        {
                            float odValue = 0F;
                            if (float.TryParse(control.Text, out odValue))
                            {
                                total += odValue;
                                num++;
                            }
                        }

                    }
                    //if(control.

                    j++;//酶标数据累加;

                }


            }
            if (num > 0)
            {
                result = (total / num).ToString();
            }

            return result;
        }


        /// <summary>
        /// 酶标板绘图
        /// </summary>
        /// <returns></returns>
        public Image m_ImageDrawingAllSample()
        {
            Bitmap image = new Bitmap(720, 820);
            graphicsAll = Graphics.FromImage(image);
            graphicsAll.Clear(Color.White);

            clsDrawComplexResult objdrawResult = null;
            int j = 0;
            //每个酶标孔的实时坐标
            PointF _poitf = poiFInt;
            //开始循环绘出酶标板
            for (int i1 = 0; i1 < SampleControl1.Rows; i1++)
            {
                for (int i2 = 0; i2 < SampleControl1.Columns; i2++)
                {


                    //开始画酶标孔
                    objdrawResult = new clsDrawComplexResult(_poitf, flaHeight, graphicsAll, penAll, fontAll, (SampleControl1.mainControls[j] as ComplexResultsControl).ODText, (SampleControl1.mainControls[j] as ComplexResultsControl).NegOrPosText, (SampleControl1.mainControls[j] as ComplexResultsControl).Text, SampleControl1.HoleModeList[j], SampleControl1.HoleStatusList[j]);



                    j++;//酶标数据累加;
                    _poitf.X += flaHeight + 7;
                }
                //开始第二行坐标初始化
                _poitf.X = poiFInt.X;
                _poitf.Y += flaHeight + 12;

            }


            return image;
        }

        public byte[] m_ByteDrawingAllSample()
        {
            Image img = m_ImageDrawingAllSample();

            return ConvertToBtye(img);
        }

        private byte[] ConvertToBtye(Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Bmp);
            byte[] imageB = new byte[ms.Length];
            ms.Position = 0;
            ms.Read(imageB, 0, Convert.ToInt32(ms.Length));
            ms.Close();
            return imageB;
        }
    }


    /// <summary>
    /// 酶标格子对象
    /// </summary>
    public class clsDrawComplexResult
    {

        #region 全局变量

        /// <summary>
        /// 初始化坐标(为左上角坐标为准)
        /// </summary>
        public PointF poiFInt { get; set; }
        /// <summary>
        /// 画线的长度
        /// </summary>
        public float flaHeight { get; set; }
        /// <summary>
        /// 全局画板
        /// </summary>
        public Graphics graphicsAll { get; set; }
        /// <summary>
        /// 全局画刷
        /// </summary>
        public Pen penAll { get; set; }
        /// <summary>
        /// OD值
        /// </summary>
        public string strOD { get; set; }
        /// <summary>
        /// 阴阳性结果
        /// </summary>
        public string strPosNeg { get; set; }
        /// <summary>
        /// 原始值
        /// </summary>
        public string strVlaue { get; set; }
        /// <summary>
        /// 全局字体
        /// </summary>
        public Font fontAll { get; set; }
        /// <summary>
        /// 孔位状态
        /// </summary>
        public string strStatus { get; set; }
        /// <summary>
        /// 孔位序号
        /// </summary>
        public string strSep { get; set; }


        #endregion


        /// <summary>
        /// 构造函数
        /// </summary>
        ///<param name="p_poiFint">左上角坐标</param>
        /// <param name="p_flaHeight">线的长度</param>
        /// <param name="p_graphicsAll">模板</param>
        /// <param name="p_pen">画笔</param>
        /// <param name="p_fontAll">字体</param>
        /// <param name="p_strOD">OD值</param>
        /// <param name="p_strPosNeg">阴阳性结果</param>
        /// <param name="strVlaue">原始值</param>
        /// <param name="p_strSep">孔位序号</param>
        /// <param name="p_strStatus">孔位状态</param>
        public clsDrawComplexResult(PointF p_poiFint, float p_flaHeight, Graphics p_graphicsAll, Pen p_pen, Font p_fontAll, string p_strOD, string p_strPosNeg, string p_strVlaue, string p_strSep, string p_strStatus)
        {
            //初始化参数
            poiFInt = p_poiFint;
            flaHeight = p_flaHeight;
            graphicsAll = p_graphicsAll;
            penAll = p_pen;
            fontAll = p_fontAll;

            strOD = p_strOD;
            strPosNeg = p_strPosNeg;
            strVlaue = p_strVlaue;
            strSep = p_strSep;
            strStatus = p_strStatus;

            //开始加载初始化图形
            this.m_mthDrawingSampleLoad();

        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public virtual void m_mthDrawingSampleLoad()
        {
            //初始化格子
            graphicsAll.DrawRectangle(penAll, poiFInt.X, poiFInt.Y, flaHeight, flaHeight);

            //赋OD值
            graphicsAll.DrawString(strOD, fontAll, penAll.Brush, poiFInt.X, poiFInt.Y + 1);
            //赋阴阳性值
            graphicsAll.DrawString(strPosNeg, fontAll, penAll.Brush, poiFInt.X, poiFInt.Y + flaHeight / 3);
            //赋原始值
            graphicsAll.DrawString(strVlaue, fontAll, penAll.Brush, poiFInt.X, poiFInt.Y + flaHeight / 3 * 2);

            //赋值孔位序号+状态
            graphicsAll.DrawString(strSep + "(" + strStatus + ")", fontAll, penAll.Brush, poiFInt.X, poiFInt.Y + flaHeight);





        }
    }
}
