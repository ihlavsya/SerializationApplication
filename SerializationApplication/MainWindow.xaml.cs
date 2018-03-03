using Avalon.Windows.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SerializationApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool handle = true;
        private string SavingPath { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (handle) Handle();
            handle = true;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            handle = !cmb.IsDropDownOpen;
            Handle();
        }

        private void Handle()
        {
            switch (cmbSelectSerialization.SelectedItem.ToString().Split(new string[] { ": " }, StringSplitOptions.None).Last())
            {
                case "Serialization using binary formatter.":
                    StartSerialization(1);
                    break;
                case "Custom serialization using binary reader and writer.":
                    StartSerialization(2);
                    break;
            }
        }

        private void ButtonPath_Click(object sender, RoutedEventArgs e)
        {
            var dlgFolder = new Avalon.Windows.Dialogs.FolderBrowserDialog()
            {
                RootPath = Environment.SystemDirectory
            };
            dlgFolder.ShowDialog();
            SavingPath = dlgFolder.SelectedPath;
            cmbSelectSerialization.IsEnabled = true;
        }

        private void ButtonDeserialization_Click(object sender, RoutedEventArgs e)
        {
            var deserializeDialog = new Microsoft.Win32.OpenFileDialog();
            deserializeDialog.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            deserializeDialog.CheckFileExists = true;

            deserializeDialog.ShowDialog();
            string path = deserializeDialog.FileName;

            string folderPath = Environment.SystemDirectory;

            var dlgFolder = new FolderBrowserDialog()
            {
                RootPath = Environment.SystemDirectory
            };

            dlgFolder.ShowDialog();
            folderPath = dlgFolder.SelectedPath;

            if (path.Contains("defaultserialization"))
            {
                DeserializeProccess deserializeProccess = new DeserializeProccess(path,folderPath);
                deserializeProccess.ForDeserializingDirectory();
            }
            else
            {
                DeserializationFileReading deserializationFileReading = new DeserializationFileReading(path, folderPath);
                deserializationFileReading.ReadFile();
            }
        }

        private void StartSerialization(int serializationType)
        {
            var dlgFolder = new Avalon.Windows.Dialogs.FolderBrowserDialog()
            {
                RootPath = Environment.SystemDirectory
            };

            dlgFolder.ShowDialog();
            string selectedPath = dlgFolder.SelectedPath;

            switch (serializationType)
            {
                case 2:
                    {
                        FolderWriting folderWriting = new FolderWriting(SavingPath,selectedPath);
                        folderWriting.StartWriting();
                        break;
                    }
                case 1:
                    {
                        SerializeProccess serializeProccess = new SerializeProccess(SavingPath,selectedPath);
                        serializeProccess.ForSerializingDirectory();
                        break;
                    }
            }
        }
    }
}
