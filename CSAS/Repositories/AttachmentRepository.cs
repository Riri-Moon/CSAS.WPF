namespace CSAS.Repositories
{
	public class AttachmentRepository : Repository<Attachments>, IAttachmentRepository
	{
		public AttachmentRepository(AppDbContext context) : base(context)
		{
		}
	}
}
