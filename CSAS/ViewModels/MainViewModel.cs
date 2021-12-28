using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using CSAS.Models;
using CSAS.Views;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using Prism.Commands;

namespace CSAS.ViewModels
{
    public class MainViewModel : BaseViewModelBindableBase
    {
        public DelegateCommand HomeCommand { get; }
        public DelegateCommand MovePrevCommand { get; }
        public DelegateCommand MoveNextCommand { get; }
        public DelegateCommand ImportCommand { get; }
        public DelegateCommand OpenExportCommand { get; }
        public ApplicationViewModel ApplicationViewModel { get; set; }
        public HomeViewModel HomeViewModel { get; set; }
        public MainViewModel(int currentGroupId)
        {
            ApplicationViewModel = new ApplicationViewModel(currentGroupId);
            ApplicationViewModel.SetCurrentGroup(currentGroupId);

            ViewItems = ViewItems = new ObservableCollection<ViewItem>(new[]
                {
                new ViewItem("Home", typeof(HomeView), ApplicationViewModel.HomeViewModel),
                new ViewItem("Dochádzka", typeof(AttendanceView), ApplicationViewModel.AttendanceViewModel ),
                new ViewItem("Vytvoriť aktivitu", typeof(ActivityView),ApplicationViewModel.ActivityViewModel),
                new ViewItem("Šablóny aktivít",typeof(ActivityTemplateView),ApplicationViewModel.ActivityTemplateViewModel),
                new ViewItem("Nastavenia",typeof(SettingsView),ApplicationViewModel.SettingsViewModel),
                new ViewItem("Konečné hodnotenie",typeof(FinalAssessmentView),ApplicationViewModel.FinalAssessmentViewModel),
                new ViewItem("Štatistika", typeof(StatisticsView)),
                //new ViewItem("Export údajov",typeof(ExportView),ApplicationViewModel.ExportViewModel),

            });

            _viewItemsView = CollectionViewSource.GetDefaultView(ViewItems);


            HomeCommand = new DelegateCommand(HomeButton);
            // ImportCommand = new DelegateCommand(_ => { OpenFileDialog dialog = new(); dialog.ShowDialog(); });
            OpenExportCommand = new DelegateCommand(OpenExport);
            MovePrevCommand = new DelegateCommand(MovePrevious);

            MoveNextCommand = new DelegateCommand(MoveNext);

        }

        private readonly ICollectionView _viewItemsView;
        private ViewItem? _selectedItem;
        private int _selectedIndex;
        private string? _searchKeyword;
        private bool _controlsEnabled = true;

        public string? SearchKeyword
        {
            get => _searchKeyword;
            set
            {
                if (SetProperty(ref _searchKeyword, value))
                {
                    _viewItemsView.Refresh();
                }
            }
        }

        public ObservableCollection<ViewItem> ViewItems { get; }

        public ViewItem? SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetProperty(ref _selectedIndex, value);
        }

        public bool ControlsEnabled
        {
            get => _controlsEnabled;
            set => SetProperty(ref _controlsEnabled, value);
        }


        private static IEnumerable<ViewItem> GenerateViewItems()
        {
            yield return new ViewItem(
                "Home",
                typeof(HomeViewModel));
        }

        private void HomeButton()
        {
            SelectedIndex = 0;
        }

        private void OpenExport()
        {
            ExportWindow exportView = new();
            exportView.DataContext = ApplicationViewModel.ExportViewModel;
            exportView.ShowDialog();
        }
        private void MovePrevious()
        {
            if (!string.IsNullOrWhiteSpace(SearchKeyword))
                SearchKeyword = string.Empty;
            if (SelectedIndex > 0)
                SelectedIndex--;
        }
        private void MoveNext()
        {
            //if (!string.IsNullOrWhiteSpace(SearchKeyword))
            //    SearchKeyword = string.Empty;
            if (SelectedIndex < ViewItems.Count - 1)
                SelectedIndex++;
        }
    }
}
