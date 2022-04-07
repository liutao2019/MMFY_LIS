using dcl.dao.interfaces;
using dcl.entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Data;
using dcl.dao.core;

namespace dcl.dao.wf
{
    [Export("wf.plugin.wf", typeof(IDaoDic<EntityReport>))]
    public class DaoReport : IDaoDic<EntityReport>
    {
        public bool Delete(EntityReport sample)
        {
            throw new NotImplementedException();
        }

        public bool Save(EntityReport sample)
        {
            throw new NotImplementedException();
        }

        public List<EntityReport> Search(object obj)
        {
            try
            {
                String sql = @"select Brep_name,Brep_code from Base_report";

                DBManager helper = new DBManager();

                DataTable dt = helper.ExecuteDtSql(sql);

                return ConvertToEntitys(dt);
            }
            catch (Exception ex)
            {
                Lib.LogManager.Logger.LogException(ex);
                return new List<EntityReport>();
            }
        }
        public List<EntityReport> ConvertToEntitys(DataTable dtSour)
        {
            List<EntityReport> list = new List<EntityReport>();
            foreach (DataRow item in dtSour.Rows)
            {
                EntityReport report = new EntityReport();
                report.RepName = item["Brep_name"].ToString();
                report.RepCode = item["Brep_code"].ToString();

                list.Add(report);
            }
            return list;
        }

        public bool Update(EntityReport sample)
        {
            throw new NotImplementedException();
        }
    }
}
