using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace dcl.entity
{
    [Serializable()]
    public class EntityDicReaUnit : EntityBase
    {
        [FieldMapAttribute(ClabName = "Runit_name", MedName = "Runit_name", WFName = "Runit_name")]
        public System.String Runit_name { get; set; }
        [FieldMapAttribute(ClabName = "py_code", MedName = "py_code", WFName = "py_code")]
        public System.String py_code { get; set; }
        [FieldMapAttribute(ClabName = "wb_code", MedName = "wb_code", WFName = "wb_code")]
        public System.String wb_code { get; set; }
        [FieldMapAttribute(ClabName = "Runit_id", MedName = "Runit_id", WFName = "Runit_id")]
        public System.String Runit_id { get; set; }
        [FieldMapAttribute(ClabName = "no_del", MedName = "del_flag", WFName = "del_flag")]
        public String del_flag { get; set; }
        [FieldMapAttribute(ClabName = "sam_seq", MedName = "sort_no", WFName = "sort_no")]
        public int sort_no { get; set; }
    }
}
