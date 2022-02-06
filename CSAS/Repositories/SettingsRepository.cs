namespace CSAS.Repositories
{
	public class SettingsRepository : Repository<Settings>, ISettingsRepository
	{
		public SettingsRepository(AppDbContext context) : base(context)
		{

		}
		public Settings GetSettingsByMainGroup(string groupId)
		{
			return Context.Settings.FirstOrDefault(x => x.MainGroup.Id == groupId);
		}
	}
}
