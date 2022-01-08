namespace CSAS.Models
{
	public class SubGroup : BaseWithNameModelBindableBaseBindableBase
	{
		public virtual MainGroup MainGroup { get; set; }
		public virtual List<Student> Students { get; set; }
		public virtual string? PathToFolder { get; set; }

	}
}
