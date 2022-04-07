using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dcl.entity
{
    public static class ConvertStringSidToListSid
    {
        /// <summary>
        /// 将1-5,7,9-15等样本号转化为List<EntitySid>
        /// </summary>
        /// <param name="inupt"></param>
        /// <returns></returns>
        public static List<EntitySid> GetListSid(string inupt)
        {
            List<EntitySid> listSid = new List<EntitySid>();
            if (inupt == null || inupt.Trim(null) == string.Empty)
            {
                return listSid;
            }

            string[] strs = inupt.Split(',');

            if (strs.Length > 0)
            {

                foreach (string str in strs)
                {
                    if (str.IndexOf('-') != -1)
                    {
                        string[] str2 = str.Split('-');

                        if (str2.Length == 2)
                        {
                            int from = 0;
                            int to = 0;

                            if (int.TryParse(str2[0], out from) && int.TryParse(str2[1], out to))
                            {
                                EntitySid sid = new EntitySid();
                                sid.StartSid = from;
                                sid.EndSid = to;

                                listSid.Add(sid);
                            }
                        }
                    }
                    else
                    {
                        string str2 = str.Trim(null);

                        int i = -1;

                        if (int.TryParse(str2, out i))
                        {
                            EntitySid sid = new EntitySid();
                            sid.StartSid = i;

                            listSid.Add(sid);
                        }
                    }
                }
            }
            return listSid;
        }

        /// <summary>
        /// 将1-5,7,9-15等序号转化为List<EntitySortNo>
        /// </summary>
        /// <param name="inupt"></param>
        /// <returns></returns>
        public static List<EntitySortNo> GetListSortNo(string inupt)
        {
            List<EntitySortNo> listSortNo = new List<EntitySortNo>();
            if (inupt == null || inupt.Trim(null) == string.Empty)
            {
                return listSortNo;
            }

            string[] strs = inupt.Split(',');

            if (strs.Length > 0)
            {

                foreach (string str in strs)
                {
                    if (str.IndexOf('-') != -1)
                    {
                        string[] str2 = str.Split('-');

                        if (str2.Length == 2)
                        {
                            int from = 0;
                            int to = 0;

                            if (int.TryParse(str2[0], out from) && int.TryParse(str2[1], out to))
                            {
                                EntitySortNo sid = new EntitySortNo();
                                sid.StartNo = from;
                                sid.EndNo = to;

                                listSortNo.Add(sid);
                            }
                        }
                    }
                    else
                    {
                        string str2 = str.Trim(null);

                        int i = -1;

                        if (int.TryParse(str2, out i))
                        {
                            EntitySortNo sid = new EntitySortNo();
                            sid.StartNo = i;

                            listSortNo.Add(sid);
                        }
                    }
                }
            }
            return listSortNo;
        }
    }
}
