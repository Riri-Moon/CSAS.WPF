namespace CSAS.Models
{
	public class MainGroup : BaseWithNameModelBindableBaseBindableBase
	{
		public virtual string Form
		{
			get => _form;
			set => SetProperty(ref _form, value);
		}
		public virtual List<SubGroup> SubGroups 
		{
			get => _subGroups;
			set => SetProperty(ref _subGroups, value);
		}
		public virtual string? Subject
		{
			get => _subject;
			set => SetProperty(ref _subject, value);
		}
		public virtual string? PathToFolder 
		{
			get => _path;
			set => SetProperty(ref _path, value);
		}
		public virtual DateTime? Created
		{
			get => _created;
			set => SetProperty(ref _created, value);
		}

		private string _subject;
		private string _path;
		private string _form;
		private List<SubGroup> _subGroups;
		private DateTime? _created = DateTime.Now;
		public MainGroup()
		{

		}
	}
}
