using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Lib.DAC.DbDriver;

namespace Lib.DAC
{
    public interface ITransaction : IDisposable
    {
        void Commit();

        void Rollback();

        bool IsActive { get; }

        bool WasRolledBack { get; }

        bool WasCommitted { get; }

        void Enlist(IDbCommand command);

        IDbDriver Driver { get; }

        IDialet Dialet { get; }
    }
}
