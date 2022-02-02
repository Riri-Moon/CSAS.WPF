namespace CSAS.Repositories
{
	public class SubGroupRepository : Repository<SubGroup>, ISubGroupRepository
	{
		public SubGroupRepository(AppDbContext context) : base(context)
		{

		}
		public IEnumerable<SubGroup> GetSubGroupByMainGroup(MainGroup group)
		{
			return Context.SubGroups.ToList().Where(x => x.MainGroup == group);
		}
		public AppDbContext GetDbContext { get { return Context; } }
	}
}
