namespace CSAS.Repositories
{
	public class UserInfoRepository : Repository<UserInfo>, IUserInfoRepository
	{
		public UserInfoRepository(AppDbContext context) : base(context)
		{
		}
	}
}
