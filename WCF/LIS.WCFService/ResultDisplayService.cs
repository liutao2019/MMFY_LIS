using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dcl.servececontract;
using dcl.svr.result;
using System.Data;

namespace dcl.svr.wcf
{
    /// <summary>
    /// 结果视窗服务层
    /// </summary>
    public class ResultDisplayService :IResultDisplay
    {
        ResultDisplayBIZ rdBiz;
        /// <summary>
        /// 构造方法
        /// </summary>
        public ResultDisplayService()
        {
            rdBiz = new ResultDisplayBIZ();
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="itr_id"></param>
        /// <param name="timer"></param>
        /// <param name="sid"></param>
        /// <returns></returns>
        public DataTable getResult(int itr_id, DateTime timer, string sid)
        {
            return rdBiz.getResult(itr_id, timer, sid);
        }



        //测试用
        public int Sun(int a, int b)
        {
            return (rdBiz.Sun(a, b));
        }


    }
}
