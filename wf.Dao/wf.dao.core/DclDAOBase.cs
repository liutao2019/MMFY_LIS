using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.dao.core
{
    public class DclDaoBase
    {
        /// <summary>
        /// DbManager对象，用于事务
        /// </summary>
        public DBManager Dbm { get; set; }

        public DBManager GetDbManager()
        {
            if (this.Dbm == null)
                return new DBManager();
            else
                return this.Dbm;
        }
    }
}
