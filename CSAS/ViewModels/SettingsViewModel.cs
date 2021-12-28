using CSAS.Models;
using CSAS.Repositories;
using Microsoft.VisualStudio.PlatformUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.ViewModels
{
    public class SettingsViewModel : BaseViewModelBindableBase
    {
        public DelegateCommand SaveCommand { get; }

        private Settings _settings;

        public Settings Settings
        {

            get { return _settings; }
            set
            {
                SetProperty(ref _settings, value);
            }
        }

        public UnitOfWork _work { get; set; }
        public SettingsViewModel(int currGroupId, ref AppDbContext context)
        {
            _work = new UnitOfWork(context); 
            Settings = new Settings();

            if (_work.Settings.GetAll().FirstOrDefault() == null)
            {
                _work.Settings.Add(Settings);
                _work.Complete();
            }
            Settings = _work.Settings.GetAll().FirstOrDefault();
            SaveCommand = new DelegateCommand(SaveSettings);
        }

        private void SaveSettings()
        {
            _work.Settings.Update(Settings);
            _work.Complete();
        }

    }
}
