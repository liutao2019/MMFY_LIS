using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC
{
    internal class DialetProvider
    {
        private static IDialet currentDialet = null;

        /// <summary>
        /// 获取当前应用程序配置的驱动
        /// </summary>
        /// <returns></returns>
        public static IDialet GetCurrentDialet()
        {
            if (currentDialet == null)
            {
                currentDialet = GetDialet(DACConfig.Current.DataBaseDialet);
            }
            return currentDialet;
        }

        public static IDialet GetDialet(EnumDataBaseDialet dbtype)
        {
            IDialet dialet = null;

            if (dbtype == EnumDataBaseDialet.Oracle8i)
            {
                dialet = new Lib.DAC.DbDialet.DialetOracle8i();
            }
            else if (dbtype == EnumDataBaseDialet.Oracle9i)
            {
                dialet = new Lib.DAC.DbDialet.DialetOracle9i();
            }
            else if (dbtype == EnumDataBaseDialet.Oracle10g)
            {
                dialet = new Lib.DAC.DbDialet.DialetOracle10g();
            }
            else if (dbtype == EnumDataBaseDialet.SQL2000)
            {
                dialet = new Lib.DAC.DbDialet.DialetSql2000();
            }
            else if (dbtype == EnumDataBaseDialet.SQL2005)
            {
                dialet = new Lib.DAC.DbDialet.DialetSql2005();
            }
            else if (dbtype == EnumDataBaseDialet.Access)
            {
                dialet = new Lib.DAC.DbDialet.DialetMSAccess();
            }
            else
            {
                dialet = new Lib.DAC.DbDialet.DialetSql2000();
            }
            return dialet;
        }
    }
}
