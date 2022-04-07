using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Lib.DAC
{
    public class DbDataAdapterEx : IDbDataAdapter
    {
        IDbDataAdapter innerAdapter = null;

        internal DbDataAdapterEx(IDbDataAdapter adapter)
        {
            this.innerAdapter = adapter;
        }

        #region IDbDataAdapter 成员

        public IDbCommand DeleteCommand
        {
            get
            {
                return this.innerAdapter.DeleteCommand;
            }
            set
            {
                if (value is DbCommandEx)
                {
                    this.innerAdapter.SelectCommand = (value as DbCommandEx).InnerCommand;
                }
                else if (value is DbCommandEx2)
                {
                    this.innerAdapter.SelectCommand = (value as DbCommandEx2).InnerCommand;
                }
                else
                {
                    this.innerAdapter.SelectCommand = value;
                }
            }
        }

        public IDbCommand InsertCommand
        {
            get
            {
                return this.innerAdapter.InsertCommand;
            }
            set
            {
                if (value is DbCommandEx)
                {
                    this.innerAdapter.SelectCommand = (value as DbCommandEx).InnerCommand;
                }
                else if (value is DbCommandEx2)
                {
                    this.innerAdapter.SelectCommand = (value as DbCommandEx2).InnerCommand;
                }
                else
                {
                    this.innerAdapter.SelectCommand = value;
                }
            }
        }

        public IDbCommand SelectCommand
        {
            get
            {
                return this.innerAdapter.SelectCommand;
            }
            set
            {
                if (value is DbCommandEx)
                {
                    this.innerAdapter.SelectCommand = (value as DbCommandEx).InnerCommand;
                }
                else if (value is DbCommandEx2)
                {
                    this.innerAdapter.SelectCommand = (value as DbCommandEx2).InnerCommand;
                }
                else
                {
                    this.innerAdapter.SelectCommand = value;
                }
            }
        }

        public IDbCommand UpdateCommand
        {
            get
            {
                return this.innerAdapter.UpdateCommand;
            }
            set
            {
                if (value is DbCommandEx)
                {
                    this.innerAdapter.SelectCommand = (value as DbCommandEx).InnerCommand;
                }
                else if (value is DbCommandEx2)
                {
                    this.innerAdapter.SelectCommand = (value as DbCommandEx2).InnerCommand;
                }
                else
                {
                    this.innerAdapter.SelectCommand = value;
                }
            }
        }

        #endregion

        public int Fill(DataTable dataTable)
        {
            return (this.innerAdapter as System.Data.Common.DbDataAdapter).Fill(dataTable);
        }

        #region IDataAdapter 成员

        public int Fill(DataSet dataSet)
        {
            return this.innerAdapter.Fill(dataSet);
        }

        public DataTable[] FillSchema(DataSet dataSet, SchemaType schemaType)
        {
            return this.innerAdapter.FillSchema(dataSet, schemaType);
        }

        public IDataParameter[] GetFillParameters()
        {
            return this.innerAdapter.GetFillParameters();
        }

        public MissingMappingAction MissingMappingAction
        {
            get
            {
                return this.innerAdapter.MissingMappingAction;
            }
            set
            {
                this.innerAdapter.MissingMappingAction = value;
            }
        }

        public MissingSchemaAction MissingSchemaAction
        {
            get
            {
                return this.innerAdapter.MissingSchemaAction;
            }
            set
            {
                this.innerAdapter.MissingSchemaAction = value;
            }
        }

        public ITableMappingCollection TableMappings
        {
            get { return this.innerAdapter.TableMappings; }
        }

        public int Update(DataSet dataSet)
        {
            return this.innerAdapter.Update(dataSet);
        }

        #endregion

    }
}
