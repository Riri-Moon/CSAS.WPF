namespace CSAS.Repositories
{
	public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
	{
		public AttendanceRepository(AppDbContext context) : base(context)
		{

		}

		public AppDbContext GetDbContext { get { return Context; } }

		public IEnumerable<Attendance> GetAttendanceByMainGroup(MainGroup group)
		{
			return Context.Attendances.ToList().Where(x => x.MainGroup == group);
		}

		public IEnumerable<Attendance> GetAttendanceBySubGroup(SubGroup group)
		{
			return Context.Attendances.ToList().Where(x => x.SubGroup == group);
		}
	}
}
