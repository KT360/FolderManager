using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ViewSample.Interfaces;

namespace ViewSample.Models
{
    public class FolderModel : ISubject, INotifyPropertyChanged
    {
        private String _name = "New Folder";

        private Point _mPosition;
        private Point _elPosition;

        private bool _isSelected = false;
        private string SelectionProperty = "IsSelected";

        private List<IObserver> _parents;

        private Panel _drawingParent;

        public FolderModel(Panel parent, String name)
        {
            _drawingParent = parent;
            _parents = new List<IObserver>();
            _name = name;
        }

        public String Name
        {
            get { return _name; }
        }

        public ICommand DragElement => new RelayCommand<MouseEventArgs>(dragFolder);
        public ICommand ClickElement => new RelayCommand<MouseButtonEventArgs>(clickFolder);

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged(SelectionProperty); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void attatch(IObserver observer)
        {
            _parents.Add(observer);
        }
         
        public void notify()
        {
            _parents.ForEach(o => { o.update(this); });
        }



        ///<summary>
        ///Save mouse and element Position
        ///Every click notify the folder panel as well as the view (for border visibility)
        ///On two clicks change the _is selected value to true do not notify the panel so that
        ///it does not think we are performing an action yet
        ///</summary>
        private void clickFolder(MouseButtonEventArgs e)
        {
            Grid folder = (Grid)e.Source;
            _mPosition = e.GetPosition(_drawingParent);
            _elPosition =folder.TransformToAncestor(_drawingParent)
                          .Transform(new Point(0, 0));
            
            
            if (e.ClickCount == 1)
            {
                _isSelected = false;
                notify();
                OnPropertyChanged(SelectionProperty);
            }
            else if (e.ClickCount == 2)
            {
                _isSelected = true;
                OnPropertyChanged(SelectionProperty);

            }//end if-elseif
        }
        /// <summary>
        /// Use saved element and mouse coordinates to modify the grid's transform
        /// </summary>
        /// <param name="e"></param>
        private void dragFolder(MouseEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {

                Grid element = (Grid)e.Source;
                Point currMousePos = e.GetPosition(_drawingParent);
                TranslateTransform tt = (TranslateTransform)element.RenderTransform;

                double dx = currMousePos.X - _mPosition.X;
                double dy = currMousePos.Y - _mPosition.Y;

                tt.X = _elPosition.X + dx;
                tt.Y = _elPosition.Y + dy;

            }
        }
    }
}
