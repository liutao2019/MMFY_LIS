using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace dcl.entity
{
    [Serializable()]
    public class EntityDicReaStorePos : EntityBase
    {
        [FieldMapAttribute(ClabName = "Rstr_position", MedName = "Rstr_position", WFName = "Rstr_position")]
        public System.String Rstr_position { get; set; }
        [FieldMapAttribute(ClabName = "Rstr_position_id", MedName = "Rstr_position_id", WFName = "Rstr_position_id")]
        public System.String Rstr_position_id { get; set; }
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
