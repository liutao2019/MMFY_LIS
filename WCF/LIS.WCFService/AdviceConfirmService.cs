using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using dcl.servececontract;
using dcl.svr.interfaces;
//using dcl.svr.interfaces;
using dcl.pub.entities.dict;
using dcl.svr.interfaces.LisDataInterface;

namespace dcl.svr.wcf
{
    public class AdviceConfirmService : WCFServiceBase, IAdviceConfirm
    {
        /// <summary>
        /// 门诊确认收费
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string AdviceConfirm_MZ(EntityDataInterfaceAdviceConfirmParameter param)
        {
            new BarcodeConfirmInterface(param).mz_ExecuteAfterSignIn();
            return null;
        }

        /// <summary>
        /// 门诊取消收费
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string AdviceCancelConfirm_MZ(EntityDataInterfaceAdviceConfirmParameter param)
        {
            new BarcodeConfirmInterface(param).mz_AdviceCancelConfirm();
            return null;
        }

        /// <summary>
        /// 住院确认收费
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string AdviceConfirm_ZY(EntityDataInterfaceAdviceConfirmParameter param)
        {
            new BarcodeConfirmInterface(param).zy_ExecuteAfterSignIn();
            return null;
        }

  
        /// <summary>
        /// 体检_条码_采集后
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string AdviceConfirmForSampleCollect_TJ(EntityDataInterfaceAdviceConfirmParameter param)
        {
            new BarcodeConfirmInterface(param).tj_ExecuteAfterSampleCollect();
            return null;
        }

        /// <summary>
        /// 门诊_条码_回退后
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string ReturnBarcodeMessage_MZ(EntityDataInterfaceAdviceConfirmParameter param)
        {
            new BarcodeConfirmInterface(param).mz_ReturnBarcodeMessage();
            return null;
        }

        /// <summary>
        /// 住院_条码_回退后
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string ReturnBarcodeMessage_ZY(EntityDataInterfaceAdviceConfirmParameter param)
        {
            new BarcodeConfirmInterface(param).zy_ReturnBarcodeMessage();
            return null;
        }

        /// <summary>
        /// 体检_条码_回退后
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string ReturnBarcodeMessage_TJ(EntityDataInterfaceAdviceConfirmParameter param)
        {
            new BarcodeConfirmInterface(param).tj_ReturnBarcodeMessage();
            return null;
        }
        /// <summary>
        /// 住院取消收费
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string AdviceCancelConfirm_ZY(EntityDataInterfaceAdviceConfirmParameter param)
        {
            new BarcodeConfirmInterface(param).zy_AdviceCancelConfirm();
            return null;
        }

        /// <summary>
        /// 体检确认收费
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string AdviceConfirm_TJ(EntityDataInterfaceAdviceConfirmParameter param)
        {
            new BarcodeConfirmInterface(param).tj_ExecuteAfterSignIn();
            return null;
        }


        /// <summary>
        /// 体检取消收费
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string AdviceCancelConfirm_TJ(EntityDataInterfaceAdviceConfirmParameter param)
        {
            new BarcodeConfirmInterface(param).tj_AdviceCancelConfirm();
            return null;
        }

        public string AdviceConfirm_MZ_WithRet(EntityDataInterfaceAdviceConfirmParameter param)
        {
            return new BarcodeConfirmInterface(param).mz_ExecuteAfterSignIn_WithRet();
        }

        public string AdviceConfirm_ZY_WithRet(EntityDataInterfaceAdviceConfirmParameter param)
        {
            return new BarcodeConfirmInterface(param).zy_AdviceCancelConfirm_WithRet();
        }

        public string AdviceConfirm_TJ_WithRet(EntityDataInterfaceAdviceConfirmParameter param)
        {
            return new BarcodeConfirmInterface(param).tj_AdviceCancelConfirm_WithRet();
        }

        /// <summary>
        /// 门诊_条码_采集后
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string AdviceConfirmForSampleCollect_MZ(EntityDataInterfaceAdviceConfirmParameter param)
        {
            new BarcodeConfirmInterface(param).mz_ExecuteAfterSampleCollect();
            return null;
        }
    }
}
