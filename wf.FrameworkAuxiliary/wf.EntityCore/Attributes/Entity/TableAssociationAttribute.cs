using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.EntityCore
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class TableAssociationAttribute : IEntityAttribute
    {
        public string AssociateName { get; set; }
        public string OtherTable { get; set; }

        private string _OtherTableAlias = null;
        public string OtherTableAlias
        {
            get
            {
                if (string.IsNullOrEmpty(_OtherTableAlias))
                {
                    return this.OtherTable;
                }
                else
                {
                    return this._OtherTableAlias;
                }
            }
            set
            {
                this._OtherTableAlias = value;
            }
        }

        public string ThisKey { get; set; }
        public string OtherKey { get; set; }
        public EnumTableAssociateType AssociateType { get; set; }

        public TableAssociationAttribute()
        {
            this.AssociateType = EnumTableAssociateType.LeftJoin;
        }
    }
}
