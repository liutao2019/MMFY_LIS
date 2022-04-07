using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.DAC.DbSchema
{
    public class DbSchemaColumnCollection : List<DbSchemaColumn>
    {
        public DbSchemaColumnCollection()
        {
            this.FieldsName = new List<string>();
        }

        public List<string> FieldsName { get; private set; }

        public new void Add(DbSchemaColumn item)
        {
            base.Add(item);
            FieldsName.Add(item.ColumnName);
        }

        public new void AddRange(IEnumerable<DbSchemaColumn> collection)
        {
            base.AddRange(collection);
            foreach (DbSchemaColumn item in collection)
            {
                FieldsName.Add(item.ColumnName);
            }
        }

        public new bool Remove(DbSchemaColumn item)
        {
            bool b = base.Remove(item);
            if (b)
            {
                this.FieldsName.Remove(item.ColumnName);
            }
            return b;
        }
    }
}
