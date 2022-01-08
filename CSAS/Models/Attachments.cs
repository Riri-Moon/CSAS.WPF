namespace CSAS.Models
{
	public class Attachments : BaseModelBindableBase
	{
		public virtual string PathToFile
		{
			get => _pathToFile;

			set => SetProperty(ref _pathToFile, value);
		}
		private string _pathToFile;

		public virtual Activity Activity
		{
			get => _activity;
			set => SetProperty(ref _activity, value);
		}
		private Activity _activity;
	}
}
