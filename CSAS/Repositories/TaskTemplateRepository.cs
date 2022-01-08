namespace CSAS.Repositories
{
	public class TaskTemplateRepository : Repository<TaskTemplate>, ITaskTemplateRepository
	{
		public TaskTemplateRepository(AppDbContext context) : base(context)
		{

		}

		public AppDbContext GetDbContext { get { return Context; } }
	}
}
