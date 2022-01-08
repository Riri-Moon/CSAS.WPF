namespace CSAS.Repositories
{
	public class FinalAssessmentRepository : Repository<FinalAssessment>, IFinalAssessmentRepository
	{
		public FinalAssessmentRepository(AppDbContext context) : base(context)
		{
		}
	}
}
