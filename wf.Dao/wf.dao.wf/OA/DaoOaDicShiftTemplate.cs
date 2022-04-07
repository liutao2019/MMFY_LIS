using dcl.common;
using dcl.dao.core;
using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoOaDicOaShiftTemplate))]
    public class DaoOaDicShiftTemplate : IDaoOaDicOaShiftTemplate
    {

        public List<EntityOaShiftTemplate> GetTemplateData()
        {
            try
            {
                DBManager helper = new DBManager();
                string strSql = @"select Dict_oa_duty_template.*
                            from   Dict_oa_duty_template";

                DataTable dt = helper.ExecuteDtSql(strSql);
            
                List<EntityOaShiftTemplate> list = EntityManager<EntityOaShiftTemplate>.ConvertToList(dt).OrderBy(w => w.TempId).ToList();
                return list;
            }
            catch (Exception ex)
            { 
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityOaShiftTemplate>();
            }
        }
    }
}
