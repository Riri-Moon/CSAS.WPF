namespace CSAS.Repositories
{
	public class SubGroupRepository : Repository<SubGroup>, ISubGroupRepository
	{
		public SubGroupRepository(AppDbContext context) : base(context)
		{

		}

		public AppDbContext GetDbContext { get { return Context; } }
	}
}
