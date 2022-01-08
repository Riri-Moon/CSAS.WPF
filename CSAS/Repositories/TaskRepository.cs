namespace CSAS.Repositories
{
	public class TaskRepository : Repository<Models.Task>, ITaskRepository
	{
		public TaskRepository(AppDbContext context) : base(context)
		{

		}

		public AppDbContext GetDbContext { get { return Context; } }
	}
}
