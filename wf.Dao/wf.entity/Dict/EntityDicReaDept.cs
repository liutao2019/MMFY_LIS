using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace dcl.entity
{
    [Serializable()]
    public class EntityDicReaDept : EntityBase
    {
        [FieldMapAttribute(ClabName = "Rclaim_Dept", MedName = "Rclaim_Dept", WFName = "Rclaim_Dept")]
        public System.String Rclaim_Dept { get; set; }
        [FieldMapAttribute(ClabName = "Rdept_id", MedName = "Rdept_id", WFName = "Rdept_id")]
        public System.String Rdept_id { get; set; }
        [FieldMapAttribute(ClabName = "py_code", MedName = "py_code", WFName = "py_code")]
        public System.String py_code { get; set; }
        [FieldMapAttribute(ClabName = "wb_code", MedName = "wb_code", WFName = "wb_code")]
        public System.String wb_code { get; set; }
        [FieldMapAttribute(ClabName = "no_del", MedName = "del_flag", WFName = "del_flag")]
        public String del_flag { get; set; }
        [FieldMapAttribute(ClabName = "sam_seq", MedName = "sort_no", WFName = "sort_no")]
        public int sort_no { get; set; }
    }
}
