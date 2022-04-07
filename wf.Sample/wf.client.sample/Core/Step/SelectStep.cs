using System;
using System.Collections.Generic;
using System.Text;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid;
using System.Drawing;

using dcl.client.common;

namespace dcl.client.sample
{
    /// <summary>
    /// 条码查询用流程
    /// </summary>
    public class SelectStep : IStep
    {


        //public override void FormatRow(GridView gvBarcode)
        //{
        //    FormatHelper.FormatRow(gvBarcode, BarcodeTable.Patient.PrintFlag, "1", Color.Red);
        //    FormatHelper.FormatRow(gvBarcode, BarcodeTable.Patient.ReachFlag, "1", Color.Blue); //送达
        //    FormatHelper.FormatRow(gvBarcode, BarcodeTable.Patient.SampleReceiveFlag, "1", Color.DarkGray);

        //    //gvBarcode.OptionsView.ShowAutoFilterRow = true;
        //}

        public override string StepCode
        {
            get { return "1"; }
        }

        public override string StepName
        {
            get { return "条码查询"; }
        }

        public override bool HasNotDoneAction()
        {
            return BaseSampMain.SampPrintFlag != 1;
        }
    }
}
