using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ViewSample.Panels
{
    /// <summary>
    /// Interaction logic for PopupModal.xaml
    /// </summary>
    public partial class PopupModal : UserControl, INotifyPropertyChanged
    {
        private string _title;
        private string _instructions;
        private ICommand _action;
        private string InstructionsProperty = "Instructions";
        public PopupModal()
        {
            InitializeComponent();
            DataContext = this;
            Visibility = Visibility.Collapsed;
        }

        public String TitleText
        {
            get { return _title; }
            set { _title = value; }
        }

        public String Instructions
        {
            get { return _instructions; }
            set { _instructions = value; OnPropertyChanged(InstructionsProperty); }
        }

        public ICommand Action
        {
            get { return _action; }

            set { _action = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }
    }
}
