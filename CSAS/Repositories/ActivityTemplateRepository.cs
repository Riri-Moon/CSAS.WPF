namespace CSAS.Repositories
{
	public class ActivityTemplateRepository : Repository<ActivityTemplate>, IActivityTemplateRepository
	{
		public ActivityTemplateRepository(AppDbContext context) : base(context)
		{
		}
	}
}
