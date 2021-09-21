using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ViewSample.Utilities
{
    class PanelAdorner : Adorner
    {

        private Thumb _thumb;
        public VisualCollection _visualChildren;
        public double _adornerWidth;
        public double _adornerHeight;
        public List<Shape> _shapes;


        public PanelAdorner(UIElement adornedElement) : base(adornedElement)
        {
            _visualChildren = new VisualCollection(this);
            _shapes = new List<Shape>();
            

            _adornerWidth = DesiredSize.Width;
            _adornerHeight = DesiredSize.Height;


            _thumb = new Thumb { Width = 10, Height = 10, Background = Brushes.Red };


            _thumb.DragDelta += new DragDeltaEventHandler(OnResizeThumbDragDelta);

            //???
            _visualChildren.Add(_thumb);
            foreach (Shape shape in _shapes)
            {
                _visualChildren.Add(shape);
            }

        }

        //Draws a rectangle at the bottom left corner of an element
        protected override void OnRender(DrawingContext drawingContext)
        {
           

        }

        
        private void OnResizeThumbDragDelta(object sender, DragDeltaEventArgs e)
        {

            changePanelSize((ColumnDefinition)AdornedElement, e);

        }

        private void changePanelSize(ColumnDefinition column, DragDeltaEventArgs e)
        {

            double width = column.ActualWidth;
            
            double xAdjust = width + e.HorizontalChange;

            xAdjust = (xAdjust > 100) ? xAdjust : 100;
           

            column.Width = new GridLength(xAdjust, GridUnitType.Pixel);
        }
        ///<summary>
        ///Every time Arrange is called (Mouse drag) Resize the selected grid-column, Reposition the thumb
        ///</summary>
        protected override Size ArrangeOverride(Size finalSize)
        {
            //First set the Width and height of the panel
            //Then, position the thumb
            Panel panel = Utils.getPanel<Panel>(AdornedElement, true);
            DependencyObject firstParent = VisualTreeHelper.GetParent(panel);


            while (!(firstParent == null) && !(firstParent is Grid))
            {
                firstParent = VisualTreeHelper.GetParent(firstParent);
            }


            Grid grid = (Grid)firstParent;
            ColumnDefinition column = grid.ColumnDefinitions[Grid.GetColumn(panel)];


            panel.Width = column.ActualWidth;
            panel.Height = grid.ActualHeight;


            double desiredWidth = AdornedElement.DesiredSize.Width;
            double desiredHeight = AdornedElement.DesiredSize.Height;


            _adornerWidth = DesiredSize.Width;
            _adornerHeight = DesiredSize.Height;


            double x = desiredWidth - _adornerWidth / 2;
            double y = desiredHeight - _adornerHeight / 2;


            _thumb.Arrange(new Rect(x, y, _adornerWidth, _adornerHeight));


            foreach (Shape shape in _shapes)
            {
                shape.Arrange(new Rect(0, 0, _adornerWidth, _adornerHeight));
            }

            return finalSize;
        }
        protected override Visual GetVisualChild(int index) { return _visualChildren[index]; }

        protected override int VisualChildrenCount { get { return _visualChildren.Count; } }



    }
}
