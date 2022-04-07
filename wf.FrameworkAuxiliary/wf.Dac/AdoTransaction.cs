using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Lib.DAC.DbDriver;

namespace Lib.DAC
{
    internal class AdoTransaction : AbstractTransaction, ITransaction
    {
        private AdoTransaction()
        {

        }

        internal AdoTransaction(IDbTransaction trans, IDbDriver driver, IDialet dialet)
        {
            this.Trans = trans;
            this.Driver = driver;
            this.Dialet = dialet;
            this.Connection = trans.Connection;
        }

        private bool committed;
        private bool rolledBack;
        private bool commitFailed;

        public void Enlist(IDbCommand command)
        {
            if (Trans == null)
            {
                command.Transaction = null;
                return;
            }
            else
            {
                command.Transaction = Trans;
            }
        }

        public void Commit()
        {
            CheckNotDisposed();
            CheckNotZombied();

            try
            {
                Trans.Commit();
                committed = true;
                Dispose();
            }
            catch// (Exception e)
            {
                commitFailed = true;
                throw;
            }
            finally
            {
                CloseIfRequired();
            }
        }

        public void Rollback()
        {
            CheckNotDisposed();
            CheckNotZombied();

            if (!commitFailed)
            {
                try
                {
                    Trans.Rollback();
                    rolledBack = true;
                    Dispose();
                }
                catch// (Exception e)
                {
                    throw;
                }
                finally
                {
                    CloseIfRequired();
                }
            }
        }

        public bool WasRolledBack
        {
            get { return rolledBack; }
        }

        public bool WasCommitted
        {
            get { return committed; }
        }

        public bool IsActive
        {
            get { return !rolledBack && !committed; }
        }

        public IsolationLevel IsolationLevel
        {
            get { return Trans.IsolationLevel; }
        }

        void CloseIfRequired()
        {
            //bool close = session.ShouldAutoClose() && !transactionContext.isClosed();
            //if (close)
            //{
            //    transactionContext.managedClose();
            //}
        }

        #region System.IDisposable Members

        private bool _isAlreadyDisposed;

        ~AdoTransaction()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool isDisposing)
        {

            if (_isAlreadyDisposed)
            {
                return;
            }

            if (isDisposing)
            {
                if (Trans != null)
                {
                    Trans.Dispose();

                    this.Connection.Dispose();
                }
            }

            _isAlreadyDisposed = true;
            GC.SuppressFinalize(this);

        }

        #endregion

        private void CheckNotDisposed()
        {
            if (_isAlreadyDisposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }

        private void CheckNotZombied()
        {
            if (Trans != null && Trans.Connection == null)
            {
                throw new Exception("Transaction not connected, or was disconnected");
            }
        }
    }

    internal abstract class AbstractTransaction
    {
        public IDbDriver Driver { get; protected set; }
        public IDialet Dialet { get; protected set; }
        public IDbTransaction Trans { get; protected set; }
        public IDbConnection Connection { get; protected set; }
    }
}
