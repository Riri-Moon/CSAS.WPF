namespace CSAS.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext _context;
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

		public UnitOfWork(AppDbContext context)
		{
			_context = context;

			Students = new StudentRepository(_context);

			MainGroup = new MainGroupRepository(_context);

			SubGroup = new SubGroupRepository(_context);

			Attendance = new AttendanceRepository(_context);

			ActivityTemplate = new ActivityTemplateRepository(_context);

			Task = new TaskRepository(_context);

			TasksTemplate = new TaskTemplateRepository(_context);

			Activity = new ActivityRepository(_context);

			Attachment = new AttachmentRepository(_context);

			Settings = new SettingsRepository(_context);

			FinalAssessment = new FinalAssessmentRepository(_context);
		}

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
