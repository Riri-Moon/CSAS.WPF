using CSAS.Models;
using CSAS.Repositories;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.ViewModels
{
    public class SelectedActivityViewModel : BaseViewModelBindableBase
    {
        public Action CloseAction { get; set; }
        public DelegateCommand SaveChangesCommand { get; }
        public UnitOfWork _work { get; set; }

        private ObservableCollection<Models.Task> _tasks;
        public ObservableCollection<Models.Task> Tasks
        {
            get { return _tasks; }
            set
            {
                SetProperty(ref _tasks, value);
            }
        }

        private Activity _activity;
        public Activity Activity
        {
            get { return _activity; }
            set
            {
                SetProperty(ref _activity, value);
            }
        }

        public SelectedActivityViewModel(int currentGroupId, int activityId)
        {
            CurrentMainGroupId = currentGroupId;
            Activity = new Activity();
            AppDbContext appDbContext = new AppDbContext();
            _work = new UnitOfWork(appDbContext);
            Activity = _work.Activity.Get(activityId);

            SaveChangesCommand = new DelegateCommand(SaveChanges);
        }

        public Models.Task? SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        private void SaveChanges()
        {
            _work.Activity.Update(Activity);
            _work.Complete();

        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetProperty(ref _selectedIndex, value);
        }
        private Models.Task? _selectedItem;
        private int _selectedIndex;
    }
}
