namespace CSAS.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly AppDbContext _context;
		private static Logger _logger = new();
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
		 public IUserInfoRepository UserInfo { get; private set; }
		public UnitOfWork(AppDbContext context)
		{
			_context = context;
			_context.Database.EnsureCreated();
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

			UserInfo = new UserInfoRepository(_context);
		}

		public void Complete()
		{
			//_context.SaveChanges();
			
			using (var dbContextTransaction = _context.Database.BeginTransaction())
			{
				try
				{
					var result = _context.SaveChanges();

					dbContextTransaction.Commit();
					//return result;
				}
				catch (Exception ex)
				{
					dbContextTransaction.Rollback();
					_logger.ErrorAsync(ex.Message);
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
