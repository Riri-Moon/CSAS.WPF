namespace CSAS.Repositories
{
	public class StudentRepository : Repository<Student>, IStudentRepository
	{
		public StudentRepository(AppDbContext context) : base(context)
		{

		}

		public IEnumerable<Student> GetStudentsByGroup(MainGroup group)
		{
			return Context.Students.Where(x => x.MainGroup == group).OrderBy(x=>x.LastName).ToList();
		}

		public IEnumerable<Student> GetStudentsBySubGroup(SubGroup subGroup)
		{
			return Context.Students.ToList().Where(x => x.SubGroup == subGroup).OrderBy(x => x.LastName).ToList();
		}

		public AppDbContext GetDbContext { get { return Context; } }
	}
}
