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
using Microsoft.Win32;

namespace CSAS.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public Command HomeCommand { get; }
        public Command MovePrevCommand { get; }
        public Command MoveNextCommand { get; }
        public Command ImportCommand { get; }
        public HomeViewModel HomeViewModel {  get; set; }
        public MainViewModel()
        {
        ViewItems = ViewItems = new ObservableCollection<ViewItem>(new[]
            {
                new ViewItem(
                    "Home",
                    typeof(HomeView)),
                new ViewItem(
                    "Attendance",
                    typeof(AttendanceView)),
                new ViewItem(
                    "Statistics",
                    typeof(StatisticsView)),

            });

            _viewItemsView = CollectionViewSource.GetDefaultView(ViewItems);


            HomeCommand = new Command(_ => { SelectedIndex = 0; });
            ImportCommand = new Command(_ => { OpenFileDialog dialog = new OpenFileDialog(); dialog.ShowDialog(); });

            MovePrevCommand = new Command(
                _ =>
                {
        if (!string.IsNullOrWhiteSpace(SearchKeyword))
            SearchKeyword = string.Empty;

        SelectedIndex--;
    },
                _ => SelectedIndex > 0);

            MoveNextCommand = new Command(
               _ =>
               {
        if (!string.IsNullOrWhiteSpace(SearchKeyword))
            SearchKeyword = string.Empty;

        SelectedIndex++;
    },
               _ => SelectedIndex<ViewItems.Count - 1);

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
        "Palette",
        typeof(HomeViewModel));
}
        }
    }
