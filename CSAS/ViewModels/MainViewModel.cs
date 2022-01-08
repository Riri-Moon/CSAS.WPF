global using Prism.Commands;
global using CSAS.Models;
global using System;
global using System.Linq;
global using System.Collections.Generic;
global using System.Collections.ObjectModel;
global using CSAS.Repositories;
global using CSAS.Services;
global using CSAS.Interfaces;
using CSAS.Views;
using System.ComponentModel;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;
using MaterialDesignColors;
using System.Windows.Media;
using CSAS.Helpers;

namespace CSAS.ViewModels
{
	public class MainViewModel : BaseViewModelBindableBase
	{
		public DelegateCommand HomeCommand { get; }
		public DelegateCommand MovePrevCommand { get; }
		public DelegateCommand MoveNextCommand { get; }
		public DelegateCommand ExitCommand { get; }
		public DelegateCommand<object?> ImportCommand { get; }
		public DelegateCommand OpenExportCommand { get; }
		public ApplicationViewModel ApplicationViewModel { get; set; }
		public HomeViewModel HomeViewModel { get; set; }

		private bool _isDarkTheme;
		public bool IsDarkTheme
		{
			get => _isDarkTheme;
			set 
			{
				ChangeTheme(value);
				SetProperty(ref _isDarkTheme, value);
			} 
		}
		//private DispatcherTimer Timer { get; set; }
		public MainViewModel(int currentGroupId)
		{
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
                //new ViewItem("Export údajov",typeof(ExportView),ApplicationViewModel.ExportViewModel),

            });

			_viewItemsView = CollectionViewSource.GetDefaultView(ViewItems);

			//RunActivityCheck(); 

			//Timer = new DispatcherTimer();
			//Timer.Interval = TimeSpan.FromHours(1);
			//Timer.Tick += Timer_Tick;
			//Timer.Start();

			HomeCommand = new DelegateCommand(HomeButton);
			ImportCommand = new DelegateCommand<object?>(ChangeGroups);
			OpenExportCommand = new DelegateCommand(OpenExport);
			MovePrevCommand = new DelegateCommand(MovePrevious);
			ExitCommand = new DelegateCommand(ExitApplication);
			MoveNextCommand = new DelegateCommand(MoveNext);
		}

		private async void Timer_Tick(object? sender, EventArgs e)
		{
			RunActivityCheck();
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
			exportView.DataContext = new ExportViewModel(ApplicationViewModel.CurrentMainGroupId, ref ApplicationViewModel.AppDbContext);
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

		private void ChangeGroups(object? window)
		{
			object[] param = window as object[];

			MainGroupView mainGroupView = new();
			mainGroupView.DataContext = new();
			mainGroupView.Show();

			MainWindow mainWindow = (MainWindow)param[0];
			mainWindow.Close();
		}

		private static async void RunActivityCheck()
		{
			NotificationHelper helper = new();
			await System.Threading.Tasks.Task.Run(() => {
				helper.SendNotificationsToMe();
				helper.SendNotification();
			});
		}

		private void ChangeTheme(bool isDark)
		{
			//var paletteHelper = new MaterialDesignThemes.Wpf.PaletteHelper();
			//ITheme theme = new MaterialDesignLightTheme();

			////Change the base theme to Dark
			//theme.SetBaseTheme(Theme.Light);
			////or theme.SetBaseTheme(Theme.Light);

			////Change all of the primary colors to Red
			//theme.SetPrimaryColor(Colors.Red);

			////Change all of the secondary colors to Blue
			//theme.SetSecondaryColor(Colors.Blue);

			////You can also change a single color on the theme, and optionally set the corresponding foreground color
			//theme.PrimaryMid = new ColorPair(Colors.Brown, Colors.White);

			////Change the app's current theme
			//paletteHelper.SetTheme(theme);
		}

		public static ITheme SetTheme(String primaryColor, String accentColor, Boolean isDarkTheme)
		{
			ITheme theme;
			Color _accentColor = SwatchHelper.Lookup[ToMaterialDesignColor(accentColor)];
			Color _primaryColor = SwatchHelper.Lookup[ToMaterialDesignColor(primaryColor)];
			if (isDarkTheme)
			{
				theme = Theme.Create(new MaterialDesignDarkTheme(), _primaryColor, _accentColor);
			}
			else
			{
				theme = Theme.Create(new MaterialDesignLightTheme(), _primaryColor, _accentColor);
			}
			return theme;
		}

		private static MaterialDesignColor ToMaterialDesignColor(String color)
		{
			return (MaterialDesignColor)Enum.Parse(typeof(MaterialDesignColor), color, false);
		}
	}
}
