using CSAS.Repositories;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.ViewModels
{
    public class BaseViewModelBindableBase : INotifyPropertyChanged
    {

        public int CurrentMainGroupId { get; set; }
        public AppDbContext AppDbContext
        {
            get => _appDbContext;
            set => SetProperty(ref _appDbContext, value);
        }
        private AppDbContext _appDbContext;
        public UnitOfWork _work
        {
            get => m_work;
            set => SetProperty(ref m_work, value);
        }
        private UnitOfWork m_work;

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
