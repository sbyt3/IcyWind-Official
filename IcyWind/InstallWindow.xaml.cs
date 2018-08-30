using IcyWind.Core.Update.IcyWind;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using Path = System.IO.Path;

namespace IcyWind
{
    /// <summary>
    /// Interaction logic for InstallWindow.xaml
    /// </summary>
    public partial class InstallWindow : Window
    {
        private readonly MainWindow _win;
        public InstallWindow(MainWindow win)
        {
            InitializeComponent();
            win.Hide();
            _win = win;
        }

        public Task<bool> Load(IcyWindVersionJson json, bool install)
        {
            return Run(json, install);
        }

        private async Task<bool> Run(IcyWindVersionJson json, bool install)
        {
            if (install)
            {
                await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                {
                    TextPanel.Children.Add(new Label
                    {
                        Content = "Starting IcyWind Installer",
                        HorizontalContentAlignment = HorizontalAlignment.Center
                    });
                }));
            }
            else
            {
                Title = "IcyWind - Updating";
                await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                {
                    TextPanel.Children.Add(new Label
                    {
                        Content = "Starting IcyWind Updater",
                        HorizontalContentAlignment = HorizontalAlignment.Center
                    });
                }));

                //If files exist delete them
                if (Directory.Exists(Path.Combine(Environment.CurrentDirectory, "IcyWindAssets")))
                {
                    Directory.Delete(Path.Combine(Environment.CurrentDirectory, "IcyWindAssets"), true);
                }

                if (Directory.Exists(Path.Combine(Environment.CurrentDirectory, "core")))
                {
                    Directory.Delete(Path.Combine(Environment.CurrentDirectory, "core"), true);
                }
            }

            using (var client = new WebClient())
            {
                //If files exist delete them
                if (File.Exists(Path.Combine(Environment.CurrentDirectory, "assists.zip")))
                {
                    File.Delete(Path.Combine(Environment.CurrentDirectory, "assists.zip"));
                }

                if (File.Exists(Path.Combine(Environment.CurrentDirectory, "core.zip")))
                {
                    File.Delete(Path.Combine(Environment.CurrentDirectory, "core.zip"));
                }

                await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                {
                    TextPanel.Children.Add(new Label
                    {
                        Content = "Downloading assists.zip",
                        HorizontalContentAlignment = HorizontalAlignment.Center
                    });
                }));

                client.DownloadFile("https://cdn.icywindclient.com/" + json.Latest.Assists,
                    Path.Combine(Environment.CurrentDirectory, "assists.zip"));


                await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                {
                    TextPanel.Children.Add(new Label
                    {
                        Content = "Downloading core.zip",
                        HorizontalContentAlignment = HorizontalAlignment.Center
                    });
                }));

                client.DownloadFile("https://cdn.icywindclient.com/" + json.Latest.Core,
                    Path.Combine(Environment.CurrentDirectory, "core.zip"));


                await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                {
                    TextPanel.Children.Add(new Label
                    {
                        Content = "Extracting assists.zip",
                        HorizontalContentAlignment = HorizontalAlignment.Center
                    });
                }));
                ExtractZipContent(Path.Combine(Environment.CurrentDirectory, "assists.zip"), null,
                    Environment.CurrentDirectory);


                await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                {
                    TextPanel.Children.Add(new Label
                    {
                        Content = "Extracting core.zip",
                        HorizontalContentAlignment = HorizontalAlignment.Center
                    });
                }));
                ExtractZipContent(Path.Combine(Environment.CurrentDirectory, "core.zip"), null,
                    Environment.CurrentDirectory);

                await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                {
                    TextPanel.Children.Add(new Label
                    {
                        Content = "Cleaning up files",
                        HorizontalContentAlignment = HorizontalAlignment.Center
                    });
                }));

                File.Delete(Path.Combine(Environment.CurrentDirectory, "assists.zip"));
                File.Delete(Path.Combine(Environment.CurrentDirectory, "core.zip"));

                await Dispatcher.BeginInvoke(DispatcherPriority.Render, (Action)(() =>
                {
                    TextPanel.Children.Add(new Label
                    {
                        Content = "Starting up...",
                        HorizontalContentAlignment = HorizontalAlignment.Center
                    });
                    Close();
                }));
            }

            return true;
        }


        public void ExtractZipContent(string fileZipPath, string password, string outputFolder)
        {
            ZipFile file = null;
            try
            {
                var fs = File.OpenRead(fileZipPath);
                file = new ZipFile(fs);

                if (!string.IsNullOrEmpty(password))
                {
                    // AES encrypted entries are handled automatically
                    file.Password = password;
                }

                foreach (ZipEntry zipEntry in file)
                {
                    if (!zipEntry.IsFile)
                    {
                        // Ignore directories
                        continue;
                    }

                    String entryFileName = zipEntry.Name;
                    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    // Optionally match entrynames against a selection list here to skip as desired.
                    // The unpacked length is available in the zipEntry.Size property.

                    // 4K is optimum
                    var buffer = new byte[4096];
                    var zipStream = file.GetInputStream(zipEntry);

                    // Manipulate the output filename here as desired.
                    var fullZipToPath = Path.Combine(outputFolder, entryFileName);
                    var directoryName = Path.GetDirectoryName(fullZipToPath);

                    if (!string.IsNullOrEmpty(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    // of the file, but does not waste memory.
                    // The "using" will close the stream even if an exception occurs.
                    using (var streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }
            finally
            {
                if (file != null)
                {
                    file.IsStreamOwner = true; // Makes close also shut the underlying stream
                    file.Close(); // Ensure we release resources
                }
            }
        }
    }
}
