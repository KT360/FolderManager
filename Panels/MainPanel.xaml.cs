using GalaSoft.MvvmLight.Command;
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
using ViewSample.Interfaces;
using ViewSample.Models;
using WinForms = System.Windows.Forms;

namespace ViewSample.Panels
{
    /// <summary>
    /// Interaction logic for PanelManager.xaml
    /// </summary>
    public partial class MainPanel : UserControl, IObserver
    {
        private FolderPanel _folderPanel;
        private FilePanel _filePanel;
        private PopupModal _modal;

        public MainPanel()
        {
            InitializeComponent();
            DataContext = this;
            _folderPanel = new FolderPanel();
            _filePanel = new FilePanel();
            _mainGrid.Children.Add(FolderPanel);
            Grid.SetColumn(_filePanel, 1);
            _mainGrid.Children.Add(_filePanel);
        }

        public FolderPanel FolderPanel
        {
            get { return _folderPanel; }
        }

        public FilePanel FilePresenter
        {
            get { return _filePanel; }
        }

        public ICommand AdornPanels => new RelayCommand<RoutedEventArgs>(mainGridLoaded);

        public ICommand CheckFolder => new RelayCommand<MouseEventArgs>(setHoverBackground);

        public ICommand SelectFolder => new RelayCommand<MouseEventArgs>(openFolderBrowser);


        private void init_Panels(DirectoryInfo mainFolder)
        {
            _folderPanel.attatch(this);
            _folderPanel.init(mainFolder);
            _modal = new PopupModal() { TitleText = "Move Files?", Instructions = "Test", Action = new RelayCommand(modalAction) };
            _parentGrid.Children.Add(_modal);
        }

        ///<summary>
        ///Sets the hover background for the selection box at the bottom left of the application
        ///</summary>
        private void setHoverBackground(MouseEventArgs e)
        {

            Grid grid = (Grid)e.Source;
            Label temp = new Label { Content = "Click to select" };

            bool canAdd = true;


            foreach (UIElement child in grid.Children)
            {
                if (child is Label)
                {
                    canAdd = false;
                    temp = (Label)child;
                }
            }


            if (canAdd)//If there is no labels, add one and set the background
            {
                grid.Background = Brushes.Aqua;
                grid.Children.Add(temp);
            }
            else //else remove the label and reset the background(Will be called on MOUSELEAVE)
            {
                grid.Background = Brushes.Transparent;
                grid.Children.Remove(temp);
            }

        }




        ///<summary>
        ///Open folder browser for selection,
        ///initialize main folder path,
        ///add folders to folder panel,
        ///activate folder panel button
        ///</summary>
        private void openFolderBrowser(MouseEventArgs e)
        {
            WinForms.FolderBrowserDialog browser = new WinForms.FolderBrowserDialog();

            Grid grid = (Grid)VisualTreeHelper.GetParent((DependencyObject)e.Source);


            if (browser.ShowDialog() == WinForms.DialogResult.OK)
            {

                Image image = new Image();
                image.Source = (ImageSource)Application.Current.FindResource("main_folder_image");

                grid.Children.Add(image);

                DirectoryInfo mainFolder = new DirectoryInfo(browser.SelectedPath);

                init_Panels(mainFolder);

                foreach (DirectoryInfo subDir in mainFolder.GetDirectories())
                {
                    FolderModel folder = new FolderModel(_folderPanel.MainCanvas, subDir.Name);
                    folder.attatch(_folderPanel);
                    _folderPanel.Models.Add(folder);
                }

                _folderPanel.CanAdd = true;
            }
        }




        
        ///<summary>
        ///Adorn the two panels inside of the grid
        ///</summary>
        private void mainGridLoaded(RoutedEventArgs e)
        {
            
            Grid grid = (Grid)e.Source;

            AdornerLayer layer = AdornerLayer.GetAdornerLayer(grid);

            for (int i = 0; i < 1; i++)
            {
                object element = grid.ColumnDefinitions[i];

                layer.Add(new PanelAdorner((UIElement)element));
            }
            

        }
        
        /// <summary>
        /// If a notification is recieved from the folder panel, update the file panel
        /// Verify if any other action must be perfomed depending on the folderpanel's variables
        /// reset the selection properties of the folders inside the folder panel
        /// </summary>
        /// <param name="subject"></param>
        public void update(ISubject subject)
        {
            if (subject is FolderPanel folderPanel)
            {
                _filePanel.updateDisplay(folderPanel.SelectedFolder);
                if (folderPanel.InSelection)
                {
                    _modal.Instructions = "Copy files from [" + folderPanel.getHilightedFolderName() + "] to [" + folderPanel.SelectedFolderName+" ]";
                    _modal.Visibility = Visibility.Visible;
                }
            }
        }

        private void modalAction()
        {
            _folderPanel.moveFiles();
            _filePanel.updateDisplay(_folderPanel.SelectedFolder);
            _folderPanel.resetFolders();
            _modal.Visibility = Visibility.Collapsed;

        }
    }
}
