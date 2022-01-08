namespace CSAS.Repositories
{
	public class ActivityRepository : Repository<Activity>, IActivityRepository
	{
		public ActivityRepository(AppDbContext context) : base(context)
		{
		}
	}
}
