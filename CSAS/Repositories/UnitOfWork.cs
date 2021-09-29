using CSAS.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CSAS.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            if (Students == null)
            {
                Students = new StudentRepository(_context);
            }
            if (MainGroup == null)
            {
                MainGroup = new MainGroupRepository(_context);
            }
        }

        public IStudentRepository Students { get; private set; }
        public IMainGroupRepository MainGroup {  get; private set; }
        public int Complete()
        {
            using (var dbContextTransaction = _context.Database.BeginTransaction())
            {
                try
                {
                   var result = _context.SaveChanges();

                    dbContextTransaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                   //var error = new TransactionException($"Transaction - {dbContextTransaction.TransactionId} has been unsuccessfull");
                    throw ex;
                }
            }
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
