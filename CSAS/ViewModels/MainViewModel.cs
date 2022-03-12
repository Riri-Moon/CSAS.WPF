global using Prism.Commands;
global using CSAS.Models;
global using System;
global using System.Linq;
global using System.Collections.Generic;
global using System.Collections.ObjectModel;
global using CSAS.Repositories;
global using CSAS.Services;
global using CSAS.Interfaces;
global using IBM.Tools.Common.Helper.Logger;
using CSAS.Views;
using System.ComponentModel;
using System.Windows.Data;
using CSAS.Helpers;
using System.Windows.Threading;

namespace CSAS.ViewModels
{
	public class MainViewModel : BaseViewModelBindableBase
	{
		readonly static Logger _logger = new();
		public DelegateCommand HomeCommand { get; }
		public DelegateCommand MovePrevCommand { get; }
		public DelegateCommand MoveNextCommand { get; }
		public DelegateCommand ExitCommand { get; }
		public DelegateCommand<object?> ImportCommand { get; }
		public DelegateCommand OpenExportCommand { get; }
		public ApplicationViewModel ApplicationViewModel { get; set; }
		public HomeViewModel HomeViewModel { get; set; }
		public string CurrentVersion { get; set; }

		private DispatcherTimer Timer { get; set; }
		public MainViewModel(string currentGroupId, string version)
		{
			CurrentVersion = $"Komplexné hodnotenie študentov - {version}";
			ApplicationViewModel = new ApplicationViewModel(currentGroupId);
			ApplicationViewModel.SetCurrentGroup(currentGroupId);
			ViewItems = ViewItems = new ObservableCollection<ViewItem>(new[]
				{
				new ViewItem("Domov", typeof(HomeView), ApplicationViewModel.HomeViewModel),
				new ViewItem("Dochádzka", typeof(AttendanceView), ApplicationViewModel.AttendanceViewModel ),
				new ViewItem("Vytvoriť aktivitu", typeof(ActivityView),ApplicationViewModel.ActivityViewModel),
				new ViewItem("Konečné hodnotenie",typeof(FinalAssessmentView),ApplicationViewModel.FinalAssessmentViewModel),
				new ViewItem("Štatistika", typeof(StatisticsView),ApplicationViewModel.StatisticsViewModel),
				new ViewItem("Šablóny aktivít",typeof(ActivityTemplateView),ApplicationViewModel.ActivityTemplateViewModel),
				new ViewItem("Nastavenia",typeof(SettingsView),ApplicationViewModel.SettingsViewModel),
			});

			_viewItemsView = CollectionViewSource.GetDefaultView(ViewItems);
#if (!DEBUG)
			try
			{
				 RunActivityCheck();

				Timer = new DispatcherTimer();
				Timer.Interval = TimeSpan.FromHours(1);
				Timer.Tick += Timer_Tick;
				Timer.Start();
				//_logger.InfoAsync("Activity check has successfully run");
			}
			catch (Exception ex)
			{
				_logger.ErrorAsync(ex.Message);
				_logger.ErrorAsync(ex.StackTrace);
			}
#endif
			HomeCommand = new DelegateCommand(HomeButton);
			ImportCommand = new DelegateCommand<object?>(ChangeGroups);
			OpenExportCommand = new DelegateCommand(OpenExport);
			MovePrevCommand = new DelegateCommand(MovePrevious);
			ExitCommand = new DelegateCommand(ExitApplication);
			MoveNextCommand = new DelegateCommand(MoveNext);
		}

		private async void Timer_Tick(object? sender, EventArgs e)
		{
			try
			{
				RunActivityCheck();
			}
			catch (Exception ex)
			{
				_logger.ErrorAsync(ex.StackTrace);
				_logger.ErrorAsync(ex.Message);
			}
		}

		private readonly ICollectionView _viewItemsView;
		private ViewItem? _selectedItem;
		private int _selectedIndex;
		private bool _controlsEnabled = true;

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

		private void ExitApplication()
		{
			App.Current.Shutdown();
		}

		private void HomeButton()
		{
			SelectedIndex = 0;
		}

		private void OpenExport()
		{
			ExportWindow exportView = new();
			exportView.DataContext = new ExportViewModel(ApplicationViewModel.CurrentMainGroupId);
			exportView.ShowDialog();
		}
		private void MovePrevious()
		{
			if (SelectedIndex > 0)
				SelectedIndex--;
		}
		private void MoveNext()
		{
			if (SelectedIndex < ViewItems.Count - 1)
				SelectedIndex++;
		}

		private void ChangeGroups(object? window)
		{
			object[] param = window as object[];

			MainGroupView mainGroupView = new();
			mainGroupView.DataContext = new MainGroupViewModel();
			mainGroupView.Show();

			MainWindow mainWindow = (MainWindow)param[0];
			mainWindow.Close();
		}

		private static async void RunActivityCheck()
		{
			try
			{
				NotificationHelper helper = new();
				await System.Threading.Tasks.Task.Run(() =>
				{
					helper.SendNotificationsToMe();
					helper.SendNotification();
				});
			}
			catch(Exception ex)
			{
				_logger.ErrorAsync(ex.Message);
				_logger.ErrorAsync(ex.StackTrace);
			}
		}
	}
}
