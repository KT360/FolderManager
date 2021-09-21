using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewSample.Models
{
    public class FileModel
    {
        private String _fileName = "new file";
        private String _fullPath = "Path";
        public FileModel(FileInfo file)
        {
            _fileName = file.Name;
            _fullPath = file.FullName;
        }

        public ICommand OpenFile => new RelayCommand(openFile);

        private void openFile()
        {
            var p = new Process();
            p.StartInfo = new ProcessStartInfo(@_fullPath)
            {
                UseShellExecute = true
            };
            p.Start();
        }
        public String FileName
        {
            get { return _fileName; }
        }
    }
}
