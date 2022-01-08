namespace CSAS.Repositories
{
	public class MainGroupRepository : Repository<MainGroup>, IMainGroupRepository
	{
		public MainGroupRepository(AppDbContext context) : base(context)
		{

		}

		public AppDbContext GetDbContext { get { return Context; } }
	}
}
