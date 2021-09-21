using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
using ViewSample.Models;

namespace ViewSample.Panels
{
    /// <summary>
    /// Interaction logic for FilePanel.xaml
    /// </summary>
    public partial class FilePanel : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<FileModel> _displayedFiles;

        private const String DisplayPropertyName = "DisplayedFiles";

        public event PropertyChangedEventHandler PropertyChanged;

        public FilePanel()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void OnDisplayFilesChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        /// <summary>
        /// Make a new list of files using the one provided
        /// assign it as the current one
        /// notify the view
        /// </summary>
        /// <param name="selectedFolder"></param>
        public void updateDisplay(DirectoryInfo selectedFolder)
        {
            ObservableCollection<FileModel> newFiles = new ObservableCollection<FileModel>();


            foreach (FileInfo f in selectedFolder.GetFiles())
            {
                newFiles.Add(new FileModel(f));
            }

            _displayedFiles = newFiles;
            OnDisplayFilesChanged(DisplayPropertyName);
        }

        public ObservableCollection<FileModel> DisplayedFiles
        {
            get { return _displayedFiles; }
        }

    }
}
