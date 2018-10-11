using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using IcyWind.Injector.Injector;
using Microsoft.Win32;

namespace IcyWind.Injector
{
    public class RunningProcess
    {
        public Process Process { get; set; }

        public string PName => Process.ProcessName;

        public string PId => Process.Id.ToString();
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public string InjectFileName { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        public void GetAllProcesses()
        {
            var allProcesses = Process.GetProcesses();
            var runProcList = allProcesses.Select(proc => new RunningProcess {Process = proc}).ToList();
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
        }
    }
}
