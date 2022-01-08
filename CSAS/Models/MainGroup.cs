namespace CSAS.Models
{
	public class MainGroup : BaseWithNameModelBindableBaseBindableBase
	{
		public virtual string Form { get; set; }
		public virtual List<SubGroup> SubGroups { get; set; }
		public virtual string? Subject { get; set; }
		public virtual string? PathToFolder { get; set; }

		public MainGroup()
		{

		}
	}
}
