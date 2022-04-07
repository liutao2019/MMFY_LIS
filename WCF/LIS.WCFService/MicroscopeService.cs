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
    /// 镜检模块服务层
    /// </summary>
    public class MicroscopeService : IMicroscope
    {
        MicroscopeBIZ mbiz;
        public int Add(int a, int b)
        {
            //return new MicroscopeBIZ().Add(a, b);
            mbiz = new MicroscopeBIZ();
            return mbiz.Add(a, b);
        }

        /// <summary>
        /// 是否存在该病人
        /// </summary>
        /// <param name="stylebook"></param>
        /// <returns></returns>
        public DataTable getPatients(string stylebook)
        {
            mbiz = new MicroscopeBIZ();
            return mbiz.getPatients(stylebook);
        }

        /// <summary>
        /// 仪器默认组合查询
        /// </summary>
        /// <param name="a">仪器代码</param>
        /// <returns></returns>
        public DataTable getCom(string instrument)
        {
            mbiz = new MicroscopeBIZ();
            
            return mbiz.getTable1(instrument);
        }

        public DataTable getCom1(string stylebook)
        {
            mbiz = new MicroscopeBIZ();
            return mbiz.getCom(stylebook);
        }

        /// <summary>
        /// 查结果
        /// </summary>
        /// <param name="stylebook"> 病人ID</param>
        /// <param name="com_id">组合ID</param>
        /// <param name="state">镜检标志： 0=非镜检 1=镜检</param>
        /// <returns></returns>
        public DataTable getResult(string pat_id)
        {
            mbiz = new MicroscopeBIZ();
           
            return mbiz.getTable1(pat_id);
        }

        public DataTable getResult1(string stylebook, string com_id, int state)
        {
            mbiz = new MicroscopeBIZ();
            return mbiz.getTable(stylebook, com_id, state);
        }



        /// <summary>
        /// 保存结果
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        public int Save(DataTable tb)
        {
            mbiz = new MicroscopeBIZ();
            return mbiz.DoSomething(tb);
        }

        /// <summary>
        /// 保存图像结果
        /// </summary>
        /// <param name="tb"></param>
        /// <returns></returns>
        public int saveImage(DataTable tb)
        {
            mbiz = new MicroscopeBIZ();
            return mbiz.saveImage(tb);
        }
    }
}
