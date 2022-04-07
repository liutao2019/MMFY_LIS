using System;
using System.Collections.Generic;
using System.Text;
using Lib.DAC.DbDialet;

namespace Lib.DAC
{
    public class DbDialetHelper
    {
        public static EnumDataBaseDialet GetDialetTypeByName(string dialetName)
        {
            EnumDataBaseDialet _databasedialet;
            switch (dialetName.ToLower())
            {
                case "sql2000":
                    _databasedialet = EnumDataBaseDialet.SQL2000;
                    break;

                case "sql":
                case "sql2005":
                    _databasedialet = EnumDataBaseDialet.SQL2005;
                    break;

                case "oracle8i":
                    _databasedialet = EnumDataBaseDialet.Oracle8i;
                    break;

                case "oracle9i":
                    _databasedialet = EnumDataBaseDialet.Oracle9i;
                    break;

                case "oracle":
                case "oracle10g":
                    _databasedialet = EnumDataBaseDialet.Oracle10g;
                    break;

                case "oracle11g":
                    _databasedialet = EnumDataBaseDialet.Oracle11g;
                    break;

                case "access":
                case "access2007":
                    _databasedialet = EnumDataBaseDialet.Access;
                    break;

                case "excel":
                    _databasedialet = EnumDataBaseDialet.Excel;
                    break;

                case "db2":
                    _databasedialet = EnumDataBaseDialet.DB2;
                    break;

                default:
                    try
                    {
                        _databasedialet = (EnumDataBaseDialet)Enum.Parse(typeof(EnumDataBaseDialet), dialetName);
                    }
                    catch
                    {
                        _databasedialet = EnumDataBaseDialet.SQL2005;
                    }
                    break;
            }
            return _databasedialet;
        }

        /// <summary>
        /// 获取当前应用程序配置的驱动
        /// </summary>
        /// <returns></returns>
        public static IDialet GetCurrentDialet()
        {
            return DACConfig.Current.Dialet;
        }

        public static IDialet GetDialet(EnumDataBaseDialet enumDbDialet)
        {
            IDialet dialet = null;

            switch (enumDbDialet)
            {
                case EnumDataBaseDialet.Access2007:
                case EnumDataBaseDialet.Access:
                    dialet = new DialetMSAccess();
                    break;

                case EnumDataBaseDialet.Excel:
                    dialet = new DialetMSExcel();
                    break;

                case EnumDataBaseDialet.Oracle11g:
                    dialet = new DialetOracle11g();
                    break;

                case EnumDataBaseDialet.Oracle10g:
                    dialet = new DialetOracle10g();
                    break;

                case EnumDataBaseDialet.Oracle8i:
                    dialet = new DialetOracle8i();
                    break;

                case EnumDataBaseDialet.Oracle9i:
                    dialet = new DialetOracle9i();
                    break;

                case EnumDataBaseDialet.SQL2000:
                    dialet = new DialetSql2000();
                    break;

                case EnumDataBaseDialet.SQL2005:
                    dialet = new DialetSql2005();
                    break;

                case EnumDataBaseDialet.SQL2008:
                    dialet = new DialetSql2008();
                    break;
            }

            return dialet;
        }
    }
}