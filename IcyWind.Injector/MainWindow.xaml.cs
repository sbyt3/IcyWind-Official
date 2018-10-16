using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using IcyWind.Injector.Injector;
using MahApps.Metro.Controls;
using Microsoft.Win32;

namespace IcyWind.Injector
{
    public class RunningProcess
    {
        public Process Process { get; set; }

        public string PName => Process.ProcessName;
        
        public string PTitle => string.IsNullOrWhiteSpace(Process.MainWindowTitle) ? "No Proc Title" : Process.MainWindowTitle;
        
        public string PId => Process.Id.ToString();
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private Process[] _allProcesses;
       
        public string InjectFileName { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            GetAllProcesses();
        }

        public void GetAllProcesses(bool onlyVisible = false)
        {
            _allProcesses = Process.GetProcesses().Where(x => 
                (!string.IsNullOrWhiteSpace(x.MainWindowTitle) || 
                !onlyVisible) && 
                x.ProcessName != "IcyWind.Injector").
                ToArray();
            var runProcList = _allProcesses.Select(proc => new RunningProcess {Process = proc}).ToList();
            Processes.ItemsSource = new ObservableCollection<RunningProcess>(runProcList);
        }

        private void OpenDll_OnClick(object sender, RoutedEventArgs e)
        { 
            var openFile = new OpenFileDialog
            {
                Filter = "Dynamic Link Library|*.dll", Title = "Select the .dll file you wish to inject"
            };
            
            if (openFile.ShowDialog() == true)
            {
                InjectFileName = openFile.FileName;
            }

            if (File.Exists(InjectFileName))
            {
                var info = FileVersionInfo.GetVersionInfo(InjectFileName);

                if (!info.FileDescription.Contains("IWJTarget-")) return;
                var getTarget = info.FileDescription.Split('-');
                FilterBox.Text = $"'{getTarget[1]}'";
                InjectAbout.Content = $"Injecting: {info.FileName.Split('\\').Last()} - {getTarget[2]}";
                FilterBox.IsEnabled = false;
            }
        }

        private void Inject_OnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(InjectFileName))
            {
                MessageBox.Show("You must select a dll to inject", "Select inject dll", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!(Processes.SelectedItem is RunningProcess selectedProcess))
                return;

            MessageBox.Show($"Starting to inject: {InjectFileName} into {selectedProcess.PName}");

            DllInjector.GetInstance.Inject(selectedProcess.Process, InjectFileName);
        }

        private void UpdateProcesses_OnClick(object sender, RoutedEventArgs e)
        {
            GetAllProcesses();
            FilterBox_OnTextChanged(this, null);
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            GetAllProcesses(true);
            FilterBox_OnTextChanged(this, null);
        }

        private void ToggleButton_OnUnchecked(object sender, RoutedEventArgs e)
        {
            GetAllProcesses();
            FilterBox_OnTextChanged(this, null);
        }

        private void FilterBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FilterBox.Text))
            {
                var runProcList = _allProcesses.Select(proc => new RunningProcess { Process = proc }).ToList();
                Processes.ItemsSource = new ObservableCollection<RunningProcess>(runProcList);
            }
            else
            {
                if (FilterBox.Text.Contains('\''))
                {
                    var runProcList = _allProcesses.Where(x => x.ProcessName == FilterBox.Text.Split('\'')[1]).
                        Select(proc => new RunningProcess { Process = proc }).ToList();
                    Processes.ItemsSource = new ObservableCollection<RunningProcess>(runProcList);
                }
                else
                {
                    var runProcList = _allProcesses.Where(x => x.ProcessName.ToLower().Contains(FilterBox.Text.ToLower())).
                        Select(proc => new RunningProcess { Process = proc }).ToList();
                    Processes.ItemsSource = new ObservableCollection<RunningProcess>(runProcList);
                }
            }
        }

        private void AboutButton_OnClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
