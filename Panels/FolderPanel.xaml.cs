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
using ViewSample.Utilities;

namespace ViewSample.Panels
{
    /// <summary>
    /// Interaction logic for FolderPanel.xaml
    /// </summary>
    public partial class FolderPanel : UserControl, INotifyPropertyChanged, IObserver, ISubject
    {
        //For data binding
        private ObservableCollection<FolderModel> _data;


        private bool _inSelection = false;
        private bool _canAdd = false;


        //Handled properties
        private const String ButtonProperty = "CanAdd";
        private const String DisplayProperty = "SelectedFolder";


        private MainPanel _mainPanel;


        //Variables to track folder information
        private DirectoryInfo _selectedFolder;
        private DirectoryInfo _parentFolder;
        private String _mainFolderPath;


        private Canvas _mainCanvas;


        private List<IObserver> _observers;


        public FolderPanel()
        {
            InitializeComponent();
            DataContext = this;
            _data = new ObservableCollection<FolderModel>();
            _itemsControl.ItemContainerGenerator.StatusChanged += handleItemsControl;
            _observers = new List<IObserver>();
            
        }

        /// <summary>
        /// Once the children and containers of the view's itemsControl are generated, save the Canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void handleItemsControl(object sender, EventArgs e)
        {
            if (_itemsControl.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
            {
                _mainCanvas = Utils.GetVisualChild<Canvas>(_itemsControl);
                _itemsControl.ItemContainerGenerator.StatusChanged -= handleItemsControl;
            }
        }


        public bool InSelection
        {
            get { return _inSelection; }
        }

        public ObservableCollection<FolderModel> Models
        {
            get { return _data; }
        }

        public ICommand AddFolder => new RelayCommand(param => addModel(), param => true);

        public DirectoryInfo SelectedFolder => _selectedFolder;



        //Segment of properties and eventhandler methods to take care of them
        public event PropertyChangedEventHandler DisplayPropertyChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnDisplayPropertyChanged(String propertyName)
        {
            if (DisplayPropertyChanged != null)
            {
                DisplayPropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            
        }

        private void OnButtonPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool CanAdd
        {
            get { return _canAdd; }
            set
            {
                if (_canAdd == value) return;
                _canAdd = value;
                OnButtonPropertyChanged(ButtonProperty);
            }
        }

        public Canvas MainCanvas => _mainCanvas;

        public String SelectedFolderName
        {
            get { return _selectedFolder.Name; }
        }

        /// <summary>
        /// Once this method is called
        /// First check wether selection is occuring
        /// if so return the selected folder's name
        /// else return null
        /// </summary>
        /// <returns></returns>
        public String getHilightedFolderName()
        {
            if (_inSelection)
            {
                string name = "";

                foreach (FolderModel model in _data)
                {
                    if (model.IsSelected)
                    {
                        name = model.Name;
                    }

                }

                return name;
            }
            else
            {
                return null;
            }
        }

        public void init(DirectoryInfo mainFolder)
        {
            _parentFolder = mainFolder;
            _mainFolderPath = _parentFolder.FullName;
        }

        /// <summary>
        /// Adds a new FolderModel object to the data bound list
        /// </summary>
        private void addModel()
        {
            String folderName = "NewFolder";

            _parentFolder.CreateSubdirectory(folderName);

            FolderModel model = new FolderModel(_mainCanvas, folderName);

            model.attatch(this);

            _data.Add(model);
        }

        /// <summary>
        /// Check if a folder had already been selected before hand,
        /// Set the selected folder to the clicked folder
        /// notify the main panel
        /// </summary>
        /// <param name="subject"></param>
        public void update(ISubject subject)
        {
            _inSelection = false;

            foreach (FolderModel model in _data)
            {
                if (model.IsSelected)
                {
                    _inSelection = true;
                }
               
            }

            FolderModel folder = (FolderModel)subject;
            //Set the selected folder using the name
            _selectedFolder = new DirectoryInfo(_mainFolderPath + "\\" + folder.Name);
            notify();
        }

        public void resetFolders()
        {
            foreach (FolderModel model in _data)
            {
                model.IsSelected = false;
            }
        }

        public void attatch(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void notify()
        {
            _observers.ForEach(o => { o.update(this); });
        }

        public void moveFiles()
        {
            DirectoryInfo source = new DirectoryInfo(_mainFolderPath + "\\" + getHilightedFolderName());
            //Debug.WriteLine(getHilightedFolderName());
            foreach (FileInfo file in source.GetFiles())
            {
                string newFilePath = _selectedFolder.FullName + "\\" + file.Name;
                File.Move(file.FullName, newFilePath);
                
            }
        }
    }
}
