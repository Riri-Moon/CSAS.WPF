namespace CSAS.Repositories
{
	public class SettingsRepository : Repository<Settings>, ISettingsRepository
	{
		public SettingsRepository(AppDbContext context) : base(context)
		{
		}
	}
}
