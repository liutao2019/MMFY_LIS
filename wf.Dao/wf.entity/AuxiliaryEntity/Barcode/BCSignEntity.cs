using System;
using System.Collections.Generic;

using System.Text;
using System.Data;

namespace dcl.entity
{
    [Serializable]
    public class BCSignEntity
    {
        public BCSignEntity()
        {
            bc_login_id = string.Empty;
            bc_name = string.Empty;
            bc_type = string.Empty;
            bc_status = string.Empty;
            bc_status_name = string.Empty;

            bc_bar_no = string.Empty;
            bc_bar_code = string.Empty;
            bc_place = string.Empty;
        }

        /// <summary>
        /// ID
        /// </summary>
        public int bc_id { get; set; }

        /// <summary>
        /// 签名时间
        /// </summary>
        public DateTime bc_date { get; set; }

        /// <summary>
        /// 签名者工号
        /// </summary>
        public string bc_login_id { get; set; }

        /// <summary>
        /// 签名者姓名
        /// </summary>
        public string bc_name { get; set; }

        /// <summary>
        /// 签名类型
        /// </summary>
        public string bc_type { get; set; }

        /// <summary>
        /// 标本状态
        /// 标本状态  (0-未打印,1-打印,2-采集, 3-已送检,4-已送达,5-签收,6-检验中,7-已检验)
        /// 关联bc_status，负数代表某步骤撤销
        /// </summary>
        public string bc_status { get; set; }


        public string bc_status_name { get; set; }

        /// <summary>
        /// 内部条码号
        /// </summary>
        public string bc_bar_no { get; set; }


        /// <summary>
        /// 外部条码号
        /// </summary>
        public string bc_bar_code { get; set; }

        /// <summary>
        /// 签名地点
        /// </summary>
        public string bc_place { get; set; }

        /// <summary>
        /// 当前操作所属流程
        /// </summary>
        public int bc_flow { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string bc_remark { get; set; }

        public override string ToString()
        {
            string text = string.Format(@"
条码号：{0}
时间：{1}
状态：{2}
签名人：{3}
签名地点：{4}

",
 bc_bar_code,
 bc_date.ToString("yyyy年MM月dd日 HH时:mm分"),
 bc_status_name,
 bc_name,
 bc_place
 );
            return text;
        }


        private static BCSignEntity FromDataRow(DataRow dr)
        {
            BCSignEntity entity = new BCSignEntity();

            entity.bc_bar_code = dr["bc_bar_code"].ToString();
            entity.bc_bar_no = dr["bc_bar_no"].ToString();
            entity.bc_date = (DateTime)dr["bc_date"];
            entity.bc_id = Convert.ToInt32(dr["bc_id"]);
            entity.bc_login_id = dr["bc_login_id"].ToString();


            entity.bc_name = dr["bc_name"].ToString();

            if (dr["bc_place"] != DBNull.Value)
            {
                entity.bc_place = dr["bc_place"].ToString();
            }

            entity.bc_status = dr["bc_status"].ToString();

            if (dr["bc_status_name"] != DBNull.Value)
            {
                entity.bc_status_name = dr["bc_status_name"].ToString();
            }

            entity.bc_remark = dr["bc_remark"].ToString();

            if (dr["bc_flow"] != DBNull.Value)
            {
                entity.bc_flow = Convert.ToInt32(dr["bc_flow"]);
            }

            return entity;
        }


        public static IEnumerable<BCSignEntity> FromDataTable(DataTable dt)
        {
            List<BCSignEntity> list = new List<BCSignEntity>();
            foreach (DataRow dr in dt.Rows)
            {
                BCSignEntity entity = FromDataRow(dr);
                list.Add(entity);
            }
            return list;
        }
    }
}
