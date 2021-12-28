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
            if (SubGroup == null)
            {
                SubGroup = new SubGroupRepository(_context);
            }
            if (Attendance == null)
            {
                Attendance = new AttendanceRepository(_context);
            }
            if (ActivityTemplate == null)
            {
                ActivityTemplate = new ActivityTemplateRepository(_context);
            }
            if (Task == null)
            {
                Task = new TaskRepository(_context);
            }
            if (TasksTemplate == null)
            {
                TasksTemplate = new TaskTemplateRepository(_context);
            }
            if (Activity == null)
            {
                Activity = new ActivityRepository(_context);
            }
            if (Attachment == null)
            {
                Attachment = new AttachmentRepository(_context);
            }
            if (Settings == null)
            {
                Settings = new SettingsRepository(_context);
            }
            if (FinalAssessment == null)
            {
                FinalAssessment = new FinalAssessmentRepository(_context);
            }
        }
        public IFinalAssessmentRepository FinalAssessment { get; private set; }
        public IStudentRepository Students { get; private set; }
        public IMainGroupRepository MainGroup { get; private set; }
        public ISubGroupRepository SubGroup { get; private set; }
        public IAttendanceRepository Attendance { get; private set; }
        public IActivityTemplateRepository ActivityTemplate { get; private set; }
        public ITaskRepository Task { get; private set; }
        public ITaskTemplateRepository TasksTemplate { get; private set; }
        public IActivityRepository Activity { get; private set; }
        public IAttachmentRepository Attachment { get; private set; }
        public ISettingsRepository Settings { get; private set; }

        public int Complete()
        {
            using var dbContextTransaction = _context.Database.BeginTransaction();
            try
            {
                var result = _context.SaveChanges();

                dbContextTransaction.Commit();
                return result;
            }
            catch (Exception ex)
            {
                dbContextTransaction.Rollback();
                throw ex;
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
