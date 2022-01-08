namespace CSAS.Models
{
	public class BaseWithNameModelBindableBaseBindableBase : BaseModelBindableBase
	{
		public virtual string Name
		{
			get => _name;
			set => SetProperty(ref _name, value);
		}
		private string _name;
	}
}
