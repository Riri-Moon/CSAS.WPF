using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CSAS.ViewModels
{
	public class BaseViewModelBindableBase : INotifyPropertyChanged
	{

		public string CurrentMainGroupId { get; set; }
		public AppDbContext AppDbContext
		{
			get => _appDbContext;
			set => SetProperty(ref _appDbContext, value);
		}
		private AppDbContext _appDbContext;
		public UnitOfWork Work
		{
			get => m_work;
			set => SetProperty(ref m_work, value);
		}
		private UnitOfWork m_work;

		private bool _isLoading = false;
		public bool IsLoading
		{
			get => _isLoading;
			set => SetProperty(ref _isLoading, value);
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		protected virtual bool SetProperty<T>(ref T member, T value, [CallerMemberName] string? propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(member, value))
			{
				return false;
			}

			member = value;
			OnPropertyChanged(propertyName);
			return true;
		}

		protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
